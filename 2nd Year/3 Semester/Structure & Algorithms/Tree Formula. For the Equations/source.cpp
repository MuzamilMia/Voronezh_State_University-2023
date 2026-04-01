#include <iostream>
#include<fstream>	
#include<Windows.h>
#include<string>
using Info = char;
struct NODE {
	Info info;
	NODE* left, * right;
	NODE(Info info=0, NODE* left_ptr = nullptr, NODE* right_ptr = nullptr) :info(info), left(left_ptr), right(right_ptr) {}

};
using Tree = NODE*;
Tree build_formula(std::ifstream& file)
{
	char c = file.get();
	Tree t = new NODE();
	if (c >='0' && c <= '9')
		t->info = c;
	else
	{ 
		t->left = build_formula(file);
		t->info = file.get();
		t->right = build_formula(file);
		c = file.get();
	}
	return t;
}

std::string Print_to_string(const Tree &t)
{
	std::string result{};
	if (!t->left && !t->right)
		result += t->info;
	else
		result = '(' + Print_to_string(t->left) + t->info + Print_to_string(t->right) + ')';
	return result;
}
int Calculate(const Tree& t)
{
	int result{};
	if (!t->left && !t->right)
		result = t->info - '0';
	else
	{
		int leftF{ Calculate(t->left) };
		int rightF{ Calculate(t->right) };
		switch (t->info)
		{
		case '+':
			result = leftF + rightF;
			break;
		case '*':
			result = leftF * rightF;
			break;
		case '-':
			result = leftF - rightF;
			break;
		}
	}
	return result;
}

int main()
{
	std::ifstream file("file.txt");
	if (file)
	{
		Tree root = build_formula(file);

		std::string tree_representation = Print_to_string(root);
		std::cout << "Tree representation: " << tree_representation << std::endl;

		int result = Calculate(root);
		std::cout << "The result is: " << result;

	}
	else
		std::cout << "The file is not exicted\n";


	std::cin.get();
	return 0;
}