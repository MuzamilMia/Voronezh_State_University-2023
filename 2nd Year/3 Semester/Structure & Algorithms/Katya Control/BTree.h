#pragma once

#include <iostream>
#include <fstream>

using TInfo = int;

struct NODE
{
	TInfo info;
	NODE* left = nullptr, * right = nullptr;
	NODE() {}
	NODE(TInfo info) :info(info) {}
	~NODE()
	{
		left = right = nullptr;
	}
};

using ptrNODE = NODE*;

struct BTREE
{
private:
	ptrNODE root{};
public:
	BTREE() { root = nullptr; }
	BTREE(const char* file_name, bool balance = true);
	bool empty();
	TInfo get_info(ptrNODE ptr)
	{
		return ptr->info;
	}
	ptrNODE get_root() const
	{
		return root;
	}
	void set_root(ptrNODE ptr)
	{
		root = ptr;
	}
	~BTREE();
	void print(std::ostream& stream = std::cout);
	void clear(ptrNODE& ptr);
	bool is_leaf(ptrNODE ptr)
	{
		return !ptr->left && !ptr->right;
	}
};