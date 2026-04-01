#include <iostream>
#include <fstream>
#include <stack>

using TInfo = int;

// структура списка в простраинстве имён списка
namespace list {
	struct NODE {
		TInfo info;
		NODE* next;
		NODE(TInfo info = 0, NODE* next = nullptr) : info(info), next(next) {}
		~NODE() { next = nullptr; }
	};
	using List = NODE*;
}

// структура дерева в простраинстве имён дерева
// сделано это, чтобы NODEы не конфликтовали
namespace tree {

	struct NODE
	{
		TInfo info;
		NODE* left{}, * right{};
		NODE(TInfo info = 0, NODE* left = nullptr, NODE* right = nullptr)
		{
			this->info = info;
			this->left = left;
			this->right = right;
		}
	};

	using Tree = NODE*;

	Tree build(std::ifstream& file, int size)
	{
		Tree result{};
		if (size)
		{
			TInfo x{};
			file >> x;
			result = new NODE(x);
			result->left = build(file, size / 2);
			result->right = build(file, size - size / 2 - 1);
		}
		return result;
	}

	void clear(Tree& t)
	{
		if (t)
		{
			clear(t->left);
			clear(t->right);
			delete t;
			t = nullptr;
		}
	}

	void print(Tree t, int level)
	{
		if (t)
		{
			print(t->right, level + 1);
			for (int i{}; i < level; ++i)
				std::cout << ' ';
			std::cout << t->info << '\n';
			print(t->left, level + 1);
		}
	}

	bool is_leaf(Tree& t)
	{
		return !t->right && !t->left;
	}
}

// функция копирования дерева без листьев с помощью стека

//Tree copy(Tree& root)
//{
//	
//	Tree new_root{};
//	if (!is_leaf(root)) // если дерево состоит только из корня, вернуть следует пустое дерево
//	{
//		std::stack<Tree> st;
//		std::stack<Tree> new_st;
//		Tree t = root;
//		new_root = new NODE();
//		Tree new_t = new_root;
//		while (t)
//		{
//			new_t->info = t->info; // записываем значение узла в копию
//			if (t->left && !is_leaf(t->left)) // если есть левый потомок - нелист
//			{
//				if (t->right && !is_leaf(t->right)) // если есть правый потомок - нелист
//				{
//					st.push(t->right);
//					new_t->right = new NODE(); // планируем вернуться к правому потомку
//					new_st.push(new_t->right);
//				}
//				t = t->left; // переходим влево
//				new_t->left = new NODE(); // добавляем в копию левого потомка
//				new_t = new_t->left;
//			}
//			else
//				if (t->right && !is_leaf(t->right)) // нет левого, но есть правый потомок - нелист
//				{
//					t = t->right; // переходим вправо
//					new_t->right = new NODE(); // добавляем в копию правого потомка
//					new_t = new_t->right;
//				}
//				else // нет ни левого, ни правого потомка - нелиста
//				{
//					if (st.empty())
//						t = nullptr; // если стек пуст, значит все узлы обработаны и цикл можно завершить
//					else
//					{
//						t = st.top(); // переходим к следующему необработанному узлу
//						st.pop();
//						new_t = new_st.top();
//						new_st.pop();
//					}
//				}
//		}
//	}
//	return new_root;
//}

// рекурсивно находим номер уровня с максимальной суммой элементов
void task5_rec(tree::Tree root, list::List& head)
{
	// чтобы разделять элементы по уровням, вынуждены хранить их суммы в списке
	list::List result{}; // зачем эта строка, не знаю, result нигде не используется, но Каплиева приняла
	if (root)
	{
		if (!head)
			head = new list::NODE(); // если нет head, значит вышли на новый уровен, и для него нужно завести свою сумму
		head->info += root->info; // добавляем к сумме
		task5_rec(root->left, head->next);
		task5_rec(root->right, head->next);
	}
}

// та же задача, но с использованием стека
void task5_not_rec(tree::Tree tree, list::List& list)
{
	std::stack<tree::Tree> st; // стек
	std::stack<list::List> heads; // и список 
	tree::Tree root = tree;
	list::List head = list;
	if (root)
		list = head = new list::NODE();
	while (root)
	{
		head->info += root->info;
		if (root->left)
		{
			if (!head->next)
				head->next = new list::NODE();
			if (root->right)
			{
				st.push(root->right);
				heads.push(head->next);
			}
			root = root->left;
			head = head->next;
		}
		else
			if (root->right)
			{
				if (!head->next)
					head->next = new list::NODE();
				root = root->right;
				head = head->next;
			}
			else
			{
				if (st.empty())
					root = nullptr;
				else
				{
					root = st.top(); st.pop();
					head = heads.top(); heads.pop();
				}
			}
	}
}

// вспомогательная функция для поиска уровня с максимальной суммой элемента
int find_max(list::List list)
{
	int max = list->info;
	int max_level = 0;

	list::List p = list->next;
	int i{ 1 }; // текущий уровень
	while (p) // полный перебор списка
	{
		if (p->info > max)
		{
			max_level = i;
			max = p->info;
		}
		p = p->next;
		++i;
	}
	return max_level;
}

// Задача: удалить максимальный элемент - лист
void task6_rec(tree::Tree t, tree::Tree& pmax, TInfo& max_elem) {
	if (t) {
		if (t->left && is_leaf(t->left) && (!pmax || t->left->info > max_elem)) { // если есть левый потомок - лист и нужно обновить максимум
			max_elem = t->left->info;
			pmax = t;
		}
		if (t->right && is_leaf(t->right) && (!pmax || t->right->info > max_elem)) {// то же для правого
			max_elem = t->right->info;
			pmax = t;
		}
		task6_rec(t->left, pmax, max_elem); // дальнейший обход
		task6_rec(t->right, pmax, max_elem);
	}
}

// задача - удалить, поэтому ищем указатель на родителя максимума 
// flag означает, что на уровень выше нужно запомнить pmax
void task6_flag(tree::Tree t, tree::Tree& pmax, TInfo& max_elem, bool& flag) {
	if (t) {
		if (is_leaf(t)) { // если лист
			if (!pmax || t->info > max_elem) {
				max_elem = t->info;
				flag = true; // если нужно обновить максимум, поднимаем флаг
			}
		}
		else { // не лист
			task6_flag(t->left, pmax, max_elem, flag); // обход дерева
			if (flag) { // если флаг поднят (на уровне ниже нашли максимум), запоминаем указатель
				pmax = t;
				flag = false;
			}
			task6_flag(t->right, pmax, max_elem, flag); // то же для правого
			if (flag) {
				pmax = t;
				flag = false;
			}
		}
	}
}

int main()
{
	std::ifstream file("data.xt");
	if (file)
	{
		int size{};
		file >> size;

		// для копирования без листьев
		/*Tree root = build(file, size);
		print(root, 0);
		std::cout << "\n============================\n";
		Tree root_copy = copy(root);
		print(root_copy, 0);
		clear(root_copy);
		clear(root);*/

		// для 5 задачи
		tree::Tree pmax{ nullptr };
		tree::Tree root = tree::build(file, size);
		tree::print(root, 0);
		std::cout << "\n============================\n";
		list::List list{};
		/*task5_not_rec(root, list);
		std::cout << find_max(list) << '\n';*/

		// для 6 задачи
		TInfo max_elem{};
		//task6_rec(root, pmax, max_elem);
		bool flag = false;
		task6_flag(root, pmax, max_elem, flag);
		//std::cout << pmax->info;
		if (is_leaf(pmax->left) && pmax->left->info == max_elem) {
			delete pmax->left; // если максимум - потомок слева, удаляем слева
			pmax->left = nullptr;
		}
		else {
			delete pmax->right; // если не слева, удаляем справа
			pmax->right = nullptr;
		}
		tree::print(root, 0);
	}
	else
		std::cout << "FILE ERROR\n";
	file.close();
}
