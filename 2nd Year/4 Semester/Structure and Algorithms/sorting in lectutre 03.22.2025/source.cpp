#include<fstream>
#include<iostream>

int readNext(std::ifstream& file, bool& eof)
{
	int elem{};
	if (!file.eof())
		file >> elem;
	else
		eof = true;
	return elem;
}

int main()
{
	std::ifstream f1("f1.txt");
	std::ifstream f2("f2.txt");
	std::ofstream f0("f0.txt");
	bool eof1{}, eof2{};
	

	int x{}, y{};
	/*f1 >> x;
	f2 >> y;*/
	x = readNext(f1, eof1);
	y = readNext(f2, eof2);


	//while (!f1.eof() && !f2.eof())
	while(!eof1 &&!eof2)
	{
		//f1 >> x; //current it is not avaliable to write like this.
		//f2 >> y; //current it is not avaliable to write like this.
		// 
		if (x < y)
		{
			f0 << x << ' ';
			//f1 >> x;
			x = readNext(f1, eof1);
		}
		else
		{
			f0 << y << ' ';
			//f2 >> y;
			y = readNext(f2, eof2);
		}
	}
	//while (!f1.eof())
	while(!eof1)
	{
		f0 << x << ' ';
		//f1 >> x;
		x = readNext(f1, eof1);
	}
	//while (!f2.eof())
	while(!eof2)
	{
		f0 << y << ' ';
		//f2 >> y;
		y = readNext(f2, eof2);
	}
	f1.close();
	f2.close();
	f0.close();
	return 0;
}
