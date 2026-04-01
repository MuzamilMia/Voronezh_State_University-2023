#include<iostream>
#include<vector>
#include<algorithm>
#include<functional>

struct ELEM
{
	int x;
	char ch;
	ELEM(){}
	ELEM(int x, char ch):x(x), ch(ch){}
	void print()
	{
		std::cout << x << "(" << ch << ")\n";
	}
	void operator()(ELEM elem) const
	{
		std::cout << elem.x << " (" << elem.ch << " )\n";
	}

};
void printF(ELEM elem)
{
	std::cout << elem.x << "(" << elem.ch << ")\n";
}
struct print
{
	void operator()(ELEM elem) const
	{
		std::cout << elem.x << " (" << elem.ch << ") \n";
	}
};

void print_vect(std::vector<ELEM> vect)
{
	for (ELEM& elem : vect)
		elem.print();
	std::cout << "-----------------------------------\n";
}
using Vector = std::vector<ELEM>;
using Iterator = Vector::iterator;

//int f()

int main()
{
	/*std::vector<int> v1;
	std::vector<int> v2(v1);
	std::vector<int> v3 = v1;
	std::vector<int>v4(5);
	std::vector<int>v5(5, 2);
	std::vector<int>v6{ 1,2,3,4,5 };
	std::vector<int>v7 = { 1,2,3,4,5 };*/

	/*std::vector<int> v1(5);
	std::vector<int> v2{ 5 };
	std::vector<int> v3(5, 2);
	std::vector<int> v4{ 5,2 };*/

	std::vector<int> v{ 1,2,3,4,5 };
	/*std::cout << v[0] << '\n';
	v[0] = 10;
	std::cout << v.at(0) << '\n';
	v.at(0) = 8;
	std::cout << v.front() << '\n';
	v.front() = 7;
	std::cout << v.back() << '\n';
	v.back() = 9;*/

	//std::cout << v[7] << '\n';
	/*try
	{
		int number = v.at(7);
	}
	catch(std::out_of_range e)
	{
		std::cout << "out of the range \n";
	}*/

	//#include<algorithm>
	//#include<functional>

	/*Vector vect;
	vect.push_back(ELEM(5, 'A'));
	vect.emplace_back(7, 'B');
	vect.insert(vect.begin(), ELEM(3, 'C'));
	vect.emplace(vect.begin(), 9, 'D');
	print_vect(vect);*/
	//Vector vect = { ELEM(1,'A'),ELEM(2,'B'), ELEM(3,'C') , ELEM(4,'D') ,ELEM(5,'E') };
	Vector vect = { {1,'A'},{2,'B'}, {3,'C'} , {4,'D'} ,{5,'E'} };

	//---- 1 Way ---
	print_vect(vect);
	std::cout << "---------------------- \n";

	//---- 2 Way ---
	for (Iterator it = vect.begin(); it != vect.end(); ++it)
		(*it).print();
	std::cout << "---------------------- \n";

	//---- 3 Way ---
	std::for_each(vect.begin(), vect.end(), printF);
	std::cout << "---------------------- \n";

	//---- 4 Way ---
	std::for_each(vect.begin(), vect.end(), print());
	std::cout << "---------------------- \n";

	//---- 5 Way ---
	std::for_each(vect.begin(), vect.end(), ELEM());
	std::cout << "---------------------- \n";

	//---- 6 Way ---
	std::for_each(vect.begin(), vect.end(), std::mem_fun(&ELEM::print));
	std::cout << "---------------------- \n";

	//---- 7 Way ---
	auto prn = std::bind(&ELEM::print, std::placeholders::_1);
	std::for_each(vect.begin(), vect.end(), std::mem_fun(&ELEM::print));
	std::cout << "---------------------- \n";
	
	//---- 8 Way ---
	auto printL = [](ELEM elem) {elem.print(); };
	std::for_each(vect.begin(), vect.end(), std::mem_fun(&ELEM::print));
	std::cout << "---------------------- \n";

	return 0;
}