Console.WriteLine("Write a sentence:");
string our_sentence = Console.ReadLine();
string pathToFile = "/home/nastia/for_new_projects/editorr/words_list.txt";
string[] separators = {" ", ".", ",", "!", "?", ":", ";"};
string[] words = our_sentence.Split(separators, StringSplitOptions.RemoveEmptyEntries); // якщо у нас пробіл не один або кома не одна, то він нам видалить ці пробіли і коми, а не зарахує як дод. елемент
string[] words_in_file = File.ReadAllLines(pathToFile);
foreach (string word in words)
{
    bool found = false;
    foreach (string word_f in words_in_file)
    {
        if (word_f.ToLower() == word.ToLower())
        {
            found = true;
            break;
        }
    }

    if (!found)
    {
        Console.WriteLine($"Looks like you have a typos in next words:{word} ");
    }
}