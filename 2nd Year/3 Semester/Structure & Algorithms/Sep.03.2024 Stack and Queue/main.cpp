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

using ptrNODE = NODE*;

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
    Tinfo top(); //взять элемент содержимое из головы 
    void view();
    void clear(); //сдвинуть указатель, топ сдвинуть
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
    void init() {head = 0; tail = -1;} //признак пустой очереди
public:
    QueueVL()
    {
        init() 
    }
    bool empty();
    bool full();
    size_t size();
    void puch(Tinfo elem);
    void pop(); //при удалении если был один элемент, то хед и тейл сдвинуться в позицию инициализации
    Tinfo front(); //взять элемент содержимое из головы 
    Tinfo back();
    void view();
    void clear(); //сдвинуть указатель, топ сдвинуть
};

//Задача: 
//В текстовом файле записана формула: <формула> ::= <цифра>|M(<формула>, <формула>)|m(<формула>,<формула>)
// <цифра> ::= 0|1|2|3|4|5|6|7|8|9
// M - max
// m - min
//Символы будут храниться в стеке, заменив тинфо, нигде ничего не должно меняться. Идея заключается в чем решение задачи, причем здесь стек, значит итак нам неважно стеклист или стеквектор, предположим, что места хватит, возьмем лист, идея - есть функционально значимые элементы формулы, а есть другие формулы, которые о чем-то говорят и в стеке не хранятся, функционально значимые - mMцифры,( Как только встретили закрывающую скобку, снимаем триаду, далее снимаем это все ушло вычисляем, если операнд М максимум, кладем обратно в стек. Формула записана верно, поэтому на вершине лежит символ, который нужно преобразовать в числовой эквивалент, раньше делали рекурсивно, встречали: рекурсивно вычислить левую формулу правую и результат. А это будем сейчас делать руками. Рекурсия всегда тяжеловесно, несмотря что на это решение задачи используем все равно структуру дополнительную, все равно это быстрее, потому что здесь не передается управление в слои рекурсии (точки возврата и т.д.), поэтому это будет быстрее, теперь у нас то была системная работа, это будет работа пользовательская, так. 

int Calculate(std::string file_name)
{
    std::ifstream file(file_name);
    StackL st;
    char c; //буферная переменная, которую из файла читать
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

