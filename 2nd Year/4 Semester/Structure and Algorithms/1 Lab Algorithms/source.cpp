#include "Person.h"
#include"Polyclinic.h"


void task(Polyclinic& poly) 
{
    int male_surg = 0;
    int female_surg = 0;
    std::list<std::string> maleSu_name;
    std::list<std::string> femaleSu_name;

    auto it = poly.get_list().begin();
   /* if (it != poly.get_list().end())
        std::cout << *it << '\n';
    else
        std::cout << "Empty the iterator \n";*/
    auto end = poly.get_list().end();

    bool flag = false;

    while (it != end && !flag) {
        Person* person_ptr = it->get();
        auto medWorker = dynamic_cast<Med_worker*>(it->get());

        if (medWorker && medWorker->getQualification() == "хирург") {
            if (medWorker->getGender() == "мужчина") {
                male_surg++;
                maleSu_name.push_back(medWorker->getName());
            }
            else if (medWorker->getGender() == "женщина") {
                female_surg++;
                femaleSu_name.push_back(medWorker->getName());
            }
            ++it;
        }
        else
            flag = true;
    }

    std::cout << "Male Surgeons: " << male_surg << '\n';
    std::cout << "Female Surgeons: " << female_surg << '\n';

    if (male_surg > female_surg)
        std::cout << "male Surgeons are more. \n";
    else if (female_surg > male_surg)
        std::cout << "female Surgeons are more. \n";
    else
        std::cout << "Equale. \n";

    std::cout << "\nMale Surgeons:" << '\n';
    for (auto& name : maleSu_name) {
        std::cout << name << '\n';
    }

    std::cout << "\nFemale Surgeons:" << '\n';
    for (const auto& name : femaleSu_name) {
        std::cout << name << '\n';
    }
}

int main()
{
	std::string filename = "file.txt";
	Polyclinic poly(filename);

	std::cout << "---- Original Data ---- \n";
	poly.print();

	std::cout << "-------------------- Sorted Data -------------------- \n";
	poly.sort();
	poly.print();

    std::cout << " -------------------- My Task -------------------- \n";
    task(poly);

	return 0;
}