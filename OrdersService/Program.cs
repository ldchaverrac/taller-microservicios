using Microsoft.EntityFrameworkCore;
using OrdersService.Application.Services;
using OrdersService.Persistence;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient();

builder.Services.AddScoped<IOrderAppService, OrderAppService>();

WebApplication app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    db.Database.Migrate();
}

app.UseAuthorization();
app.MapControllers();

app.Run("http://+:7000");