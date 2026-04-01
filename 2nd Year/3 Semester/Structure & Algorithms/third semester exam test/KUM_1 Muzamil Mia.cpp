#include"Binary_Tree.h"
#include"Trie_Tree.h"
#include<stack>
#include<queue>
// ------------ task1 --------------

void doubleLastChar(ttree::TTREE& tree, ttree::ptrNODE& root)
{
	if (root)
	{
		for (int i = 0; i < 26; ++i)
		{
			if (root->ptrs[i])
			{
				if (root->ptrs[i]->eow)
				{
					root->ptrs[i]->ptrs[i] = new ttree::NODE();
					root->ptrs[i]->eow = false;
					root->ptrs[i]->ptrs[i]->eow = true;

				}
				else
					doubleLastChar(tree, root->ptrs[i]);
			}
		}
	}
}

// ------------ task2 --------------

// ------------ task3 --------------

// ------------ task4 --------------

int  main()
{
	btree::BTREE root("file.txt");
	root.print();
	//-------------------------------------------------
	std::cout << "\n My task 1 is: \n";
	//task1
	//root.print();
	//std::cout << "\n My task 2 is: \n";
	////task2

	////-------------------------------------------------
	std::cout << '\n';
	ttree::TTREE trie_tree("trie.txt");
	trie_tree.print(true);
	ttree::ptrNODE root1 = trie_tree.get_root();
	doubleLastChar(trie_tree, root1);
	trie_tree.print("");
	////-------------------------------------------------
	//std::cout << "My task 3 is: \n";

	////Elem(30);
	////task3


	////-------------------------------------------------
	//std::cout << "My task 4 is: \n";
	////task4

	std::cin.get();
	return 0;
}
