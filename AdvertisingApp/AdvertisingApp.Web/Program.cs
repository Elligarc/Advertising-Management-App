using System.Text.Json.Serialization;
using AdvertisingApp.Core.Interfaces;
using AdvertisingApp.Core.Services;
using AdvertisingApp.Infrastructure.Data;
using AdvertisingApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
;

builder.Services.AddOpenApi();
// Подключение PostgreSQL через Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация универсального репозитория
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped(typeof(IClientService), typeof(ClientService));
builder.Services.AddScoped(typeof(IConstructionService), typeof(ConstructionService));
builder.Services.AddScoped(typeof(ISurfaceService), typeof(SurfaceService));
builder.Services.AddScoped(typeof(IContractService), typeof(ContractService));

// Настройка Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo { Title = "Advertising API", Version = "v1" });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()   // Разрешить запросы с любого источника
            .AllowAnyMethod()   // Разрешить любые методы (GET, POST, и т.д.)
            .AllowAnyHeader();  // Разрешить любые заголовки
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Advertising API v1"));
}

app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Инициализация начальных данных
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();
    await SeedData.InitializeAsync(context);
}

app.Run();