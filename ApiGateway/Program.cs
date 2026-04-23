using Ocelot.DependencyInjection;
using Ocelot.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// 1. Cargar el archivo de configuración de Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// 2. Inyectar los servicios de Ocelot
builder.Services.AddOcelot(builder.Configuration);

WebApplication app = builder.Build();

// 3. Usar el middleware de Ocelot
await app.UseOcelot();

// Asignar el puerto 4000 al Gateway
app.Run("http://localhost:4000");