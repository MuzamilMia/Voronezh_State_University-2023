#include "Flist.h"

void FList::add_by_pointer(ptrNODE& ptr, TInfo value)
{
    ptr = new NODE(value, ptr);
}

FList::FList(std::ifstream& file)
{
    TInfo value;
    tail = head = nullptr;
    auto insert = [this](TInfo value) -> ptrNODE
        {
            ptrNODE result = head;
            while (result->next && result->next->info > value)
                result = result->next;
            return result;
        };

    while (file >> value)
    {
        if (empty() || head->info <= value)
            add_to_head(value);
        else
            add_by_pointer(insert(value)->next, value);
    }
}

FList::FList(const FList& other)
{
    ptrNODE ptr = other.head;
    this->head = this->tail = nullptr;
    while (ptr)
    {
        this->add_to_tail(ptr->info);
        ptr = ptr->next;
    }
}

FList& FList::operator=(const FList& other)
{
    if (this != &other)
    {
        clear();
        ptrNODE ptr = other.head;
        while (ptr)
        {
            this->add_to_tail(ptr->info);
            ptr = ptr->next;
        }
    }
    return *this;
}

FList::FList(FList&& tmp)
{
    this->head = tmp.head;
    this->tail = tmp.tail;
    tmp.tail = tmp.head = nullptr;
}

FList& FList::operator=(FList&& tmp)
{
    if (this != &tmp)
    {
        this->clear();
        this->head = tmp.head;
        this->tail = tmp.tail;
        tmp.head = nullptr;
        tmp.tail = nullptr;
    }
    return *this;
}

FList::~FList()
{
    clear();
}

bool FList::empty()
{
    return !head;
}

void FList::add_to_head(TInfo value)
{
    add_by_pointer(head, value);
}

void FList::add_to_tail(TInfo value)
{
    if (tail)
    {
        add_by_pointer(tail->next, value);
        tail = tail->next;
    }
    else
        add_to_head(value);
}

void FList::print()
{
    ptrNODE ptr = head;
    while (ptr)
    {
        std::cout << ptr->info << "---------------------\n";
        ptr = ptr->next;
    }
}

void FList::del_from_head()
{
    ptrNODE p = head;
    head = head->next;
}

void FList::clear()
{
    while (head)
        del_from_head();
    head = tail = nullptr;
}
