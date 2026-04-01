#include "Book.h"

std::istream& operator>>(std::istream& in, Book& book)
{
    std::string s;
    int value;

    std::getline(in, s);
    book.set_author(s);

    std::getline(in, s);
    book.set_name(s);

    in >> value;
    book.set_years(value);

    in >> value;
    book.set_pages(value);
    in.ignore();

    std::getline(in, s);
    book.set_specialty(s);

    in.ignore();
    book.count += 1;
    return in;
}

std::ostream& operator<<(std::ostream& out, Book& book)
{
    out << book.author << '\n'
        << book.name << '\n'
        << book.year << '\n'
        << book.pages << '\n'
        << book.specialty << '\n';
    return out;
}
