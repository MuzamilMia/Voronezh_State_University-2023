#pragma once
#include <string>
#include <vector>
#include <fstream>
#include <iostream>

struct ELEM
{
	std::string key;
	int other;

	ELEM() {};

	ELEM(std::ifstream& file)
	{
		file >> key;
		file >> other;
	}
};

struct CELL
{
	ELEM elem;
	int used; // -1 .. 1
	CELL() { used = 0; }
};

struct HashTable_1
{
private:
	std::vector<CELL> data;
	int size = 150;
public:
	int hash(std::string word)
	{
		int sum = 0;
		for (char sym : word)
			sum += sym;
		return sum % size;
	}

	HashTable_1()
	{
		data.resize(150);
		std::string key;
		int other;
		std::ifstream file("Data_2.txt");
		while (!file.eof())
		{
			file >> key >> other;
			int i = hash(key);
			while (data[i].used == 1)
				i = (i + 1) % size;
			data[i].elem.key = key;
			data[i].elem.other = other;
			data[i].used = 1;
		}
	}

	void print()
	{
		for (int i = 0; i < size; ++i)
			if (data[i].used == 1)
				std::cout << data[i].elem.key << ' ' << data[i].elem.other << ' ' << i << ' ' << hash(data[i].elem.key) << '\n';
	}
	//bool remove(HashTable_1& table, std::string key);
	////KUM 1
	//bool remove(std::string key)
	//{
	//	int i = hash(key);
	//	int originalIndex = i;
	//	bool first = true;
	//	bool removed = false;
	//	while (data[i].used != 0 && (first || i != originalIndex) && !removed)
	//	{
	//		if (data[i].used == 1 && data[i].elem.key == key)
	//		{
	//			data[i].used = -1; 
	//			removed = true;
	//		}
	//		else
	//		{
	//			i = (i + 1) % size;
	//			first = false;
	//		}
	//	}
	//	return removed;
	//}

	bool remove(std::string key);
	bool remove1(HashTable_1& table, std::string key);
};