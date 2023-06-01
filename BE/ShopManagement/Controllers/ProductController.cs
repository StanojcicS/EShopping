using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopManagement.Common.Models.Inbound;
using ShopManagement.Services.Contracts;

namespace ShopManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "Seller")]
        public async Task<ActionResult> AddNewProduct([FromForm] NewProduct product)
        {
            product.SellerId = User.FindFirst("UserId").Value;
            return Ok(await _productService.AddNewProduct(product));
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _productService.GetAll());
        }

        [HttpGet]
        [Route("/seller/products")]
        [Authorize(Roles = "Seller")]
        public async Task<ActionResult> GetAllBySeller()
        {
            var userId = User.FindFirst("UserId").Value;
            return Ok(await _productService.GetAllBySellerId(userId.ToString()));
        }

        [HttpPut]
        [Route("products/{productId}/update")]
        [Authorize(Roles = "Seller")]
        public async Task<ActionResult> Update([FromForm] UpdateProduct product)
        {
            product.SellerId = User.FindFirst("UserId").Value;
            return Ok(await _productService.Update(product));
        }

        [HttpDelete]
        [Route("products/{productId}/delete")]
        [Authorize(Roles = "Seller")]
        public async Task<ActionResult> Delete(DeleteProduct product)
        {
            product.SellerId = User.FindFirst("UserId").Value;
            return Ok(await _productService.Delete(product));
        }
    }
}
