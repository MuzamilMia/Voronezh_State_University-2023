//#include<iostream>
//#include<fstream>
//#include<ostream>
//#include<stack>
//#include<queue>
//using Tinfo = int;
//struct NODE
//{
//	Tinfo info;
//	int count = 1;
//	NODE* left, * right;
//	NODE(Tinfo info = 0, NODE* ptr_left = nullptr, NODE* ptr_right = nullptr)
//		:info(info), left(ptr_left), right(ptr_right) {};
//	~NODE()
//	{
//		left = right = nullptr;
//	}
//};
//
//using Tree = NODE*;
//Tree Build_Balance(int count, std::ifstream& file)
//{
//	Tree result{};
//	if (count)
//	{
//		int count_left = count / 2;
//		int count_right = count - count_left - 1;
//		
//		result = new NODE();
//		file >> result->info;
//		result->left = Build_Balance(count_left,file);
//		result->right = Build_Balance(count_right, file);
//
//		/*int x;
//		file >> x;
//		result = new NODE(x, Build_Balance(count_left, file), Build_Balance(count_right, file));*/
//	}
//	return result;
//}
//void Clear(Tree& root)
//{
//	if (root)
//	{
//		Clear(root->left);
//		Clear(root->right);
//		delete root;
//		root = nullptr;
//	}
//}
//void Print(Tree root, int level)
//{
//	if (root)
//	{
//		Print(root->right, level + 1);
//
//		for (int i = 1; i < level; ++i)
//			std::cout << "   ";
//		std::cout << root->info << '\n';
//		Print(root->left, level + 1);
//	}
//}
//int count_even(Tree t)
//{
//	int result{};
//	if (t)
//		result = (t->info % 2 == 0 ? 1 : 0) + count_even(t->left) + count_even(t->right);
//	return result;
//}
//int count_even_Stack(Tree t)
//{
//	std::stack<Tree> s;
//	int result{};
//	Tree root = t;
//	while (t)
//	{
//		result += (t->info % 2 == 0 ? 1 : 0);
//		if (t->left)
//		{
//			if (t->right)
//				s.push(t->right);
//			t = t->left;
//		}
//		else
//			if (t->right)
//				t = t->right;
//			else
//				if (s.empty())
//					t = nullptr;
//				else
//				{
//					t = s.top();
//					s.pop();
//				}
//	}
//	return result;
//}
//int count_even_Queue(Tree root)
//{
//	int result{};
//	std::queue<Tree> q;
//	Tree t{};
//	q.push(root);
//	while (!q.empty())
//	{
//		t = q.front(); q.pop();
//		result += (t->info % 2 == 0 ? 1 : 0);
//		if (t->left)
//			q.push(t->left);
//		if (t->right)
//			q.push(t->right);
//	}
//	return result;
//}
//void Add(Tree& t, Tinfo elem)
//{
//	if (!t)
//		t = new NODE(elem);
//	else
//		if (elem < t->info)
//			Add(t->left, elem);
//		else
//			if (elem > t->info)
//				Add(t->right, elem);
//			else
//				++t->count;
//}
//Tree Build_Search(std::ifstream& file)
//{
//	Tinfo elem;
//	Tree root{};
//	while (file >> elem)
//		Add(root, elem);
//	return root;
//}
//void Print_order(Tree t)
//{
//	if (t)
//	{
//		Print_order(t->left);
//		for (int i{}; i < t->count; ++i)
//			std::cout << t->info << ' ';
//		Print_order(t->right);
//	}
//}
//int count(Tree& root)
//{
//	int sum = 0;
//	if (root)
//	{
//		sum = (root->info % 2 == 0 ? 1 : 0) + count(root->left) + count(root->right);
//	}
//	return sum;
//}
//int coutn_queue(Tree& root)
//{
//	if (root)
//	{
//		int result = 0;
//		std::queue<Tree> q;
//		q.push(root);
//		while (!q.empty())
//		{
//			Tree t = q.front();
//			q.pop();
//			result += (t->info % 2 == 0 ? 1 : 0);
//			if (t->right)
//				q.push(t->right);
//			if (t->left)
//				q.push(t->left);
//		}
//		return result;
//	}
//}
//int count_stack(Tree& r)
//{
//	int result{};
//	std::stack<Tree> st;
//	Tree root = r;
//	while (root)
//	{
//		result += root->info % 2 == 0 ? 1 : 0;
//		if (root->left)
//		{
//			if (root->right)
//				st.push(root->right);
//			root = root->left;
//		}
//		else
//			if (root->right)
//				root = root->right;
//			else
//				if (st.empty())
//					root = nullptr;
//				else
//				{
//					root = st.top();
//					st.pop();
//				}
//	}
//	return result;
//}
//
////Tree Search(const Tree& root, Tinfo elem)
////{
////	Tree t{ root }, ptr{};
////	while (t && !ptr)
////	{
////		if (elem < t->left)
////			t = t->left;
////		else
////			if (elem > t->info)
////				t = t->right;
////			else
////				ptr = t;
////	}
////}
//
//
//void Find_elem(Tree& r, Tree& q)
//{
//	if (r->right)
//		Find_elem(r->right, q);
//	else
//	{
//		q->info = r->info;
//		q->count = r->count;
//		q = r;
//		r = r->left;
//	}
//}
//
//bool Delete(Tree& t, Tinfo elem) 
//{
//	bool result = false;
//
//	if (t) {
//		if (elem < t->info) {
//			result = Delete(t->left, elem);
//		}
//		else if (elem > t->info) {
//			result = Delete(t->right, elem);
//		}
//		else {
//			result = true;
//			if (t->count > 1) {
//				--t->count;
//			}
//			else {
//				Tree q = t;
//				if (!q->right) {
//					t = q->left;
//				}
//				else if (!q->left) {
//					t = q->right;
//				}
//				else {
//					Find_elem(q->left, q);
//				}
//				delete q;
//			}
//		}
//	}
//
//	return result;
//}
//
//
//
//
////int main()
////{
////	std::ifstream file("file.txt");
////	int cnt{};
////	file >> cnt;
////	if(file)
////	{ 
////		Tree t;
////		Tree root = Build_Balance(cnt, file);
////		Print(root, 1);
////		//Add(root, 8);
////		std::cout << "\n \n \n ";
////		//Print(root, 1);
////		/*bool root1 = Delete(root, 30);
////		if (root1)
////			std::cout << "deleted \n";
////		Print(root, 1);*/
////		/*bool result= Delete(root, 14);
////		if (result)
////			std::cout << "The elemnt is deleted \n";
////		else
////			std::cout << "THe elemt is not deleted \n";*/
////		//Print(root, 1);
////		//std::cout << count(root)<<"\n";
////		//std::cout << coutn_queue(root) << "\n";
////		//std::cout << count_stack(root) << "\n";
////
////	}
////	return 0;
////}