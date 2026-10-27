using Montreal.Application.Common;
using Montreal.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();
app.MapGet("/ping", async (IMessagePublisher publisher) =>
{
    await publisher.PublishAsync("test-queue", new { Hello = "world" });
    return "ok";
});

app.Run();
