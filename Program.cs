using Microsoft.EntityFrameworkCore;
using BusinessManagement.Database;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Profiles;
using BusinessManagementApi.Extensions;
using BusinessManagementApi.Models;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddDbContext<ApplicationContext>(
    options => options.UseNpgsql("Host=localhost;Port=5432;Username=root;Password=root;Database=dev_business_management")
);

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));
// builder.Services.AddTransient<DatabaseSeeder>();
builder.Services.AddAuthentication();
builder.Services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<ApplicationContext>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
    Title = "Business Management API",
    Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[]{}
        }
    });
});

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBusinessInfoRepository, BusinessInfoRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddAutoMapper(typeof(BusinessManagementProfile));

//Inject the MediatR to oun DI
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies((typeof(Program)).Assembly));

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.ConfigureExtensionHandler());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.MapIdentityApi<User>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

