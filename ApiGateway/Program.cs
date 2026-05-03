using SharedLibrary;

    var builder = WebApplication.CreateBuilder(args);

// RabbitMQ Helper'ı DI Container'a ekle
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "API Gateway is running!");

app.Run();
