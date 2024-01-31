using Microsoft.EntityFrameworkCore;
using BusinessManagement.Database;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Services;   
using BusinessManagementApi.Profiles;
using AutoMapper;
using BusinessManagementApi.Extensions;

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
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBusinessInfoRepository, BusinessInfoRepository>();
builder.Services.AddScoped<IBusinessInfoService, BusinessInfoService>();
builder.Services.AddAutoMapper(typeof(BusinessManagementProfile));

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.ConfigureExtensionHandler());
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

