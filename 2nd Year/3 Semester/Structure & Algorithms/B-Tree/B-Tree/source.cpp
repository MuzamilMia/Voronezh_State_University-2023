#include<iostream>	
#include<fstream>	
#include<string>	
#include<winddi.h>

const int n = 2;
struct Page;

struct Elem
{
	int key;
	int count;
	Page* ptr;
	void init(int key) { this->key = key; count = 1; ptr = nullptr; }
};
struct Page 
{
	int m{};
	Page* ptr0;
	Elem arr[2 * n + 1];
	Page():ptr0(nullptr), m(0){}
	bool isFull() { return m == 2 * m; }
};
struct BTree
{
public:
	Page* root;
	BTree() { root = new Page(); }

	void add(int key) {
		Page* r = root;
		if(r->isFull())
		{
			Page* s = new Page();
			s->ptr0 = r;
			splitChild(s, 0, r);
			root = s;
			insertChild
		}
	}

};
#include<iostream>
#include<fstream>
#include<string>

const int n = 2;  // B-tree order
struct Page;

// Element in the page
struct Elem
{
    int key;      // key value
    int count;    // count of occurrences (can be used if keys can repeat)
    Page* ptr;    // pointer to the child page (right child)

    // Initialize the element
    void init(int key) {
        this->key = key;
        count = 1;
        ptr = nullptr;
    }
};

// Page in the B-tree
struct Page
{
    int m{};          // Number of elements in the page
    Page* ptr0;       // Pointer to the leftmost child (left child)
    Elem arr[2 * n + 1];  // Array of elements, can hold up to 2n+1 elements temporarily

    Page() : ptr0(nullptr), m(0) {}

    bool isFull() { return m == 2 * n; }  // Check if the page is full
};

// B-tree
class BTree {
public:
    Page* root;

    BTree() {
        root = new Page();  // Start with an empty page
    }

    // Insert a key into the B-tree
    void insert(int key) {
        Page* r = root;
        if (r->isFull()) {  // If root is full, split it
            Page* s = new Page();
            s->ptr0 = r;
            splitChild(s, 0, r);
            root = s;
            insertNonFull(s, key);
        }
        else {
            insertNonFull(r, key);
        }
    }

private:
    // Split the child 'y' of page 'x' at index 'i'
    void splitChild(Page* x, int i, Page* y) {
        Page* z = new Page();  // Create a new page to hold the second half of y's elements
        z->m = n;

        // Copy the last n elements of y into z
        for (int j = 0; j < n; j++) {
            z->arr[j] = y->arr[j + n + 1];
        }

        if (y->ptr0 != nullptr) {
            for (int j = 0; j <= n; j++) {
                z->arr[j].ptr = y->arr[j + n + 1].ptr;
            }
        }

        y->m = n;

        // Shift elements of x to make space for the new element
        for (int j = x->m; j >= i + 1; j--) {
            x->arr[j + 1] = x->arr[j];
        }

        // Link the new page z to x
        x->arr[i].ptr = z;

        // Move the middle element of y into x
        x->arr[i] = y->arr[n];

        // Increment the number of elements in x
        x->m++;
    }

    // Insert a key into a non-full page
    void insertNonFull(Page* x, int key) {
        int i = x->m - 1;  // Start from the rightmost element

        if (x->ptr0 == nullptr) {  // If x is a leaf page
            // Shift elements to the right to make space for the new key
            while (i >= 0 && x->arr[i].key > key) {
                x->arr[i + 1] = x->arr[i];
                i--;
            }

            // Insert the new key
            x->arr[i + 1].init(key);
            x->m++;
        }
        else {  // x is an internal node
            // Find the child to recurse into
            while (i >= 0 && x->arr[i].key > key) {
                i--;
            }
            i++;

            if (x->arr[i].ptr != nullptr && x->arr[i].ptr->isFull()) {
                // If the child is full, split it
                splitChild(x, i, x->arr[i].ptr);

                // After split, decide where to insert the key
                if (key > x->arr[i].key) {
                    i++;
                }
            }

            // Recursively insert into the child
            insertNonFull(x->arr[i].ptr, key);
        }
    }
};

// Test the B-tree implementation
int main() {
    BTree bTree;

    // Inserting keys into the B-tree
    bTree.insert(10);
    bTree.insert(20);
    bTree.insert(5);
    bTree.insert(6);
    bTree.insert(12);
    bTree.insert(30);
    bTree.insert(7);
    bTree.insert(17);

    // Since we have no direct traversal yet, we can add one or debug to see the tree structure
    // In this example, we focus on the insertion process.
    return 0;
}
