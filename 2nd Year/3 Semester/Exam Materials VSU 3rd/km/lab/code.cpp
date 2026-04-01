#include"mycode.h"

TInfo DLIST::get_elem(const ptrDNODE ptr)
{
	return (ptr ? ptr->info : nullptr);
}

void DLIST::first_node(TInfo elem)
{
	head = new DNODE(TInfo(elem));
	tail = head;
}

ptrDNODE DLIST::get_head()
{
	return head;
}

void DLIST::insert_after(ptrDNODE ptr, TInfo elem)
{
	ptrDNODE p = new DNODE(elem, ptr->next, ptr);
	if (ptr == tail)
		tail = p;
	else
		ptr->next->prev = p;
	ptr->next = p;
}

void DLIST::insert_before(ptrDNODE ptr, TInfo elem)
{
	ptrDNODE p = new DNODE(elem, ptr, ptr->prev);
	if (ptr == head)
		head = p;
	else
		ptr->prev->next = p;
	ptr->prev = p;
}

void DLIST::print()
{
	ptrDNODE p = head;
	while (p)
	{
		(p->info)->print();
		p = p->next;
	}
	std::cout << "\n";
}

DLIST::DLIST(std::ifstream& file)
{
	TInfo elem = new Book(file);
	first_node(elem);
	while (!file.eof())
	{
		elem = new Book(file);
		insert_after(tail, elem);
	}
	file.close();
}

DLIST::DLIST(std::ifstream& file, std::function<int(Book, Book)> compare)
{
	TInfo elem = new Book(file);
	first_node(elem);
	auto find_place = [this, compare](TInfo elem)->ptrDNODE
		{
			ptrDNODE p = head;
			while (p && compare(*(p->info), *elem))
				p = p->next;
			return p;
		};

	ptrDNODE place{};
	while (!file.eof())
	{
		elem = new Book(file);
		place = find_place(elem);
		if (place)
			insert_before(place, elem);
		else
			insert_after(tail, elem);
	}
	file.close();
}

Book::Book(std::ifstream& file)
{
	file.getline(author, m);
	file.getline(title, m);
	file >> year;
	file >> pages;
	file.ignore();
	file.getline(specialty, m);
	if (!file.eof())
	{
		char delim_file[255];
		file.getline(delim_file, 255);
	}
}

void Book::print()
{
	std::cout << author << '\n';
	std::cout << title << '\n';
	std::cout << year << '\n';
	std::cout << pages << '\n';
	std::cout << specialty << '\n';
	std::cout << "----------\n";
}

void DLIST::remove(ptrDNODE& ptr)
{
	ptrDNODE p = ptr;
	if (ptr == head)
	{
		head = p->next;
		ptr = head;
		if (p->next)
			p->next->prev = nullptr;
	}
	else
		if (ptr == tail)
		{
			tail = p->prev;
			ptr = tail;
			if (p->prev)
				p->prev->next = nullptr;
		}
		else
		{
			ptr = ptr->next;
			p->next->prev = p->prev;
			p->prev->next = p->next;
		}

	delete p;

}
