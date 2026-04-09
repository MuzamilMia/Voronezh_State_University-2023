
//Recursive Approach. 
//#include <iostream>
//
//int binarySearch(int a[], int item, int left, int right)
//{
//	if (right <= left)
//		return (item > a[left]) ? (left + 1) : left;
//
//	int mid = (left + right) / 2;
//
//	if (item == a[mid])
//		return mid + 1;
//
//	if (item > a[mid])
//		return binarySearch(a, item, mid + 1, right);
//	return binarySearch(a, item, left, mid - 1);
//}
//
//void insertionSort(int a[], int n)
//{
//	int  loc, j, k, selected;
//
//	for (int i = 1; i < n; ++i)
//	{
//		j = i - 1;
//		selected = a[i];
//
//		// find location where selected should be inserted
//		loc = binarySearch(a, selected, 0, j);
//
//		// Move all elements after location to create space
//		while (j >= loc)
//		{
//			a[j + 1] = a[j];
//			j--;
//		}
//		a[j + 1] = selected;
//	}
//}
//void print(int arr[], int size)
//{
//	for (int i = 0; i < size; ++i)
//		std::cout << arr[i] << "  ";
//	std::cout << '\n';
//}
//
//int main()
//{
//	int arr[] = { 3,7,1,4,6,2,5 };
//	int size = sizeof(arr) / sizeof(arr[0]);
//	std::cout << "Before the sorting: \n";
//	print(arr, size);
//
//	insertionSort(arr, size);
//
//	std::cout << "Sorted array: \n";
//	print(arr, size);
//
//	return 0;
//}


//Iterative Approach
#include <iostream>

int binarySearch(int a[], int item, int left, int right)
{
	while (left <= right) {
		int mid = left + (right - left) / 2;
		if (item == a[mid])
			return mid + 1;
		else if (item > a[mid])
			left = mid + 1;
		else
			right = mid - 1;
	}

	return left;
}

void insertionSort(int a[], int size)
{
	int loc, j, k, selected;

	for (int i = 1; i < size; ++i) {
		j = i - 1;
		selected = a[i];

		// find location where selected should be inserted
		loc = binarySearch(a, selected, 0, j);

		// Move all elements after location to create space
		while (j >= loc) {
			a[j + 1] = a[j];
			j--;
		}
		a[j + 1] = selected;
	}
}

void print(int arr[], int size)
{
	for (int i = 0; i < size; ++i)
		std::cout << arr[i] << "  ";
	std::cout << "\n";
}

int main()
{
	int arr[] = { 3,7,1,4,6,2,5 };
	int size = sizeof(arr) / sizeof(arr[0]);

	std::cout << "Before the Sorting: \n";
	print(arr, size);

	insertionSort(arr, size);

	std::cout << "Sorted array: \n";
	print(arr, size);

	return 0;
}
