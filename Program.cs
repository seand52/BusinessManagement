using Microsoft.EntityFrameworkCore;
using BusinessManagement.Database;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

// add postgres cofiguration to DI

builder.Services.AddDbContext<ApplicationContext>(
    options => options.UseNpgsql("Host=localhost;Port=5432;Username=root;Password=root;Database=dev_business_management")
);

// builder.Services.AddTransient<DatabaseSeeder>();
builder.Services.AddControllers();
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

app.Run();
