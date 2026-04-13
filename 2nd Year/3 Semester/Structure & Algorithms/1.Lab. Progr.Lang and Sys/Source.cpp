
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
using namespace std;

const int M = 3; 
const int N = 3;  
const int K = 3;  

void input_array(std::string arr[M][N][K]) {
	cout << "Insert the Symbols for 3D array(size" << M << "x" << N << "x" << K << "):" << endl;
	for (int i = 0; i < M; i++) {
		for (int j = 0; j < N; j++) {
			for (int l = 0; l < K; l++) {
				cin >> arr[i][j][l];
			}
		}
	}
}


void display_array(std::string arr[M][N][K]) {
	cout << "3D Array:" << endl;
	for (int i = 0; i < M; i++) {
		for (int j = 0; j < N; j++) {
			for (int l = 0; l < K; l++) {
				cout << arr[i][j][l] << ' ';
			}
			cout << endl;
		}
		cout << endl;
	}
}


void find_symbol(std::string arr[M][N][K], std::string symbol) {
	bool found = false;
	cout << "Coordinates of the symbol '" << symbol << "' in Array:" << endl;
	for (int i = 0; i < M; i++) {
		for (int j = 0; j < N; j++) {
			for (int l = 0; l < K; l++) {
				if (arr[i][j][l] == symbol) {
					cout << "Found in the position: [" << i << "][" << j << "][" << l << "]" << endl;
					found = true;
				}
			}
		}
	}
	if (!found) {
		cout << "Symbol not found!!!." << endl;
	}
}


void read_array_from_file(std::string arr[M][N][K], const string& filename) {
	ifstream file(filename);

	if (!file) {
		cout << "Error in file!" << endl;
		return;
	}
	if (!file.eof())
	{
		for (int i = 0; i < M; i++) {
			for (int j = 0; j < N; j++) {
				for (int l = 0; l < K; l++) {
					file >> arr[i][j][l];
				}
			}
		}
		std::cout << "File read!\n";
	}	
	file.close();
	std::cin.clear();
	std::cin.ignore(255, '\n');
}

void write_array_to_file(std::string arr[M][N][K], const string& filename) {
	fstream file(filename);
	if (!file) {
		cout << "Error in file!" << endl;
		return;
	}
	else
	{
		for (int i = 0; i < M; i++) {
			for (int j = 0; j < N; j++) {
				for (int l = 0; l < K; l++) {
					file << arr[i][j][l] << ' ';
				}
				file << endl;
			}
			file << endl;
		}
		cout << "Array is written to the file." << endl;
	}
	file.close();
	std::cin.clear();
	std::cin.ignore(255, '\n');
}

void menu(std::string arr[M][N][K]) {
	int choice;
	std::string symbol;
	string filename;

	do {
		cout << "\nMenu:" << endl;
		cout << "1. Insert from Keyboard" << endl;
		cout << "2. Show Array to the screen" << endl;
		cout << "3. Find the Symbol in array" << endl;
		cout << "4. Read the Array from file" << endl;
		cout << "5. Write the Array to the File" << endl;
		cout << "6. Exite" << endl;
		cout << "Choose the position: ";
		cin >> choice;

		switch (choice) {
		case 1:
			input_array(arr);
			break;
		case 2:
			display_array(arr);
			break;
		case 3:
			cout << "Insert the Symbol for searching: ";
			cin >> symbol;
			find_symbol(arr, symbol);
			break;
		case 4:
			cout << "Insert the File name for Reading: ";
			cin >> filename;
			read_array_from_file(arr, filename);
			break;
		case 5:
			cout << "Insert the file name for writting: ";
			cin >> filename;
			write_array_to_file(arr, filename);
			break;
		case 6:
			cout << "Exite the program." << endl;
			break;
		default:
			cout << "Not Correct!! Try again!!" << endl;
			std::cin.clear();
			std::cin.ignore(255, '\n');
		}
	} while (choice != 6);
}

int main() {
	std::cout << "Writing to the file is: write.txt \n";
	std::cout << "Reading from the file is: read\n";


	std::string arr[M][N][K]; 
	menu(arr); 
	std::cin.clear();
	std::cin.ignore(255, '\n');
	return 0;
}