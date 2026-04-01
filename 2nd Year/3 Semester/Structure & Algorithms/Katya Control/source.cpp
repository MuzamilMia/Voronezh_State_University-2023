#include"BTree.h"
#include<stack>
//task1--------------
ptrNODE task1(int n, int current_level) {
    if (current_level > n) {
        return nullptr;
    }

    ptrNODE node = new NODE(current_level);

    node->left = task1(n, current_level + 1);
    node->right = task1(n, current_level + 1);

    return node;
}


//task2--------------------
int my_fun(ptrNODE node, int cur_lev, int tar_lev) {
    if (node) {

        if (cur_lev == tar_lev) {
            return (node->info % 2 == 0 ? node->info : 0); 
        }

        return my_fun(node->left, cur_lev + 1, tar_lev) + my_fun(node->right, cur_lev + 1, tar_lev);
    }
}

int Task2(BTREE& tree, int target_level) {
    return my_fun(tree.get_root(), 1, target_level);
}



//int main()
//{
//    std::ifstream file("file.txt");
//    if (file)
//    {
//        BTREE tree("file.txt",true); // , false - дерево поиска 
//        tree.print();
//        std::cout << "------------------------ \n";
//        tree.set_root(task1(4, 1));
//        tree.print();
//        std::cout << "------------------------ \n";
//        std::cout << Task2(tree, 2);
//
//        // std::cout << "------------------------";
//    }
//    else
//        std::cout << "error\n";
//    std::cin.get();
//}

int task3(BTREE& tree) {
 

    std::stack<ptrNODE> nodeStack; 
    ptrNODE currentNode = tree.get_root();
    int maxLeafValue = INT_MIN; 

    nodeStack.push(currentNode); 

    while (!nodeStack.empty()) {
        currentNode = nodeStack.top();
        nodeStack.pop();
        if (currentNode && !currentNode->left && !currentNode->right) {
            if (currentNode->info > maxLeafValue) {
                maxLeafValue = currentNode->info;
            }
        }

        if (currentNode->left) {
            nodeStack.push(currentNode->left);
        }
        if (currentNode->right) {
            nodeStack.push(currentNode->right);
        }
    }

    return maxLeafValue;
}

int main()
{
    std::ifstream file("file.txt");
    if (file)
    {
        BTREE tree("file.txt"); // , false - дерево поиска 
        BTREE tree2("file.txt");
        BTREE tree3("file.txt");
        tree.print();
        std::cout << "\n------------1------------\n";
        tree.set_root(task1(4, 1));
        tree.print();
        std::cout << "\n------------2------------\n";
        std::cout << " sum " << Task2(tree2, 4);
        std::cout << "\n------------3------------\n";
        std::cout << task3(tree3);
        // std::cout << " Maximum " << find_max_leaf_value(tree);
    }
    else
        std::cout << "error\n";
    std::cin.get();
}