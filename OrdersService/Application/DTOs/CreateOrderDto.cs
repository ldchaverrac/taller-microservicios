namespace OrdersService.Application.DTOs;

public class CreateOrderDto {
  public string UserId { get; set; } = string.Empty;
  public List<OrderItemRequestDto> Items { get; set; } = [];
}