#include<iostream>
#include<fstream>
#include<ostream>
using Tinfo = int;
struct NODE
{
	Tinfo info;
	NODE* next;
	NODE(Tinfo info, NODE* ptr = nullptr)
	{
		this->info = info;
		next = ptr;
	}
	~NODE()
	{
		next = nullptr;
	}
};
const size_t MaxSize = { 100 };
using Array = Tinfo[MaxSize];
struct StackV
{
private:
	Array elements;
	int head;
public:
	StackV();
	bool empty();
	size_t size();
	void push(Tinfo elem);
	int pop();
	Tinfo top();
	void view();
	void clear();
	bool full();
};
int task(const std::string& expression) {
	StackV s;
	int i = 0;
	while (i < expression.length())
	{
		char ch = expression[i];
		if (isdigit(ch)) {

			s.push(ch - '0');
		}
		else {
			int b = s.pop();
			int a = s.pop();

			switch (ch) {
			case '+':
				s.push(a + b);
				break;
			case '-':
				s.push(a - b);
				break;
			case '*':
				s.push(a * b);
				break;
			case '^':
				s.push(pow(a, b));
				break;
			default:
				std::cout << "Nothing for operation! " << ch << '\n';
				exit(1);
			}
		}
		++i;
	}
	// in stack remain one element. result. 
	return s.pop();
}

int main()
{
	StackV st;
	std::ifstream file("file.txt");
	if (!file) {
		std::cout << "No file" << '\n';
		return 1;
	}

	std::string expression;
	file >> expression;
	file.close();

	int result = task(expression);
	std::cout << "Result: " << result <<'\n';

	return 0;
}

StackV::StackV()
{
	head = -1;
}

bool StackV::empty()
{
	return head == -1;
}

size_t StackV::size()
{
	return head + 1;
}

void StackV::push(Tinfo elem)
{
	if (head != MaxSize - 1)
	{
		elements[++head] = elem;
	}
	else
		std::cout << "Stack is Full \n";
}

int StackV::pop()
{
	if (top() < 0)
	{
		std::cout << "Stack is Empty!\n";
		return -1;
	}
		return elements[head--];
}

Tinfo StackV::top()
{
	return elements[head];
}

void StackV::view()
{
	for (int i = head; i >= 0; --i)
		std::cout << elements[i] << " ";
}

void StackV::clear()
{
	head = -1;
}

bool StackV::full()
{
	return head == MaxSize - 1;
}
