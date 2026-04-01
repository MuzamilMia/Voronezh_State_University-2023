#pragma once
#include<iostream>
#include<fstream>	
#include<Windows.h>
#include<string>
#include <functional>

//Структура записи :
//	втор
//	Название 
//	Год издания
//	Количество страниц
//	Специальность
//1. Получить новый список, удалив все записи о книгах, изданных ранее указанной даты.


struct Book
{
	std::string Author;
	std::string title;
	int year;
	int pages;
	std::string specialty;

	Book() :year(0), pages(0) {}
	std::string to_string()const {
		return Author + "\n" + title + "\n" + std::to_string(year) + "\n" +
			std::to_string(pages) + "\n" + specialty;
	}

};
//using Tinfo = Book*;
struct Node
{
	Book book;
	Node* next;
	Node(const Book& book) : book(book), next(nullptr) {}
};

using ptrnode = Node*;

class ArrayList
{
private:
	ptrnode head;
	void clear();
public:
	ArrayList() :head(nullptr) {}
	ArrayList(std::ifstream& file);
	ArrayList(std::ifstream& file, std::function<int(Book, Book)> compare);
	void insert(const Book& book);
	void print(std::ostream& stream = std::cout)const;
	void sort(std::function<int(Book, Book)> compare);

	ptrnode get_head();
	void mytask(int year);
	~ArrayList() { clear(); }
};
//class DArray 
//{
//private:
//	Tinfo Array;
//
//	DArray* prev;
//	DArray* next;
//	
//public:
//	DArray(Tinfo book, DArray* next = nullptr, DArray* prev = nullptr) :Array(book), next(next), prev(prev) {}
//	Tinfo getArray() { return Array; }
//
//	~DArray()
//	{
//		prev = nullptr;
//		next = nullptr;
//	}
//
//};
//
//using ptrArray = DArray*;
//
//class ArrayList
//{
//private:
//	ptrArray head, tail;
//	size_t size; 
//public:
//	ArrayList() { head = nullptr; tail = nullptr; }
//	ArrayList(std::ifstream& file);
//	ArrayList(std::ifstream& file, std::function<int(Book, Book)>compare);
//	Tinfo get_elem(const ptrArray ptr);
//	void first_node(Tinfo elem);
//	ptrArray get_head();
//	void insert_after(ptrArray ptr, Tinfo elem);
//	void insert_before(ptrArray ptr, Tinfo elem);
//	void print();
//};
