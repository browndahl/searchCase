using Shared;
using SearchAPI.Database;
using SearchAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var redisConnectionString = builder.Configuration["Redis:ConnectionString"];

if (!string.IsNullOrEmpty(redisConnectionString))
{
    Console.Write("Initializing Redis Cache");
    builder.Services.AddSingleton<ICacheService, RedisCacheService>();
}
else
{
    Console.WriteLine("Initializing In Memory Cache");
    builder.Services.AddSingleton<ICacheService, InMemoryCacheService>();
}

builder.Services.AddSingleton<IDatabase, Database>();
builder.Services.AddSingleton<ConfigModel>();

//  CORS-politik for at tillade anmodninger fra Blazor
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin() // This should be changed to a single port once we get k8s up and running
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(); //Activating Cors

app.UseAuthorization();

app.MapControllers();

app.Run();