
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient();

var randomApi = builder.Configuration.GetValue<string>("RandomApi");
var blacklist = builder.Configuration
                       .GetSection("Settings:BlackList")
                       .Get<List<string>>();
var taskConfig = new TaskConfiguration()
{
    ApiUrl = randomApi!,
    BlackList = blacklist!
};

var maxParallelRequests = builder.Configuration
                       .GetSection("Settings:ParallelLimit")
                       .Get<int>();

var parallelConfig = new ParallelConfiguration()
{
	MaxParallelRequests = maxParallelRequests
};


builder.Services.AddSingleton<TaskConfiguration>(taskConfig);
builder.Services.AddSingleton<ParallelConfiguration>(parallelConfig);
builder.Services.AddScoped<TaskService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}"
);
app.UseParallelCountMiddleware();

await app.RunAsync();
