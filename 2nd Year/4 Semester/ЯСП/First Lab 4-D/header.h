#pragma once
#include <iostream>
#include <fstream>
#include <forward_list>
#include <algorithm>
#include <numeric>
#include <random>
#include <iterator>
#include <cmath>

/* задача 4-D:
Заменить каждое число на корень из модуля произведения его самого
и минимального из чисел. Ответ округлить до целых.
Автор: Mia Muzamil
*/

std::fstream fillFile_Random_Loop(const std::string& filename, int N, int M);
std::fstream fillFile_Random_Generate(const std::string& filename, int N, int M);
std::forward_list<int> read_numbers_fromFile(const std::string& filename);
// Functions modify
std::forward_list<int> modify_for(const std::forward_list<int>& container);
std::forward_list<int> modify_transform(const std::forward_list<int>& container);
std::forward_list<int> modify_forEach(const std::forward_list<int>& container);

int calculate_sum(const std::forward_list<int>& container);
double calculate_average(const std::forward_list<int>& container);

//void print_container(const std::forward_list<int>& container, const std::string& filename);
void print_container(const std::forward_list<int>& container, std::ostream& os = std::cout);

void displayMenu();
