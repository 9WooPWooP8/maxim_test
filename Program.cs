namespace maxim_test;

class Program
{
    static void Main(string[] args)
    {
        var input = Console.ReadLine();
        if (input == null) return;

        var validationResult = ValidateInput(input);

        if (validationResult.Count > 0)
        {
            Console.WriteLine("invalid symbols found in input: {0}", String.Join("", validationResult));
            return;
        }

        string? result = null;

        if (input.Length % 2 == 0)
        {
            var firstHalf = input.Substring(0, input.Length / 2);
            var secondHalf = input.Substring(input.Length / 2, input.Length / 2);

            result = ReverseString(firstHalf) + ReverseString(secondHalf);
        }
        else
        {
            result = ReverseString(input) + input;
        }

		var charsCount = CountSymbols(result);

        foreach (KeyValuePair<char, int> charCount in charsCount)
        {
			Console.WriteLine("{0}: {1}", charCount.Key, charCount.Value);
        }

        Console.WriteLine(result);
    }

    static string ReverseString(string input)
    {
        var charArray = input.ToCharArray();
        Array.Reverse(charArray);
        return new String(charArray);
    }

    static List<Char> ValidateInput(string input)
    {
        var invalidSymbols = new List<Char>();
        foreach (var character in input.ToCharArray())
        {
            if (!(character >= 'a' && character <= 'z'))
            {
                invalidSymbols.Add(character);
            }
        }

        return invalidSymbols;
    }

    static Dictionary<char, int> CountSymbols(string input)
    {
        var charCount = input.GroupBy(c => c)
                             .OrderBy(c => c.Key)
                             .ToDictionary(g => g.Key, g => g.Count());

        return charCount;
    }


}

