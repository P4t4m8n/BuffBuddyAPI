using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace BuffBuddyAPI;

public class ProgramEditDTO : IIDEdit
{
    public string? Id { get; set; }
    [Required(ErrorMessage = "required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Must be between {2} and {1} characters long")]
    [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Only alphanumeric characters and spaces are allowed.")]
    public required string Name { get; set; }
    public string? Note { get; set; } = string.Empty; // Note for the program
    [Required(ErrorMessage = "required")]
    public required DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; } = DateTime.Now.AddDays(30); // Default to 30 days from now
    public bool IsActive { get; set; } = true;
    public ICollection<ProgramExerciseEditDTO>? ProgramExercises { get; set; }
}
