using System;

namespace task01;

public static class StringExtensions
{
        public static bool IsPalindrome(this string input)
    {
        string inputCleaned = "";

        foreach (char element in input.ToLower())
        {
            if (char.IsPunctuation(element) == false && char.IsWhiteSpace(element) == false)
            {
            inputCleaned += element;
            }
        }

        char[] temporaryCharArray = inputCleaned.ToCharArray();
        Array.Reverse(temporaryCharArray);
        string inputReversed = new string(temporaryCharArray);

        return inputCleaned != "" && inputCleaned == inputReversed;
    }
}
