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
        public ICollection<DaysOfWeek>? DaysOfWeek { get; set; } // Days of the week when the exercise is scheduled

    public string? Note { get; set; } = string.Empty; // Note for the exercise in the program
}
