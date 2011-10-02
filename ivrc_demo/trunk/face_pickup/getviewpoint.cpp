//---------------------------------------------------------------
//author: kudo
//�摜���Q�l�����C�֊s���擾
//�֊s�̒��S���}�[�J�[�_������
//�}�[�J�[�_�̏d�S�Ƃ��Ď��_���W���o��

//2011.09.23 ������C#�R�[�h���ڐA�D�e�֊s�̏d�S���o��
//09.24 �[�Ɍ����m�C�Y�֊s�������C�S�̂̏d�S��_���o��
//09.28 �f�o�b�O�֘A��ǉ��C���\�b�h��xy��z�ɕ���
//09.29 �]�v�ȓ_������
//10.01 ��������_��xy: �E�[, z: ���[�ɐݒ�
//---------------------------------------------------------------

#include "detection_thread.h"

//#define DEBUG

#define BLOCK 55
#define MARK_THRESH -100

//using namespace cv;
using namespace std;

vector<cv::Point2d> getviewpoint(cv::Mat gray_img){

	// �K���I��臒l����
	cv::Mat cont_img;
	// �����i���͉摜�C�o�͉摜�CmaxVal�C臒l�����@�C臒l������@�CblockSize�CConst�j
	cv::adaptiveThreshold(gray_img, cont_img, 255, cv::ADAPTIVE_THRESH_MEAN_C, cv::THRESH_BINARY, BLOCK, MARK_THRESH);
	//cv::threshold(gray_img, adaptive_img, 0, 255, cv::THRESH_BINARY|cv::THRESH_OTSU);
	//cv::threshold(gray_img, cont_img, 200, 255, cv::THRESH_BINARY);
	vector<vector<cv::Point>> contours;
	// �֊s�̌��o(EXTERNAL or LIST)
	cv::findContours(cont_img, contours, CV_RETR_LIST, CV_CHAIN_APPROX_NONE);

#ifdef DEBUG
	//��l�摜�̏o��
	cv::drawContours(cont_img, contours, -1, cv::Scalar(100), 2, 8);
	cv::namedWindow("bin image", CV_WINDOW_AUTOSIZE|CV_WINDOW_FREERATIO);
	cv::imshow("bin image", cont_img);
#endif 

	//�}�[�J�[�_��averages�Ɋi�[����
	vector<cv::Point2d> averages;  
	cv::Point2d point;

	for(int i = 0; i < contours.size(); ++i) {
		size_t count = contours[i].size();
		
		//����������E�傫������֊s������
#ifdef DEBUG
		if(count < 50 || count > 300) continue;
#else
		if(count < 30 || count > 100) continue;  //9/25�ύXyada
#endif

		cv::Moments mom = cv::moments(contours[i]);
		point.x = mom.m10/mom.m00;
		point.y = mom.m01/mom.m00;

		//�[�Ɍ����]�v�ȗ֊s���͂����i10/01�ύX�j
		if( abs(point.x - gray_img.cols/2) > 4* gray_img.cols/9) continue; //���E1/18
		if( abs(point.y - gray_img.rows/2) > 2 * gray_img.rows/5) continue; //�㉺1/10
		if( (point.y - gray_img.rows/2) > 3 * gray_img.rows/10) continue; //����1/5

		//�}�[�J�[�_�ɒǉ�
		averages.push_back(point);

		//�ȉ~�`��
#ifdef DEBUG
		cv::Mat pointsf;
		cv::Mat(contours[i]).convertTo(pointsf, CV_32F);
		cv::RotatedRect box = cv::fitEllipse(pointsf);
		cv::ellipse(gray_img, box, cv::Scalar(0,0,255), 2, CV_AA);
#endif
	}
	//�ȉ~�t�B�b�g�摜�̏o��
#ifdef DEBUG
	cv::namedWindow("fit ellipse", CV_WINDOW_AUTOSIZE|CV_WINDOW_FREERATIO);
	cv::imshow("fit ellipse", gray_img);
#endif
	return averages;
}


//�����\�[�g�̏�����Ԃ��֐��ia.x >b.x�Ȃ�~���j
bool ascendingorder(cv::Point2d a, cv::Point2d b){
	if (a.x == b.x)
		return a.y < b.y;
	else
		return a.x < b.x;
}

//�~���\�[�g
bool descendingorder(cv::Point2d a, cv::Point2d b){
	if (a.x == b.x)
		return a.y < b.y;
	else
		return a.x > b.x;
}


cv::Point2d getpointxy(vector<cv::Point2d> averages){
	//x���W�����\�[�g
	std::sort (averages.begin(), averages.end(), ascendingorder);
	
	//�[�̂Q�_���߂�����ꍇ�C�w�b�h�z���㑤�̃}�[�J�[�Ƃ݂Ȃ��ď���(��䗦1/5)
	int n = averages.size();
	if( (averages[n-1].x - averages[0].x) > 5*(averages[n-1].x - averages[n-2].x) ){
		averages.pop_back();
	};
	
	//�d�S�����߂�
	cv::Point2d sum;// = accumulate( averages.begin(), averages.end(), 0 ); /// averages.size();
	for( int i = 0; i < averages.size(); ++i ){
		sum += averages[i];
#ifdef DEBUG
		cout << averages[i] << endl;
#endif
	}
	
	cv::Point2d viewpoint = sum*(1.0/averages.size());
	return viewpoint;
}

cv::Point2d getpointz(vector<cv::Point2d> averages){
	//���Α��̃}�[�J�[�����o���ꂽ�ꍇ�̓���
	if(averages.size() > 2){
		//x���W�~���\�[�g
		std::sort (averages.begin(), averages.end(), descendingorder);
		//��ԉE�[�ix���W���j�̓_������
		averages.pop_back();
	}

	//�d�S�����߂�
	cv::Point2d sum;
	for( int i = 0; i < averages.size(); ++i ){
		sum += averages[i];
#ifdef DEBUG
		cout << averages[i] << endl;
#endif
	}

	cv::Point2d viewpoint = sum*(1.0/averages.size());
	return viewpoint;
}


cv::Point2d getviewpointxy(cv::Mat gray_img){
	vector<cv::Point2d> averages = getviewpoint(gray_img);
	//9/27�ǉ� yada
	if(averages.size() < 2 ){
		return cv::Point2d(-1,-1);
	}
	cv::Point2d viewpoint = getpointxy(averages);
	return(viewpoint);
}

cv::Point2d getviewpointz(cv::Mat gray_img){
	vector<cv::Point2d> averages = getviewpoint(gray_img);

	//9/27�ǉ� yada
	if(averages.size() < 2 ){
		return cv::Point2d(-1,-1);
	}
	cv::Point2d viewpoint = getpointz(averages);
	return(viewpoint);
}
