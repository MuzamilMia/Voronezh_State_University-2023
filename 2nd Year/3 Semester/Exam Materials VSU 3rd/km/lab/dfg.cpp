#include"mycode.h"


void TaskBySpecialty(DLIST& list, const std::string& specialty)
{
	bool flag = true;
	ptrDNODE ptr_task = list.get_head();
	while (ptr_task && flag)
	{
		ptrDNODE next_node = ptr_task->next;

		if (std::string(ptr_task->info->get_specialty()) == specialty)
		{
			list.remove(ptr_task);
			flag = false;
		}
		ptr_task = next_node;
	}
}

std::function<int(Book, Book)> compare_specialty = [](Book a, Book b)
{
	return std::string(a.get_specialty()) < std::string(b.get_specialty());
};

int main()
{
	std::ifstream file("input.txt");

	if (file)
	{
		//DLIST dlist(file);
		DLIST dlist(file, compare_specialty);
		dlist.print();
		file.close();
		std::cout << "***************************************************\n";
		std::string specialty;
		std::cout << "Enter the specialty: "; 
		std::cin >> specialty;

		TaskBySpecialty(dlist, specialty);
		dlist.print();

	}
	else
		std::cout << "Not Exist\n";
	std::cin.get();
	return 0;
}

