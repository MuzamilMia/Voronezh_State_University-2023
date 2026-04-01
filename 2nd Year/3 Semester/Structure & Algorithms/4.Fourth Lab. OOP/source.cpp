#include "source.h"

//Book::Book(std::ifstream& file)
//{
//	file.getline(Author,m);
//	file.getline(title, m);
//	file >> year;
//	file >> pages;
//	file.ignore();
//	file.getline(specialty, m);
//}
//
//void Book::print()
//{
//	std::cout << Author << '\n';
//	std::cout << title << '\n';
//	std::cout << year << '\n';
//	std::cout << pages << '\n';
//	std::cout << specialty << '\n';
//	std::cout << "---------------------------- \n";
//
//}
//
//Tinfo ArrayList::get_elem(const ptrArray ptr)
//{
//	return (ptr ? ptr->getArray() : nullptr);
//}
//
//void ArrayList::first_node(Tinfo elem)
//{
//	head = new ptrArray(DArray(elem));
//}

void ArrayList::clear()
{
	while (head)
	{
		ptrnode tmp = head;
		head = tmp->next;
		delete tmp;
	}

}

ArrayList::ArrayList(std::ifstream& file):head(nullptr)
{
	if (file)
	{
		std::string line;
		while (std::getline(file, line))
		{
			if (line.empty() || line == "---") continue;
			Book book;
			book.Author = line;
			std::getline(file, book.title);
			file >> book.year; file.ignore();
			file >> book.pages; file.ignore();
			std::getline(file, book.specialty);
			std::getline(file, line);
				
			insert(book);
		}
	}
	file.close();
}

ArrayList::ArrayList(std::ifstream& file, std::function<int(Book, Book)> compare):head(nullptr)
{
	if (file)
	{
		std::string line;
		while (std::getline(file, line))
		{
			if (line.empty() || line == "---")
				continue;
			Book book;
			book.Author = line;
			std::getline(file, book.title);
			file >> book.year; file.ignore();
			file >> book.pages; file.ignore();
			std::getline(file, book.specialty);
			std::getline(file, line);

			insert(book);
		}
		sort(compare);
	}
}

void ArrayList::insert(const Book& book)
{
	ptrnode newnode = new Node(book);

	if (!head)
		head = newnode;
	else
	{
		ptrnode temp = head;
		while (temp->next)
			temp = temp->next;
		temp->next = newnode;

	}
}

void ArrayList::print(std::ostream& stream) const
{
	ptrnode tmp = head;
	while (tmp)
	{
		stream << tmp->book.to_string() <<'\n';
		tmp = tmp->next;
		std::cout << "-----------------\n";
	}
}

void ArrayList::sort(std::function<int(Book, Book)> compare)
{
	if (!head || !head->next) return;

	bool swapped;
	do {
		swapped = false;
		Node* current = head;
		while (current && current->next) {
			if (compare(current->book, current->next->book) > 0) {
				std::swap(current->book, current->next->book);
				swapped = true;
			}
			current = current->next;
		}
	} while (swapped);
}

ptrnode ArrayList::get_head()
{
	return head;
}


void ArrayList::mytask(int year)
{
	/*if (!head)
		return;*/

	ptrnode tmp;
	while (head && head->book.year < year) {
		ptrnode temp = head;
		head = head->next;
		delete temp;
	}

	ptrnode current = head;
	while (current->next && current->next->book.year < year) {
		ptrnode temp = current->next;
		current->next = current->next->next;
		delete temp;
	}

	//while (head && head->book.year < year) {
	//	Node* temp = head;
	//	head = head->next;
	//	delete temp;
	//}
	//Node* current = head;
	//while (current && current->next) {
	//	if (current->next->book.year < year) {
	//		Node* temp = current->next;
	//		current->next = current->next->next;
	//		delete temp;
	//	}
	//	else {
	//		current = current->next;
	//	}
	//}

}
