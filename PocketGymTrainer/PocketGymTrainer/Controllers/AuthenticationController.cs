using PocketGymTrainer.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PocketGymTrainer.Models.DTOs;
using PocketGymTrainer.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using PocketGymTrainer.Data;
using Microsoft.EntityFrameworkCore;

namespace PocketGymTrainer.Controllers;

[Route("auth")]
public class AuthenticationController : ApiController
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ApiDbContext _context;
    private readonly TokenValidationParameters _tokenValidationParameters;
    //private readonly JwtConfig _jwtConfig;

    public AuthenticationController(
        UserManager<IdentityUser> userManager,
        IConfiguration configuration,
        ApiDbContext context,
        TokenValidationParameters tokenValidationParameters
        //JwtConfig jwtConfig,
        )
    {
        _userManager = userManager;
        _configuration = configuration;
        _context = context;
        _tokenValidationParameters = tokenValidationParameters;
        //_jwtConfig = jwtConfig;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDtos requestDto)
    {
        if (ModelState.IsValid)
        {
            var user_exist = await _userManager.FindByEmailAsync(requestDto.Email);

            if (user_exist != null)
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

            if (is_created.Succeeded)
            {
                var token = GenerateJwtToken(new_user);

                return Ok(token);
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
        if (ModelState.IsValid)
        {
            var existing_user = await _userManager.FindByEmailAsync(requestDto.Email);

            if (existing_user == null)
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

            if (!isCorrect)
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

            var jwtToken = await GenerateJwtToken(existing_user);

            return Ok(jwtToken);
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

    private async Task<AuthResult> GenerateJwtToken(IdentityUser user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Name, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
                }
            ),

            Expires = DateTime.UtcNow.Add(TimeSpan.Parse(_configuration.GetSection("JwtConfig:ExpiryTimeFrame").Value)),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);

        var refreshToken = new RefreshToken()
        {
            JwtId = token.Id,
            Token = RandomStringGeneration(23),
            AddedDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddMonths(6),
            IsRevoked = false,
            IsUsed = false,
            UserId = user.Id
        };

        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();

        return new AuthResult()
        {
            Token = jwtToken,
            RefreshToken = refreshToken.Token,
            Result = true
        };
    }

    [HttpPost]
    [Route("RefreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
    {
        if (ModelState.IsValid)
        {
            var result = await VerifyAndGenerateToken(tokenRequest);

            if (result == null)
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                        {
                            "Invalid tokens"
                        },
                    Result = false
                }
                );

            return Ok(result);
        }

        return BadRequest(new AuthResult()
        {
            Errors = new List<string>()
                {
                    "Invalid parameters"
                },
            Result = false
        }
        );
    }

    private async Task<AuthResult> VerifyAndGenerateToken(TokenRequest tokenRequest)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        try
        {
            _tokenValidationParameters.ValidateLifetime = false;

            var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validatedToken);

            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                if (result == false)
                    return null;
            }

            var utcExpiryDate = long.Parse(tokenInVerification.Claims
            .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);
            if (expiryDate > DateTime.Now)
            {
                return new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Expired token"
                    },
                };
            }

            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

            if(storedToken == null)
                return new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Invalid tokens"
                    },
                };

            if(storedToken.IsUsed)
                return new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Invalid tokens"
                    },
                };

            if(storedToken.IsRevoked)
                return new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Invalid tokens"
                    },
                };

            var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            
            if(storedToken.JwtId != jti)
                return new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Invalid tokens"
                    },
                };

            if(storedToken.ExpiryDate < DateTime.UtcNow)
                return new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Expired tokens"
                    },
                };

            storedToken.IsUsed = true;
            _context.RefreshTokens.Update(storedToken);
            await _context.SaveChangesAsync();

            var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);
            return await GenerateJwtToken(dbUser);
        }
        catch (Exception e)
        {
            return new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Server error"
                    },
                };
        }
    }

    private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
        var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

        return dateTimeVal;
    }

    public string RandomStringGeneration(int length)
    {
        var random = new Random();
        var chars = "ABCDEFGHIJKLMNOPRSTUVWXZ1234567890abcdefghijklmnoprstuvwxyz_";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}