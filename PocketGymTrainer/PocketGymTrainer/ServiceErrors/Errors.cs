using ErrorOr;

namespace PocketGymTrainer.ServiceErrors;

public static class Errors
{
    public static class Section
    {
        //public static Error InvalidName => Error.Validation(
        //    code: "Section.NotFound",
        //    description: $"Section name must be at least {Models.Section.MinNameLength}" + 
        //        $" characters long and at most {Models.Section.MaxNameLength} characters long."
        //);

        public static Error NotFound => Error.NotFound(
            code: "Section.NotFound",
            description: "Section not found"
        );
    }

    public static class Exercise
    {
        //public static Error InvalidName => Error.Validation(
        //    code: "Exercise.NotFound",
        //    description: $"Exercise name must be at least {Models.Exercise.MinNameLength} characters long"
        //);

        public static Error NotFound => Error.NotFound(
            code: "Exercise.NotFound",
            description: "Exercise not found"
        );
    }

    public static class Workout
    {
        //public static Error InvalidName => Error.Validation(
        //    code: "Exercise.NotFound",
        //    description: $"Exercise name must be at least {Models.Exercise.MinNameLength} characters long"
        //);

        public static Error NotFound => Error.NotFound(
            code: "Workout.NotFound",
            description: "Workout not found"
        );
    }
}