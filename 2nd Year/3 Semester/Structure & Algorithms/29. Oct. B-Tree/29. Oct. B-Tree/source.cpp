//#include<iostream>
//#include<fstream>
//#include<string>
//
//const int n = 2;
//struct Page;
//
//struct Elem
//{
//	int key;
//	int xount;
//	Page* ptr0;
//	//void init()
//};
//
//struct Page {
//	int m;
//	Page* p0;
//	Elem arr[2 * n + 1];
//	void shift_to_right(int index, int size)
//	{
//		for (int i{ size }; i >= index + 1; --i)
//			arr[i] = arr[i - 1];
//	}
//	int binry_search(int left, int right, int key);
//	Page() {
//		ptr0 = nullptr;
//		for (int i{}; i < 2 * n; ++i)
//			arr[i].ptr = nullptr;
//	}
//};
//using ptrPage = Page*;
//
//struct BTree
//{
//	ptrPage root;
//	BTree() { root = nullptr; }
//	BTree(int key) { root = new Page; root->m = 1; root->arr[0].init(key); }
//	void clear(ptrPage& t);
//	void add_rec(ptrPage& t, int key, bool& hight, Elem& elem);
//	void print_rec(ptrPage& t, std::string space = "");
//	void add(int key)
//	{
//		bool hight{};
//		Elem elem;
//		add_rec(root, key, hight, elem);
//		if (hight)
//		{
//			ptrPage tmp = root;
//			root = new Page;
//			root->m = 1;
//			root->ptr0 = tmp;
//			root->arr[0] = elem;
//			tmp = nullptr;
//		}
//	}
//
//	void print()
//	{
//		print_rec(root);
//	}
//	BTree() { clear(root); }
//
//};
//
//int main()
//{
//	return 0;
//}
//
//int Page::binry_search(int left, int right, int key)
//{
//	while (left < right)
//	{
//		int middle = (left + right) / 2;
//		if(arr[middle])
//	}
//}

#include<iostream>
#include<fstream>
#include<windows.h>
#include<string>

const int n = 2;

struct Page;
struct Elem
{
	int key;
	int count;
	Page* ptr;
	void init(int key) { this->key = key; count = 1; ptr = nullptr; }
};
struct Page
{
	int m;
	Page* ptr0;
	Elem arr[2 * n + 1];
	void shit_to_right(int index, int size)
	{
		for (int i{ size }; i >= index + 1; --i)
			arr[i] = arr[i - 1];
	}

	int binary_search(int left, int right, int key);
	Page()
	{
		ptr0 = nullptr;
		for (int i{}; i < 2 * n; ++i)
			arr[i].ptr = nullptr;
	}
};

using ptrPage = Page*;
struct BTree
{
	ptrPage root;
	BTree() { root = nullptr; }
	BTree(int key) { root = new Page; root->m = 1; root->arr[0].init(key); }
	void clear(ptrPage& t);
	void add_rec(ptrPage& t, int key, bool& hegith, Elem& elm);
	void print_rec(ptrPage& t, std::string space = "");
	void add(int key)
	{
		bool height{};
		Elem elem;
		add_rec(root, key, height, elem);
		if (height)
		{
			ptrPage tmp = root;
			root = new Page;
			root->m = 1;
			root->ptr0 = tmp;
			root->arr[0] = elem;
			tmp = nullptr;
		}
	}

	void print()
	{
		print_rec(root);
	}
	//BTree() { clear(root); }
};

int main()
{
	srand(GetTickCount());
	int x = rand() % 100;
	BTree BT(x);
	for (int i{}; i < 45; ++i)
	{
		x = rand() % 100;
		BT.add(x);
	}
	BT.print();

	std::cin.get();
	return 0;
}

int Page::binary_search(int left, int right, int key)
{
	while (left < right)
	{
		int middle = (left + right) / 2;
		if (arr[middle].key <= key)
			left = middle + 1;
		else
			right = middle;
	}
	return right - 1;
}

void BTree::clear(ptrPage& t)
{
	if (t)
	{
		clear(t->ptr0);
		for (int i{}; i < t->m; ++i)
			clear(t->arr[i].ptr);
		delete t;
		t = nullptr;
	}
}

void BTree::add_rec(ptrPage& t, int key, bool& hegith, Elem& elm)
{
	Elem tmp_elem;
	if (!t)
	{
		hegith = true;
		elm.init(key);
	}
	else
	{
		int index = t->binary_search(0, t->m, key);
		if (index >= 0 && t->arr[index].key == key)
			t->arr[index].count += 1;
		else
		{
			if (index < 0)
				add_rec(t->ptr0, key, hegith, tmp_elem);
			else
				add_rec(t->arr[index].ptr, key, hegith, tmp_elem);
			if (hegith)
			{
				if (t->m < 2 * n)
				{
					hegith = false;
					t->m += 1;
					t->shit_to_right(index, t->m);
					t->arr[index + 1] = tmp_elem;
				}
				else
				{
					Page* tmp_page = new Page();
					if (index < n)
					{
						if (index == n - 1)
							elm = tmp_elem;
						else
						{
							elm = t->arr[n - 1];
							t->shit_to_right(index + 1, n - 1);
							t->arr[index + 1] = tmp_elem;
						}
						for (int i{}; i < n; ++i)
							tmp_page->arr[i] = t->arr[i + n];
					}
					else
					{
						index -= n;
						elm = t->arr[n];
						for (int i{}; i < index + 1; ++i)
							tmp_page->arr[i] = t->arr[i + n + 1];
						tmp_page->arr[index] = tmp_elem;
						for (int i{ index + 1 }; i < n; ++i)
							tmp_page->arr[i] = t->arr[i + n];
					}
					t->m = n;
					tmp_page->m = n;
					tmp_page->ptr0 = elm.ptr;
					elm.ptr = tmp_page;
					tmp_page = nullptr;
				}
			}
		}
	}
}

void BTree::print_rec(ptrPage& t, std::string space)
{
	if (t)
	{
		for (int i = t->m - 1; i >= 0; --i)
		{
			print_rec(t->arr[i].ptr, space + "  ");
			std::cout << space << t->arr[i].key << '\n';
		}
		print_rec(t->ptr0, space + "  ");
	}
}
