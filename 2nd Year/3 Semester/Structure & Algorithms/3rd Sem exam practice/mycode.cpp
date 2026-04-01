#include "Binary_Tree.h"
#include "Trie_Tree.h"
#include<stack>
#include<queue>
int getLevel(btree::ptrNODE root, int target, int level) {

    if (root->info == target) {
        return level;
    }

    // Recursively call for left and right subtrees
    int leftLevel = getLevel(root->left, target, level + 1);
    if (leftLevel != -1) {
        return leftLevel;
    }

    return getLevel(root->right, target, level + 1);
}

int find_level(btree::ptrNODE root, int number, int level=0)
{
    if (root)
    {
        if (root->info == number)
            return level;
        if (number > root->info)
            find_level(root->right, number, level + 1);
        else
            find_level(root->left, number, level + 1);
    }
    else
        return -1;
}

int find_sum_level(btree::ptrNODE root, int target_level, int current_level = 0)
{
    if (root)
    {
        if (target_level == current_level)
            return root->info % 2 == 0 ? root->info : 0;
        find_sum_level(root->left, target_level, current_level + 1) +
            find_sum_level(root->right, target_level, current_level + 1);
    }
}

int find_sum_level_queue(btree::ptrNODE root, int target_level, int current_level = 0)
{
    std::queue<btree::ptrNODE> q;
    int result{};
    btree::ptrNODE ro{};
    q.push(root);
    while (!q.empty())
    {
        ro = q.front(); q.pop();
        if (target_level == current_level)
            result += root->info % 2 == 0 ? root->info : 0;
        else
            current_level++;
        if (ro->left)
            q.push(ro->left);
        if (ro->right)
            q.push(ro->right);

    }
    return result;
}

int find_sum_stack(btree::ptrNODE root, int target_level, int current_level = 0)
{
    std::stack<btree::ptrNODE>s;
    int result = 0;
    //btree::ptrNODE ro = root;
    while (root)
    {
        if (target_level == current_level)
        {
            result += root->info % 2 == 0 ? root->info : 0;
        }
        else
            current_level++;
        if (root->left)
        {
            if (root->right)
                s.push(root->right);
            root = root->left;
        }
        else
            if (root->right)
                root = root->right;
            else
            {
                if (s.empty())
                    root = nullptr;
                else
                {
                    root = s.top();
                    s.pop();
                }
            }
    }
    return result;

}
int find_sum_level_queue1(btree::ptrNODE root, int target_level)
{
    std::queue<btree::ptrNODE> q; // Queue for level-order traversal
    q.push(root);

    int current_level = 0;
    int result = 0;

    while (!q.empty())
    {
        
        int level_size = q.size(); // Number of nodes at the current level

        // Process all nodes at the current level
        for (int i = 0; i < level_size; i++)
        {
            btree::ptrNODE ro = q.front();
            q.pop();

            if (current_level == target_level) // Check if we're at the target level
            {
                if (ro->info % 2 == 0) // Check if the value is even
                    result += ro->info;
            }

            // Add left and right children to the queue
            if (ro->left)
                q.push(ro->left);
            if (ro->right)
                q.push(ro->right);
        }

        // Increment the level after processing all nodes at the current level
        if (current_level == target_level)
            break; // No need to continue if we've processed the target level

        current_level++;
    }

    return result;
}

bool list(btree::ptrNODE root)
{
    return (root->left == nullptr && root->right == nullptr) ? true : false;
}

void delet_leaf(btree::ptrNODE& root)
{
    if (root)
    {
        if (list(root))
        {
            delete root;
            root = nullptr;
            return;
        }
        delet_leaf(root->left);
        delet_leaf(root->right);
    }
}

int find_collection(btree::ptrNODE root)
{
    int result{};
    if (root)
    {
        result += root->info < 0 ? 1 : 0;
        result += find_collection(root->left);
        result += find_collection(root->right);
    }
    return result;
}
int find_collection_stack(btree::ptrNODE root)
{
    int result{};
    std::stack<btree::ptrNODE>s;
    while (root)
    {
        if (root->info < 0)
            result++;

        if (root->left)
        {
            if (root->right)
                s.push(root->right);
            root = root->left;
        }
        else
            if (root->right)
                root = root->right;
            else
            {
                if (s.empty())
                    root = nullptr;
                else
                {
                    root = s.top();
                    s.pop();
                }
            }
    }
    return result;
}

int find_collection_level(btree::ptrNODE root, int level, int current_level=0)
{
    int result{};
    if (root)
    {
        if (root->info < 0 && level == current_level)
            result++;
        result += find_collection_level(root->left, level, current_level + 1);
        result += find_collection_level(root->right, level, current_level + 1);
    }
    return result;
}
//int find_collection_level_stack(btree::ptrNODE root, int level, int currentlevel = 0)
//{
//    int result{};
//    std::stack<std::pair<btree::ptrNODE, int>>s;
//
//    while (root)
//    {
//        if (root->info < 0 && level == currentlevel)
//            result++;
//        if (root->left)
//        {
//            if (root->right)
//                s.push({ root->right,1 });
//            root = root->left;
//        }
//        else
//            if (root->right)
//                root = root->right;
//            else
//            {
//                if (s.empty())
//                    root = nullptr;
//                else
//                {
//                    root = s.top({ root,level });
//                    s.pop();
//                }
//            }
//    }
//    return result;
//}
int find_collection_level_stack(btree::ptrNODE root, int level)
{
    int currentlevel;
    int result = 0;
    std::stack<std::pair<btree::ptrNODE, int>> s;
    s.push({ root, 0 }); 

    while (!s.empty())
    {
        root = s.top().first;
        currentlevel = s.top().second;
        s.pop();
        if (currentlevel == level && root->info < 0)
        {
            result++;
        }
        if (root->right)
        {
            s.push({ root->right, currentlevel + 1 });
        }
        if (root->left)
        {
            s.push({ root->left, currentlevel + 1 });
        }
    }

    return result;
}

int max_number(btree::ptrNODE root)
{
    
    if (root)
    {
        int result = root->info;
        int left = max_number(root->left);
        int right = max_number(root->right);
        if (result < left)
            result = left;
        if (result < right)
            result = right;
        return result;
    }
}
int find_collection_level_stack_sum(btree::ptrNODE root, int level )
{
    int result{};
    int cur_le = 0;
    std::queue < std::pair<btree::ptrNODE, int>>st;
    st.push({ root, 0 });
    while (!st.empty())
    {
        root = st.front().first;
        cur_le = st.front().second;
        st.pop();
        if (root->info % 2 != 0 && cur_le == level)
            result += root->info;
        if (root->left)
            st.push({ root->left, cur_le + 1 });
        if (root->right)
            st.push({ root->right, cur_le + 1 });
    }
    return result;

}
//int find_leftmost()
int levelCount_number(btree::ptrNODE root, int n, int m, int level=0 )
{
    int result{};
  /*  int cur_level{};
    std::stack<std::pair<btree::ptrNODE, int>>st;
    st.push({ root, 0 });
    while (!st.empty())
    {
        root = st.top().first;
        cur_level = st.top().second;
        st.pop();
        if (cur_level >= n && cur_level <= m)
            result++;
        if (root->left)
            st.push({ root->left, cur_level + 1 });
        if (root->right)
            st.push({ root->right, cur_level + 1 });
    }*/
    if (root)
    {
        if (level >= n && level <= m)
            result++;
        result += levelCount_number(root->left, n, m, level + 1);
        result += levelCount_number(root->right, n, m, level + 1);
    }
    return result;
}

btree::ptrNODE make_tree(int n, int level)
{
    if (!(level>n))
    {
        btree::ptrNODE node = new btree::NODE(level);
        node->left = make_tree(n, level + 1);
        node->right = make_tree(n, level + 1);
        return node;
    }
}
btree::ptrNODE create_tree(int currentLevel, int maxDepth) {
    if (currentLevel > maxDepth) return nullptr;

    btree::ptrNODE node = new btree::NODE(maxDepth - currentLevel + 1);

    node->left = create_tree(currentLevel + 1, maxDepth);
    node->right = create_tree(currentLevel + 1, maxDepth);

    return node;
}

btree::ptrNODE creattree(int cur_le, int defest)
{
    if (cur_le > defest)
        return nullptr;
    btree::ptrNODE node = new btree::NODE(cur_le);
    node->left = creattree(cur_le + 1, defest);
    node->right = creattree(cur_le + 1, defest);

    return node;
}

btree::ptrNODE copy_tree(btree::ptrNODE root)
{
    if (!root)
        return nullptr;
    if (list(root))
        return nullptr;
    btree::ptrNODE newNode = new btree::NODE(root->info);

    // Recursively copy the left and right subtrees
    newNode->left = copy_tree(root->left);
    newNode->right = copy_tree(root->right);

    return newNode;

}

int find_number_level(btree::ptrNODE node, int E, int depth = 0)
{
    if (!node) return -1; 

    if (node->info == E) return depth; 
    int left_result = find_number_level(node->left, E, depth + 1);
    if (left_result != -1) return left_result; 
    int right_result = find_number_level(node->right, E, depth + 1);
    return right_result; 
}
int find_number_level_stack(btree::ptrNODE tree, int E)
{
    std::stack<std::pair<btree::ptrNODE, int>>st;
    int level{};
    st.push({ tree, 0 });

    while (!st.empty())
    {
        tree = st.top().first;
        level = st.top().second;
        st.pop();
        if (tree->info == E)
            return level;
        if (tree->left)
            st.push({ tree->left,level + 1 });
        if (tree->right)
            st.push({ tree->right, level + 1 });
    }
    return -1;
}

int find_number_level11(btree::ptrNODE root, int number)
{
    int result{};
    int level{1};
    std::stack<std::pair<btree::ptrNODE, int>>st;
    st.push({ root,1 });
    while (!st.empty())
    {
        root = st.top().first;
        level = st.top().second;
        st.pop();
        if (number == root->info)
            result = level;
        if (root->left)
            st.push({ root->left,level + 1 });
        if (root->right)
            st.push({ root->right  ,level + 1 });
    }
    return result;
}

btree::ptrNODE copy_withoutleaves(btree::ptrNODE root)
{
    if (root)
    {
        if (root->right == nullptr && root->left == nullptr)
            return nullptr;

        btree::ptrNODE newnode = new btree::NODE(root->info);
        newnode->left = copy_withoutleaves(root->left);
        newnode->right = copy_withoutleaves(root->right);
        return newnode;
        
    }
}
int sum_of_descendents(btree::ptrNODE root)
{
    if (root)
    {
        int result{};
        if (root->left && root->right)
            result = 1;
        return result + sum_of_descendents(root->left) + sum_of_descendents(root->left);
    }
}

ttree::ptrNODE copytrie(ttree::ptrNODE root)
{
    ttree::ptrNODE result{};
    if (root)
    {
        result = new ttree::NODE();
        result->eow = root->eow;
        for (int i{}; i < 26; ++i)
            if (root->ptrs[i])
                result->ptrs[i] = copytrie(root->ptrs[i]);
    }
    return result;
}

ttree::ptrNODE copy_witha(ttree::ptrNODE root) 
{
    ttree::ptrNODE result{};
    if (root)
    {
        result = new ttree::NODE();
        result->eow = root->eow;
        for (int i = 0; i < 26; ++i)
        {
            if (root->ptrs[i])
                result->ptrs[i] = copy_witha(root->ptrs[i] + 'a');
        }
    }
    return result;
}


void delete_elem_level(btree::ptrNODE root, int curlevel, int targetlevel)
{
    if (root)
    {
        if (curlevel == targetlevel - 1)
        {
            delete root->left;
            delete root->right;
            root->left = root->right = nullptr;
        }
        delete_elem_level(root->left, curlevel + 1, targetlevel);
        delete_elem_level(root->right, curlevel + 1, targetlevel);

    }
}

void delete_elem_level_stack(btree::ptrNODE root, int targetleavel)
{
    std::stack<std::pair<btree::ptrNODE, int>>st;
    int current_level = 0;
    st.push({ root,0 });

    while (!st.empty())
    {
        root = st.top().first;
        current_level = st.top().second;
        st.pop();
        if (current_level == targetleavel - 1)
        {
            delete root->left;
            delete root->right;
            root->left = root->right = nullptr;
        }
        if (root->left)
            st.push({ root->left, current_level + 1 });
        if (root->right)
            st.push({ root->right, current_level + 1 });
    }
}

int branch_sum(btree::ptrNODE root)
{
    if (!root) 
        return 0;

    std::stack<std::pair<btree::ptrNODE, int>> st; 
    int totalSum{}; 

    st.push({ root, 0 }); 

    while (!st.empty())
    {
        root = st.top().first;
        int currentSum = st.top().second;
        st.pop();
        currentSum += root->info;

        if (root->info % 2 != 0)
        {
            totalSum += currentSum - root->info; 
            continue; 
        }

        if (!root->left && !root->right)
        {
            totalSum += currentSum;
            continue;
        }
        if (root->right)
            st.push({ root->right, currentSum });
        if (root->left)
            st.push({ root->left, currentSum });
    }

    return totalSum; 
}

ttree::ptrNODE copytrie11(ttree::ptrNODE root)
{
    if (!root)
        return nullptr;

    ttree::ptrNODE result = new ttree::NODE();
    result->eow = root->eow;

    for (int i = 0; i < 26; ++i)
    {
        if (root->ptrs[i]) 
        {
            result->ptrs[i] = copytrie11(root->ptrs[i]);
        }
    }

    if (result->eow)
    {
        int index = 'a' - 'a'; 
        if (!result->ptrs[index]) 
        {
            result->ptrs[index] = new ttree::NODE();
            result->ptrs[index]->eow = true;
        }
    }

    return result; 
}

int task2_sum(btree::ptrNODE root)
{
    std::stack<btree::ptrNODE> st; 
    int result{};                 

    while (root )
    {
        result += (root->info % 2 == 0 ? root->info : 0);
        if (root->left)
        {
            if (root->right)
                st.push(root->right);
            root = root->left;
        }
        else
            if (st.empty())
                root = nullptr;
            else
            {
                root = st.top();
                st.pop();
            }
    }

    return result;

}

void delete_oddnumber(btree::ptrNODE root)
{
    if (root)
    {
        if (root->info % 2 == 0)
        {
            delete root->left;
            delete root->right;
            root->left = root->right = nullptr;
        }
        delete_oddnumber(root->left);
        delete_oddnumber(root->right);
    }
}
int task2(btree::ptrNODE root, int targetlevel)
{
    int result{};
    int cur_level = 0;
    std::queue<std::pair<btree::ptrNODE, int >>que;
    que.push({ root,0 });
    while (!que.empty())
    {
        root = que.front().first;
        cur_level = que.front().second;
        que.pop();

        if (cur_level >= targetlevel)
        {
            result += root->info;
        }
        if (root->left)
            que.push({ root->left, cur_level + 1 });
        if (root->right)
            que.push({ root->right, cur_level + 1 });
       
    }
    return result;
}

ttree::ptrNODE copy_and_remove_last(ttree::ptrNODE node, const std::string& word = "")
{
    if (!node) return nullptr;

    ttree::ptrNODE new_node = new ttree::NODE();
    if (node->eow && !word.empty())
    {
        new_node->eow = true;
        std::cout << "Adding shortened word: " << word << std::endl;
    }

    for (int i = 0; i < 26; ++i)
    {
        if (node->ptrs[i])
            new_node->ptrs[i] = copy_and_remove_last(node->ptrs[i], word + char('a' + i));
    }

    return new_node;
}

void add_zero_layer(btree::ptrNODE t) {
    if (!t) return; 

    if (!t->left ) {
        t->left = new btree::NODE(0);  
    }
    if(!t->right)
        t->right = new btree::NODE(0);
    if (!t->left && !t->right)
    {
        t->left = new btree::NODE(0);
        t->right = new btree::NODE(0);
    }
    else {
        if (t->left) add_zero_layer(t->left);
        if (t->right) add_zero_layer(t->right);
    }
}

//int sumUntilFirstEven(btree::ptrNODE root) {
//    if (root == nullptr) {
//        return 0;
//    }
//
//    std::stack<std::pair<btree::ptrNODE, int>> s;
//    s.push({ root, root->info }); 
//
//    int totalSum = 0;
//
//    while (!s.empty()) {
//        root = s.top().first;
//        int currentSum = s.top().second;
//        s.pop();
//
//        if (root->info % 2 == 0) {
//            totalSum += currentSum;
//        }
//        else {
//            if (root->left != nullptr) {
//                s.push({ root->left, currentSum + root->left->info });
//            }
//            if (root->right != nullptr) {
//                s.push({ root->right, currentSum + root->right->info });
//            }
//        }
//    }
//
//    return totalSum;
//}

int untilFirstEven(btree::ptrNODE root)
{
    int result{};
    std::stack<btree::ptrNODE>st;
    while (root)
    {
        if (root->info % 2 == 0)
            return result;
        else
            result += root->info;
        if (root->left)
        {
            if (root->right)
                st.push(root->right);
            root = root->right;
        }
        else
            if (root->right)
                root = root->right;
            else
            {
                if (st.empty())
                    root = nullptr;
                else
                {
                    root = st.top();
                    st.pop();
                }
            }
    }
    return result;
}

int findSumToOdd(btree::ptrNODE root)
{
    if (!root)
        return 0;
    std::stack<btree::ptrNODE>st;
    st.push(root);
    int sum{};
    while (!st.empty())
    {
        bool flag = false;
        btree::ptrNODE current = st.top();
        st.pop();
        sum += current->info;
        if (current->info % 2 == 0)
        {
            flag = true;
        }

        if (current->left && !flag)
            st.push(current->left);
        if (current->right && !flag)
            st.push(current->right);

    }
    return sum;
}
int sum_chet(btree::ptrNODE t)
{
    int res{};
    std::stack<btree::ptrNODE> st;
    if (t->info % 2 == 1) st.push(t);
    while (!st.empty())
    {
        btree::ptrNODE cur = st.top();
        st.pop();
        res += cur->info;
        if (cur->right != nullptr && cur->right->info % 2 == 1)
            st.push(cur->right);
        if (cur->left != nullptr && cur->left->info % 2 == 1)
            st.push(cur->left);
    }

    return res;
}

void add_zero_lvl(btree::ptrNODE node)
{
    if (!node) return;

    else if (!node->left && !node->right)
    {
        node->left = new btree::NODE(0);
        node->right = new btree::NODE(0);
    }
    else
    {
        add_zero_lvl(node->left); add_zero_lvl(node->right);
    }
}

int main()
{
    btree::BTREE tree("file.txt");
	tree.print();
    std::cout << "\n";
   // std::cout << find_level(tree.get_root(), 10) << "\n";
  //  std::cout << find_sum_level(tree.get_root(), 1) << "\n";
    //std::cout << branch_sum(tree.get_root()) << "\n";
   // std::cout << find_sum_level_queue1(tree.get_root(),2) << "\n";
    //std::cout << find_collection_level(tree.get_root(), 2) << "\n";
    //delet_leaf(tree.get_root());
    //std::cout << find_collection_level_stack_sum(tree.get_root(), 3) << "\n";
   // std::cout << delete_oddnumber(tree.get_root(),) << "\n";
    //add_zero_layer(tree.get_root());
   // tree.print();
    //tree.print();
    //std::cout << find_number_level(tree.get_root(), -15) << "\n";
    //std::cout << task2(tree.get_root(), 2) << "\n";
   /* tree.set_root(copy_withoutleaves(tree.get_root()));
    tree.print();*/
    add_zero_lvl(tree.get_root());
    tree.print();
    //std::cout << untilFirstEven(tree.get_root()) << "\n";
    /*ttree::TTREE Trie_tree("file1.txt");
    Trie_tree.print(true);
    std::cout << "\n";*/

    //ttree::TTREE copied_trie;
    //std::cout << "----------------- \n";
    //copied_trie.set_root(copy_and_remove_last(Trie_tree.get_root()));
    //copied_trie.print(true);

    /*ttree::TTREE modified_trie;

    modified_trie.set_root(copy_and_remove_last(Trie_tree.get_root()));

    std::cout << "\nModified Trie Words (last character removed):\n";
    modified_trie.print(true);*/
    
    /*delete_elem_level(tree.get_root(), 0, 2);
    tree.print();
    std::cout << "----------------- \n";
    delete_elem_level_stack(tree.get_root(), 2);
    tree.print();*/
    //std::cout << "----------------- \n";
    //branch_sum_to_first_odd(tree.get_root());
    /*Trie_tree.set_root(copytrie11(Trie_tree.get_root()));
    Trie_tree.print(true);*/
    /*ttree::TTREE trie("file1.txt");
    trie.print(true);*/


   /* 101
        50
        121
        30
        60
        110
        130
        20
        32
        55
        70
        106
        115
        125
        150*/
   

	return 0;
}