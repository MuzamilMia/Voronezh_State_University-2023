#include"Source.h"

//4. Дано Trie - дерево.Посчитать количество слов, которые содержат определенное количество гласных.

bool is_vowel(char c) {
    const std::string vowels = "aeiou";
    for (char v : vowels) {
        if (c == v) {
            return true;
        }
    }
    return false;
}

//My task. 

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

int main()
{
    std::ifstream file("file.txt");
    if (file)
    {
        ttree::TTREE tree("file.txt");
        tree.print(true);
        ttree::ptrNODE root = tree.get_root();
        std::cout << "\n------------------------------\n";

        int result = count_words_with_vowels(tree.get_root(), "", 2);
        std::cout << "The result for the vowels is: " << result << '\n';
   

    }
    std::cin.get();
    return 0;
}