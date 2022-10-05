using cartshopping.webapi.Business.Abstract;
using cartshopping.webapi.Entity.ViewModels;
using cartshopping.webapi.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cartshopping.webapi.Controllers
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
    
        [HttpGet("Get")]
        public async Task<ActionResult<ResultViewModel>> GetAsync()
        {
            ResultViewModel result = new ResultViewModel();
            var resultProduct = await _productService.GetAllAsync();
            if (resultProduct.IsSuccess)
            {
                result.IsSuccess = true;
                result.Result = resultProduct.Result;
            }
            else { 
                result.IsSuccess = false; 
                result.Message = resultProduct.Message;
            }
            return Ok(result);
        }
        [HttpPost("Create")]
        public async Task<ActionResult<ResultViewModel>> Create(Product product)
        {
            ResultViewModel result = new ResultViewModel();
            var resultProduct = await _productService.CreateAsync(product);
            result.IsSuccess = resultProduct.IsSuccess;
            result.Result = resultProduct.Result;
            return Ok(result);
        }

        [HttpGet("Get/{id:length(24)}")]
        public async Task<ActionResult<Product>> Get(string id)
        {
            var result = await _productService.FindAsync(id);
            return Ok(result);
        }
        [HttpPut("Update/{id:length(24)}")]
        public async Task<ActionResult<ResultViewModel>> UpdateAsync(string id, Product product)
        {
            var productResult = await _productService.FindAsync(id);

            if (productResult is null)
            {
                return NotFound();
            }

            var result = await _productService.UpdateAsync(product);

            return Ok(result);
        }

        [HttpDelete("Delete/{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var resultFind = await _productService.FindAsync(id);

            if (!resultFind.IsSuccess)
            {
                return NotFound();
            }

            var result = await _productService.RemoveAsync(id);

            return Ok(result);
        }

    }
}
