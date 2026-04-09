#include "Person.h"

void Patient::print(std::ostream& out)
{
	out << "Information about the patient: \n"
		<< "Name: " << name << '\n'
		<< "Gender: " << gender << '\n'
		<< "Age: " << age << '\n'
		<< "medical Policy Number : " << med_Policy_Num << '\n';
}

void Med_worker::print(std::ostream& out)
{
	out << "Information about the Medworker: \n"
		<< "Name: " << name << '\n'
		<< "Gender: " << gender << '\n'
		<< "Qualification: " << qualification << '\n'
		<< "experience: " << experience << '\n'
		<< "category: " << category << '\n';
}
