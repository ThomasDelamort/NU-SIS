var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Server is up and running!");

app.MapGet("/health-check", () => "Server is healthy!");

app.Run();
