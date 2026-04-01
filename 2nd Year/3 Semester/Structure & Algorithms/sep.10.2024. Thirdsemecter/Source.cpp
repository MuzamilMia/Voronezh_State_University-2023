
#include <iostream>
#include <fstream>
#include <string>

using Tinfo = int;

struct NODE
{
	Tinfo info;
	NODE* next;
	NODE(Tinfo info, NODE* ptr = nullptr) : info(info), next(ptr) {}
	~NODE()
	{
		next = nullptr;
	}
};

using ptrNODE = NODE *;

struct StackL
{
private:
	ptrNODE head;
	size_t count;
public:
	StackL()
	{
		head = nullptr;
		count = 0;
	}
	bool empty()
	{
		return head == nullptr;
		//return count == 0;
	}
	size_t size();
	void push(Tinfo elem);
	void pop();
	Tinfo top()
	{
		return head->info;
	}
	void view();
	void clear();
	~StackL()
	{
		clear();
		count = 0;
	}
};

const size_t MaxCount = 100;

using Array = Tinfo[MaxCount];

struct StackV
{
private:
	Array elements;
	int head;
	size_t count;
public:
	StackV();
	bool empty();
	bool full();
	size_t size();
	void puch(Tinfo elem);
	void pop();
	Tinfo top(); 
	void view();
	void clear(); 
};

struct QueueL
{
private:
	ptrNODE head, tail;
	size_t count;
public:
	QueueL();
	bool empty();
	size_t size();
	void push(Tinfo elem);
	void pop();
	Tinfo front();
	Tinfo back();
	void view();
	void clear()
		~QueueL();
};

struct QueueVL
{
private:
	Array elements;
	int head, tail;
	void init() { head = 0; tail = -1; } 
public:
	QueueVL()
	{
		init()
	}
	bool empty();
	bool full();
	size_t size();
	void puch(Tinfo elem);
	void pop(); //Ð¿Ñ€Ð¸ ÑƒÐ´Ð°Ð»ÐµÐ½Ð¸Ð¸ ÐµÑÐ»Ð¸ Ð±Ñ‹Ð» Ð¾Ð´Ð¸Ð½ ÑÐ»ÐµÐ¼ÐµÐ½Ñ‚, Ñ‚Ð¾ Ñ…ÐµÐ´ Ð¸ Ñ‚ÐµÐ¹Ð» ÑÐ´Ð²Ð¸Ð½ÑƒÑ‚ÑŒÑÑ Ð² Ð¿Ð¾Ð·Ð¸Ñ†Ð¸ÑŽ Ð¸Ð½Ð¸Ñ†Ð¸Ð°Ð»Ð¸Ð·Ð°Ñ†Ð¸Ð¸
	Tinfo front(); //Ð²Ð·ÑÑ‚ÑŒ ÑÐ»ÐµÐ¼ÐµÐ½Ñ‚ ÑÐ¾Ð´ÐµÑ€Ð¶Ð¸Ð¼Ð¾Ðµ Ð¸Ð· Ð³Ð¾Ð»Ð¾Ð²Ñ‹ 
	Tinfo back();
	void view();
	void clear(); //ÑÐ´Ð²Ð¸Ð½ÑƒÑ‚ÑŒ ÑƒÐºÐ°Ð·Ð°Ñ‚ÐµÐ»ÑŒ, Ñ‚Ð¾Ð¿ ÑÐ´Ð²Ð¸Ð½ÑƒÑ‚ÑŒ
};

//Ð—Ð°Ð´Ð°Ñ‡Ð°: 
//Ð’ Ñ‚ÐµÐºÑÑ‚Ð¾Ð²Ð¾Ð¼ Ñ„Ð°Ð¹Ð»Ðµ Ð·Ð°Ð¿Ð¸ÑÐ°Ð½Ð° Ñ„Ð¾Ñ€Ð¼ÑƒÐ»Ð°: <Ñ„Ð¾Ñ€Ð¼ÑƒÐ»Ð°> ::= <Ñ†Ð¸Ñ„Ñ€Ð°>|M(<Ñ„Ð¾Ñ€Ð¼ÑƒÐ»Ð°>, <Ñ„Ð¾Ñ€Ð¼ÑƒÐ»Ð°>)|m(<Ñ„Ð¾Ñ€Ð¼ÑƒÐ»Ð°>,<Ñ„Ð¾Ñ€Ð¼ÑƒÐ»Ð°>)
// <Ñ†Ð¸Ñ„Ñ€Ð°> ::= 0|1|2|3|4|5|6|7|8|9
// M - max
// m - min
//Ð¡Ð¸Ð¼Ð²Ð¾Ð»Ñ‹ Ð±ÑƒÐ´ÑƒÑ‚ Ñ…Ñ€Ð°Ð½Ð¸Ñ‚ÑŒÑÑ Ð² ÑÑ‚ÐµÐºÐµ, Ð·Ð°Ð¼ÐµÐ½Ð¸Ð² Ñ‚Ð¸Ð½Ñ„Ð¾, Ð½Ð¸Ð³Ð´Ðµ Ð½Ð¸Ñ‡ÐµÐ³Ð¾ Ð½Ðµ Ð´Ð¾Ð»Ð¶Ð½Ð¾ Ð¼ÐµÐ½ÑÑ‚ÑŒÑÑ. Ð˜Ð´ÐµÑ Ð·Ð°ÐºÐ»ÑŽÑ‡Ð°ÐµÑ‚ÑÑ Ð² Ñ‡ÐµÐ¼ Ñ€ÐµÑˆÐµÐ½Ð¸Ðµ Ð·Ð°Ð´Ð°Ñ‡Ð¸, Ð¿Ñ€Ð¸Ñ‡ÐµÐ¼ Ð·Ð´ÐµÑÑŒ ÑÑ‚ÐµÐº, Ð·Ð½Ð°Ñ‡Ð¸Ñ‚ Ð¸Ñ‚Ð°Ðº Ð½Ð°Ð¼ Ð½ÐµÐ²Ð°Ð¶Ð½Ð¾ ÑÑ‚ÐµÐºÐ»Ð¸ÑÑ‚ Ð¸Ð»Ð¸ ÑÑ‚ÐµÐºÐ²ÐµÐºÑ‚Ð¾Ñ€, Ð¿Ñ€ÐµÐ´Ð¿Ð¾Ð»Ð¾Ð¶Ð¸Ð¼, Ñ‡Ñ‚Ð¾ Ð¼ÐµÑÑ‚Ð° Ñ…Ð²Ð°Ñ‚Ð¸Ñ‚, Ð²Ð¾Ð·ÑŒÐ¼ÐµÐ¼ Ð»Ð¸ÑÑ‚, Ð¸Ð´ÐµÑ - ÐµÑÑ‚ÑŒ Ñ„ÑƒÐ½ÐºÑ†Ð¸Ð¾Ð½Ð°Ð»ÑŒÐ½Ð¾ Ð·Ð½Ð°Ñ‡Ð¸Ð¼Ñ‹Ðµ ÑÐ»ÐµÐ¼ÐµÐ½Ñ‚Ñ‹ Ñ„Ð¾Ñ€Ð¼ÑƒÐ»Ñ‹, Ð° ÐµÑÑ‚ÑŒ Ð´Ñ€ÑƒÐ³Ð¸Ðµ Ñ„Ð¾Ñ€Ð¼ÑƒÐ»Ñ‹, ÐºÐ¾Ñ‚Ð¾Ñ€Ñ‹Ðµ Ð¾ Ñ‡ÐµÐ¼-Ñ‚Ð¾ Ð³Ð¾Ð²Ð¾Ñ€ÑÑ‚ Ð¸ Ð² ÑÑ‚ÐµÐºÐµ Ð½Ðµ Ñ…Ñ€Ð°Ð½ÑÑ‚ÑÑ, Ñ„ÑƒÐ½ÐºÑ†Ð¸Ð¾Ð½Ð°Ð»ÑŒÐ½Ð¾ Ð·Ð½Ð°Ñ‡Ð¸Ð¼Ñ‹Ðµ - mMÑ†Ð¸Ñ„Ñ€Ñ‹,( ÐšÐ°Ðº Ñ‚Ð¾Ð»ÑŒÐºÐ¾ Ð²ÑÑ‚Ñ€ÐµÑ‚Ð¸Ð»Ð¸ Ð·Ð°ÐºÑ€Ñ‹Ð²Ð°ÑŽÑ‰ÑƒÑŽ ÑÐºÐ¾Ð±ÐºÑƒ, ÑÐ½Ð¸Ð¼Ð°ÐµÐ¼ Ñ‚Ñ€Ð¸Ð°Ð´Ñƒ, Ð´Ð°Ð»ÐµÐµ ÑÐ½Ð¸Ð¼Ð°ÐµÐ¼ ÑÑ‚Ð¾ Ð²ÑÐµ ÑƒÑˆÐ»Ð¾ Ð²Ñ‹Ñ‡Ð¸ÑÐ»ÑÐµÐ¼, ÐµÑÐ»Ð¸ Ð¾Ð¿ÐµÑ€Ð°Ð½Ð´ Ðœ Ð¼Ð°ÐºÑÐ¸Ð¼ÑƒÐ¼, ÐºÐ»Ð°Ð´ÐµÐ¼ Ð¾Ð±Ñ€Ð°Ñ‚Ð½Ð¾ Ð² ÑÑ‚ÐµÐº. Ð¤Ð¾Ñ€Ð¼ÑƒÐ»Ð° Ð·Ð°Ð¿Ð¸ÑÐ°Ð½Ð° Ð²ÐµÑ€Ð½Ð¾, Ð¿Ð¾ÑÑ‚Ð¾Ð¼Ñƒ Ð½Ð° Ð²ÐµÑ€ÑˆÐ¸Ð½Ðµ Ð»ÐµÐ¶Ð¸Ñ‚ ÑÐ¸Ð¼Ð²Ð¾Ð», ÐºÐ¾Ñ‚Ð¾Ñ€Ñ‹Ð¹ Ð½ÑƒÐ¶Ð½Ð¾ Ð¿Ñ€ÐµÐ¾Ð±Ñ€Ð°Ð·Ð¾Ð²Ð°Ñ‚ÑŒ Ð² Ñ‡Ð¸ÑÐ»Ð¾Ð²Ð¾Ð¹ ÑÐºÐ²Ð¸Ð²Ð°Ð»ÐµÐ½Ñ‚, Ñ€Ð°Ð½ÑŒÑˆÐµ Ð´ÐµÐ»Ð°Ð»Ð¸ Ñ€ÐµÐºÑƒÑ€ÑÐ¸Ð²Ð½Ð¾, Ð²ÑÑ‚Ñ€ÐµÑ‡Ð°Ð»Ð¸: Ñ€ÐµÐºÑƒÑ€ÑÐ¸Ð²Ð½Ð¾ Ð²Ñ‹Ñ‡Ð¸ÑÐ»Ð¸Ñ‚ÑŒ Ð»ÐµÐ²ÑƒÑŽ Ñ„Ð¾Ñ€Ð¼ÑƒÐ»Ñƒ Ð¿Ñ€Ð°Ð²ÑƒÑŽ Ð¸ Ñ€ÐµÐ·ÑƒÐ»ÑŒÑ‚Ð°Ñ‚. Ð ÑÑ‚Ð¾ Ð±ÑƒÐ´ÐµÐ¼ ÑÐµÐ¹Ñ‡Ð°Ñ Ð´ÐµÐ»Ð°Ñ‚ÑŒ Ñ€ÑƒÐºÐ°Ð¼Ð¸. Ð ÐµÐºÑƒÑ€ÑÐ¸Ñ Ð²ÑÐµÐ³Ð´Ð° Ñ‚ÑÐ¶ÐµÐ»Ð¾Ð²ÐµÑÐ½Ð¾, Ð½ÐµÑÐ¼Ð¾Ñ‚Ñ€Ñ Ñ‡Ñ‚Ð¾ Ð½Ð° ÑÑ‚Ð¾ Ñ€ÐµÑˆÐµÐ½Ð¸Ðµ Ð·Ð°Ð´Ð°Ñ‡Ð¸ Ð¸ÑÐ¿Ð¾Ð»ÑŒÐ·ÑƒÐµÐ¼ Ð²ÑÐµ Ñ€Ð°Ð²Ð½Ð¾ ÑÑ‚Ñ€ÑƒÐºÑ‚ÑƒÑ€Ñƒ Ð´Ð¾Ð¿Ð¾Ð»Ð½Ð¸Ñ‚ÐµÐ»ÑŒÐ½ÑƒÑŽ, Ð²ÑÐµ Ñ€Ð°Ð²Ð½Ð¾ ÑÑ‚Ð¾ Ð±Ñ‹ÑÑ‚Ñ€ÐµÐµ, Ð¿Ð¾Ñ‚Ð¾Ð¼Ñƒ Ñ‡Ñ‚Ð¾ Ð·Ð´ÐµÑÑŒ Ð½Ðµ Ð¿ÐµÑ€ÐµÐ´Ð°ÐµÑ‚ÑÑ ÑƒÐ¿Ñ€Ð°Ð²Ð»ÐµÐ½Ð¸Ðµ Ð² ÑÐ»Ð¾Ð¸ Ñ€ÐµÐºÑƒÑ€ÑÐ¸Ð¸ (Ñ‚Ð¾Ñ‡ÐºÐ¸ Ð²Ð¾Ð·Ð²Ñ€Ð°Ñ‚Ð° Ð¸ Ñ‚.Ð´.), Ð¿Ð¾ÑÑ‚Ð¾Ð¼Ñƒ ÑÑ‚Ð¾ Ð±ÑƒÐ´ÐµÑ‚ Ð±Ñ‹ÑÑ‚Ñ€ÐµÐµ, Ñ‚ÐµÐ¿ÐµÑ€ÑŒ Ñƒ Ð½Ð°Ñ Ñ‚Ð¾ Ð±Ñ‹Ð»Ð° ÑÐ¸ÑÑ‚ÐµÐ¼Ð½Ð°Ñ Ñ€Ð°Ð±Ð¾Ñ‚Ð°, ÑÑ‚Ð¾ Ð±ÑƒÐ´ÐµÑ‚ Ñ€Ð°Ð±Ð¾Ñ‚Ð° Ð¿Ð¾Ð»ÑŒÐ·Ð¾Ð²Ð°Ñ‚ÐµÐ»ÑŒÑÐºÐ°Ñ, Ñ‚Ð°Ðº. 

int Calculate(std::string file_name)
{
	std::ifstream file(file_name);
	StackL st;
	char c; //Ð±ÑƒÑ„ÐµÑ€Ð½Ð°Ñ Ð¿ÐµÑ€ÐµÐ¼ÐµÐ½Ð½Ð°Ñ, ÐºÐ¾Ñ‚Ð¾Ñ€ÑƒÑŽ Ð¸Ð· Ñ„Ð°Ð¹Ð»Ð° Ñ‡Ð¸Ñ‚Ð°Ñ‚ÑŒ
	while (!file.get(c).eof())
	{
		if (c == 'M' || c == 'm' || c >= '0' && c <= '9')
			st.push(c);
		else
			if (c == ')')
			{
				char x = st.top(); st.pop();
				char y = st.top(); st.pop();
				char op = st.top(); st.pop();
				switch (op)
				{
				case 'M':
					if (x > y)
						st.push(x);
					else
						st.push(y);
					break;
				case 'm':
					st.push(x < y ? x : y);
					/*if (x < y)
						st.push(x);
					else
						st.push(y);*/
					break;
				default:
					break;
				}
			}
	}
	file.close();
	int result = st.top() - '0';
	st.pop();
	return result;
}

int main()
{

}
struct QueueL
{
private:
	ptrNode	head, tail;
	size_t count;
public:
	QueueL() :head(nullptr), tail(nullptr), count(0) {};
	bool empty()
	{
		return count == 0;
	}
	size_t size()
	{
		return tail - head + 1;
	}
	void push(Tinfo elem);
	void pop();


};

void QueueL::push(Tinfo elem)
{
	ptrNODE newnode = new NODE(elem);
	if (tail)
	{
		tail->next = newnode;
		tail = tail->next;
	}
	if (!head)
		head = tail = newnode;
	count++;
}

void QueueL::pop()
{
	if (empty())
		std::cout << "Error";
	else
	{
		ptrNODE temp = head;
		head = head->next;
	}
}
