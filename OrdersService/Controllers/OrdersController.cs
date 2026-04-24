using Microsoft.AspNetCore.Mvc;
using OrdersService.Application.DTOs;
using OrdersService.Application.Services;

namespace OrdersService.Controllers;

[ApiController]
[Route("orders")]
public class OrdersController(IOrderAppService orderService) : ControllerBase {
  [HttpPost]
  public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto) {
    try {
      var order = await orderService.CreateOrderAsync(dto);
      return StatusCode(201, order);
    } catch (Exception ex) {
      return BadRequest(new { message = ex.Message });
    }
  }

  [HttpGet]
  public async Task<IActionResult> GetAllOrders() {
    var orders = await orderService.GetAllOrdersAsync();
    return Ok(orders);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetOrderById(int id) {
    var order = await orderService.GetOrderByIdAsync(id);
    if (order == null) return NotFound(new { message = $"Pedido con ID {id} no encontrado." });
    return Ok(order);
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteOrder(int id) {
    var deleted = await orderService.DeleteOrderAsync(id);
    if (!deleted) return NotFound(new { message = $"Pedido con ID {id} no encontrado." });
    return NoContent();
  }
}