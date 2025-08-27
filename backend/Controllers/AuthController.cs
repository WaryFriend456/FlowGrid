using backend.Dtos;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, TokenService tokenService) : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly TokenService _tokenService = tokenService;

        /// <summary>
        /// Registers a new User in the system
        /// </summary>
        /// <param name="registerDto">The User's registraion details (username, email, password). </param>
        /// <returns>A JWT if registrationis successful</returns>
        /// <response code="200">Returns a JWT token</response>
        /// <response code="400">If the User details are invalid</response> 
        [HttpPost("register")]
        [ProducesResponseType(typeof(TokenDto), 200)]
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

            return Ok(new TokenDto
            {
                token = await _tokenService.CreateToken(appUser)
            });
        }

        /// <summary>
        /// Logs in an existing user
        /// </summary>
        /// <param name="loginDto">The User's login details (username, password).</param>
        /// <returns>A JWT if login is successful</returns>
        /// <response code="200">Returns a JWT token</response>
        /// <response code="400">If the User details are invalid</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenDto), 200)]
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
                return Unauthorized(new { message = "Invalid Credentials" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized(new { message = "Invalid Credentials" });
            }

            return Ok(
                new TokenDto{
                    token = await _tokenService.CreateToken(user)
                }
            );
        }

        [HttpGet("protected")]
        [Authorize]
        public IActionResult GetProtectedData()
        {
            //var username = User.FindFirst(JwtRegisteredClaimNames.GivenName)?.Value;
            var username = User.Identity?.Name;
            return Ok(new { message = $"Hello {username}, welcome to the protected route!" });
        }
    }
}
