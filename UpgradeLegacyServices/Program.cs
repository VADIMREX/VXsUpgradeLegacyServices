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

    app.Use(async (context, next) => {
        Console.WriteLine($$"""
        Incoming request:
        - Path: {{context.Request.Path}}
        """);

        Endpoint? endpoint = context.GetEndpoint();
 
        if (endpoint != null) Console.WriteLine("- No endpoint");

        await next();
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseLegacyAsp();

app.Run();
