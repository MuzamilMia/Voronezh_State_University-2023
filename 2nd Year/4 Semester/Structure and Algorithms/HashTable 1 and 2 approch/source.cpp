#include<iostream>
#include<string>
#include<fstream>
#include<vector>
using Pair = std::pair<std::string, std::string>;

struct Cell {
	Pair elem{};
	int state{};
	Cell(Pair elem, int state) :elem(elem), state(state){}

};
using Vector = std::vector<Cell>;

class Hash_table_1
{
	Vector table;
	size_t max_size;
	size_t size;
public:
	Hash_table_1(size_t max_size) :max_size(max_size)
	{
		table.resize(max_size);
	}
	size_t hash(std::string key)
	{
		size_t res{};
		for (char c : key)
			res += c;


		return res % max_size;
	}
	void fill(std::ifstream& file)
	{
		Pair elem;
		while (file >> elem.first >> elem.second)
		{
			add(elem);
		}

	}
	int find(std::string key);
	bool remove(std::string key);
	bool add(std::string);
	void print();

private:

};

int main()
{


	std::cin.ignore();
	return 0;
}

int Hash_table_1::find(std::string key)
{
	int res{ -1 };
	size_t index = hash(key);
	size_t start_index = index;
	while (!(table[index].state == 0 || table[index].state == 1 && table[index].elem.first == key))
		index = (index + 1) % max_size;
	if (table[index].state == 1)
		res = index;
	return res;
}

bool Hash_table_1::remove(std::string key)
{
	bool  res{ false };
	size_t index = hash(key);
	size_t start_index = index;
	while (!(table[index].state == 0 || table[index].state == 1 && table[index].elem.first == key))
		index = (index + 1) % max_size;
	if (table[index].state == 1)
	{
		res = true;
		size--;
		table[index].state= - 1;
	}
	return res;
}

bool Hash_table_1::add(std::string key)
{
	bool  res{ false };
	size_t index = hash(pair.first);
	size_t start_index = index;
	while ( table[index].state == 1 && table[index].elem.first != pair.first)
		index = (index + 1) % max_size;
	if (table[index].state == -1)
	{
		int push_index = index;
		while (!(table[index].state == 0 || table[index].state == 1 && table[index].elem.first == pair.first))
			index = (index + 1) % max_size;
		if (table[index].state == 0)
		{
			res = true;
			size++;
			table[push_index].state = 1;
			table[push_index].elem = pair;
		}
	}
	else
		if (table[index].state == 0)
		{
			res = true;
			size++;
			table[index].state = -1;
		}
	return res;
}

void Hash_table_1::print()
{
	for (int i{}; i < max_size; ++i)
	{
		if(table[i].state==1)
			std::cout<<i<<
	}
}
