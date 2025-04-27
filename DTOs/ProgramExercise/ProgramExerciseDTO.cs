namespace BuffBuddyAPI;

public class ProgramExerciseDTO : IID
{
    public required string Id { get; set; }
    public required ExerciseDTO Exercise { get; set; }
    public int Order { get; set; }
    public ICollection<SetDTO>? Sets { get; set; }
    public string? Note { get; set; } = string.Empty; // Note for the exercise in the program
}
