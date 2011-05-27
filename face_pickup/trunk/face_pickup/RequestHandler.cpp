#include "network.h"
#include "connection.h"
#include "photo.h"
#include "cv.h"
#include <boost/format.hpp>
#include <ctime>

#define MOULIN_PHOTO_SAVE_DIR "C:\\photo\\"
#define MOULIN_PHOTO_LATEST_DIR "C:\\photo\\latest\\"

int latestcount = 0;

void RequestHandler::Handle(std::string msg)
{
	std::vector<std::string> results;
	std::string::size_type index = msg.find("||");
	std::string header, body;
	if (index == std::string::npos)
	{
		header = msg;
	}
	else
	{
		header = msg.substr(0, index);
		body = msg.substr(index + 2);
	}
	std::cout << "header : " << header << " body : " << body << std::endl;

	if (header == "take_photo")
	{
		IplImage* camimage = this->tracker->GetCurrentCamImage();
		FaceComDetector detector;
		std::vector<FaceComDetectionResult> results = detector.Detect(camimage);
		IplImage* photo = MoulinPhotoMaker::GetMoulinPhoto(camimage, results);

		//äÁâÊëúÉfÅ[É^ÇÃï€ë∂
		time_t t = time(0);
		tm *x = localtime(&t);
		std::string dirname(MOULIN_PHOTO_SAVE_DIR);
		std::string dirname_tmp(MOULIN_PHOTO_LATEST_DIR);
		std::stringstream stream;
		stream << "photo_" << x->tm_year << x->tm_mon << x->tm_mday << "_" << x->tm_hour << x->tm_min << "_" << x->tm_sec << "_" << rand() << ".jpg";
		std::string filename = stream.str();
		filename = dirname + filename;
		cvSaveImage(filename.c_str(), photo);

		int latestnum = latestcount % 5;
		std::stringstream stream_tmp;
		stream_tmp << "photo_latest_" << latestnum << ".jpg";
		latestcount++;
		std::string filename_tmp = stream_tmp.str();
		filename_tmp = dirname_tmp + filename_tmp;
		cvSaveImage(filename_tmp.c_str(), photo);

		cvReleaseImage(&camimage);
		cvReleaseImage(&photo);
	}

	else if (header == "reset_game")
	{
		this->ploop->ResetState();
	}
}