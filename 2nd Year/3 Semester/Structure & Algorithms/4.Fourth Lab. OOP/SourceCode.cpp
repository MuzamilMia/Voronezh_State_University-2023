#include"source.h"

auto Compare_year = [](Book a, Book b)
{
	return a.year > b.year;
};


int main()
{
	std::ifstream file("file.txt");
	if (file)
	{
		ArrayList list(file);
		list.print();
		file.close();
		std::cout << "Sorted List\n*****************************************************************************\n";
		file.open("file.txt");
		ArrayList list2(file, Compare_year);
		list2.print();
		std::cout << "My task\n--------------------------------------------------------\n";
		list2.mytask(2017);
		list2.print();
	}
	else
		std::cout << "The file is not Exist !!! \n";


	std::cin.get();
	return 0;
}