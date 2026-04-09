#include "Polyclinic.h"

Polyclinic::Polyclinic( std::string fname)
{
    std::ifstream file(fname);
    if (file) {

        std::string line;
        while (std::getline(file, line)) {
            if (line == "Patient") {
                std::string name, gender, number_policy;
                int age;
                std::getline(file, name);
                std::getline(file, gender);
                file >> age;
                file.ignore();
                std::getline(file, number_policy);
                list.push_back(std::make_unique<Patient>(name, gender, age, number_policy));
            }
            else if (line == "MedWorker") {
                std::string name, gender, qualification, category;
                int experience;
                std::getline(file, name);
                std::getline(file, gender);
                std::getline(file, qualification);
                file >> experience;
                file.ignore();
                std::getline(file, category);
                list.push_back(std::make_unique<Med_worker>(name, gender, qualification, experience, category));
            }
            std::getline(file, line); 
        }
        file.close();
    }
}

void Polyclinic::sort()
{
    auto comparator = [](const std::unique_ptr<Person>& a, const std::unique_ptr<Person>& b) {
        auto medWorkerA = dynamic_cast<Med_worker*>(a.get());
        auto medWorkerB = dynamic_cast<Med_worker*>(b.get());


        bool isASurgeon = (medWorkerA && medWorkerA->getQualification() == "хирург");
        bool isBSurgeon = (medWorkerB && medWorkerB->getQualification() == "хирург");

        bool flag = false;

        if (medWorkerA && medWorkerB) {
            if (isASurgeon && !isBSurgeon)
                flag = true;

        }
        else if (medWorkerA && !medWorkerB)
            flag = true;

        return flag;
        };

    list.sort(comparator);
}

void Polyclinic::print(std::ostream& out)
{
	out << name << "\n---------------------------------\n";
	for (auto& elem : list)
	{
		elem->print(out);
		out << "---------------------------------\n";
	}
}
