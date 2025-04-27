namespace BuffBuddyAPI;

public class ProgramDTO : IID
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Note { get; set; } = string.Empty; // Note for the program
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<ProgramExerciseDTO>? ProgramExercises { get; set; }
}
