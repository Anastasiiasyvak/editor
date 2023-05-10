Console.WriteLine("Write a sentence:");
string our_sentence = Console.ReadLine();
string[] separators = { " ", ",", ".", "!", "?", ":", ";" };
string[] our_words = our_sentence.Split(separators, StringSplitOptions.RemoveEmptyEntries);
string PathToFile = "/home/nastia/for_new_projects/editorr/words_list.txt";
string[] file_words = File.ReadAllLines(PathToFile);
List<string> incorrect_words = new List<string>();
Dictionary<string, List<string>> word_replacement = new Dictionary<string, List<string>>();
foreach (string word in our_words)
{
    bool found = false;
    foreach (string word_f in file_words)
    {
         if (word.ToLower() == word_f.ToLower())
        {
            found = true;
            break;
        }
    }

    if (!found)
    {
        incorrect_words.Add(word);
        List<string> possible_words = PossibleWords(word, file_words);
        word_replacement[word] = possible_words;
    }
    
}

if (incorrect_words.Count > 0)
{
    Console.WriteLine($"Looks like you have typos in next words: {string.Join(",", incorrect_words)}");
    foreach (string word in incorrect_words)
    {
        if (word_replacement.TryGetValue(word, out List<string> possible_words)) // якщо ми маємо word у word_repl. і за допомогою out ми вносимо можливі слова до possible words
        {
            Console.WriteLine($"Word suggestions for {word}: {string.Join(",", possible_words)}");
        }
    }
}


List<string> PossibleWords(string word, string[] file_words)
{
    List<string> possible_words = new List<string>();
    int MaxDistance = int.MaxValue;
    foreach (string word_f in file_words)
    {
        if (word.ToLower() == word_f.ToLower())
        {
            possible_words.Clear();
            possible_words.Add(word_f);
            return possible_words;
        }
        int distance = LevenshteinDistance(word, word_f);
        if (distance < MaxDistance)
        {
            possible_words.Clear();
            possible_words.Add(word_f);
            MaxDistance = distance;
        }
        else if (distance == MaxDistance && possible_words.Count <= 5)
        {
            possible_words.Add(word_f);
        }

        if (Math.Abs(word.Length - word_f.Length) == 0 && ChangedOrder(word, word_f))
        {
            possible_words.Add(word_f);
        }
    }

    return possible_words;
}
bool ChangedOrder(string first_word, string second_word)
{
    if (first_word.Length != second_word.Length || first_word.Length < 2)
    {
        return false;
    }

    int WordLength = first_word.Length;
    int transpositions = 0;
    int i = 0;
    while (i < WordLength && first_word[i] == second_word[i])
    {
        i++;
    }

    if (i == WordLength)
    {
        return false;
    }

    
    while (i < WordLength)
    {
        if (first_word[i] != second_word[i])
        {
            int s = i + 1;
            while (s < WordLength && first_word[s] != second_word[i])
            {
                s++;
            }

            if (s == WordLength)
            {
                return false;
            }
            
            char swap_char = first_word[i];
            first_word = first_word.Remove(i, 1).Insert(i, second_word[i].ToString());
            first_word = first_word.Remove(s, 1).Insert(s, swap_char.ToString());
            transpositions++;
        }

        i++;
    }

    return transpositions == 1;
}

int LevenshteinDistance(string row, string column)
{
    int[,] matrix = new int[row.Length + 1, column.Length+1];
    for (int i = 0; i <= row.Length; i++)
    {
        matrix[i, 0] = i;
    }
    for (int s = 0; s <= column.Length; s++)
    {
        matrix[0, s] = s;
    }
    for (int i = 1; i <= row.Length; i++)
    {
        for (int s = 1; s <= column.Length; s++)
        {
            int cost = (char.ToLower(row[i - 1]) == char.ToLower(column[s - 1])) ? 0 : 1;
            matrix[i, s] = Math.Min(Math.Min(matrix[i - 1, s] + 1, matrix[i, s - 1] + 1), matrix[i - 1, s - 1] + cost);
        }
    }

    return matrix[row.Length, column.Length];
}



