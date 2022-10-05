using cartshopping.webapi.Business.Abstract;
using cartshopping.webapi.Entity.ViewModels;
using cartshopping.webapi.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cartshopping.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IUserService _userService;
        public CartController(ICartService cartService,IUserService userService)
        {
            _cartService = cartService; 
            _userService = userService;
        }
        [HttpGet("Get")]
        public async Task<ActionResult<ResultViewModel<CartViewModel>>> GetAsync()
        {
            ResultViewModel<CartViewModel> result = new ResultViewModel<CartViewModel>();

            var resultUser = await _userService.FindAsync("resuloglumurad@gmail.com", "123.Mr");

            if (resultUser.IsSuccess)
            {
                var resultCart =await _cartService.GetCartByUserId(resultUser.Result.Id);
                if(resultCart.IsSuccess)
                {
                    result = resultCart;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = resultCart.Message;
                    result.Result = resultCart.Result;
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Kullanıcı bulunamadı";
            }
            return Ok(result);
        }
    }
}
