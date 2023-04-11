using Microsoft.AspNetCore.Mvc;

namespace PocketGymTrainer.Controllers;

public class ErrorsController : ControllerBase
{
    [Route("/error")]
    public IActionResult Error()
    {
        return Problem();
    }
}