using System.ComponentModel.DataAnnotations;

namespace PocketGymTrainer.Models.DTOs;

public class UserLoginRequestDtos
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}