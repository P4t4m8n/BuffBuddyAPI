using System.ComponentModel.DataAnnotations;
namespace BuffBuddyAPI;

public class ExerciseEditDTO
{
    public string? Id { get; set; }
    [Required(ErrorMessage = "required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Must be between {2} and {1} characters long")]
    [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Only alphanumeric characters and spaces are allowed.")]
    public string? Name { get; set; }
    [Required(ErrorMessage = "required")]
    [Url(ErrorMessage = "Not a valid URL")]
    public string? YoutubeUrl { get; set; }
    [Required(ErrorMessage = "required")]
    public string? ExerciseTypeId { get; set; }
    [Required(ErrorMessage = "required")]
    public string? ExerciseEquipmentId { get; set; }
    [Required(ErrorMessage = "required")]
    public string? ExerciseMuscleId { get; set; }

}
