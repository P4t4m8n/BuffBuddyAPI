namespace BuffBuddyAPI;

public class ProgramExerciseDTO : IID
{
    public required string Id { get; set; }
    public required ExerciseDTO Exercise { get; set; }
    public int Order { get; set; }
    public ICollection<SetDTO>? Sets { get; set; }
    public ICollection<DaysOfWeek>? DaysOfWeek { get; set; } // Days of the week when the exercise is scheduled

    public string? Note { get; set; } = string.Empty; // Note for the exercise in the program
}
