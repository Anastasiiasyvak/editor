// bool Subsequence(string s, string t)
// {
//     int S = 0;
//     int T = 0;
//
//     if (s.Length > t.Length)
//     {
//         return false;
//     }
//     
//     while (S < s.Length && T < t.Length)
//     {
//         if (s[S] == t[T])
//         {
//             S++;
//         }
//         T++;
//     }
//
//     return S == s.Length;
// }
//
// string s = Console.ReadLine()!;
// string t = Console.ReadLine()!;
//
// var subsequence = Subsequence(s, t);
//
// Console.WriteLine(subsequence);