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
        private readonly ICartItemService _cartItemService;
        private readonly IUserService _userService;
        public CartController(ICartService cartService,ICartItemService cartItemService,IUserService userService)
        {
            _cartService = cartService;
            _cartItemService = cartItemService;
            _userService = userService;

        }
        [HttpGet("GetCart")]
        public async Task<ActionResult<ResultViewModel<CartViewModel>>> GetCartAsync()
        {
            var username = Request.Cookies["username"];
            if (username ==null)
            {
                username ="resuloglumurad@gmail.com";
            }
            ResultViewModel<CartViewModel> result = new ResultViewModel<CartViewModel>();
            // User varlığının dpoğrulanması
            var resultUser =  _userService.GetByUsername(username);
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
                result.Message = "Login olmanız gerekiyor.";
                result.Url = "/login";
            }
            return Ok(result);
        }

        [HttpPost("AddToCart/{productId}/{quantity}")]
        public async Task<ActionResult<ResultViewModel>> AddToCart(string productId,int quantity)
        {
            ResultViewModel<CartViewModel> result = new ResultViewModel<CartViewModel>();
            var username = Request.Cookies["username"];
            var resultUser = _userService.GetByUsername(username);
            if (resultUser.IsSuccess)
            {
                // product daha önce eklenmişmi?
                var cartResult =await _cartService.GetCartByUserId(resultUser.Result.Id);
                if (cartResult.IsSuccess)
                {
                    var cartItemViewModel = cartResult.Result.CartItems.FirstOrDefault(x =>x.ProductId==productId);
                   
                    if (cartItemViewModel != null)
                    {
                        var cartItemResult = await _cartItemService.FindAsync(cartItemViewModel.CartItemId);
                        CartItem cartItem = cartItemResult.Result;
                        cartItem.Quantity += quantity;
                        var resultUpdate = await _cartItemService.UpdateAsync(cartItem);
                        result.IsSuccess = resultUpdate.IsSuccess;
                        result.Message = "Updated  box";
                    }
                    else
                    {
                        CartItem newCartItem = new CartItem()
                        {
                            CartId = cartResult.Result.Id,
                            ProductId = productId,
                        };
                        newCartItem.Quantity =  quantity;
                        var resultUpdate = await _cartItemService.CreateAsync(newCartItem);
                        result.IsSuccess = true;
                        result.Message = "Added box";
                    }
                }
                else
                {
                    var cartItem = new CartItem {
                        ProductId = productId,
                        CartId = cartResult.Result.Id,
                        Quantity =quantity,
                    };
                    var cartItemResult = await _cartItemService.CreateAsync(cartItem);
                    result.IsSuccess = cartItemResult.IsSuccess;
                    result.Message = cartItemResult.Message;
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "You need to be login";
                result.Url = "/User/Login";
            }
            return Ok(result);
        }

        [HttpPost("DeleteFromCart/{cartItemId}")]
        public async Task<ActionResult<ResultViewModel>> DeleteCartItem(string cartItemId)
        {
            ResultViewModel<CartViewModel> result = new ResultViewModel<CartViewModel>();
            var resultCartItem = await _cartItemService.RemoveAsync(cartItemId);
            result.IsSuccess = resultCartItem.IsSuccess;
            result.Message = result.Message;
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<ResultViewModel>> Create(Cart cart)
        {
            ResultViewModel result = new ResultViewModel();
            var resultCart = await _cartService.CreateAsync(cart);
            result.IsSuccess = resultCart.IsSuccess;
            result.Result = resultCart.Result;
            return Ok(result);
        }

        [HttpGet("Get/{id:length(24)}")]
        public async Task<ActionResult<ResultViewModel>> Get(string id)
        {
            var result = await _cartService.FindAsync(id);
            return Ok(result);
        }
        [HttpPut("Update/{id:length(24)}")]
        public async Task<ActionResult<ResultViewModel>> UpdateAsync(string id, Cart cart)
        {
            var result = new ResultViewModel();
            var cartResult = await _cartService.FindAsync(id);

            if (!cartResult.IsSuccess)
            {
                result.IsSuccess= cartResult.IsSuccess;
                result.Result = cartResult.Result;
                result.Message = result.Message;
            }
            else
            {
                cartResult = await _cartService.UpdateAsync(cart);
                result.IsSuccess =cartResult.IsSuccess;
                result.Result = result.Result;
                result.Message = result.Message;
            }

            return Ok(result);
        }

        [HttpDelete("Delete/{id:length(24)}")]
        public async Task<ActionResult<ResultViewModel>> Delete(string id)
        {
            var result = new ResultViewModel();
            var resultFind = await _cartService.FindAsync(id);

            if (!resultFind.IsSuccess)
            {
                result.IsSuccess= resultFind.IsSuccess;
                result.Message = resultFind.Message;
            }
            else
            {
                var resultView = await _cartService.RemoveAsync(id);
                result.IsSuccess = resultView.IsSuccess;
                result.Message = resultView.Message;
            }
            return Ok(result);
           
        }
    }
}
