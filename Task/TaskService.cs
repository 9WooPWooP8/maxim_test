using System.Text.Json;

public class TaskService
{
    private readonly IHttpClientFactory _httpClientFactory = null!;
    private readonly TaskConfiguration _taskConfiguration = null!;

    public TaskService(IHttpClientFactory httpClientFactory, TaskConfiguration taskConfiguration)
    {
        _httpClientFactory = httpClientFactory;
        _taskConfiguration = taskConfiguration;
    }

    public async Task<TaskResultDTO> FormatStringTask(string input)
    {
        var taskResult = new TaskResultDTO();

        if (input == null || input == "")
            throw new Exception("empty string received");

        var validationResult = ValidateInput(input);

        if (validationResult.Count > 0)
        {
            throw new Exception($"invalid symbols found: {String.Join(",", validationResult.Distinct())}");
        }

		if (!ValidateInputBlacklist(input)){
			throw new Exception("word is in blacklist!");
		}

        string result = FormatString(input);

        taskResult.FormattedString = result;

        var charsCount = CountSymbols(result);

        taskResult.SymbolCountResult = charsCount;
        taskResult.LongestSubstring = FindLongestSubstring(result);

        var resultCharArray = result.ToCharArray();
        Quicksort(resultCharArray, 0, result.Length - 1);
        taskResult.SortedFormattedString = string.Join("", resultCharArray);

        var randomIndex = -1;
        try
        {
            randomIndex = await GetRandomFromAPI(0, result.Length - 1);
        }
        catch
        {
            Console.WriteLine("Error occurred while getting random number from api");
            Random rnd = new Random();
            randomIndex = rnd.Next(result.Length);
        }

        var shortenedResult = result.Remove(randomIndex, 1);
        taskResult.ShortenedFormattedString = shortenedResult;

        return taskResult;
    }

    private string FormatString(string input)
    {
        if (input.Length % 2 == 0)
        {
            var firstHalf = input.Substring(0, input.Length / 2);
            var secondHalf = input.Substring(input.Length / 2, input.Length / 2);

            return ReverseString(firstHalf) + ReverseString(secondHalf);
        }
        else
        {
            return ReverseString(input) + input;
        }

    }

    private async Task<int> GetRandomFromAPI(int lowerBound, int higherBound)
    {
        using HttpClient httpClient = this._httpClientFactory.CreateClient();

        string url = $"{_taskConfiguration.ApiUrl}/api/v1.0/random?min={lowerBound}&max={higherBound}";

        var response = await httpClient.GetAsync(url);

        var responseString = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<int>>(responseString)[0];
    }

    string ReverseString(string input)
    {
        var charArray = input.ToCharArray();
        Array.Reverse(charArray);
        return new String(charArray);
    }

    List<Char> ValidateInput(string input)
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

    bool ValidateInputBlacklist(string input)
    {
        return !_taskConfiguration.BlackList.Contains(input);
    }

    Dictionary<char, int> CountSymbols(string input)
    {
        var charCount = input.GroupBy(c => c)
                             .OrderBy(c => c.Key)
                             .ToDictionary(g => g.Key, g => g.Count());

        return charCount;
    }

    string FindLongestSubstring(string input)
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

    private void Quicksort(char[] input, int low, int high)
    {
        int pivotIndex = 0;

        if (low < high)
        {
            pivotIndex = Partition(input, low, high);
            Quicksort(input, low, pivotIndex - 1);
            Quicksort(input, pivotIndex + 1, high);
        }
    }

    private int Partition(char[] input, int low, int high)
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
