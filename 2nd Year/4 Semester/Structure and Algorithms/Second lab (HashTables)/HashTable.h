//#pragma once
//#include <iostream>
//#include <fstream>
//#include <vector>
//#include <list>
//#include <string>
//#include "Person.h"
//
//
//using List = std::list<Person*>;
//using Vector = std::vector<List>;
//
//
//
//class HashTableList
//{
//private:
//
//	Vector table;
//	size_t max_size;
//	size_t size;
//	const int MAX_LIST_SIZE = 2;
//	void rehash_table()
//	{
//		std::cout << "------------------------ ReHashing ----------------------- \n";
//		Vector new_table;
//		max_size *= 2;
//		new_table.resize(max_size);
//		for (auto lst : table)
//		{
//			for (auto elem : lst)
//			{
//				int index = hash(elem->getName());
//				new_table[index].push_back(elem);
//			}
//		}
//		table = new_table;
//	}
//
//public:
//
//	HashTableList(size_t max_size) : max_size(max_size)
//	{
//		size = 0;
//		table.resize(max_size);
//	}
//
//	~HashTableList()
//	{
//		for (auto lst : table)
//			for (auto elem : lst)
//			{
//				delete elem;
//				elem = nullptr;
//			}
//	}
//
//	int hash(std::string key)
//	{
//		int result = 0;
//		for (char c : key)
//			result += c;
//		return result % max_size;
//	}
//
//	bool find(std::string key, Person*& elem)
//	{
//		bool result = false;
//		int index = hash(key);
//
//		List::iterator it = std::find_if(table[index].begin(), table[index].end(),
//			[key](Person* tmp) { return tmp->getName() == key; }
//		);
//		if (it != table[index].end())
//		{
//			result = true;
//			elem = *it;
//		}
//
//		return result;
//	}
//
//	bool add(Person* elem)
//	{
//		bool result = false;
//		int index = hash(elem->getName());
//		if (table[index].size() > MAX_LIST_SIZE)
//			rehash_table();
//		List::iterator it = std::find_if(table[index].begin(), table[index].end(),
//			[elem](Person* tmp) { return tmp->getName() == elem->getName(); }
//		);
//		if (it == table[index].end())
//		{
//			result = true;
//			table[index].push_back(elem);
//			++size;
//
//		}
//		return result;
//	}
//
//	bool erase(std::string key)
//	{
//		bool result = false;
//		int index = hash(key);
//
//		List::iterator it = std::find_if(table[index].begin(), table[index].end(),
//			[key](Person* tmp) { return tmp->getName() == key; }
//		);
//		if (it != table[index].end())
//		{
//			result = true;
//			delete* it;
//			*it = nullptr;
//			table[index].erase(it);
//			--size;
//		}
//
//		return result;
//	}
//
//	void fill(std::ifstream& file)
//	{
//		std::string type{};
//		while (file >> type)
//		{
//			file.ignore(255, '\n');
//			if (type == "Patient")
//			{
//				std::string name{};
//				std::getline(file, name);
//
//				std::string gender{};
//				std::getline(file, gender);
//
//				int age{};
//				file >> age;
//				file.ignore(255, '\n');
//
//				std::string policy{};
//				std::getline(file, policy);
//
//				Patient* patient = new Patient(name, gender, age, policy);
//				add(patient);
//			}
//			else
//				if (type == "MedWorker")
//				{
//					std::string name{};
//					std::getline(file, name);
//
//					std::string gender{};
//					std::getline(file, gender);
//
//					std::string qual{};
//					std::getline(file, qual);
//
//					size_t expr{};
//					file >> expr;
//					file.ignore(255, '\n');
//
//					std::string catag{};
//					std::getline(file,catag);
//
//					Med_worker* medworker = new Med_worker(name, gender, qual, expr, catag);
//					add(medworker);
//				}
//		}
//	}
//
//	void print()
//	{
//		int index{};
//		for (auto lst : table)
//		{
//			++index;
//			if (!lst.empty())
//			{
//				for (auto elem : lst)
//				{
//					std::cout << index << '\n';
//					elem->print();
//				}
//				++index;
//			}
//		}
//	}
//};


//this is the last night. 
/*#pragma once
#include "Person.h"
#include <vector>
#include <string>
#include <iostream>
#include <fstream>

struct Cell {
    bool flag = false;     
    Person* value = nullptr;
};

class HashTable {
private:
    std::vector<Cell> table;
    int max_size;
    int count;

    int hashFunction(const std::string& key) const;
    void rehash_table();

public:
    HashTable(int size = 10);
    ~HashTable();

    bool add(Person* p);
    bool find(const std::string& name, Person*& found);
    bool erase(const std::string& name);
    void print(std::ostream& out = std::cout);
    void fill(std::ifstream& file);
};
*/
//-----------------------------

#pragma once

#include "Person.h"

struct Cell {
    Person* ptr = nullptr;
    bool state = false;

    Cell() = default;
    Cell(Person* p, bool st) : ptr(p), state(st) {}
};

class HashTable {
    std::vector<Cell> table;
    size_t max_size;
    size_t size;

public:
    HashTable(size_t max_size) : max_size(max_size), size(0) {
        table.resize(max_size);
    }
    size_t hash(const std::string& key) const {
        size_t res = 0;
        for (char c : key) 
            res += c;
        return res % max_size;
    }
    void reHash();
    bool add(Person* p);
    bool remove(const std::string& key);
    bool find(const std::string& key, Person*& out);
    void fill(std::ifstream& file);
    void print() const;
};
