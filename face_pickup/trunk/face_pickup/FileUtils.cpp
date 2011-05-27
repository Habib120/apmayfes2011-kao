#include <windows.h>
#include "util.h"
#include <iostream>

void FileUtils::DeleteAllTmpImages()
{
	HANDLE hFind;
	WIN32_FIND_DATAA fd;
	FILETIME ft;
	SYSTEMTIME st;

	/* 最初のファイル検索 */
	hFind = FindFirstFileA("c:\\tmp\\*.jpg", &fd);

	/* 検索失敗? */
	if(hFind == INVALID_HANDLE_VALUE) {
		printf("no tmp files are found.\n");
		return; 
	}

	do {
		if(fd.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) {
			continue;
		}
		/* 結果の表示 */
		std::cout << "removed : " << fd.cFileName << std::endl;
		std::string filepath = "c:\\tmp\\";
		filepath += fd.cFileName;
		DeleteFileA(filepath.c_str());

	} while (FindNextFileA(hFind, &fd)); 

	/* 検索終了 */
	FindClose(hFind);
}