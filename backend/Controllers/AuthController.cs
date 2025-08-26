using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, TokenService tokenService) : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly TokenService _tokenService = tokenService;
    
        [HttpPost("register")]
    
        public async Task<IActionResult> Register([FromBody] Dtos.RegisterDto registerDto)
        {
                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}

            var appUser = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };

            var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);
            if (!createdUser.Succeeded)
            {
                return BadRequest(createdUser.Errors);
            }

            var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
            
            if (!roleResult.Succeeded)
            {
                return BadRequest(roleResult.Errors);
            }

            return Ok(new
            {
                token = await _tokenService.CreateToken(appUser)
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Dtos.LoginDto loginDto)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginDto.Username.ToLower());
            var user = await _userManager.FindByNameAsync(loginDto.Username);

            if(user == null)
            {
                return Unauthorized("Invalid Credentials");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized("Invalid Credentials");
            }

            return Ok(
                new {
                    token = await _tokenService.CreateToken(user)
                }
            );
        }
    }
}
