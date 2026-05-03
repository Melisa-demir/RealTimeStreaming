using StreamingService.Hubs;
using SharedLibrary;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(sp =>
{
    return new RabbitMqHelper(
        hostname: "localhost",
        username: "guest",
        password: "guest"
        );
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var app = builder.Build();
app.MapGet("/", () => "Streaming Service is running!");
app.MapHub<StreamingHub>("/streamingHub");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
