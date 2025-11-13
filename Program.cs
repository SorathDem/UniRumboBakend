using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text.Json.Serialization;
using UniRumbo.Services;
using UniRumbo.Services.Interfaces;
using UniRumbo.Repositories;

var builder = WebApplication.CreateBuilder(args);

// === CONTROLADORES ===
builder.Services.AddControllers()
    // 👇 Esto le dice al runtime que también cargue los controladores del ensamblado UniRumbo.Controllers
    .AddApplicationPart(typeof(UniRumbo.Controllers.ReportesController).Assembly)
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });


// === CONFIGURAR CORS ===
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000" , "https://frontunirumbo.onrender.com") // Puertos del frontend
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // ✅ Permitir envío de cookies o headers de autorización
    });
});

// === Entity Framework (DbContext) ===
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UniRumboDb")));

// === Conexión para Dapper ===
// ⚠️ Este bloque es lo que faltaba: registra IDbConnection para los servicios como RutasService
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("UniRumboDb");
    return new SqlConnection(connectionString);
});

// === Inyección de dependencias ===
builder.Services.AddHttpClient();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<AlojamientoRepository>();
builder.Services.AddScoped<AlojamientoService>();

builder.Services.AddScoped<IRutasService, RutasService>();
builder.Services.AddScoped<ISolicitudesService, SolicitudesService>();

// === Swagger ===
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UniRumbo API", Version = "v1" });
});

// === PIPELINE ===
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "UniRumbo API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

// ✅ CORS debe ir antes de Authorization
app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();


app.Run();
