class Program
{
    static void Main()
    {
        Console.WriteLine("Write a sentence:");
        string our_sentence = Console.ReadLine();
        string pathToFile = "/home/nastia/for_new_projects/editorr/words_list.txt";
        string[] separators = { " ", ".", ",", "!", "?", ":", ";" };
        string[] words = our_sentence.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        string[] words_in_file = File.ReadAllLines(pathToFile);
        List<string> incorrectWords = new List<string>();
        Dictionary<string, List<string>> wordSuggestions = new Dictionary<string, List<string>>();

        foreach (string word in words)
        {
            bool found = false;
            foreach (string word_f in words_in_file)
            {
                if (string.Equals(word_f, word, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                incorrectWords.Add(word);

                // Find suggestions for the incorrect word
                List<string> suggestions = FindWordSuggestions(word, words_in_file);
                wordSuggestions[word] = suggestions;
            }
        }

        if (incorrectWords.Count > 0)
        {
            Console.WriteLine($"Looks like you have typos in the following words: {string.Join(", ", incorrectWords)}.");

            // Display word suggestions
            foreach (string word in incorrectWords)
            {
                if (wordSuggestions.TryGetValue(word, out List<string> suggestions))
                {
                    Console.WriteLine($"Suggestions for '{word}': {string.Join(", ", suggestions)}");
                }
            }
        }
    }

    static List<string> FindWordSuggestions(string word, string[] words)
    {
        List<string> suggestions = new List<string>();
        int maxDistance = int.MaxValue;

        foreach (string word_f in words)
        {
            int distance = CalculateLevenshteinDistance(word, word_f);
            if (distance < maxDistance)
            {
                suggestions.Clear();
                suggestions.Add(word_f);
                maxDistance = distance;
            }
            else if (distance == maxDistance && suggestions.Count < 5)
            {
                suggestions.Add(word_f);
            }
        }

        return suggestions;
    }

    static int CalculateLevenshteinDistance(string a, string b)
    {
        int[,] dp = new int[a.Length + 1, b.Length + 1];

        for (int i = 0; i <= a.Length; i++)
            dp[i, 0] = i;

        for (int j = 0; j <= b.Length; j++)
            dp[0, j] = j;

        for (int i = 1; i <= a.Length; i++)
        {
            for (int j = 1; j <= b.Length; j++)
            {
                int cost = (a[i - 1] == b[j - 1]) ? 0 : 1;
                dp[i, j] = Math.Min(Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1), dp[i - 1, j - 1] + cost);
            }
        }

        return dp[a.Length, b.Length];
    }
}
