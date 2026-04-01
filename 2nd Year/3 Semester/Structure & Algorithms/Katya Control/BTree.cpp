#include "BTree.h"

void add(ptrNODE& t, int elem)
{
    if (!t)
        t = new NODE(elem);
    else
        if (elem < t->info)
            add(t->left, elem);
        else
            if (elem > t->info)
                add(t->right, elem);
}

ptrNODE Build_balance(std::ifstream& file, int count)
{
    ptrNODE result{};
    if (count)
    {
        result = new NODE();
        file >> result->info;
        int count_left{ count / 2 };
        result->left = Build_balance(file, count_left);
        result->right = Build_balance(file, count - count_left - 1);
    }
    return result;
}

BTREE::BTREE(const char* file_name, bool balance)
{
    std::ifstream file(file_name);

    if (!balance)
    {
        TInfo x;
        while (file >> x)
            add(root, x);
    }
    else
    {
        int count{};
        file >> count;
        root = Build_balance(file, count);
    }
    file.close();
}

bool BTREE::empty()
{
    return !root;
}

void clearr(ptrNODE& t)
{
    if (t)
    {
        clearr(t->left);
        clearr(t->right);
        delete t;
        t = nullptr;
    }
}

BTREE::~BTREE()
{
    clearr(root);
}

void printt(ptrNODE t, int level, std::ostream& stream)
{
    if (t)
    {
        printt(t->right, level + 1, stream);
        for (int i = 1; i <= level; i++)
            stream << "  ";
        std::cout.width(2);
        stream << t->info << std::endl;
        printt(t->left, level + 1, stream);
    }
}
void BTREE::print(std::ostream& stream)
{
    printt(root, 0, stream);
}

void BTREE::clear(ptrNODE& ptr)
{
    clearr(ptr);
}