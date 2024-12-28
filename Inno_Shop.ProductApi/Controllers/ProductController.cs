using Inno_Shop.BusinessLogic.Constants;
using Inno_Shop.BusinessLogic.Dto.Product;
using Inno_Shop.BusinessLogic.Services.ProductService;
using Inno_Shop.DataAccess.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inno_Shop.ProductApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(
    IProductService productService
    ) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] QueryObject query)
    {
        var products = await productService.GetAllProducts(query);
        return Ok(products);
    }
    
    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> GetProductInfo([FromRoute] Guid id)
    {
        var product = await productService.GetProductInfo(id.ToString());
        return Ok(product);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddProduct([FromBody] CreateProductDto product)
    {
        var prod = await productService.CreateProduct(product);
        return Ok(prod);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] UpdateProductDto product)
    {
        var prod = await productService.UpdateProduct(id.ToString(), product);
        return Ok(prod);
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
    {
        var res = await productService.DeleteProduct(id.ToString());
        return Ok(res);
    }
    
    [HttpPatch("user/{id:guid}/delete")]
    public async Task<IActionResult> BlockUserProducts([FromRoute] Guid id)
    {
        await productService.DeleteProductsByUserId(id.ToString());
        return Ok();
    }
}