using PocketGymTrainer.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PocketGymTrainer.Models.DTOs;
using PocketGymTrainer.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace PocketGymTrainer.Controllers;

[Route("auth")]
public class AuthenticationController : ApiController
{
    private readonly UserManager<IdentityUser> _userManager;
    //private readonly JwtConfig _jwtConfig;
    private readonly IConfiguration _configuration;

    public AuthenticationController(
        UserManager<IdentityUser> userManager,
        //JwtConfig jwtConfig,
        IConfiguration configuration
        )
    {
        _userManager = userManager;
        //_jwtConfig = jwtConfig;
        _configuration = configuration;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDtos requestDto)
    {
        if(ModelState.IsValid)
        {
            var user_exist = await _userManager.FindByEmailAsync(requestDto.Email);

            if(user_exist != null)
            {
                return BadRequest(new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>(){
                            "Email already exists"
                        }
                    }
                );
            }

            var new_user = new IdentityUser()
            {
                Email = requestDto.Email,
                UserName = requestDto.Email
            };

            var is_created = await _userManager.CreateAsync(new_user, requestDto.Password);

            if(is_created.Succeeded)
            {
                var token = GenerateJwtToken(new_user);

                return Ok(new AuthResult()
                    {
                        Result = true,
                        Token = token
                    }
                );
            }

            return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Server Error"
                    },
                    Result = false
                }
            );
        }

        return BadRequest();
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequestDtos requestDto)
    {
        if(ModelState.IsValid)
        {
            var existing_user = await _userManager.FindByEmailAsync(requestDto.Email);

            if(existing_user == null)
            {
                return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>()
                        {
                            "Invalid payload"
                        },
                        Result = false
                    }
                );
            }

            var isCorrect = await _userManager.CheckPasswordAsync(existing_user, requestDto.Password);
        
            if(!isCorrect)
            {
                return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>()
                        {
                            "Invalid credentials"
                        },
                        Result = false
                    }
                );
            }

            var jwtToken = GenerateJwtToken(existing_user);

            return Ok(new AuthResult()
                {
                    Token = jwtToken,
                    Result = true
                }
            );
        }

        return BadRequest(new AuthResult()
            {
                Errors = new List<string>()
                {
                    "Invalid payload"
                },
                Result = false
            }
        );
    }

    private string GenerateJwtToken(IdentityUser user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new []
                {
                    new Claim("id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Name, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
                }
            ),

            Expires = DateTime.Now.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        return jwtTokenHandler.WriteToken(token);
    }
}