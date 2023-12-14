public class ParallelCountMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ParallelConfiguration _parallelConfiguration;
	private static int parallelRequestCount = 0;

	public ParallelCountMiddleware(RequestDelegate next, ParallelConfiguration parallelConfiguration)
	{
		_next = next;
		_parallelConfiguration = parallelConfiguration;
	}

	public async Task Invoke(HttpContext httpContext)
	{
		if (parallelRequestCount >= _parallelConfiguration.MaxParallelRequests){
			return;
		}

		parallelRequestCount += 1;
		await _next(httpContext);
		parallelRequestCount -= 1;
	}
}

public static class CustomMiddlewareExtensions
{
	public static IApplicationBuilder UseParallelCountMiddleware(this IApplicationBuilder builder)
	{
		return builder.UseMiddleware<ParallelCountMiddleware>();
	}
}
