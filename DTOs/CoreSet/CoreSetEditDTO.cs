using System.ComponentModel.DataAnnotations;

namespace BuffBuddyAPI;

public class SetEditDTO : IIDEdit
{
    public string? Id { get; set; }
    public int TargetReps { get; set; }
    public int Weight { get; set; } // in kg
    public int RestTime { get; set; } // in seconds
    public int Order { get; set; } // Order of the set in the exercise
    public int RepsInReserve { get; set; } = 0; // Indicates if the reps are in reserve

    public bool IsWarmup { get; set; } = false; // Indicates if the set was a warmup set
    // Foreign keys
    public string? ProgramExerciseId { get; set; } // Foreign key to ProgramExercise

}

