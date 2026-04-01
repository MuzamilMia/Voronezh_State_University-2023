#include<Windows.h>
#include<iostream>
#include<fstream>
#include<functional>
#include <format>

const int m = 50;
struct Book
{
private:
	char author[m];
	char title[m];
	int year;
	int pages;
	char specialty[m];

public:
	Book() {};
	Book(std::ifstream& file);
	void print();
	int get_year()
	{
		return year;
	}
	const char* get_specialty() const { return specialty; }
};

using TInfo = Book*;
using ptrBook = Book*;

struct DNODE
{
	TInfo info;
	DNODE* next, * prev;
	DNODE(TInfo info, DNODE* next = nullptr, DNODE* prev = nullptr) :info(info), next(next), prev(prev) {}
	~DNODE()
	{
		next = nullptr;
		prev = nullptr;
	}
};
using ptrDNODE = DNODE*;

struct DLIST
{
private:
	ptrDNODE head, tail;
	size_t size;

public:
	DLIST() { head = nullptr, tail = nullptr; }
	DLIST(std::ifstream& file);
	DLIST(std::ifstream& file, std::function<int(Book, Book)>compare);
	TInfo get_elem(const ptrDNODE ptr);
	void first_node(TInfo elem);
	ptrDNODE get_head();
	void insert_after(ptrDNODE ptr, TInfo elem);
	void insert_before(ptrDNODE ptr, TInfo elem);
	void remove(ptrDNODE& ptr);
	void print();

};