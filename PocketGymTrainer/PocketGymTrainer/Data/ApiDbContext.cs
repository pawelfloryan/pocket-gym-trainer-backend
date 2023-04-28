using Microsoft.EntityFrameworkCore;
using PocketGymTrainer.Contracts.Section;
using PocketGymTrainer.Models;

namespace PocketGymTrainer.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<Section> Section {get; set;}

    public DbSet<Exercise> Exercise {get; set;}

}
