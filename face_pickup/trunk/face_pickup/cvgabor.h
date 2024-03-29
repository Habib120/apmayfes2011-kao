#ifndef CVGABOR_H
#define CVGABOR_H

#include <iostream>
#include <cv.h>
#include <highgui.h>

#define PI 3.14159265
#define CV_GABOR_REAL 1
#define CV_GABOR_IMAG 2
#define CV_GABOR_MAG  3
#define CV_GABOR_PHASE 4

class CvGabor{

public:
	CvGabor();
	~CvGabor();
	
	CvGabor(int iMu, int iNu);

	bool IsInit();
	long mask_width();
	void Init(int iMu, int iNu, double dSigma, double dF);

	void conv_img(IplImage *src, IplImage *dst);
	cv::Mat getKernelImage(int type);
	void get_value(IplImage *src, CvMat *omat);
	void get_mat(cv::Mat src, cv::Mat mat);
	cv::Mat convertToByte(cv::Mat mat, double scale = 1);

protected:
    double Sigma;
    double F;
    double Kmax;   
    double K;
    double Phi;
    bool bInitialised;
    bool bKernel;
    long Width;
    CvMat *Imag;
    CvMat *Real;

private:
    void creat_kernel();
};

#endif