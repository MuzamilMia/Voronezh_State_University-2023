#include "Person.h"
#include"HashTable.h"
#include<fstream>

int main()
{
	std::ifstream file("file.txt");
	if (file)
	{
		HashTable HT(3);
		HT.fill(file);
		HT.print();

		/*std::cout << "---------- Finding the Person ----------\n";
		Person* pt = nullptr;
		if (HT.find("Петрова_Анна", pt))
			pt->print();
		else
			std::cout << "Not found\n";
		std::cout << "----------------------------------------\n";*/


		std::cout << "---------- Adding the Person ---------- \n";
		if (!HT.add(new Med_worker({ "Петрова_Анна","мужчина","хирург", 11,"Second"})))
			std::cout << "No add\n";
		HT.print();
		std::cout << "--------------------------------------- \n";

		/*std::cout << "--------- Remving the Person ---------- \n";
		HT.remove("Сидоров_Сергей133");
		HT.print();
		std::cout << "--------------------------------------- \n";*/

	}
	else
		std::cout << "File is not opened!\n";

	file.close();

	std::cin.ignore();
	return 0;
}
