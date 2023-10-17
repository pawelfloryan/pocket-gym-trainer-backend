using ErrorOr;
using PocketGymTrainer.Contracts.Exercise;
using PocketGymTrainer.ServiceErrors;
using System.ComponentModel.DataAnnotations;

namespace PocketGymTrainer.Models;

public class PreparedExercise
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string MuscleGroup { get; set; }
    public string Level { get; set; }
    public string P_P { get; set; }

    private PreparedExercise()
    {

    }

    private PreparedExercise(
        Guid id,
        string name,
        string muscleGroup,
        string level,
        string p_p
        )
    {
        Id = id;
        Name = name;
        MuscleGroup = muscleGroup;
        Level = level;
        P_P = p_p;
    }
}