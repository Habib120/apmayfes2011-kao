#include "sm_api.h"
#include "tracker.h"
#include "cv.h"
#include "util.h"
#include "structure.h"
#include <boost/ref.hpp>

void STDCALL recieveHeadPose(void *,smEngineHeadPoseData head_pose, smCameraVideoFrame video_frame);
boost::mutex mt_pose;
 boost::mutex mt_headdata;
boost::mutex mt_camimage;
HeadPose pose;

void STDCALL recieveHeadPose(void *,smEngineHeadPoseData head_pose, smCameraVideoFrame video_frame)
{
	boost::mutex::scoped_lock lk(mt_pose);
    smImageInfo video_frame_image_info;
    smImageGetInfo(video_frame.image_handle, &video_frame_image_info);     
	pose.x = head_pose.head_pos.x;
	pose.y = head_pose.head_pos.y;
	pose.z = head_pose.head_pos.z;
	pose.rx = head_pose.head_rot.x_rads;
	pose.ry = head_pose.head_rot.y_rads;
	pose.rz = head_pose.head_rot.z_rads;
	pose.rot = head_pose.head_rot.z_rads;
}

void HeadTracker::Start(bool with_dbg)
{
	this->with_dbg = with_dbg;

	
	/**
	 * Start Camera Capture
	 */
	//cap = cvCreateCameraCapture(0);
	//cframe = cvQueryFrame(cap);

	/**
	 * Start FaceAPI
	 */
	smAPIInit();
	smCameraRegisterType(SM_API_CAMERA_TYPE_WDM);	//カメラタイプの登録（必須）
	
	smEngineCreate(SM_API_ENGINE_TYPE_HEAD_TRACKER_V2,&engine);	
	smHTRegisterHeadPoseCallback(engine,0,recieveHeadPose);
	if (this->with_dbg)
	{
		smVideoDisplayCreate(engine, &video_display_handle,0,TRUE);
		smVideoDisplaySetFlags(video_display_handle,SM_API_VIDEO_DISPLAY_HEAD_MESH);
	}
    smEngineStart(engine);

	cframe = cvCreateImage(cvSize(640, 480), IPL_DEPTH_8U, 3);
	cvSetZero(cframe);
	loop_thread = new boost::thread(boost::ref(*this));
}

/**
 * キャプチャのメイン処理
 */
void HeadTracker::operator()()
{
	while (!stop)
	{	
		smAPIProcessEvents();

		//Getting current head pose(Thread safe)
		boost::mutex::scoped_lock lock(mt_pose, boost::defer_lock);
		HeadPose *cpose = 0;
		lock.lock();
		{
			cpose = new HeadPose(pose);
		}
		lock.unlock();
		
		if( cpose->isValueSet() )
		{   
			IplImage* face_img;
			//Get new camera capture frame(Thread safe)
			boost::mutex::scoped_lock clock(mt_camimage, boost::defer_lock);
			clock.lock();
				//cframe = cvQueryFrame(cap);
				face_img = cvCreateImage(cvSize(100, 100), IPL_DEPTH_8U, 3);
			clock.unlock();
			
			//Getting current head data(Thread safe)
			boost::mutex::scoped_lock hlock(mt_headdata, boost::defer_lock);
			hlock.lock();
			{
				//Release an old HeadData object
				if (data != 0)
				{
					data->ReleaseImage(); //HeadDataの画像データは明示的に開放する必要がある。
					delete data;
					data = 0;
				}
				data = new HeadData();
				data->SetImage(face_img);
				data->pose = *cpose;
			}
			hlock.unlock();
		}
		else 
		{
			//Release an old HeadData object
			if (data != 0)
			{
				data->ReleaseImage(); //HeadDataの画像データは明示的に開放する必要がある。
				delete data;
				data = 0;
			}
		}
		delete cpose;
	}
}
HeadPose HeadTracker::GetCurrentHeadPose()
{
	boost::mutex::scoped_lock plock(mt_pose);
	return pose;
}

HeadData* HeadTracker::GetCurrentHeadData()
{
	boost::mutex::scoped_lock plock(mt_pose);
	boost::mutex::scoped_lock hlock(mt_headdata);
	{
		if (data == 0)
		{
			return 0;
		}
		HeadData* ret = new HeadData();
		IplImage* src_img = data->GetImage();
		IplImage* ret_img = cvCreateImage(cvGetSize(src_img), src_img->depth, src_img->nChannels);
		cvCopy(src_img, ret_img);
		ret->SetImage(ret_img);
		ret->pose = pose;

		return ret;
	}
}

IplImage* HeadTracker::GetCurrentCamImage()
{
	boost::mutex::scoped_lock lock(mt_camimage);
	IplImage *ret_img = cvCreateImage(cvGetSize(cframe), cframe->depth, cframe->nChannels);
	cvCopy(cframe, ret_img);
	return ret_img;
}

void HeadTracker::Stop()
{
	stop = true;
	loop_thread->join();

	//Quit FaceAPI
	smAPIProcessEvents();
	smEngineDestroy(&engine);
	if (this->with_dbg)
	{
		smVideoDisplayDestroy(&video_display_handle);
	}
	smAPIQuit();
	//Quit camera capture
	//なぜかこうしないとキャプチャの解放ができない。
	//cap = cvCreateCameraCapture(0);
	//cvReleaseCapture(&cap);
}