using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using UniRumbo.Backend.Services;
using UniRumbo.Backend.Services.Interfaces;
using UniRumbo.Repositories;
using UniRumbo.Services;
using UniRumbo.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// CORS para el front (Vite en 5173)
builder.Services.AddCors(p => p.AddPolicy("ui",
    c => c.WithOrigins("http://localhost:5173", "http://localhost:5174")
          .AllowAnyHeader()
          .AllowAnyMethod()));

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UniRumboDb")));

// Servicios existentes
builder.Services.AddScoped<IAuthService, AuthService>();

// 👇 NUEVOS servicios HU3
builder.Services.AddHttpClient(); // para OSRM/Nominatim
builder.Services.AddScoped<IRutasService, RutasService>();
builder.Services.AddScoped<ISolicitudesRutaService, SolicitudesRutaService>();

// 👇 Servicio de reportes PDF
builder.Services.AddScoped<IReportesService, ReportesService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UniRumbo API", Version = "v1" });
});

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

// 👇 habilita CORS del front
app.UseCors("ui");

app.UseAuthorization();
app.MapControllers();

app.Run();
