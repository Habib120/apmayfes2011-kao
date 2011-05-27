#include <windows.h>
#include "util.h"
#include <iostream>

void FileUtils::DeleteAllTmpImages()
{
	HANDLE hFind;
	WIN32_FIND_DATAA fd;
	FILETIME ft;
	SYSTEMTIME st;

	/* �ŏ��̃t�@�C������ */
	hFind = FindFirstFileA("c:\\tmp\\*.jpg", &fd);

	/* �������s? */
	if(hFind == INVALID_HANDLE_VALUE) {
		printf("no tmp files are found.\n");
		return; 
	}

	do {
		if(fd.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) {
			continue;
		}
		/* ���ʂ̕\�� */
		std::cout << "removed : " << fd.cFileName << std::endl;
		std::string filepath = "c:\\tmp\\";
		filepath += fd.cFileName;
		DeleteFileA(filepath.c_str());

	} while (FindNextFileA(hFind, &fd)); 

	/* �����I�� */
	FindClose(hFind);
}