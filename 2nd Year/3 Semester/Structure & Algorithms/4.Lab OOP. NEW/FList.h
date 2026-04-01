#pragma once

#include"Book.h"

using TInfo = Book;

class NODE
{
public:
	TInfo info;
	NODE* next;
	NODE() {}
	NODE(TInfo info, NODE* ptr = nullptr) : info(info), next(ptr) {}
	~NODE() { next = nullptr; }
};

using ptrNODE = NODE*;
class FList
{
private:
	ptrNODE head, tail;
	void add_by_pointer(ptrNODE& ptr, TInfo value);
public:
	FList() { head = tail = nullptr; }
	FList(std::ifstream& file);
	FList(const FList& other);
	FList& operator=(const FList& other);
	FList(FList&& tmp);
	FList& operator=(FList&& tmp);
	~FList();

	ptrNODE get_head() const { return head; }
	bool empty();
	void add_to_head(TInfo value);
	void add_to_tail(TInfo value);
	void print();
	void del_from_head();
	void clear();
};
