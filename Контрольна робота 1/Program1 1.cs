using System;


namespace Program1
{
    public static class StringExtensions
    {

        public static bool IsPalindrome(this string str)
        {
            var cleaned = new string(str.Where(char.IsLetterOrDigit).ToArray()).ToLower();
            return cleaned.SequenceEqual(cleaned.Reverse());
        }
    }


    class Program
    {
        static void Main()
        {
            Console.WriteLine("Enter a sentence:");
            string input = Console.ReadLine().TrimEnd('.');
            var words = input.Split(' ');
            int palindromeCount = words.Count(word => word.IsPalindrome());
            string resultSentence = string.Join(" ", words.Where(word => !word.IsPalindrome())) + ".";

            Console.WriteLine($"Number of palindrome words: {palindromeCount}");
            Console.WriteLine($"Sentence without palindromes: {resultSentence}");
        }
    }
}








