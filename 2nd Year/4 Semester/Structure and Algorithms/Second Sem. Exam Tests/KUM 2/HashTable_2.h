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
	bool used;
	CELL() { used = false; }
};

struct HashTable_2
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

	HashTable_2()
	{
		data.resize(150);
		std::string key;
		int other;
		std::ifstream file("Data_2.txt");
		while (!file.eof())
		{
			file >> key >> other;
			int i = hash(key);
			while (data[i].used)
				i = (i + 1) % size;
			data[i].elem.key = key;
			data[i].elem.other = other;
			data[i].used = true;
		}
	}

	void print()
	{
		for (int i = 0; i < size; ++i)
			if (data[i].used)
				std::cout << data[i].elem.key << ' ' << data[i].elem.other << ' ' << i << ' ' << hash(data[i].elem.key) << '\n';
	}
	//Search function added.
	bool search(HashTable_2& table, const std::string& key, ELEM& result);
};