#pragma once
#include <iostream>
#include <string>
#include <fstream>
#include <vector>

class Person {
protected:
    std::string name;
    std::string gender; 

public:
    Person(std::string _name, std::string _gender) : name(_name), gender(_gender) {}

    std::string getName() const { return name; }
    std::string getGender() const { return gender; }

    virtual void print(std::ostream& out = std::cout) = 0;
};

class Patient : public Person {
private:
    int age;
    std::string med_Policy_Num;

public:
    Patient(std::string _name, std::string _gender, int _age, std::string policy)
        : Person(_name, _gender), age(_age), med_Policy_Num(policy) {
    }

    void print(std::ostream& out = std::cout);

};

class Med_worker : public Person {
private:
    std::string qualification;
    int experience;
    std::string category;

public:
    Med_worker(std::string _name, std::string _gender, std::string qual, int exp, std::string cat)
        : Person(_name, _gender), qualification(qual), experience(exp), category(cat) {
    }

    void print(std::ostream& out = std::cout);
   
};

