#pragma once

#include<iostream>
#include<fstream>
#include<functional>
#include<string>

class Book
{
private:
	std::string author, name, specialty;
	int year, pages;
 int count;
public:
	Book() {};
	Book(std::string author, std::string name, int years, int pages_, std::string specialty) :
		author(author), name(name), year(years), pages(pages_), specialty(specialty) {
		count += 1;
	};

	std::string get_author() { return author; };
	std::string get_name() { return name; };
	std::string get_specialty() { return specialty; };
	int get_year() { return year; };
	int get_count_of_pages() { return pages; };
	int get_count() { return count; }

	void set_author(std::string s) { author = s; };
	void set_name(std::string s) { name = s; };
	void set_specialty(std::string s) { specialty = s; };
	void set_years(int years) { year = years; };
	void set_pages(int page) { pages = page; };

	bool operator==(Book other) { return year == other.get_year(); };
	bool operator!=(Book other) { return year != other.get_year(); };
	bool operator>(Book other) { return year > other.get_year(); };
	bool operator>=(Book other) { return year >= other.get_year(); };
	bool operator<(Book other) { return year < other.get_year(); };
	bool operator<=(Book other) { return year <= other.get_year(); };

	friend std::istream& operator>>(std::istream& in, Book& book);
	friend std::ostream& operator<<(std::ostream& out, Book& book);


};
