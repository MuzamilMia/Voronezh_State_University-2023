//#include<iostream>	
//#include<fstream>
//#include<Windows.h>	
//#include<list>	
//#include<vector>
//#include <map>
//
////adding the funciton to the class. 
//
//void add(T elem)
//{
//	s.insert(elem);
//}
////--------------------------
//
////By Balakhonov
//My_set<std::string>find_uniqe_words(std::ifstream& file)
//{
//	std::string word{};
//	My_set<std::string>all{}, repeats{};
//	while (file >> word)
//	{
//		//if (all.get().find(word) != all.get().end())
//		if (all.get().count(word) )
//			repeats.add(word);
//		else
//			all.add(word);
//	}
//	return all - repeats;
//}
////
///////BY Pashaaa
//#include<map>
//#include<sstream>
//#include <set>
//
//using Map = std::map <std::string, std::set<int>>;
//
//Map Taks(std::ifstream& file)
//{
//	Map map{};
//	std::string line{}, word{};
//	int num{};
//
//	std::stringstream ss{};
//	while (getline(file, line))
//	{
//		ss << line;
//		++num;
//		while (ss >> word)
//		{
//			map[word].insert(num);
//		}
//		ss.clear();
//	}
//	return map;
//}
//void print_map(Map map)
//{
//	for (auto elem : map)
//	{
//		std::cout << elem.first << ": ";
//
//		for (auto elem2 : elem.second)
//			std::cout << elem2 << '\n';
//		std::cout << '\n';
//	}
//}
////----pashat done
//
////--------Olyanaaa------
////int F(int n)
////{
////	return n >= 2025 ? n : F(n + 1) - F(n + 2) + 7;
////}
////
////using Map = std::map <int, int>;
////Map map{};
////int F1(int n)
////{
////	int res{};
////	if (n >= 2025)
////	{
////		res = n;
////		map.insert({ n,n });
////	}
////	else
////	{
////		int x{}, y{};
////		if (map.count(n + 1))
////			x = map[n + 1];
////		else
////		{
////			x = F1(n + 1);
////			//map.insert({ n + 1, x });
////		}
////		if (map.count(n + 2))
////			y = map[n + 2];
////		else
////		{
////			y = F1(n + 2);
////			//map.insert({ n + 2, y });
////		}
////
////		res = x - y + 7;
////		map.insert({ n,res });
////	}
////	return res;
////}
//
//
//
//
//int main()
//{
//	std::fstream file("file.txt");
//	//My_set<std::string>set = find_uniqe_words(file);
//
//	/*Map map = task(file);
//	print_map(map);*/
//
//
//	std::cout << F(15) - F(14);
//
//	return 0;
//}