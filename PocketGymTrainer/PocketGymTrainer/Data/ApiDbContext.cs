using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PocketGymTrainer.Models;

namespace PocketGymTrainer.Data;

public class ApiDbContext : IdentityDbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<Section> Section {get; set;}

    public DbSet<Exercise> Exercise {get; set;}

    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Workout> Workout { get; set; }
}
