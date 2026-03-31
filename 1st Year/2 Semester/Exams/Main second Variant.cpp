#include "FLIST.h"
#include <functional>

using std::cin;
using std::cout;
using std::string;
string token{ " \n,.:;" };

//void create_matrix(std::ifstream& file, int matrix[][8])
//{
//	int x{}, i{};
//	while (file >> x)
//	{
//		matrix[0][i] = x;
//		++i;
//	}
//}
//void print_matrix(int matrix[][8])
//{
//	int i{};
//	while (i != 80)
//	{
//		if (i % 8 == 0) cout << '\n';
//		cout << matrix[0][i] << ' ';
//		++i;
//	}
//}
//bool check(int* arr)
//{
//	bool res{};
//	int i{};
//
//	while (!res && i < 8)
//	{
//		int* current = arr + i;
//		int cnt{1};
//		for (int* begin = current + 1; begin < arr + 8; ++begin)
//		{
//			if (*current % 10 == *begin % 10) ++cnt;
//		}
//		if (cnt > 2) res = true;
//		++i;
//	}
//
//	return res;
//}
//bool Task(int* begin, int* end, std::string& ROWS)
//{
//	bool flag{};
//	int cnt_rows{1};
//
//	for (begin; begin < end; begin += 8)
//	{
//		if (cnt_rows % 2 != 0 && check(begin))
//		{
//			ROWS += std::to_string(cnt_rows);
//			ROWS += ' ';
//			flag = true;
//		}
//		++cnt_rows;
//	}
//	return flag;
//}
//
//int Task2(std::ifstream& file, int n)
//{
//	auto check_word = [](string word)
//		{
//			int res{};
//			if (word.size() % 2 != 0)
//			{
//				string first_sym{ word[0] };
//				if (word.find(first_sym, 1) != string::npos) ++res;
//			}
//			return res;
//		};
//
//	string line{};
//	string word_draft{};
//	bool flag{};
//	int cnt_line{};
//
//	while (!flag && getline(file, line))
//	{
//		++cnt_line;
//		size_t start{}, end{};
//		int cnt{};
//		while ((start = line.find_first_not_of(token, end)) != string::npos)
//		{
//			end = line.find_first_of(token, start);
//			word_draft = line.substr(start, end - start);
//			cnt += check_word(word_draft);
//		}
//		if (cnt == n) flag = true;
//	}
//
//	if (!flag) cnt_line = 0;
//	return cnt_line;
//}


void create_matrix(std::ifstream& file, int matrix[][8])
{
	int i{}, x{};
	while (file >> x)
	{
		matrix[0][i] = x;
		++i;
	}
}
void print_matrix(int matrix[][8])
{
	for (int i{}; i < 80; ++i)
	{
		if (i % 8 == 0) cout << '\n';
		cout << matrix[0][i] << ' ';
	}
	cout << "\n";
}
bool check_line(int* arr)
{
	int i{};
	bool check{};
	while (!check && i < 8)
	{
		int* current = arr + i;
		int cnt{1};
		for (int* begin = current + 1; begin < arr + 8; ++begin)
		{
			if (abs(*current) % 10 == abs(*begin) % 10) ++cnt;
		}
		if (cnt > 2) check = true;
		++i;

	}
	return check;
	
}
int Task1(int* begin, int* end)
{
	int cnt{};

	for (begin; begin < end; begin += 16)
	{
		if (check_line(begin)) ++cnt;
	}
	return cnt;
}

int Task2(std::ifstream& file, int n)
{
	auto check_word = [](string word)
		{
			int res{};
			if (word.size() % 2 != 0)
			{
				string first_sym{ word[0] };
				if (word.find(first_sym, 1) != string::npos) ++res;
			}
			return res;
		};


	bool flag{};
	string line{}, word_draft{};
	int cnt_line{};
	while (!flag && getline(file, line))
	{
		++cnt_line;
		size_t start{}, end{};
		int cnt{};

		while ((start = line.find_first_not_of(token, end)) != string::npos)
		{
			end = line.find_first_of(token, start);
			word_draft = line.substr(start, end - start);
			cnt += check_word(word_draft);
		}
		if (cnt == n) flag = true;
	}
	if (!flag) cnt_line = 0;

	return cnt_line;
}
void del_list(FLIST& list, ptrNODE& ptr1, ptrNODE& ptr2)
{

	while (ptr1->next != ptr2)
	{
		if (ptr1->next->info > 9 && ptr1->next->info < 100)
			list.del_after(ptr1);
		else ptr1 = ptr1->next;
	}
}
void Task3(FLIST& list, std::function<bool(int)> lambda)
{
	ptrNODE ptr1{}, ptr2{}, beg{ list.get_head()->next };
	while (beg && (!ptr1 || !ptr2))
	{
		if (lambda(beg->info))
		{
			if (!ptr1) ptr1 = beg;
			else ptr2 = beg;
		}
		beg = beg->next;
	}
	if (ptr1 && ptr2) del_list(list, ptr1, ptr2);
	
}
void Task4(FLIST& list)
{
	ptrNODE ptr_max{ list.get_head() };
	ptrNODE ptr{ptr_max->next};

	while (ptr->next)
	{
		if (ptr->next->info > ptr_max->next->info) ptr_max = ptr;
		ptr = ptr->next;
	}

	ptrNODE tmp = ptr_max->next;
	ptr_max->next = ptr_max->next->next;
	tmp->next = list.get_head()->next;
	list.get_head()->next = tmp;
	
}

ptrNODE copy(const ptrNODE ptr)
{
	ptrNODE result = nullptr;
	if (ptr)
		result = new NODE(ptr->info, copy(ptr->next));
	return result;
}




int** create_matrix(int ROWS, int COLS)
{
	int** matrix = new int* [ROWS] {};
	for (int i{}; i < COLS; ++i) matrix[i] = new int[COLS] {};
	return matrix;
}
void print_matrix(int** matrix, int ROWS, int COLS)
{
	for (int i{}; i < ROWS; ++i)
	{
		for (int j{}; j < COLS; ++j) cout << matrix[i][j] << ' ';
		cout << '\n';
	}
}
void fill_matrix(std::ifstream& file, int** matrix, int ROWS, int COLS)
{
	for (int i{}; i < ROWS; ++i)
		for (int j{}; j < COLS; ++j) file >> matrix[i][j];
}
bool check(int* arr, int size)
{
	int cnt{};
	for (int i{}; i < size - 1; ++i)
		for (int j{}; j < size; ++j)
			if ((arr[i] % 7 == 0 && arr[j] % 7 != 0) || (arr[i] % 7 != 0 && arr[j] % 7 == 0))
				++cnt;

	return cnt % 2 == 0;
}
int Task1_2(int** begin, int** end, int COLS)
{
	bool flag{};

	while (!flag && begin < end)
	{
		if (check(*begin, COLS)) flag = true;
		++begin;
	}

	return end - begin + 1;
}


int main()
{
	SetConsoleOutputCP(1251);

	std::ifstream file("file.txt");

	if (file)
	{
		/*int matrix[10][8]{};
		create_matrix(file, matrix);
		print_matrix(matrix);
		cout << Task1(matrix[0], matrix[0] + 80);*/

		/*cout << "¬ведите n:";
		int n{};
		cin >> n;
		cout << Task2(file, n);*/

		/*auto lambda = [](int x)
			{
				return x % 3 == 0;
			};

		FLIST list;
		list.create_by_stack(file);
		list.print("----");
		Task3(list, lambda);
		list.print("))))))))");*/

		/*FLIST list;
		list.create_by_stack(file);
		list.print("0----)");
		Task4(list);
		list.print("----");*/

		FLIST list, list_copy;
		list.create_by_stack(file);
		ptrNODE phead{ list.get_head()->next };
		list_copy.get_head()->next = copy(phead);
		list_copy.print("-----");
		
	}
	else cout << "File Error";

	cin.ignore();
	cin.get();
	return 0;
}