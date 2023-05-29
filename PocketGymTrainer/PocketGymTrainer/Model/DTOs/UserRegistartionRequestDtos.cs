using System.ComponentModel.DataAnnotations;

namespace PocketGymTrainer.Models.DTOs;

public class UserRegistrationRequestDtos
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}