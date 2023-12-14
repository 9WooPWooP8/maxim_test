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


builder.Services.AddSingleton<TaskConfiguration>(taskConfig);
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

await app.RunAsync();
