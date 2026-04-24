using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using OrdersService.Application.DTOs;
using OrdersService.Models;
using OrdersService.Persistence;

namespace OrdersService.Application.Services;

public class OrderAppService(OrderDbContext context, IHttpClientFactory httpClientFactory) : IOrderAppService {
  private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
  private const string UsersServiceUrl = "http://localhost:3000";
  private const string ProductsServiceUrl = "http://localhost:5000";

  public async Task<Order> CreateOrderAsync(CreateOrderDto dto) {
    var client = httpClientFactory.CreateClient();

    var userResponse = await client.GetAsync($"{UsersServiceUrl}/users/{dto.UserId}");
    if (!userResponse.IsSuccessStatusCode) throw new Exception($"Usuario con ID '{dto.UserId}' no encontrado.");

    var productsResponse = await client.GetAsync($"{ProductsServiceUrl}/products");
    productsResponse.EnsureSuccessStatusCode();
    var products = await productsResponse.Content.ReadFromJsonAsync<List<ProductDto>>(JsonOptions) ?? [];

    decimal total = 0;
    var items = new List<OrderItem>();

    foreach (var itemDto in dto.Items) {
      var product = products.FirstOrDefault(p => p.Id == itemDto.ProductId) ?? throw new Exception($"Producto con ID {itemDto.ProductId} no encontrado o sin stock.");
      if (product.Stock < itemDto.Quantity) throw new Exception($"Stock insuficiente para '{product.Name}'. Disponible: {product.Stock}, solicitado: {itemDto.Quantity}.");
      total += product.Price * itemDto.Quantity;
      items.Add(new OrderItem { ProductId = itemDto.ProductId, Quantity = itemDto.Quantity });
    }

    var order = new Order { UserId = dto.UserId, CreatedAt = DateTime.UtcNow, TotalAmount = total, Items = items };

    context.Orders.Add(order);
    await context.SaveChangesAsync();

    foreach (var itemDto in dto.Items) await client.PatchAsJsonAsync($"{ProductsServiceUrl}/products/{itemDto.ProductId}/stock", new { quantity = itemDto.Quantity });

    return order;
  }

  public async Task<Order[]> GetAllOrdersAsync() {
    return await context.Orders.Include(o => o.Items).ToArrayAsync();
  }

  public async Task<Order?> GetOrderByIdAsync(int id) {
    return await context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
  }

  public async Task<bool> DeleteOrderAsync(int id) {
    var order = await context.Orders.FindAsync(id);
    if (order == null) return false;
    context.Orders.Remove(order);
    await context.SaveChangesAsync();
    return true;
  }
}