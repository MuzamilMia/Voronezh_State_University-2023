#pragma once

#include <iostream>
#include <vector>
#include <fstream>
#include "person.h"
#include <list>

using List = std::list<std::unique_ptr<Person>>;

class Polyclinic {
private:
    std::string name;
    List list;

public:
    Polyclinic(std::string fname);

    std::list<std::unique_ptr<Person>>& get_list() { return list; }

    void set_list(List& list)
    {
        this->list = std::move(list);
    }

    void sort();

    void print(std::ostream& out = std::cout);
};

