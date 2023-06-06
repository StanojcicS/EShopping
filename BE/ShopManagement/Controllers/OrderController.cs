using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopManagement.Common.Models.Inbound;
using ShopManagement.Services.Contracts;

namespace ShopManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpPost]
        [Route("")]
        [Authorize(Roles = "Buyer")]
        public async Task<ActionResult> CreateNewOrder([FromBody] NewOrder order)
        {
            order.UserId = User.FindFirst("UserId").Value;
            return Ok(await _orderService.CreateNewOrder(order));
        }
    }
}
