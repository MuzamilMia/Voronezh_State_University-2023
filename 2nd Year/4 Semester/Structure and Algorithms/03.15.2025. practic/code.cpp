#include <iostream>
#include <fstream>
#include <string>
#include <map>
#include <set>
#include <vector>
#include <algorithm>
#include <cctype>

using namespace std;

void task(std::ifstream& file,set<int>&number)
{
    int num;
    if (file)
    {
        while (file >> num)
        {
            number.insert(num);
        }
    }

}

void taks(std::ifstream& file, map<string, int>& mymap)
{
    string data;
    int age;
    if (file)
    {
        while (file >> data >> age)
            mymap[data] = age;
    }
}

int main() 
{
    ifstream file("file.txt");
 /*   set<int> numbers;
    task(file, numbers);
    for (auto num : numbers)
        cout << num << "\n";*/

    map<string, int>my_map;
    taks(file, my_map);

    for (auto print : my_map)
        std::cout << print.first << " --- " << print.second << "\n";
    return 0;
}