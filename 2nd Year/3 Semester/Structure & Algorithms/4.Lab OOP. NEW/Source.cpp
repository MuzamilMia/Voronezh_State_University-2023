#include"Flist.h"
#include<Windows.h>

void my_task(const FList& list, int year)
{
	FList tmp;
	ptrNODE head = tmp.get_head();
	while (head && head->info.get_year() < year) {
		ptrNODE temp = tmp.get_head();
		head = head->next;
		delete temp;
	}
	ptrNODE current = list.get_head();
	while (current->next && current->next->info.get_year() < year) {
		ptrNODE temp = current->next;
		current->next = current->next->next;
		delete temp;
	}
}

FList remove_books_By_Year(const FList& list, int year) {
	FList newList;
	ptrNODE ptr = list.get_head();

	while (ptr) {
		if (ptr->info.get_year() >= year)
		{
			newList.add_to_tail(ptr->info);
		}
		ptr = ptr->next;
	}

	return newList;
}

int main()
{
	SetConsoleOutputCP(1251);
	std::ifstream file("file.txt");
	if (file)
	{
		FList list(file);
		list.print();
		std::cout << "*************************** My Task ************************************\n";
		int year;
		std::cout << "Enter the Year: ";
		std::cin >> year;

		FList result = remove_books_By_Year(list, year);
		std::cout << "\n\n";
		result.print();

		//my_task(list, year);
		//list.print();

		//std::cout << "Total number of books: " << Book::get_count() << std::endl;

		/*std::cout << "----------------\n";
		FList a(list);
		a = result;
		FList b(std::move(list));
		b = std::move(result);*/


	}
	else
		std::cout << "Empty file!\n";

	return 0;
}