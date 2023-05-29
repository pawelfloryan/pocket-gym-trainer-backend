using PocketGymTrainer.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PocketGymTrainer.Models.DTOs;

namespace PocketGymTrainer.Controllers;

[Route("auth")]
public class AuthenticationController : ApiController
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtConfig _jwtConfig;

    public AuthenticationController(
        UserManager<IdentityUser> userManager,
        JwtConfig jwtConfig)
    {
        _userManager = userManager;
        _jwtConfig = jwtConfig;
    }

    [HttpPost]
    [Route("/register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDtos requestDto)
    {
        if(ModelState.IsValid)
        {

        }

        return BadRequest();
    }
}