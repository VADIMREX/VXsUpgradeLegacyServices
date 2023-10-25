using LegacyMockLib;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Use(request => {
    Console.WriteLine($$"""
    Incoming request:
    - Target: {{request.Target?.GetType().Name ?? "null"}}
    - Method: {{request.Method?.Name ?? "null"}}
    """);
    return request;
});

app.UseLegacyAsp();

app.Run();
