using Microsoft.AspNetCore.Mvc;

[ApiController]
public class StringFormatController : ControllerBase
{
    private readonly TaskService _taskService = null!;

    public StringFormatController(TaskService taskService)
    {
        _taskService = taskService;
    }

	[ProducesResponseType(typeof(TaskResultDTO), StatusCodes.Status200OK)]
    [HttpGet("/task/{input}")]
    public async Task<IActionResult> FormatString([FromRoute] string input)
    {
		TaskResultDTO result = null;

		Thread.Sleep(10000);
		
        try
        {
            result = await _taskService.FormatStringTask(input);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);

        }
        return Ok(result);
    }
}
