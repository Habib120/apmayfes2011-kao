#include "util.h"
#include "structure.h"

/**
 * 画像を回転させるロジック
 */
void ImageUtils::rotateImage( IplImage *img, double angle, cv::Point2f center)
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

/**
 * smAPIの顔姿勢データを基に、カメラ画像から顔領域を抽出する
 */
IplImage* ImageUtils::clipHeadImage(IplImage* src, HeadPose pose)
{
	IplImage *result;
	IplImage *large;

	result = cvCreateImage(cvSize(128,128), IPL_DEPTH_8U, 3);
	cv::Size src_size(cvGetSize(src));
	large = cvCreateImage(cvSize(2*src_size.width,2*src_size.height),IPL_DEPTH_8U, 3);
	cv::Size large_size(cvGetSize(large));
	cvSetZero(result);
	cvSetZero(large);

	//顔が検出されていない場合は何も返さない
	if (!pose.isValueSet())
	{
		cvReleaseImage(&large);
		return result;
	}

	int center_row,center_col,start_col,end_col,start_row,end_row;
	cv::Point2f center;

	center_row = large_size.height/2 + (25 - pose.y*(src_size.height/0.4)*cos(pose.rot))*0.4/pose.z;
	center_col = large_size.width/2 + (((pose.x*src_size.width+4)/0.4)*cos(pose.rot) + pose.y*(src_size.height/0.3)*sin(pose.rot))*0.4/pose.z;
	center.x=center_col;
	center.y=center_row;
	
	//cout << src_size.height << endl;


	cv::Size sz;
	sz.width = 2*floor(src_size.height/(pose.z*3));
	//cout << src_size.width << endl;
	sz.height = sz.width;

	
	start_col = center_col - sz.width/2;
	end_col = center_col + sz.width/2;
	start_row = center_row - sz.height/2;
	end_row = center_row + sz.height/2;
	
	//rot = cv::getRotationMatrix2D(center,rad,1);
	//mat.create(src_size.height,src_size.width,CV_16SC3);
	cvFlip(src, src, CV_16SC3);

	cv::Mat dst(large);
	cv::Rect rect(large_size.width/2-src_size.width/2,large_size.height/2-src_size.height/2,src_size.width,src_size.height);
	cv::Mat subdst = dst(rect);	
	cv::Mat in(src);
	in.copyTo(subdst);

    *large = dst;
	ImageUtils::rotateImage(large,-pose.rot,center);
	//result = large;
	//return result;
	
	cv::Mat dst1(large);
	cv::Mat dst2(sz,CV_8U);	
	
	if(start_row > 0 && end_row < large_size.height-20 && start_col > 10 && end_col < large_size.width-30){
		dst1 = dst1.rowRange(large_size.height/2-sz.height/2,large_size.height/2+sz.height/2);
		dst1 = dst1.colRange(large_size.width/2-sz.width/2,large_size.width/2+sz.width/2);
		//cv::getRectSubPix(mat,src_size,center,dst);			
		//cv::imshow("Capture", dst);

		double r=0;
		dst1.copyTo(dst2);
		IplImage dimg = dst2;
		cvResize(&dimg, result, CV_INTER_LINEAR);

		cvReleaseImage(&large);
		return result;
	}
	else
	{
		cvReleaseImage(&large);
		return result;
	}


}

bool ImageUtils::drawAlphaImage(IplImage* bgImg, IplImage* rgbImg, IplImage* alphaImg, int x, int y, int width, int height){
	if(x+width <= 0 || y+height <= 0 || x >= bgImg->width || y >= bgImg->height 
		/*||width >= (bgImg->width-x) || height >= (bgImg->height-y)*/)return false;
	int tmpwidth = x>=0? (width<(bgImg->width-x)? width:(bgImg->width-x)):(width+x);
	int tmpheight= y>=0? (height<(bgImg->height-y)? height:(bgImg->height-y)):(height+y);
	IplImage* tmp = cvCreateImage(cvSize(tmpwidth,tmpheight), IPL_DEPTH_8U, 3);
	IplImage* rgbtmp = cvCreateImage(cvSize(tmpwidth,tmpheight), IPL_DEPTH_8U, 3);
	IplImage* alphatmp = cvCreateImage(cvSize(tmpwidth,tmpheight), IPL_DEPTH_8U, 3);
	IplImage* resize =cvCreateImage(cvSize(width,height), IPL_DEPTH_8U, 3);
	cvSetImageROI(bgImg, cvRect(x>=0? x:0,y>=0? y:0,tmpwidth,tmpheight));
	cvCopy(bgImg,tmp);
	cvResize(rgbImg,resize,CV_INTER_LINEAR);
	cvSetImageROI(resize, cvRect(x>=0? 0:(-x),y>=0? 0:(-y),tmpwidth,tmpheight));
	cvCopy(resize,rgbtmp);
	cvResetImageROI(resize);
	cvResize(alphaImg,resize,CV_INTER_LINEAR);
	cvSetImageROI(resize, cvRect(x>=0? 0:(-x),y>=0? 0:(-y),tmpwidth,tmpheight));
	cvCopy(resize,alphatmp);
	cvResetImageROI(resize);
	for(int dy=0 ; dy<tmpheight ; dy++){
		for(int dx=0 ; dx<tmpwidth ; dx++){
			for(int ch=0 ; ch<3 ; ch++){
				CV_IMAGE_ELEM(tmp,unsigned char,dy,dx*3+ch)=
					(char)(CV_IMAGE_ELEM(tmp,unsigned char,dy,dx*3+ch)*((255-CV_IMAGE_ELEM(alphatmp,unsigned char,dy,dx*3))/255.0f)
					+CV_IMAGE_ELEM(rgbtmp,unsigned char,dy,dx*3+ch)*(CV_IMAGE_ELEM(alphatmp,unsigned char,dy,dx*3)/255.0f));
			}
		}
	}
	cvCopy(tmp,bgImg);
	cvReleaseImage(&tmp);
	cvReleaseImage(&rgbtmp);
	cvReleaseImage(&alphatmp);
	cvReleaseImage(&resize);
	cvResetImageROI(bgImg);
	return true;
}

void ImageUtils::adjustSaturation(IplImage* src, IplImage* dst, unsigned char sparam)
{
	typedef unsigned char byte;
	IplImage *channels[4] = {0, 0, 0, 0};
	for (int i = 0; i < 3; i++)
	{
		channels[i] = cvCreateImage(cvGetSize(src), src->depth, 1);
	}
	cvSplit(src, channels[0], channels[1], channels[2], 0);

	for (int i = 0; i < src->height; i++)
	{
		for (int j = 0; j < src->width; j++)
		{
			int idxs[3] = {0, 1, 2};
			for (int k = 0; k < 3; k++)
			{
				for (int l = 0; l < 2; l++)
				{
					byte r1 = CV_IMAGE_ELEM(channels[idxs[l]], byte, i, j);
					byte r2 = CV_IMAGE_ELEM(channels[idxs[l+1]], byte, i, j);
					if (r2 > r1)
					{
						int tmp = idxs[l];
						idxs[l] = idxs[l+1];
						idxs[l+1] = tmp;
					}
				}
				byte max = CV_IMAGE_ELEM(channels[idxs[0]], byte, i, j);
				byte min = CV_IMAGE_ELEM(channels[idxs[2]], byte, i, j);

				CV_IMAGE_ELEM(channels[idxs[0]], byte, i, j) = (byte)std::min(255, max + sparam);
				CV_IMAGE_ELEM(channels[idxs[2]], byte, i, j) = min;
			}
		}
	}

	cvMerge(channels[0], channels[1], channels[2], 0, dst);

	for (int i = 0; i < 3; i++)
	{
		cvReleaseImage(&channels[i]);
	}
}

void ImageUtils::adjustBrightnessAndContrast(IplImage* src, IplImage* dst, int bparam, int cparam)
{
	IplImage *dstImg[ 4 ] = {0, 0, 0, 0};
	int  sch = src->nChannels;
	for (int idx = 0; idx < sch; idx++) {
		dstImg[ idx ] = cvCreateImage (cvSize (src->width, src->height), src->depth, 1);
	}

	// スプリット
	cvSplit(src, dstImg[0], dstImg[1], dstImg[2], dstImg[3]);
	uchar lut[256];
	CvMat* lut_mat = cvCreateMatHeader( 1, 256, CV_8UC1 );
	cvSetData( lut_mat, lut, 0 );

	if( cparam > 0 )
	{
		double delta = 127. * cparam / 100;
		double a = 255. / ( 255. -delta * 2 );
		double b = a * ( bparam -delta );
		for( int idx = 0; idx < 256; idx++ )
		{
			int v = cvRound( a * idx + b );
			if( v < 0 ) v = 0;
			if( v > 255 ) v = 255;
			lut[ idx ] = (uchar)v;
		}
	}
	else
	{
		double delta = -128. * cparam / 100;
		double a = ( 256. -delta * 2 ) / 255.;
		double b = a * bparam + delta;
		for( int idx = 0; idx < 256; idx++ )
		{
			int v = cvRound( a * idx + b );
			if( v < 0 ) v = 0;
			if( v > 255 ) v = 255;
			lut[ idx ] = (uchar)v;
		}
	}
	for (int idx = 0; idx < sch; idx++) {
		cvLUT( dstImg[ idx ], dstImg[ idx ], lut_mat );
	}

	// マージ
	cvMerge( dstImg[0], dstImg[1], dstImg[2], dstImg[3], dst );
	for( int idx = 0; idx < sch; idx++ ) cvReleaseImage( &dstImg[ idx ] );
}


