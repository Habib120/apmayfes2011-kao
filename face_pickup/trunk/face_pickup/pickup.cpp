//#define _CRTDBG_MAP_ALLOC

#include <stdio.h>
#include <cv.h>
#include <highgui.h>
#include <ctype.h>
#include <iostream>
#include <sm_api_cxx.h>
#include <conio.h>
#include "sm_api.h"
#include <cmath>
#include <time.h>
#include <ml.h>
#include "cvgabor.h"
//#include <stdlib.h>
//#include <crtdbg.h>

using std::cout;
using std::endl;
using std::cerr;
using std::cin;
using std::string;
using namespace sm::faceapi;

double x,y,z,rad;


double predict(IplImage *src)
{
			//cvNamedWindow("test1",1);
			//cvShowImage("test1",&dimg);
            //clock_t start, end;
            //cout << "初期化: ";
            //start = clock();
            //IplImage *src=0;
			//IplImage *input=0;
			//input = &dimg;
            //input = cvLoadImage( "./face_0082.jpg" , CV_LOAD_IMAGE_GRAYSCALE);
            //src = cvCreateImage(cvSize(128,128), IPL_DEPTH_8U, 1);
			//dst = cv/CreateImage(cvSize(128,128), IPL_DEPTH_8U, 1);
            int size = 400; //工藤参上！

            //cvResize(input, src, CV_INTER_LINEAR); //画像を縮小
            //end = clock();
            //printf( "%.5f秒かかりました\n", (double)(end - start) /CLOCKS_PER_SEC );

            CvMat *mat = cvCreateMat(src->width, src->height, CV_32FC1); //画像と同じサイズの行列を生成
			CvMat *smat = cvCreateMat(20, 20, CV_32FC1); //縮小先の行列を生成
            CvMat *feat = cvCreateMat(1, size*40, CV_32FC1); //特徴量を格納するベクトルを生成
            CvMat row_header, *row;
            CvMat tmp;
			//cvNamedWindow("test2");
			//cvShowImage("test1",input);
             int k = 0;
             for (int u = 0; u < 5; u++)
            {
                         for (int v = 0; v < 8; v++)
                        {
									//cvShowImage("test1",&src);
                                    //cout << " カーネルの計算 : ";
                                    //start = clock();
                                    CvGabor gabor(u, v);
                                    //end = clock();
                                    //printf( "%.5f秒かかりました\n", (double)(end - start) /CLOCKS_PER_SEC );
                                    //cout << " 特徴量の計算 : ";
                                    //start = clock();
									//gabor.conv_img(src, dst);
									//cvNamedWindow("dst");
									//cvShowImage("dst", dst);
									//cvWaitKey();
									//cvDestroyWindow("dst");
									gabor.get_value(src, mat);
									//cvShowImage("test2", mat);
									//for (int n = 0; n < 10; n++)
									//{
									//	cout << CV_MAT_ELEM(*mat, float, 0, n) << endl;
									//}
                                    //end = clock();
                                    //printf( "%.5f秒かかりました\n", (double)(end - start) /CLOCKS_PER_SEC );
									cvResize(mat, smat, CV_INTER_LINEAR); //行列を縮小
                                    //cout << " 値の代入: ";
                                    //start = clock();
									row = cvReshape(smat, &row_header, 0, 1);
									//cout << CV_MAT_ELEM(*row, float, 0, size-1) << endl;
                                    cvGetCols(feat, &tmp, size*k, size*(k+1));
									cvCopy(row, &tmp);
                                    //end = clock();
                                    //printf( "%.5f秒かかりました\n", (double)(end - start) /CLOCKS_PER_SEC );
                                    k = k+1;
                        }
            }
            //cout << CV_MAT_ELEM(*feat, float, 0, size-1) << endl;

            //cvNamedWindow("src");
            //cvShowImage("src", src);
            //cvWaitKey();
            //cvDestroyWindow("src");
			//cvReleaseImage(&input);
			cvReleaseImage(&src);
			//cvReleaseImage(&dst);
			cvReleaseMat(&mat);
			cvReleaseMat(&smat);

			//end = clock();
			//printf( "%.5f秒かかりました\n", (double)(end - start) /CLOCKS_PER_SEC );

			/*****RandomTrees*****/
			CvMat *test = cvCreateMat(1, size*40, CV_32FC1);
			double r;
			CvERTrees ert;
			ert.load("./test.xml");
			//cout << ert.get_tree_count() << endl;
			r = ert.predict(test);
			//cout << "認識結果：" << r << endl;

			cvReleaseMat(&feat);

			return r;
}




void STDCALL recieveHeadPose(void *,smEngineHeadPoseData head_pose, smCameraVideoFrame video_frame)
{
    smImageInfo video_frame_image_info;
    smImageGetInfo(video_frame.image_handle, &video_frame_image_info);     
	x = head_pose.head_pos.x;
	y = head_pose.head_pos.y;
	z = head_pose.head_pos.z;
	//cout << z << endl;
	//char msg[256];
	//sprintf(msg, "%5f", x);
	//cout << msg << endl;
	rad = head_pose.head_rot.z_rads;


}

void rotateImage( IplImage *img, double angle, cv::Point2f center)
{
	IplImage *tmp = cvCreateImage(cvGetSize(img), IPL_DEPTH_8U, 3);
	CvMat M;
	float m[6];

	m[0] = (float) (cos (angle));
	m[1] = (float) (-sin (angle));
	m[2] = center.x;
	m[3] = -m[1];
	m[4] = m[0];
	m[5] = center.y;
	cvInitMatHeader (&M, 2, 3, CV_32FC1, m, CV_AUTOSTEP);
	cvGetQuadrangleSubPix(img,tmp,&M);

	cvCopy(tmp, img, NULL);
	cvReleaseImage(&tmp);
}

int main ()
{
  //_CrtDumpMemoryLeaks();
  int center_row,center_col,start_col,end_col,start_row,end_row;
  cv::Point2f center;
  cv::Mat mat,dst;
  cv::Size sz,cam_sz;

  cv::VideoCapture cap(CV_CAP_ANY);
  if(!cap.isOpened()) 
        return -1;

  smAPIInit();	//faceAPI初期化	
  smCameraRegisterType(SM_API_CAMERA_TYPE_WDM);	//カメラタイプの登録（必須）
	
  //faceAPIエンジン作成	
  smEngineHandle engine = 0;	
  smEngineCreate(SM_API_ENGINE_TYPE_HEAD_TRACKER_V2,&engine);	
  smHTRegisterHeadPoseCallback(engine,0,recieveHeadPose);
  //faceAPI表示ディスプレイ作成
  smVideoDisplayHandle video_display_handle = 0;
  smVideoDisplayCreate(engine, &video_display_handle,0,TRUE);
  smVideoDisplaySetFlags(video_display_handle,SM_API_VIDEO_DISPLAY_HEAD_MESH);

  //faceAPIトラッキング開始
  smEngineStart(engine);

  cv::namedWindow("Capture",CV_WINDOW_AUTOSIZE);
  cap >> mat;


  for(;;)
    {
		smAPIProcessEvents();
        const int frame_period_ms = 30;
        Sleep(frame_period_ms); 
		if( z != 0 )
		{   
			cam_sz = mat.size();
			center_row = cam_sz.height/2 + (25 - y*(cam_sz.height/0.4)*cos(rad))*0.4/z;
			center_col = cam_sz.width/2 + (((x*cam_sz.width+4)/0.4)*cos(rad) + y*(cam_sz.height/0.3)*sin(rad))*0.4/z;
			center.x=center_col;
			center.y=center_row;
			//cout << cam_sz.height << endl;
			sz.width = 2*floor(cam_sz.height/(z*10));
			//cout << sz.width << endl;
			sz.height = sz.width;
			start_col = center_col - sz.width/2;
			end_col = center_col + sz.width/2;
			start_row = center_row - sz.height/2;
			end_row = center_row + sz.height/2;
			//rot = cv::getRotationMatrix2D(center,rad,1);
			//mat.create(sz.height,sz.width,CV_16SC3);
			cap >> mat;
			//cout << "(" << center.x << "," << center.y << ")" << endl;
			cv::flip(mat,mat,CV_16SC3);
			IplImage img = mat;
			rotateImage(&img,-rad,center);
			cv::Mat dst(&img);
			cv::Mat dst2 = dst;
	
			if(start_row > 0 && end_row < cam_sz.height-20 && start_col > 10 && end_col < cam_sz.width-30){
			dst = dst.rowRange(cam_sz.height/2-sz.height/2,cam_sz.height/2+sz.height/2);
			dst = dst.colRange(cam_sz.width/2-sz.width/2,cam_sz.width/2+sz.width/2);
				//cv::getRectSubPix(mat,sz,center,dst);			
			//cv::imshow("Capture", dst);

			double r=0;
			cv::cvtColor(dst,dst2,CV_RGB2GRAY,0);
			IplImage dimg = dst2;
			IplImage *src;
			src = cvCreateImage(cvSize(128,128), IPL_DEPTH_8U, 1);
			cvResize(&dimg, src, CV_INTER_LINEAR);
			//cv::imshow("Capture", dst);
			cvShowImage("Capture",src);
			r = predict(src);
			cout << "result" << r << endl;
			}
		}

		if(cv::waitKey(30) >= 0){
			break;
		}
  }
  smEngineDestroy(&engine);
  smVideoDisplayDestroy(&video_display_handle);

  smAPIQuit();

  cvDestroyWindow("Capture");
  
  return 0;
}