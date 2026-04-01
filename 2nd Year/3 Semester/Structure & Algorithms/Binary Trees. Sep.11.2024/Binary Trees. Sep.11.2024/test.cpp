
#include<iostream>
#include<fstream>
#include<ostream>
#include<stack>
#include<queue>
using Tinfo = int;
struct NODE
{
	Tinfo info;
	int count = 1;
	NODE* left, * right;
	NODE(Tinfo info = 0, NODE* ptr_left = nullptr, NODE* ptr_right = nullptr) :info(info), left(ptr_left), right(ptr_right) {};
	~NODE()
	{
		left = right = nullptr;
	}
};

using Tree = NODE*;
Tree Build_Balance(int count, std::ifstream& file)
{
	Tree result{};
	if (count)
	{
		int count_left = count / 2;
		int count_right = count - count_left - 1;

		result = new NODE();
		file >> result->info;
		result->left = Build_Balance(count_left, file);
		result->right = Build_Balance(count_right, file);
	}
	return result;
}
void Clear(Tree& root)
{
	if (root)
	{
		Clear(root->left);
		Clear(root->right);
		delete root;
		root = nullptr;
	}
}
void Print(Tree root, int level)
{
	if (root)
	{
		Print(root->right, level + 1);
		for (int i = 1; i < level; ++i)
			std::cout << "   ";
		std::cout << root->info << '\n';
		Print(root->left, level + 1);
	}
}

int Depth_tree_recursion(Tree root)
{
	int result{ -1 };
	if (root)
	{
		int leftMax = Depth_tree_recursion(root->left);
		int rightMax = Depth_tree_recursion(root->right);

		result = std::max(leftMax, rightMax) + 1;
	}
	return result;
}
int Depth_by_stack(Tree root)
{
	Tree t = root;
	int max_depth{};
	std::stack<std::pair<Tree, int>> s;
	int depth{};
	while (t)
	{
		if (t->left)
		{
			if (t->right)
				s.push({ t->right,depth + 1 });
			t = t->left;
			depth += 1;
		}
		else
			if (t->right)
			{
				t = t->right;
				depth += 1;
			}
			else
				if (s.empty())
					t = nullptr;
				else
				{
					max_depth = std::max(max_depth, depth);
					t = s.top().first;
					depth = s.top().second;
					s.pop();

				}
	}
	return max_depth;
}
int Depth_by_Queue(Tree root) {
	if (root)
	{
		int depth{ -1 };

		std::queue<Tree> q;

		q.push(root);
		q.push(nullptr);

		while (!q.empty()) {
			Tree curr = q.front();
			q.pop();

			if (!curr) {
				depth++;
				if (!q.empty()) {
					q.push(nullptr);
				}
			}
			else {
				if (curr->left)
					q.push(curr->left);
				if (curr->right)
					q.push(curr->right);
			}
		}
		return depth;
	}
}

int main()
{
	std::ifstream file("file.txt");
	int cnt{};
	file >> cnt;
	if (file)
	{
		Tree root = Build_Balance(cnt, file);
		Print(root, 1);
		std::cout << "The Max depth of the tree by recursion is: " << Depth_tree_recursion(root);
		std::cout << "\nThe Max depth of the tree by stack is: " << Depth_by_stack(root);
		std::cout << "\nThe Max depth of the tree by Queue is: " << Depth_by_Queue(root);
	}
}