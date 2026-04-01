#include"Trie_Tree.h"

//void print_words(ttree::ptrNODE root, std::string sub_st, int i)
//{
//    if (root && root->ptrs[sub_st[i] - 'a'])
//    {
//        if (i < sub_st.length())
//            print_words(root->ptrs[sub_st[i] - 'a'], sub_st, i + 1);
//        else
//            ttree::printW(root, sub_st);
//    }
//}

//bool is_vowel(char c)
//{
//    return c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u';
//}
//
//int count_vowel(ttree::ptrNODE root, int cur_vowel)
//{
//    int count{};
//    if (root)
//    {
//        if (count > 5)
//            return 0;
//        if (root->eow)
//            ++count;
//        for (int i = 0; i < 26; ++i)
//        {
//            if (root->ptrs[i])
//                count += count_vowel(root->ptrs[i], cur_vowel + (is_vowel('a' + i) ? 1 : 0));
//        }
//    }
//    return count;
//}


bool is_vowel(char c) {
    const std::string vowels = "aeiou";
    for (char v : vowels) {
        if (c == v) {
            return true;
        }
    }
    return false;
}

int count_words_with_vowels(ttree::ptrNODE root, std::string word, int target_vowels)
{

    if (root)
    {
        int count = 0;
        if (root->eow) 
        {
            int vowel_count = 0;
            for (char c : word) 
            {
                if (is_vowel(c)) 
                {
                    vowel_count++;
                }
            }
            if (vowel_count == target_vowels) 
            {
                count++;
                std::cout << word << "\n";
            }
        }

        for (int i = 0; i < 26; i++) {
            if (root->ptrs[i]) {
                count += count_words_with_vowels(root->ptrs[i], word + char('a' + i), target_vowels);
            }
        }
        
        return count;
    }
}
//---------------- in lab -------------------
void task3(ttree::TTREE &tree, ttree::ptrNODE &root, std::string word, int lvl)
{
    if (root)
    {
        if (lvl == word.length())
            tree.clear(root);
        else
        {
            //if(root->ptrs[word[lvl]-'a'])
            task3(tree, root->ptrs[word[lvl] - 'a'], word, lvl + 1);
            if (tree.all_prts_empty(root) && !root->eow)
            {
                delete root;
                root = nullptr;
            }

        }
    }
}

void task_3_sub(ttree::TTREE& tree, ttree::ptrNODE& root, std::string word, int lvl, bool& isDelete, ttree::ptrNODE beg)
{
    if (root)
    {
        if (lvl == word.length() - 1)
        {
            tree.clear(root);
            isDelete = true;
        }
        else
            for (size_t i = 0; i < 26; ++i)
            {
                if (root->ptrs[i])
                {
                    if (word[lvl] == char('a' + i))
                    {
                        if (lvl == 0)
                            beg = root->ptrs[i];
                        task_3_sub(tree, root->ptrs[i], word, lvl + 1, isDelete, beg);
                    }
                    else
                        task_3_sub(tree, root->ptrs[i], word, 0, isDelete, beg);
                    if(root->ptrs[i]==beg && isDelete)
                        task_3_sub(tree, root->ptrs[i], word, 0, isDelete, beg);
                }
            }
        if(isDelete)
        {

            if (tree.all_prts_empty(root) && !root->eow)
            {
                delete root;
            }
        }
    }
}

int main()
{
    std::ifstream file("file.txt");
    if (file)
    {
        ttree::TTREE tree("file.txt");
        tree.print(true);
        ttree::ptrNODE root = tree.get_root();
        std::cout << "\n------------------------------\n";

        //ttree::ptrNODE root = tree.get_root();
        ///*print_words(root, "ge", 0); */
        int result = count_words_with_vowels(tree.get_root(), "", 2);
        std::cout << "The result for the vowels is: " << result << '\n';
        //std::cout << "\n------------------------------ \n the delete \n";
        //task3(tree, root, "dog", 0);
        //task3(tree, root, "try", 0);
        //tree.print(true);
       
    }


    return 0;
}
//bool is_vowel(char c)
//{
//    return c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u';
//}
//
//// DFS function to count words with up to 5 vowels
//int count_words_with_vowels(ttree::ptrNODE root, int current_vowels)
//{
//    if (!root)
//        return 0;
//
//    // If vowels exceed 5, stop further processing
//    if (current_vowels > 5)
//        return 0;
//
//    int count = 0;
//
//    // If this is the end of a word and vowels are 5 or fewer, count it
//    if (root->eow)
//        count++;
//
//    // Recursively check each child
//    for (int i = 0; i < 26; i++)
//    {
//        if (root->ptrs[i])
//        {
//            char current_char = 'a' + i; // Get the current character
//            count += count_words_with_vowels(root->ptrs[i],
//                current_vowels + (is_vowel(current_char) ? 1 : 0));
//        }
//    }
//
//    return count;
//}