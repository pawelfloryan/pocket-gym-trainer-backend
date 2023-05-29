namespace PocketGymTrainer.Models;

public class AuthResult
{
    public string Token { get; set; }
    public bool Result { get; set; }
    public List<String> Errors { get; set; }
}