#include<iostream>
#include<fstream>
#include<vector>
#include<memory>
#include<functional>
#include<algorithm>
#include<set>
//class ELEM
//{
//public:
//	virtual void print() = 0;
//
//};
//
//class Char :public ELEM
//{
//private:
//	char ch;
//public:
//	Char(char ch) :ch(ch) {};
//	void print()
//	{
//		std::cout << "the print is:" << ch << "\n";
//	}
//	
//};
//
//class Int :public ELEM
//{
//private:
//	int num;
//public:
//	Int(int num) :num(num) {};
//	void print()
//	{
//		std::cout << num << '\n';
//	}
//	int get_num()
//	{
//		return num;
//	}
//};
//
//class Vector
//{
//private:
//	std::vector < std::unique_ptr<ELEM>> vect;
//public:
//	Vector(std::ifstream& file)
//	{
//		char type{};
//		char ch{};
//		int num{};
//		while (file >> type)
//		{
//			if (type == 'c')
//			{
//				file >> ch;
//				vect.push_back(std::make_unique<Char>(num));
//			}
//			else
//			{
//				file >> num;
//				vect.push_back(std::make_unique<Int>(num));
//			}
//
//		}
//	}
//	void print()
//	{
//		for (auto&x : vect)
//		{
//			x->print();
//		}
//	}
//
//	void sort(std::function<bool(std::unique_ptr<ELEM>&, std::unique_ptr<ELEM>)&> compare)
//	{
//		std::sort(vect.begin(), vect.end(), compare);
//	}
//
//	void my_very_good_remove(std::function<bool(std::unique_ptr<ELEM>&, std::unique_ptr<ELEM>)> compare)
//	{
//		std::sort(vect.begin(), vect.end(), compare);
//	}
//};
//
////void task(Vector& vect)
////{
////	bool stop = 0;
////	auto it =
////	while ()
////	{
////
////	}
////}

template <typename T>

class My_set {
private:
	set::set<T>s;
public:
	My_set(){}
	My_set(std::string str) 
	{	
		for (auto i : str)
			s.insert(i);
	}
	My_set operator+(My_set& X);
	My_set<T>::operator+(My_set<T>& X)
	{
		My_set<T> tmp = X;
		for (const T i : s)
			tmp.s.insert(i);
	}
	std::set<T> get()
	{
		return s;
	}
	void print()
	{
		for (T i : s)
			std::cout << i << ' ';
		std::cout << "\n";
	}
};
int main()
{
	/*
	std::fstream file("file.txt");
	Vector vect(file);
	vect.print();
	auto compare = [](std::function<bool(std::unique_ptr<ELEM>& x, std::unique_ptr<ELEM>& y)
	{
		Int* ptr_x = dynamic_cast<Int*>(x.get());
		Int* ptr_y = dynamic_cast<Int*>(y.get());
		bool res = false;
		if (ptr_x && ptr_y)
			res = ptr_x->get_num() % 2 < ptr_y->get_num() % 2;
		return res;
	}*/

	My_set <char> a("abs"), b("bcd"), c;
	c = a + b;
	c.print();
}
