namespace maxim_test;

class Program
{
    static void Main(string[] args)
    {
        var input = Console.ReadLine();
        if (input == null || input == "") return;

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

        Console.WriteLine("main result: {0}", result);
        Console.WriteLine("longest substring: {0}", FindLongestSubstring(result));
        var resultCharArray = result.ToCharArray();
        Quicksort(resultCharArray, 0, result.Length - 1);
        Console.WriteLine("sorted result (Quicksort): {0}", string.Join("", resultCharArray));

        var binarySearchTree = new BinarySearchTree(result);
        var BSTSortResult = binarySearchTree.Traverse();
		BSTSortResult.Reverse();

        Console.WriteLine("sorted result (Tree sort): {0}", string.Join("", BSTSortResult));
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

    static string FindLongestSubstring(string input)
    {
        string substringSymbols = "aeiouy";

        int startIndex = -1;
        int endIndex = -1;

        int i = -1;
        foreach (var character in input)
        {
            i++;
            if (!substringSymbols.Contains(character))
                continue;

            if (startIndex == -1)
            {
                startIndex = i;
                continue;
            }

            endIndex = i;
        }

        if (startIndex == -1)
            return "";

        if (endIndex == -1)
            return input[startIndex].ToString();

        return input.Substring(startIndex, endIndex - startIndex + 1);
    }

    private static void Quicksort(char[] input, int low, int high)
    {
        int pivotIndex = 0;

        if (low < high)
        {
            pivotIndex = Partition(input, low, high);
            Quicksort(input, low, pivotIndex - 1);
            Quicksort(input, pivotIndex + 1, high);
        }
    }

    private static int Partition(char[] input, int low, int high)
    {
        int pivot = input[high];
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (input[j] <= pivot)
            {
                i++;
                var tempValue = input[j];
                input[j] = input[i];
                input[i] = tempValue;
            }
        }

        var temp = input[i + 1];
        input[i + 1] = input[high];
        input[high] = temp;
        return i + 1;
    }



}


public class BinarySearchTree
{
    public BinarySearchTree(string input)
    {
        foreach (var c in input)
        {
            this.Insert(c);
        }

    }

    Node? root;

    public class Node
    {
        public char val;
        public Node? left, right;

        public Node(char val)
        {
            this.val = val;
        }

        public void Insert(char newVal)
        {
            if (this.val <= newVal)
            {
                if (this.left == null)
                {
                    this.left = new Node(newVal);
                }
                else
                {
                    this.left.Insert(newVal);
                }
            }
            else
            {
                if (this.right == null)
                {
                    this.right = new Node(newVal);
                }
                else
                {
                    this.right.Insert(newVal);
                }
            }
        }
    }

    public void Insert(char val)
    {
        if (root == null)
        {
            root = new Node(val);
            return;
        }

        root.Insert(val);
    }

    public List<char> Traverse()
    {
        var traversalList = new List<char>();

        TraverseTree(traversalList, root);

        return traversalList;
    }

    private void TraverseTree(List<char> traversalList, Node? node = null)
    {
        if (node == null){
            return;
		}

        TraverseTree(traversalList, node.left);
        traversalList.Add(node.val);
        TraverseTree(traversalList, node.right);
        return;
    }
}
