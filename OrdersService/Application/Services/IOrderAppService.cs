using OrdersService.Application.DTOs;
using OrdersService.Models;

namespace OrdersService.Application.Services;

public interface IOrderAppService
{
    Task<Order> CreateOrderAsync(CreateOrderDto dto);
    Task<Order[]> GetAllOrdersAsync();
    Task<Order?> GetOrderByIdAsync(int id);
    Task<bool> DeleteOrderAsync(int id);
}