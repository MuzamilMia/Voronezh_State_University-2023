//
//#include "HashTable.h"
//
//
//HashTable::HashTable(int size) : max_size(size), count(0) {
//    table.resize(max_size);
//}
//
//HashTable::~HashTable() {
//    for (auto& cell : table) {
//        if (cell.flag && cell.value != nullptr) {
//            delete cell.value;
//        }
//    }
//}
//
//int HashTable::hashFunction(const std::string& key) const {
//    unsigned int hash = 0;
//    for (char ch : key)
//        hash = 31 * hash + ch;
//    return hash % max_size;
//}
//
//void HashTable::rehash_table() {
//    std::vector<Cell> old_table = table;
//    max_size *= 2;
//    count = 0;
//    table.clear();
//    table.resize(max_size);
//
//    for (auto& slot : old_table) {
//        if (slot.flag && slot.value != nullptr) {
//            add(slot.value); // transfer ownership
//        }
//    }
//}
//
//bool HashTable::add(Person* p) {
//    if (static_cast<double>(count) / max_size > 0.7) 
//        rehash_table();
//    
//    bool result = false;
//
//    std::string key = p->getName();
//    int hashIndex = hashFunction(key);
//    int originalIndex = hashIndex;
//    int i = 0;
//
//    while (table[hashIndex].flag && table[hashIndex].value != nullptr) {
//        if (table[hashIndex].value->getName() == key) {
//            result = false;
//            //return result;
//        }
//        hashIndex = (originalIndex + ++i) % max_size;
//    }
//
//    table[hashIndex].flag = true;
//    table[hashIndex].value = p;
//    ++count;
//    result = true;
//
//    return result;
//}
//
//bool HashTable::find(const std::string& name, Person*& found) {
//    bool result = false;
//    found = nullptr;
//
//    int hashIndex = hashFunction(name);
//    int originalIndex = hashIndex;
//    int i = 0;
//
//    while (table[hashIndex].flag) {
//        if (table[hashIndex].value != nullptr && table[hashIndex].value->getName() == name) {
//            found = table[hashIndex].value;
//            result = true;
//            break;
//        }
//        hashIndex = (originalIndex + ++i) % max_size;
//    }
//
//    return result;
//}
//
//bool HashTable::erase(const std::string& name) {
//    bool result = false;
//
//    int hashIndex = hashFunction(name);
//    int originalIndex = hashIndex;
//    int i = 0;
//
//    while (table[hashIndex].flag) {
//        if (table[hashIndex].value != nullptr && table[hashIndex].value->getName() == name) {
//            delete table[hashIndex].value;
//            table[hashIndex].value = nullptr;
//            --count;
//            result = true;
//            break;
//        }
//        hashIndex = (originalIndex + ++i) % max_size;
//    }
//
//    return result;
//}
//
//void HashTable::print(std::ostream& out) {
//    out << "\n----- Hash Table Contents -----\n";
//    for (size_t i = 0; i < table.size(); ++i) {
//        out << "[" << i << "]: ";
//        if (!table[i].flag) {
//            out << "Not used\n";
//        }
//        if (table[i].value == nullptr) {
//            out << "Nullptr or Deleted\n";
//        }
//        else if  (table[i].value != nullptr)
//        {
//            table[i].value->print(out);
//            out << '\n';
//        }
//       //}
//    }
//    out << "-------------------------------------------------\n";
//}
//
//void HashTable::fill(std::ifstream& file) {
//    std::string line;
//    while (getline(file, line)) {
//        if (line == "Patient") {
//            std::string name, gender, policy;
//            int age;
//
//            getline(file, name);
//            getline(file, gender);
//            file >> age;
//            file.ignore();
//            getline(file, policy);
//            getline(file, line); // separator
//
//            Person* p = new Patient(name, gender, age, policy);
//            add(p);
//        }
//        else if (line == "MedWorker") {
//            std::string name, gender, qual, cat;
//            int exp;
//
//            getline(file, name);
//            getline(file, gender);
//            getline(file, qual);
//            file >> exp;
//            file.ignore();
//            getline(file, cat);
//            getline(file, line); // separator
//
//            Person* p = new Med_worker(name, gender, qual, exp, cat);
//            add(p);
//        }
//    }
//}
//

#include "HashTable.h"
#include <iostream>
#include <sstream>
#include "Person.h"

void HashTable::reHash()
{
    size_t new_max_size = max_size * 2;  
    std::vector<Cell> new_table(new_max_size);

    for (const auto& cell : table) {
        if (cell.state) {
            size_t index = hash(cell.ptr->getName()) % new_max_size;
            while (new_table[index].state) {
                index = (index + 1) % new_max_size;
            }
            new_table[index] = cell;  
        }
    }
    table = std::move(new_table);
    max_size = new_max_size;
}

bool HashTable::add(Person* p) {
    if ((double)size / max_size >= 0.7) 
        reHash();  

    size_t index = hash(p->getName());
    bool res = false;

    // Linear probing to find an empty slot or matching key
    while (table[index].state && table[index].ptr->getName() != p->getName()) {
        index = (index + 1) % max_size;
    }

    if (!table[index].state) {
        table[index].ptr = p;
        table[index].state = true;
        size++;
        res = true;
    }

    return res;
}

bool HashTable::remove(const std::string& key) {
    size_t index = hash(key);
    int window = -1;

    auto dist = [this](int a, int b) {
        return (b - a + max_size) % max_size;
        };

    while (table[index].state && table[index].ptr->getName() != key)
        index = (index + 1) % max_size;

    //if (!table[index].state) return false;

    delete table[index].ptr;
    table[index].ptr = nullptr;
    table[index].state = false;

    window = index;
    index = (index + 1) % max_size;

    while (table[index].state) {
        if (dist(hash(table[index].ptr->getName()), index) >= dist(window, index)) {
            table[window] = table[index];
            window = index;
            table[index].state = false;
            table[index].ptr = nullptr;
        }
        else {
            index = (index + 1) % max_size;
        }
    }

    --size;
    return true;
}

bool HashTable::find(const std::string& key, Person*& out) {
    size_t index = hash(key);
    while (table[index].state && table[index].ptr->getName() != key)
        index = (index + 1) % max_size;

    if (table[index].state) {
        out = table[index].ptr;
        return true;
    }

    return false;
}


//void HashTable::fill(std::ifstream& file)
//{
//    std::string line;
//    while (getline(file, line)) {
//        if (line == "Patient") {
//            std::string name, gender, policy;
//            int age;
//
//            getline(file, name);
//            getline(file, gender);
//            file >> age;
//            file.ignore();
//            getline(file, policy);
//            getline(file, line); // separator
//
//            Person* p = new Patient(name, gender, age, policy);
//            add(p);
//        }
//        else if (line == "MedWorker") {
//            std::string name, gender, qual, cat;
//            int exp;
//
//            getline(file, name);
//            getline(file, gender);
//            getline(file, qual);
//            file >> exp;
//            file.ignore();
//            getline(file, cat);
//            getline(file, line); // separator
//
//            Person* p = new Med_worker(name, gender, qual, exp, cat);
//            add(p);
//        }
//    }
//}
void HashTable::fill(std::ifstream& file) {
    std::string line;

    while (std::getline(file, line)) {
        if (line == "Patient") {
            std::string name, gender, policy;
            int age;
            std::getline(file, name);
            std::getline(file, gender);
            file >> age;
            file.ignore();
            std::getline(file, policy);
            std::getline(file, line); 

            Person* p = new Patient({ name, gender, age, policy });
            add(p);
        }
        else if (line == "MedWorker") {
            std::string name, gender, specialization, degree;
            int exper;
            std::getline(file, name);
            std::getline(file, gender);
            std::getline(file, specialization);
            file >> exper;
            file.ignore();
            std::getline(file, degree);
            std::getline(file, line); 
            Person* p = new Med_worker({ name, gender, specialization, exper, degree });
            add(p);
        }
    }
}


void HashTable::print() const {
    for (size_t i = 0; i < max_size; ++i) {
        if (table[i].state) {
            size_t key = hash(table[i].ptr->getName()); // calculate hash key
            std::cout << i << " (hash: " << key << "): ";
            table[i].ptr->print();
        }
        std::cout << "\n";
    }
}
