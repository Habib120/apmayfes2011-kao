// Hoi.cpp : �R���\�[�� �A�v���P�[�V�����̃G���g�� �|�C���g���`���܂��B
//

#include <stdio.h>
#include <stdlib.h>
#include <time.h>


#define GU		0
#define CHOKI	1
#define PA		2
#define END		3

#define USER	0
#define COM		1
#define AIKO	2

#define RIGHT	0
#define UP		1	
#define LEFT	2
#define DOWN	3



class User
{
public:
	int GetHand();
	int GetDirect();
};

int User::Gethand()
{
	int ans;

	printf("Select your hand.\n");
	printf("0:�O�[, 1:�`���L, 2:�p�[, 3:�I��� = ");
	scanf("%d", &ans);
/*	printf("Your hand : ");
	switch(ans)
	{
	case 0 : puts("�O�[");		break;
	case 1 : puts("�`���L");	break;
	case 2 : puts("�p�[");		break;
	}
*/
	return ans;
}

int User::GetDirect(){
	int ans;

	printf("Select your Direction.\n");
	printf("0:��, 1:��, 2:��, 3:�� = ");
	scanf("%d", &ans);

	return ans;
}


class Com
{
public:
	int GetHand();
	int GetDirect();
};

int Com::GetHand()
{
	int ans;

	srand(time(NULL));
	ans = rand() % 3;
	printf("Computer = ");
	switch(ans)
	{
	case 0 : puts("�O�[");		break;
	case 1 : puts("�`���L");	break;
	case 2 : puts("�p�[");		break;
	}
	return ans;
}

int Com::GetDirect()
{
	int ans;

	srand(time(NULL));
	ans = rand() % 4;
	printf("Computer put ");
	switch(ans)
	{
	case 0 : puts("�u���v");	break;
	case 1 : puts("�u���v");	break;
	case 2 : puts("�u���v");	break;
	case 3 : puts("�u���v");	break;
	}

	return ans;
}


class Judge
{
public:
	int GetJanWinner(int u, int c);
	void JanToHoi(int w);
	int GetHoiWinner(int u, int c, int winner);
	void ShowWinner(int w);
};

int Judge::GetJanWinner(int u, int c)
{
	int ans;

	if (u == c)
		ans = AIKO;
	else if ((u-c+3)%3 == 2)
		ans = USER;
	else
		ans = COM;
	
	return ans;
}

void Judge::JanToHoi(int w)
{
	switch(w)
	{
	case USER :	printf("Your turn! �����������āE�E�E\n");	break;
	case COM :	printf("Com's turn! �����������āE�E�E\n");	break;
	case AIKO : printf("�������ł��B������x�I\n");			break;
	}
}

int Judge::GetHoiWinner(int u, int c, int winner)
{
	int rpt;

	if (u == c){
		if( winner == USER)
			puts("You Win!");
		else
			puts("You Lose.");
		//printf("Next Game? 0: ,1: "); scanf();
		rpt = 0;
	}
	else
		rpt = 1;
	
	return rpt;
}


int main()
{
	int uj, cj, winner;
	int uh, ch, repeat;
	User user;
	Computer com;
	Judge judge;

	do{
		do{
			uj = user.GetHand();
			if(u == END)
				return 0;

			cj = com.GetHand();

			winner = judge.GetJanWinner(uj, cj);

			judge.JanToHoi(winner);

		}while(winner == AIKO);

		uh = user.GetDirect();

		ch = com.GetDirect();

		repeat = judge.GetHoiWinner(uh,ch,winner);

	}while(repeat);

	return 0;
}

