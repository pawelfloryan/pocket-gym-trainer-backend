using System.ComponentModel.DataAnnotations;
using ErrorOr;
using PocketGymTrainer.Contracts.Workout;

namespace PocketGymTrainer.Models;

public class Workout
{
    [Key]
    public Guid Id { get; set; }
    public int Time { get; set; }
    public DateTime WorkoutDate { get; set; }
    public string UserId { get; set; }

    private Workout(Guid id, int time, DateTime workoutDate, string userId)
    {
        Id = id;
        Time = time;
        WorkoutDate = workoutDate;
        UserId = userId;
    }

    public static ErrorOr<Workout> Create(int time, DateTime workoutDate, string userId, Guid? id = null)
    {
        return new Workout(id ?? Guid.NewGuid(), time, workoutDate, userId);
    }

    public static ErrorOr<Workout> From(CreateWorkoutRequest request)
    {
        return Create(
            request.time,
            request.workoutDate,
            request.userId
        );
    }

    //public static ErrorOr<Section> From(Guid id, UpsertWorkoutRequest request)
    //{
    //    return Create(
    //        request.time,
    //        request.weekDay,
    //        request.userId,
    //        id
    //    );
    //}
}