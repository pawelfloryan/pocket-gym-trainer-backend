using ErrorOr;

namespace PocketGymTrainer.ServiceErrors;

public static class Errors
{
    public static class Section
    {

        public static Error NotFound => Error.NotFound(
            code: "Section.NotFound",
            description: "Section not found"
        );

        public static Error TooMany => Error.Conflict(
            code: "Section.TooMany",
            description: "You can only create 10 sections"
        );
    }

    public static class Exercise
    {
        public static Error NotFound => Error.NotFound(
            code: "Exercise.NotFound",
            description: "Exercise not found"
        );

        public static Error TooMany => Error.Conflict(
            code: "Exercise.TooMany",
            description: "You can only create 15 exercises"
        );
    }

    public static class PreparedExercise
    {
        public static Error NotFound => Error.NotFound(
            code: "PreparedExercise.NotFound",
            description: "Prepared exercise not found"
        );
    }

    public static class Workout
    {
        public static Error NotFound => Error.NotFound(
            code: "Workout.NotFound",
            description: "Workout not found"
        );
    }

    public static class UserStats
    {
        public static Error NotFound => Error.NotFound(
            code: "UserStats.NotFound",
            description: "UserStats not found"
        );
    }
}