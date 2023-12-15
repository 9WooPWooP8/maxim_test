public class Tests
{
    public sealed class DefaultHttpClientFactory : IHttpClientFactory, IDisposable
    {
        private readonly Lazy<HttpMessageHandler> _handlerLazy = new(() => new HttpClientHandler());

        public HttpClient CreateClient(string name) => new(_handlerLazy.Value, disposeHandler: false);

        public void Dispose()
        {
            if (_handlerLazy.IsValueCreated)
            {
                _handlerLazy.Value.Dispose();
            }
        }
    }

    private TaskService _taskService;

    [SetUp]
    public void Setup()
    {
        var httpClientFactory = new DefaultHttpClientFactory();
        var taskConfiguration = new TaskConfiguration();
        taskConfiguration.ApiUrl = "http://www.randomnumberapi.com";
        taskConfiguration.BlackList = new List<string>() { "asd" };

        _taskService = new TaskService(httpClientFactory, taskConfiguration);
    }

    [Test]
    public void TaskStringWithInvalidSymbolsThrowsException1()
    {
        Assert.ThrowsAsync<Exception>(async () => await _taskService.FormatStringTask("AAAA"));
    }
    [Test]
    public void TaskStringWithInvalidSymbolsThrowsException2()
    {
        Assert.ThrowsAsync<Exception>(async () => await _taskService.FormatStringTask("jjkksjHJss"));
    }
    [Test]
    public async Task TaskStringIsFormattedCorrectly1()
    {
        var result = await _taskService.FormatStringTask("a");
        Assert.That(result.FormattedString, Is.EqualTo("aa"));
    }
    [Test]
    public async Task TaskStringIsFormattedCorrectly2()
    {
        var result = await _taskService.FormatStringTask("abc");
        Assert.That(result.FormattedString, Is.EqualTo("cbaabc"));
    }
    [Test]
    public async Task TaskStringIsFormattedCorrectly3()
    {
        var result = await _taskService.FormatStringTask("abcd");
        Assert.That(result.FormattedString, Is.EqualTo("badc"));
    }
    [Test]
    public async Task TaskStringLongestSubstringIsCorrect1()
    {
        var result = await _taskService.FormatStringTask("a");
        Assert.That(result.LongestSubstring, Is.EqualTo("aa"));
    }
    [Test]
    public async Task TaskStringLongestSubstringIsCorrect2()
    {
        var result = await _taskService.FormatStringTask("abc");
        Assert.That(result.LongestSubstring, Is.EqualTo("aa"));
    }
    [Test]
    public async Task TaskStringLongestSubstringIsCorrect3()
    {
        var result = await _taskService.FormatStringTask("abecu");
        Assert.That(result.LongestSubstring, Is.EqualTo("ucebaabecu"));
    }
    [Test]
    public async Task FormattedStringIsSortedCorrectly()
    {
        var result = await _taskService.FormatStringTask("abecu");
        Assert.That(result.SortedFormattedString, Is.EqualTo("aabbcceeuu"));
    }
}
