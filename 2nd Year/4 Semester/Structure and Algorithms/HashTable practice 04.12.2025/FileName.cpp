#include<iostream>
#include<string>
#include<fstream>
#include<vector>
#include<list>
#include<Windows.h>	
#include<functional>

using Pair = std::pair<std::string, std::string>;
std::string to_string(const Pair& pair) {
	return pair.first + "  " + pair.second;
}
using List = std::list < Pair >;
using Vector = std::vector<List>;


class Hash_table_0
{
	Vector table;
	size_t max_size;
public:
	Hash_table_0(size_t max_size):max_size(max_size)
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
	bool find(std::string key, Pair& pair);
	bool add(const Pair& pair);
	bool remove(std::string key);
	void print();


};

bool Hash_table_0::find(std::string key, Pair& pair)
{
	bool result{ false };

	size_t index = hash(key);
	List::iterator it = std::find_if(table[index].begin(), table[index].end(), 
		[key](Pair temp) {return temp.first == key; });
	if (it != table[index].end())
	{
		result = true;
		pair = *it;
	}
	return result;

}

bool Hash_table_0::add(const Pair& pair)
{
	bool result{ false };

	size_t index = hash(pair.first);
	//Pair temp{ pair.first, "" };
	List::iterator it = std::find_if(table[index].begin(), table[index].end(), [pair](Pair temp) {return temp.first == pair.first; });
	if (it == table[index].end())
	{
		result = true;
		table[index].push_back(pair);
	}
	return result;
}

bool Hash_table_0::remove(std::string key)
{
	bool result{ false };
	size_t index = hash(key);
	List::iterator it = std::find_if(table[index].begin(), table[index].end(),
		[key](Pair temp) {return temp.first == key; });
	if (it != table[index].end())
	{
		table[index].erase(it);
		result = true;
	}
	return result;
}

void Hash_table_0::print()
{
	size_t index{};
	for (auto list:table)
	{
		if (!list.empty())
		{
			for (auto pair : list)
				std::cout << index << ": " << to_string(pair) << '\n';
			++index;
		}
	}
}

int main()
{
	SetConsoleCP(1251);
	SetConsoleOutputCP(1251);

	std::ifstream file("file.txt");

	if (file)
	{
		Hash_table_0 HT(20);
		HT.fill(file);
		HT.print();
		std::cout << "-------------------------- \n";
		Pair elem{};
		//Pair elem{ "cat","" };
		if (HT.remove("cat"))
		{
			std::cout << to_string(elem) << '\n';
		}
		else
			std::cout << "NO \n";
		
		std::cout << "-------------------------- \n";
		HT.print();
	}
	return 0;
}