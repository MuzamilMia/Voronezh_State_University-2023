#include<iostream>
#include<string>

//1 soution
/*const int dx[8] = {2,3,1,-1,-2,-2,-1,1};
const int dy[8] = { -1,1,2,2,1,-1,-2,-2 };
const int n = 5;

int a[n][n];
bool Try(int m, int x, int y)
{
	int i{ -1 }, u{}, v{};
	bool result{};
	do 
	{
		result = false;
		++i;
		u = x + dx[i];
		v = y + dy[i];
		if (u >= 0 && u < n && v >= 0 && v < n && a[u][v] == 0)
		{
			a[u][v] = m;
			if (m < n * n)
			{
				result = Try(m + 1, u, v);
				if (!result)
					a[u][v] = 0;
			}
			result = true;
		}
	} while (!result && i < 8);
	return result;
}

void init(int a[][n])
{ }
void print(int a[][n])
{ }
*/

//second soultion
/*
const int n = 5;
int x[n];
int y[n];

bool a[2 * n - 1]; // "\"
bool b[2 * n - 1]; // "\"


bool Try(int i)
{
	int j{ -1 };
	bool result{};
	do
	{
		++j;
		result = false;
		if (y[j] && a[i - j + n - 1] && b[i + j])
		{
			x[i] = j;
			y[j] = false;
			a[i - j + n - 1] = false;
			b[i + j] = false;
			if (i < n - 1)
			{
				result = Try(i + 1);
				if (!result)
				{
					y[j] = true;
					a[i - j + n - 1] = true;
					b[i + j] = true;
				}
			}
			else
				result = true;
		}

	} while (i < n && !result);
	return result;
}*/

//next way
const int n = 5;
int x[n];
int y[n];

bool a[2 * n - 1]; // "\"
bool b[2 * n - 1]; // "\"

bool result = false;

bool Try(int i)
{
	for()
	do
	{
		++j;
		result = false;
		if (y[j] && a[i - j + n - 1] && b[i + j])
		{
			x[i] = j;
			y[j] = false;
			a[i - j + n - 1] = false;
			b[i + j] = false;
			if(i < n - 1)
			{ 

			}
			else
			{
				result = true;
				//print solution 
			}
			
			y[j] = true;
			a[i - j + n - 1] = true;
			b[i + j] = true;
				
		}

	} while (i < n && !result);
}
void init(int a[][n])
{ }
void print(int a[][n])
{ }


//--------------------------------------------------

/// ---------------------   question 5 -----------------;

const int n = 5;
int map[n][n];
int road[n];
bool include[n];

void fill(int a[][n])
{ }
void print(int [][n])
{ }

bool result{ false };
void Try(int start, int finish, int i)
{
	if (start == finish)
	{
		result = true;
		///print solution (road)
	}
	else
	{
		for (int j{}; j < n; ++j)
		{
			if (map[start][j] != 0 && !include[j])
			{
				road[i] = j;
				include[j] = true;
				Try(j, finish, i + 1);
				include[j] = false;
			}
		}
	}
}
//---------------------------------------- question 5 fisnish---------------


// ----------------- OPtimal soultion for question 5 ------------------

const int n = 5;
int map[n][n];
int road[n];
int min_road[n];
bool include[n];
int len{ 0 }, min{ 0 };

void fill(int a[][n])
{
}
void print(int[][n])
{
}
void Try(int start, int finish, int i)
{
	if (start == finish)
	{
		min = len;
		//min_road = road;
	}
	else
	{
		for (int j{}; j < n; ++j)
		{
			if (map[start][j] != 0 && !include[j]&& (len+map[start][j]<min || min==0))
			{
				road[i] = j;
				include[j] = true; 
				len += map[start][j];
				Try(j, finish, i + 1);
				include[j] = false;
				len -= map[start][j];
			}
		}
	}
}
//  ------------------------- optimal soution 5 concluion ------------------------
int main()
{
	/*
	init(a);
	int x{ 3 }, y{ 3 };
	a[x][y] = 1;
	if (Try(2, x, y))
		print(a);
	else
		std::cout << "No solution \n";*/

	//-------- solution 2 -------------
	if (Try(0))
	{
		//print  x;
	}
	else
		std::cout << "NO solution";

	//---------- solution 

	Try(0);
	if (!result)
		std::cout << "no";

	// -----------------  question 5 --------------------;

	fill(map);
	int start{}, finsish{  };
	include[start] = true;
	road[0] = start;
	Try(start, finsish, 1);


	// --------------------------- finsish soultoin question 5 ---------------------

	//optimal solution question 5;
	fill(map);
	int start{}, finsish{  };
	include[start] = true;
	road[0] = start;
	Try(start, finsish, 1);
	if (!min)
		std::cout << "No";
	else
		//print the min. 
	//-----------------

	return 0;
}