using AuthenticationAPI.DTOS;
using AuthenticationAPI.Interfaces;
using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<ApplicationUser> userManager, ITokenService tokenService, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequest request)
         {
            if(await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return BadRequest(new
                {
                    message = $"{request.Email} EmailId already exists"
                });
                    
            }
            if(await _userManager.FindByNameAsync(request.LoginId) != null)
            {
                return BadRequest(new
                {
                    message =  $"{request.LoginId} LoginId already exists"
                });
            }
            ApplicationUser appUser = new ApplicationUser
            {

                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.LoginId,
                PhoneNumber = request.ContactNumber,

            };

            appUser.SecurityStamp = Guid.NewGuid().ToString();

            _logger.LogInformation($"Registering a new User");
            var result = await _userManager.CreateAsync(appUser, request.Password);

            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(appUser, "Member");
                _logger.LogInformation($"Assigned Member role for the newly registered user - {request.LoginId}");
                return Ok(new
                {
                    message = "Registration Successfull"
                });
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            bool check=false;
            _logger.LogInformation("Validating LoginId and password of user for Login");           
            var user = await _userManager.FindByNameAsync(request.LoginId);
            if(user != null)
            {
                check = await _userManager.CheckPasswordAsync(user, request.Password);
            }
            if (user == null || !check)
            {
                return BadRequest(new
                {
                    message = "Invalid LoginId/Password"
                });
            }
            _logger.LogInformation($"{request.LoginId} logged in");
            _logger.LogInformation("Generating token after succesful login");
            var token = await _tokenService.CreateToken(user);
            return Ok(new { loginId = user.UserName, jwtToken = token });
            
        }

        [HttpPut]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.LoginId);
            if (user == null) 
            {
                return BadRequest(new
                {
                  message= $"{request.LoginId} User doesn't exists"
                });
            }
            _logger.LogInformation("Generating password reset token");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            return Ok(new
            {
               message="Password updated successfully"
            });
        }
    }
}
