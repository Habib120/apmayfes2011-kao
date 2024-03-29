#include "cvgabor.h"
#include <iostream>
using namespace std;

CvGabor::CvGabor()    //constructor
{
}

CvGabor::~CvGabor()   //Deconstructor
{
	cvReleaseMat( &Real );
	cvReleaseMat( &Imag );
}

CvGabor::CvGabor(int iMu, int iNu)
{
	double dSigma = sqrt(2.0 * PI); 
	F = sqrt(2.0);
	Init(iMu, iNu, dSigma, F);
}

//Determine whether the gabor has been initlized - variables F, K, Kmax, Phi, Sigma are filled.
bool CvGabor::IsInit()
{
    return bInitialised;
}

//Return the width of mask (should be NxN) by the value of Sigma and iNu.
long CvGabor::mask_width()
{
    long lWidth;
    if (IsInit() == false)  {
       perror ("Error: The Object has not been initilised in mask_width()!\n");
       return 0;
    }
    else {
       //determine the width of Mask
      double dModSigma = Sigma/K;
      double dWidth = (int)(dModSigma*6 + 1);
      //test whether dWidth is an odd.
      if (fmod(dWidth, 2.0)==0.0) dWidth++;
      lWidth = (long)dWidth;

      return lWidth;
    }
}


//Create 2 gabor kernels - REAL and IMAG, with an orientation and a scale 
void CvGabor::creat_kernel()
{
    if (IsInit() == false) {perror("Error: The Object has not been initilised in creat_kernel()!\n");}
    else {
      CvMat *mReal, *mImag;
      mReal = cvCreateMat( Width, Width, CV_32FC1);
      mImag = cvCreateMat( Width, Width, CV_32FC1);
      
      /**************************** Gabor Function ****************************/ 
      int x, y;
      double dReal;
      double dImag;
      double dTemp1, dTemp2, dTemp3;

	  //cout << "Phi:" << Phi << " K:" << K << " Sigma:" << Sigma << endl;

      for (int i = 0; i < Width; i++)
      {
          for (int j = 0; j < Width; j++)
          {
              x = i-(Width-1)/2;
              y = j-(Width-1)/2;
              dTemp1 = (pow(K,2)/pow(Sigma,2))*exp(-(pow((double)x,2)+pow((double)y,2))*pow(K,2)/(2*pow(Sigma,2)));
              dTemp2 = cos(K*cos(Phi)*x + K*sin(Phi)*y) - exp(-(pow(Sigma,2)/2));
              dTemp3 = sin(K*cos(Phi)*x + K*sin(Phi)*y);
              dReal = dTemp1*dTemp2;
              dImag = dTemp1*dTemp3;
              //gan_mat_set_el(pmReal, i, j, dReal);
	      //cvmSet( (CvMat*)mReal, i, j, dReal );
		cvSetReal2D((CvMat*)mReal, i, j, dReal );
              //gan_mat_set_el(pmImag, i, j, dImag);
              //cvmSet( (CvMat*)mImag, i, j, dImag );
		cvSetReal2D((CvMat*)mImag, i, j, dImag );

          } 
       }//FOR
       /**************************** Gabor Function ****************************/
       bKernel = true;
       cvCopy(mReal, Real, NULL);
       cvCopy(mImag, Imag, NULL);
      //printf("A %d x %d Gabor kernel with %f PI in arc is created.\n", Width, Width, Phi/PI);
       cvReleaseMat( &mReal );
       cvReleaseMat( &mImag );
     }
}

//Initilize the.gabor with the orientation iMu, the scale iNu, the sigma dSigma, the frequency dF, it will call the function creat_kernel(); So a gabor is created.
void CvGabor::Init(int iMu, int iNu, double dSigma, double dF)
{
  //Initilise the parameters 
    bInitialised = false;
    bKernel = false;

    Sigma = dSigma;
    F = dF;
    
    Kmax = PI/2;
    
    // Absolute value of K
    K = Kmax / pow(F, (double)iNu);
    Phi = PI*iMu/8;
    bInitialised = true;
    Width = mask_width();
    Real = cvCreateMat( Width, Width, CV_32FC1);
    Imag = cvCreateMat( Width, Width, CV_32FC1);
    creat_kernel();  
}

void CvGabor::conv_img(IplImage *src, IplImage *dst)
{
  double ve;
  
   CvMat *mat = cvCreateMat(src->width, src->height, CV_32FC1);
  for (int i = 0; i < src->width; i++)
  {
    for (int j = 0; j < src->height; j++)
    {
      ve = CV_IMAGE_ELEM(src, uchar, j, i);
      CV_MAT_ELEM(*mat, float, i, j) = (float)ve;
    }
  }
  
  CvMat *rmat = cvCreateMat(src->width, src->height, CV_32FC1);
  CvMat *imat = cvCreateMat(src->width, src->height, CV_32FC1);
  
  cvFilter2D( (CvMat*)mat, (CvMat*)rmat, (CvMat*)Real, cvPoint( (Width-1)/2, (Width-1)/2));
  cvFilter2D( (CvMat*)mat, (CvMat*)imat, (CvMat*)Imag, cvPoint( (Width-1)/2, (Width-1)/2));

  cvPow(rmat,rmat,2); 
  cvPow(imat,imat,2);
  cvAdd(imat,rmat,mat); 
  cvPow(mat,mat,0.5); 
  
  if (dst->depth == IPL_DEPTH_8U)
  {
    cvNormalize((CvMat*)mat, (CvMat*)mat, 0, 255, CV_MINMAX);
    for (int i = 0; i < mat->rows; i++)
    {
      for (int j = 0; j < mat->cols; j++)
      {
        ve = CV_MAT_ELEM(*mat, float, i, j);
        CV_IMAGE_ELEM(dst, uchar, j, i) = (uchar)cvRound(ve);
      }
    }
  }
  
  if (dst->depth == IPL_DEPTH_32F)
  {
    for (int i = 0; i < mat->rows; i++)
    {
      for (int j = 0; j < mat->cols; j++)
      {
        ve = cvGetReal2D((CvMat*)mat, i, j);
        cvSetReal2D( (IplImage*)dst, j, i, ve );
      }
    }
  }
  cvReleaseMat(&imat);
  cvReleaseMat(&rmat);
  cvReleaseMat(&mat);
}

void CvGabor::get_value(IplImage *src, CvMat *omat)
{
  double ve;
  
  CvMat *mat = cvCreateMat(src->width, src->height, CV_32FC1);

  for (int i = 0; i < src->height; i++)
  {
	for (int j = 0; j < src->width; j++)
	{
      ve = CV_IMAGE_ELEM(src, uchar, i, j);
      CV_MAT_ELEM(*mat, float, i, j) = (float)ve;
	}
  }

  CvMat *rmat = cvCreateMat(src->width, src->height, CV_32FC1);
  CvMat *imat = cvCreateMat(src->width, src->height, CV_32FC1);
  
  cvFilter2D( (CvMat*)mat, (CvMat*)rmat, (CvMat*)Real, cvPoint( (Width-1)/2, (Width-1)/2));
  cvFilter2D( (CvMat*)mat, (CvMat*)imat, (CvMat*)Imag, cvPoint( (Width-1)/2, (Width-1)/2));

  cvPow(rmat,rmat,2);
  cvPow(imat,imat,2);
  cvAdd(imat,rmat,mat); 
  cvPow(mat,mat,0.5); 
/*
  cvNormalize((CvMat*)mat, (CvMat*)mat, 0, 255, CV_MINMAX);
  IplImage *dst = cvCreateImage(cvSize(128,128), IPL_DEPTH_8U, 1);
  for (int i = 0; i < mat->rows; i++)
  {
      for (int j = 0; j < mat->cols; j++)
      {
  		ve = CV_MAT_ELEM(*mat, float, i, j);
  		CV_IMAGE_ELEM(dst, uchar, j, i) = (uchar)cvRound(ve);
  	 }
  }

  cvNamedWindow("dst");
  cvShowImage("dst", dst);
  cvWaitKey();
  cvDestroyWindow("dst");
  */
  cvCopy(mat, omat);
  
  cvReleaseMat(&imat);
  cvReleaseMat(&rmat);
  cvReleaseMat(&mat);
}

cv::Mat CvGabor::convertToByte(cv::Mat mat, double scale)
{
	cv::Mat tmp = mat.clone();
	cv::normalize(mat, tmp, 255, 0, CV_MINMAX);
	cv::Mat byte_img(mat.rows, mat.cols, CV_8UC1);
	cv::Mat disp_img((int)(mat.rows * scale), (int)(mat.cols * scale), CV_8UC1);
	
	tmp.convertTo(byte_img, CV_8UC1, 1, 0);
	cv::resize(byte_img, disp_img, disp_img.size());
	return disp_img;
}

cv::Mat CvGabor::getKernelImage(int type)
{
	cv::Mat ret(Real->width, Real->height, Real->type);
	cv::Mat mReal(Real);
	cv::Mat mImag(Imag);
	cv::Mat dReal = mReal.clone();
	cv::Mat dImag = mImag.clone();
	switch (type)
	{
		case CV_GABOR_IMAG :
			return convertToByte(dImag);
		case CV_GABOR_REAL :
			return convertToByte(dReal);
		default :
			cv::pow(dReal, 2, dReal);
			cv::pow(dImag, 2, dImag);
			cv::add(dReal, dImag, ret);
			
			return convertToByte(ret);
	}
}

void CvGabor::get_mat(cv::Mat src, cv::Mat mat)
{ 
	mat = src.clone();
	cv::Mat mReal(Real);
	cv::Mat mImag(Imag);

	IplImage iplsrc = src;
	IplImage iplrmat = cv::Mat(src.size(), CV_32FC1);
	IplImage iplimat = cv::Mat(src.size(), CV_32FC1);
	cout << "src_channels:" << src.depth() << "dst_channels" << iplrmat.depth << endl;
	cvFilter2D(&iplsrc, &iplrmat, Real, cv::Point((Width-1)/2, (Width-1)/2));
    cvFilter2D(&iplsrc, &iplimat, Imag, cv::Point((Width-1)/2, (Width-1)/2));

	cv::Mat imat(&iplrmat);
	cv::Mat rmat(&iplimat);
	
	cv::pow(rmat, 2, rmat);
	cv::pow(imat, 2, imat);
	cv::add(imat, rmat, mat);
	cv::pow(mat, 0.5, mat);
}