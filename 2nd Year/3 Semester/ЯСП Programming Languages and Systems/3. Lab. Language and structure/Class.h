#pragma once

#include <iostream>
#include <fstream>
#include <set>
#include <iterator>
#include <algorithm>

// Описание задачи:6
// Реализовать шаблонный класс множества Set<X> с методами для добавления, удаления,
// проверки существования элемента, а также выполнения операций объединения, пересечения,
// разности и вывода в файл и консоль. Множество инициализируется из файла.
// Автор: Muzamil Mia

template <typename X>
class Set {
private:
    std::set<X> elements; // Стандартный контейнер set для хранения элементов множества
    // array

public:
    void add(const X& element) {
        elements.insert(element);
    }

    void remove(const X& element) {
        elements.erase(element);
    }

    bool contains(const X& element) const {
        return elements.find(element) != elements.end();
    }
    // Объединение 
    Set<X> unite(const Set<X>& other) const {
        Set<X> result;
        result.elements = elements;
        result.elements.insert(other.elements.begin(), other.elements.end());
        return result;
    }
    // Пересечение (Intersect) of Sets
    Set<X> intersect(const Set<X>& other) const {
        Set<X> result;
        for (const auto& elem : elements) {
            if (other.contains(elem)) {
                result.add(elem);
            }
        }
        return result;
    }
    // Разность (Difference between two Sets)
    Set<X> difference(const Set<X>& other) const {
        Set<X> result;
        for (const auto& elem : elements) {
            if (!other.contains(elem)) {
                result.add(elem);
            }
        }
        return result;
    }

    void printToFile(const std::string& filename) const {
        std::ofstream file(filename);
        if (file.is_open()) {
            for (const auto& elem : elements) {
                file << elem << std::endl;
            }
            file.close();
        }
        else {
            std::cerr << "NOt Found!" << std::endl;
        }
    }

    void print() const {
        if (elements.empty()) {
            std::cout << "Empty Set!" << std::endl;
            return;
        }
        for (const auto& elem : elements) {
            std::cout << elem << " ";
        }
        std::cout << std::endl;
    }

    void initFromFile(const std::string& filename) {
        std::ifstream file(filename);
        if (file.is_open()) {
            X elem;
            while (file >> elem) {
                add(elem);
            }
            file.close();
        }
        else {
            std::cerr << "Not Found!" << std::endl;
        }
    }
};



