using SharedLibrary;
using StreamingService.Consumers;
using StreamingService.Hubs;

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

builder.Services.AddSingleton(sp =>
{
    return new RabbitMqHelper(
        hostname: "localhost",
        username: "guest",
        password: "guest"
    );
});

builder.Services.AddHostedService<RabbitMqConsumer>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();
app.UseCors("AllowReact");
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
