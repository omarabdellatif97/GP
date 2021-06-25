using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GP_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        public AuthenticationController(UserManager<ApplicationUser> _userManager, IConfiguration _configuration)
        {
            userManager = _userManager;
            configuration = _configuration;
        }

        public class UserLoginModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public class UserRegisterModel
        {
            [Required]
            [StringLength(150)]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }


            [Required]
            [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; }
        }

        public class CustomResponse
        {
            public string Status { get; set; }
            public string Message { get; set; }
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {

                var authClaims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(12),
                    claims:authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                var tokenId = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new
                {
                    idToken = tokenId,
                    expiresIn = (token.ValidTo - DateTime.Now).TotalSeconds,
                    email = user.Email,
                    localId = user.Id,
                    userName = user.Name
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            if (model.Password != model.ConfirmPassword)
                return StatusCode(StatusCodes.Status400BadRequest,
                    new CustomResponse { Status = "Error", Message = "Password dosn't match Confirmed Password" });

            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status400BadRequest, new CustomResponse { Status = "Error", Message = "Email already exists!" });

            userExists = await userManager.FindByNameAsync(model.Email);

            if (userExists != null)
                return StatusCode(StatusCodes.Status400BadRequest, new CustomResponse { Status = "Error", Message = "Username already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new CustomResponse { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new CustomResponse { Status = "Success", Message = "User created successfully!" });
        }


        //[HttpPost]
        //[Route("logout")]
        //public async Task<IActionResult> Logout(string token)
        //{

        //    return default;
        //}


    }
}
