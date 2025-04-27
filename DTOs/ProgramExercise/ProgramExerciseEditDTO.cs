using System.ComponentModel.DataAnnotations;

namespace BuffBuddyAPI;

public class ProgramExerciseEditDTO : IIDEdit
{
    public string? Id { get; set; }
    public string? ProgramId { get; set; }
    [Required(ErrorMessage = "required")]
    public required string ExerciseId { get; set; }
    public int Order { get; set; }
    public ICollection<SetEditDTO>? Sets { get; set; }
    public string? Note { get; set; } = string.Empty; // Note for the exercise in the program
}
