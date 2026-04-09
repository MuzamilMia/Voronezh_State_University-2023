#include<iostream>
#include<Windows.h>
#include<process.h>

const size_t COUNT = 110;
const size_t NUM_THRADS = 4;
struct INFORM
{
	int* arr;
	size_t left{}, right{};
	int sum{};
};

int sum_nonparallel(int* arr)
{
	int result{};
	for (size_t i{}; i < COUNT; ++i)
		result += arr[i];
	return result;
}
unsigned int __stdcall sum(void* arg)
{
	
	INFORM* inform = (INFORM*)arg;
	inform->sum = 0;
	for (int i{ inform->left }; i < inform->right; ++i)
		inform->sum += inform->arr[i];
	std::cout << "ID from the WinAPI: " << ::GetCurrentThreadId() << ' ' << inform->sum << '\n';
	if(inform->right!=COUNT)

	_endthreadex(0);
	return 0;
}

int sum_parallel(int* arr)
{
	HANDLE thr[NUM_THRADS - 1];
	//unsigned theID[NUM_THRADS - 1];
	INFORM informs[NUM_THRADS];
	size_t chunk = COUNT / NUM_THRADS;
	for (size_t i{}; i < NUM_THRADS; ++i)
	{
		informs[i].arr = arr;
		informs[i].left = i * chunk;
		informs[i].sum = 0;
		if (i == NUM_THRADS - 1)
			informs[i].right = COUNT;
		else
			informs[i].right = (i + 1) * chunk;
		if (i < NUM_THRADS)
			thr[i] = (HANDLE)_beginthreadex(nullptr, 0, &sum, &informs[i], 0, nullptr);
	}
	sum((void*)(informs + NUM_THRADS - 1));
	//sum((void*)&informs [NUM_THRADS - 1]);

	WaitForMultipleObjects(NUM_THRADS - 1, thr, true, INFINITE);
	int global_sum{};
	for(size_t i{}; i < NUM_THRADS; ++i)
		global_sum += informs[i].sum;

	for(size_t i{}; i < NUM_THRADS - 1; ++i)
		CloseHandle(thr[i]);

	return global_sum;
}

void fill(int* arr)
{
	for (size_t i{}; i < COUNT; ++i)
		arr[i] = rand() % 100;
}

//DWORD WINAPI func_thread2(LPVOID t)
//{
//	std::cout << "Second thread \n";
//	for (int i = {}; i < 100; ++i)
//		std::cout << 2;
//	std::cout << '\n';
//	return 0;
//}
//unsigned long __stdcall func_thread3(void* t)
//{
//	std::cout << "thrid thread \n";
//	for (int i = {}; i < 100; ++i)
//		std::cout << 3;
//	std::cout << '\n';
//	return 0;
//}
int main()
{
	//HANDLE thread2 = CreateThread(NULL, 0, func_thread2, NULL, 0, NULL);
	////HANDLE thread3 = CreateThread(NULL, 0, func_thread3, NULL, 0, NULL);
	//std::cout << "First thread \n";
	//for (int i{}; i < 100; ++i)
	//	std::cout << 1;
	//std::cout << '\n';
	int arr[COUNT];
	//srand(GetTickCount64);
	fill(arr);
	std::cout << "Sum nonParallel: " << sum_nonparallel(arr) << '\n';
	std::cout << "Sum nonParallel: " << sum_parallel(arr) << '\n';
	std::cin.ignore();
	return 0;
}