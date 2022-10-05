using cartshopping.webapi.Business.Abstract;
using cartshopping.webapi.Entity.ViewModels;
using cartshopping.webapi.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cartshopping.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<ResultViewModel>> Login(LoginViewModel loginViewModel)
        {
            ResultViewModel result = new ResultViewModel();
            var resultUser = await _userService.FindAsync(loginViewModel);
            if (resultUser.IsSuccess)
            {
                result.IsSuccess = resultUser.IsSuccess;
                result.Result = resultUser.Result;
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddMinutes(30)
                };
                Response.Cookies.Append("username",loginViewModel.UserName, cookieOptions);
            }
            else
            {
                result.IsSuccess = false;
                result.Result = resultUser.Result;
                result.Message = resultUser.Message;
            }
            return Ok(result);
        }
        [HttpGet("Get")]
        public async Task<ActionResult<ResultViewModel>> GetAsync()
        {
            ResultViewModel result = new ResultViewModel();
            var resultUser = await _userService.GetAllAsync();
            if (resultUser.IsSuccess)
            {
                result.IsSuccess = true;
                result.Result = resultUser.Result;
            }
            return Ok(result);
        }
        [HttpPost("Register")]
        public async Task<ActionResult<ResultViewModel>> Create(User user)
        {
            ResultViewModel result = new ResultViewModel();
             var resultUser = await _userService.CreateAsync(user);
            result.IsSuccess = resultUser.IsSuccess;
            result.Result = resultUser.Result;
            return Ok(result);
        }
       
        [HttpGet("Get/{id:length(24)}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            var result = await _userService.FindAsync(id);
            return Ok(result);
        }        
        [HttpPut("Update/{id:length(24)}")]
        public async Task<ActionResult<ResultViewModel>> UpdateAsync(string id, User user)
        {
            var userResult = await _userService.FindAsync(id);

            if (userResult is null)
            {
                return NotFound();
            }
            
             var result =await _userService.UpdateAsync(user);

            return Ok(result);
        }

        [HttpDelete("Delete/{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var resultFind = await _userService.FindAsync(id);

            if (!resultFind.IsSuccess)
            {
                return NotFound();
            }
            
            var result= await _userService.RemoveAsync(id);

            return Ok(result);
        }
    }
}