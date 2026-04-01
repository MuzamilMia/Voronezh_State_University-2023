//#include<iostream>
//#include<istream>
//#include<fstream>
//#include<string>
//using namespace std;
//const int m=3, n=3, k=3;
//
//void InputFromKeyboard(char array[n][m][k])
//{
//	for (int i = 0; i < n; ++i)
//	{
//		for (int j = 0; j < m; ++j)
//		{
//			for (int r = 0; r < k; ++r)
//			{
//				std::cout << "[" << i << "]" << "[" << j << "]" << "[" << r << "]: ";
//				std::cin >> array[i][j][r];
//			}
//		}
//	}
//	std::cout << "---------------------------------------\n";
//	//calling to the print fucntion;
//}
//
//void InputFromFile(char array[n][m][k], const std::string& filename)
//{
//	std::ifstream Myfile(filename);
//	if (!Myfile)
//	{
//		std::cout << "File is not Exist!! \n";
//		return;
//	}
//	for (int i = 0; i < n; ++i)
//	{
//		for (int j = 0; j < m; ++j)
//		{
//			for (int r = 0; r < k; ++r)
//			{
//				Myfile >> array[i][j][r];
//			}
//		}
//	}
//	std::cout << "-------3";
//	//printFunction Call. 
//}
//
////void WriteToFile(char array[n][m][k], const std::string& filename)
////{
////	std::ofstream Myfile(filename);
////	if (!Myfile)
////	{
////		std::cout << "Error";
////		return;
////	}
////	for (int i = 0; i < n; ++i)
////	{
////		for (int j = 0; j < m; ++j)
////		{
////			for (int r = 0; r < k; ++r)
////			{
////				Myfile << array[i][j][r];
////			}
////		}
////	}
////	Myfile.close();
////	std::cout << "completed..."; 
////	
////}
//void writeArrayToFile(char arr[m][n][k], const string& filename) {
//	ofstream outFile(filename);
//	if (!outFile) {
//		cout << "Ошибка открытия файла!" << endl;
//		return;
//	}
//	for (int i = 0; i < m; i++) {
//		for (int j = 0; j < n; j++) {
//			for (int l = 0; l < k; l++) {
//				outFile << arr[i][j][l] << ' ';
//			}
//			outFile << endl;
//		}
//		outFile << endl;
//	}
//	outFile.close();
//	cout << "Массив успешно записан в файл." << endl;
//}
//
//void FindingTheSymbol(char array[n][m][k], char symbol)
//{
//	for (int i = 0; i < n; ++i)
//	{
//		for (int j = 0; j < m; ++j)
//		{
//			for (int r = 0; r < k; ++r)
//			{
//				if (symbol == array[i][j][k])
//					std::cout << array[i][j][k] << "\n";
//				else
//					std::cout << "No Symbol!!!";
//			}
//		}
//	}
//}
//
//void Print(char array[n][m][k])
//{
//	for (int i = 0; i < n; ++i)
//	{
//		for (int j = 0; j < m; ++j)
//		{
//			for (int r = 0; r < k; ++r)
//				std::cout << array[i][j][r];
//			std::cout << "\n";
//		}
//		std::cout << "\n";
//	}
//}
//
//void menu(char array[n][m][k])
//{
//	int choice;
//	std::string filename;
//	char symbol;
//
//	
//	do
//	{	
//		std::cout << "1. Input from Keyboard \n";
//		std::cout << "2. Input from file \n";
//		std::cout << "3. Write to File \n";
//		std::cout << "4. Finding the Symbol \n";
//		std::cout << "5. Print \n";
//		std::cout << "6. Exit \n";
//		std::cout << "Choose the option: ";
//		std::cin >> choice;
//		switch (choice)
//		{
//		case 1: 
//			InputFromKeyboard(array);
//			break;
//		case 2:
//			std::cout << "Enter the file name: ";
//			std::cin >> filename;
//			InputFromFile(array, filename);
//			break;
//		case 3:
//			std::cout << "Enter the File name: ";
//			std::cin >> filename;
//		//	WriteToFile(array, filename);
//			writeArrayToFile(array, filename);
//			break;
//		case 4:
//			std::cout << "Enter the Symbol to find: ";
//			std::cin >> symbol;
//			FindingTheSymbol(array, symbol);
//			break;
//		case 5:
//			Print(array);
//			break;
//		case 6:
//			std::cout << "Exited the program. ";
//			break;
//		default:
//			std::cout << "Try again!!! \n";
//		}
//
//		std::cout << '\n';
//
//	} while (choice != 6);
//}
//
//
//
//int main()
//{
//	char array[m][n][k];
//	menu(array);
//
//	return 0;
//}

/*
	Name: Muzamil Mia.

	Требование к программам:
	− Язык программирования — С++, в чистом виде, без расширений типа .net.
	− Программа – консольное приложение с меню, задача не завершается после одной
		итерации (а только при выборе пункта меню «выход»).
	− Ввод-вывод во всех вариантах с клавиатуры и из файла.
	− В коде обязательно условие задачи и автор (в комментариях).
	− Задача должна быть не просто разбита на функции, а поделена осмысленно.
	− Не использовать в данной задаче: var, лямбда-функции, контейнеры вроде
		std::vector, std::list

	Задача 23: Дан трехмерный массив символов mxnxk.
	Для введенного пользователем символа вывести все три его координаты (всех вхождений).
	Также добавить возможность вывода трехмерного массива на консоль.
*/

#include <iostream>
#include <fstream>

const int m = 3;
const int n = 3; 
const int k = 3; 

void inputArray(std::string arr[m][n][k]) {
	std::cout << "Enter the symbols to 3D Array (size " << m << "x" << n << "x" << k << "):" << std::endl;
	for (int i = 0; i < m; i++) {
		for (int j = 0; j < n; j++) {
			for (int l = 0; l < k; l++) {
				std::cin >> arr[i][j][l];
			}
		}
	}
}


void displayArray(std::string arr[m][n][k]) {
	std::cout << "3D Array:" << std::endl;
	for (int i = 0; i < m; i++) {
		for (int j = 0; j < n; j++) {
			for (int l = 0; l < k; l++) {
				std::cout << arr[i][j][l] << ' ';
			}
			std::cout << std::endl;
		}
		std::cout << std::endl;
	}
}


void findSymbol(std::string arr[m][n][k], std::string symbol) {
	bool found = false;
	std::cout << "Coordinates of the Symbol '" << symbol << "' in Array:" << std::endl;
	for (int i = 0; i < m; i++) {
		for (int j = 0; j < n; j++) {
			for (int l = 0; l < k; l++) {
				if (arr[i][j][l] == symbol) {
					std::cout << "Found in position: [" << i << "][" << j << "][" << l << "]" << std::endl;
					found = true;
				}
			}
		}
	}
	if (!found) {
		std::cout << "Symbol not found." << std::endl;
	}
}


void readArrayFromFile(std::string arr[m][n][k],  std::string& filename) {
	std::ifstream inFile(filename);
	if (!inFile ) {
		std::cout << "File is not Exist or Open!!!" << std::endl;
		return;
	}
	else
	{
		for (int i = 0; i < m; i++) {
			for (int j = 0; j < n; j++) {
				for (int l = 0; l < k; l++) {
					inFile >> arr[i][j][l];
				}
			}
		}
		inFile.close();
		std::cout << "Array is written to the file." << std::endl;
	}
}


void writeArrayToFile(std::string arr[m][n][k], const std::string& filename) {
	std::ofstream outFile(filename);
	if (!outFile) {
		std::cout << "File is not Exist or Open!!!" << std::endl;
		return;
	}
	else {

		for (int i = 0; i < m; i++) {
			for (int j = 0; j < n; j++) {
				for (int l = 0; l < k; l++) {
					outFile << arr[i][j][l] << ' ';
				}
				outFile << std::endl;
			}
			outFile << std::endl;
		}
	}
	outFile.close();
	std::cout << "Array is written to the file." << std::endl;
}


void menu(std::string arr[m][n][k]) {
	int choice;
	std::string symbol;
	std::string filename;

	do {
		std::cout << "\nMenu:" << std::endl;
		std::cout << "1. Input of the Array from Keyboard" << std::endl;
		std::cout << "2. Print the Array to the screen" << std::endl;
		std::cout << "3. Find the Symbol in Array" << std::endl;
		std::cout << "4. Read Array from File" << std::endl;
		std::cout << "5. Write Array to File" << std::endl;
		std::cout << "6. Exit" << std::endl;
		std::cout << "Choose the option: ";
		std::cin >> choice;

		switch (choice) {
		case 1:
			inputArray(arr);
			break;
		case 2:
			displayArray(arr);
			break;
		case 3:
			std::cout << "Write the Symbol: ";
			std::cin >> symbol;
			findSymbol(arr, symbol);
			break;
		case 4:
			std::cout << "Enter the file name for reading: ";
			std::cin >> filename;
			readArrayFromFile(arr, filename);
			break;
		case 5:
			std::cout << "Enter file name for inputing: ";
			std::cin >> filename;
			writeArrayToFile(arr, filename);
			break;
		case 6:
			std::cout << "Exite program." << std::endl;
			break;
		default:
			std::cout << "Not correct: Try Again!!!." << std::endl;
			std::cin.clear();
			std::cin.ignore(255, '\n');
		}
	} while (choice != 6);
}

int main() {
	std::cout << "Input File name is: input\n";
	std::cout << "output File name is: output\n";
	std::cout << "---------------------------------------";
	std::string arr[m][n][k]; 
	menu(arr);
	std::cin.clear();
	std::cin.ignore(255, '\n');

	return 0;
}