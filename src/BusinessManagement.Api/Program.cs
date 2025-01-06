using BusinessManagement.DAL;
using Microsoft.EntityFrameworkCore;
using BusinessManagement.Database;
using BusinessManagement.Templates;
using BusinessManagementApi.Extensions;
using BusinessManagementApi.Models;
using ContosoUniversity.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(
    options => options.UseNpgsql("Host=localhost;Port=5432;Username=root;Password=root;Database=dev_business_management")
);


builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));
builder.Services.AddAuthentication();
builder.Services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<ApplicationContext>()
    .AddUserManager<UserManager<User>>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
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

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IInvoiceDocumentBuilder, InvoiceDocumentBuilder>();
builder.Services.AddScoped<ISalesOrderBuilder, SalesOrderBuilder>();

//Inject the MediatR to oun DI
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies((typeof(Program)).Assembly));

var app = builder.Build();

QuestPDF.Settings.License = LicenseType.Community;
QuestPDF.Settings.EnableDebugging = true;


app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.ConfigureExtensionHandler());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// TODO: this is temporary during dev phase
app.UseCors("AllowAll");
app.UseSerilogRequestLogging();

app.MapIdentityApi<User>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }