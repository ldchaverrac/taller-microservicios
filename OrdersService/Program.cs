using Microsoft.EntityFrameworkCore;
using OrdersService.Application.Services;
using OrdersService.Persistence;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Agregar Controladores
builder.Services.AddControllers();

// Configurar SQL Server
builder.Services.AddDbContext<OrderDbContext>();

// Configurar HttpClient para comunicarse con los otros microservicios
builder.Services.AddHttpClient();

// Registrar nuestra capa de aplicación
builder.Services.AddScoped<IOrderAppService, OrderAppService>();

WebApplication app = builder.Build();

app.UseAuthorization();
app.MapControllers();

app.Run("http://localhost:7000");