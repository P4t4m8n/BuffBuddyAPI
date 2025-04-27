namespace BuffBuddyAPI;

public class ProgramEditDTO : IIDEdit
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Note { get; set; } = string.Empty; // Note for the program
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<ProgramExerciseEditDTO>? ProgramExercises { get; set; }
}
