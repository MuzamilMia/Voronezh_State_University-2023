#include<iostream>
#include<fstream>

struct NODE
{
	bool eow{};
	NODE* ptrs[26];
	NODE() {
		eow = false;
		for (int i{}; i < 26; ++i)
			ptrs[i] = nullptr;
	}

};
using TrieTree = NODE*;

void intit(TrieTree& root)
{
	root = nullptr;
}

bool empty(TrieTree root)
{
	return root == nullptr;
}
//(i) is razmishat.(Placed)
void add(TrieTree& root, const std::string& word, int i)
{
	if (!root)
	{
		root = new NODE();
	}
	if (word.length() - 1 < i)
		root->eow = true;
	else
		add(root->ptrs[word[i] - 'a'], word, i + 1);
}

bool all_ptrs_Empty(TrieTree root)
{
	bool result{ true };
	int i{};
	while (i < 26 && result)
		if (root->ptrs[i])
			result = false;
		else
			++i;
	return result;
}

void delete_(TrieTree& root, const std::string& word, int i)
{
	if (root)
	{
		if (i <= word.length() - 1)
			delete_(root->ptrs[word[i] - 'a'], word, i + 1);
		else
		{
			root->eow = false;
			if (all_ptrs_Empty(root))
			{
				delete root;
				root = nullptr;
			}
		}
	}
}

void print(TrieTree root, std::string word)
{
	if (root->eow)
		std::cout << word << '\n';
	for (int i{}; i < 26; ++i)
		if (root->ptrs[i])
			print(root->ptrs[i], word + char(i + 'a'));
}

void clear(TrieTree& root)
{
	for (int i{}; i < 26; ++i)
		if (root->ptrs[i])
			clear(root->ptrs[i]);
	delete root;
	root = nullptr;
}

TrieTree copy(TrieTree root)
{
	TrieTree result{};
	if (root)
	{
		result = new NODE;
		result->eow = root->eow;
		for (int i{}; i < 26; ++i)
			if (root->ptrs[i])
				result->ptrs[i] = copy(root->ptrs[i]);
	}
	return result;
}
int main()
{
	std::ifstream file("file.txt");
	if (file)
	{
		TrieTree root{};
		intit(root);
		std::string word{};
		while (file >> word)
		{
			if (word.length())
				add(root, word, 0);
		}
		print(root, "");
		delete_(root, "dog", 0);
		std::cout << '\n';
		print(root, "");
		std::cout << "\n----------------------------\n The copy of the tree \n";
		TrieTree copy_tree = copy(root);
		print(copy_tree, " ");

	}

	return 0;
}