using System.ComponentModel.DataAnnotations;

namespace BuffBuddyAPI;

public class SetEditDTO : IIDEdit
{
    public string? Id { get; set; }
    public int ActualReps { get; set; }
    public int TargetReps { get; set; }
    public int Weight { get; set; } // in kg
    public int RestTime { get; set; } // in seconds
    public int Order { get; set; } // Order of the set in the exercise
    public bool IsCompleted { get; set; } = false; // Indicates if the set was completed
    public bool IsMuscleFailure { get; set; } = false; // Indicates if the set was done to muscle failure
    public bool IsWarmup { get; set; } = false; // Indicates if the set was a warmup set
    public bool JointPain { get; set; } = false; // Indicates if the user experienced joint pain during the set
    // Foreign keys
    public string? ProgramExerciseId { get; set; } // Foreign key to ProgramExercise

}

