#include<iostream>
#include<windows.h>
#include<string>
#include<iostream>
#include <functional>

class DArray
{
private:
	size_t size, max_size;
	int* array;
	void destroy() { if(array) delete[] array; array = nullptr; }
	int number;
	static int counter;
	//static inline int counter{};


public:
	std::string my_string() const;
	void print(std::ostream& stream = std::cout) const;
	void set_size(int size)
	{
		this->size = size;
		array = new int[size];
	}
	//---------------------------
	DArray(){}
	DArray(int max_size);
	DArray(int max_size, int size, int A, int B);
	DArray(int max_size, std::istream& stream);

	int get_elem(int index);
	void set_elem(int elem, int index);
	int get_size()const { return size; }
	int get_max_size()const { return max_size; };
	int get_number()const { return number; }

	static void print_count()
	{
		std::cout << "create: " << counter << "objects " << "\n";
	}

	friend void change(DArray&, std::function<void(int&)>);

	~DArray() { destroy(); }

};

int DArray::counter = 0;
void change(DArray& arr, std::function<void(int&)>action)
{
	for (int i = 0; i < arr.size; ++i)
		action(arr.array[i]);
}



int main()
{
	/*DArray arr(100, 10, -100, 100);
	std::cout << "\n --------------------------------------  \n";
	DArray arr2(100);*/

	//////arr.my_string();
	//arr.print();

	DArray DA1(100, 10, -100, 100);
	DA1.print();
	std::cout << DA1.get_number();
	std::cout << "\n --------------------------------------  \n";
	DArray DA2(100, 10, -100, 100);
	DA2.print();
	std::cout << DA2.get_number();
	std::cout << "\n --------------------------------------  \n";
	change(DA2, [](int& x) {x *= 2; });
	DA2.print();

	std::cout << "\n --------------------------------------  \n";
	DArray::print_count();


	std::cin.get();
	return 0;
}

std::string DArray::my_string() const
{
	std::string result= "";
	if (!array)
		result = "Array is empty ";
	else
	{
		for (size_t i{}; i < size; ++i)
			result += std::to_string(array[i]) + " ";
	}
		
	return result;
}

void DArray::print(std::ostream& stream) const
{
	stream << my_string() << '\n';
}

DArray::DArray(int max_size)
{
	array = new int[max_size];
	number = ++counter;
}

DArray::DArray(int max_size, int size, int A, int B)
{
	srand(GetTickCount());
	array = new int[max_size];
	this->size = size < max_size ? size : max_size;

	for (int i = 0; i < this->size; ++i)
		array[i] = A + rand() % (B - A);

	number = ++counter;

}

DArray::DArray(int max_size, std::istream& stream)
{
	array = new int[max_size];
	stream >> size;
	for (int i = 0; i < size; ++i)
		stream >> array[i];
	number = ++counter;
}

int DArray::get_elem(int index)
{
	int result{};
	if (index >= 0 && index < size)
		result = array[index];

	return result;
}

void DArray::set_elem(int elem, int index)
{
	if (index >= 0 && index < size)
		array[index] = elem;
}
