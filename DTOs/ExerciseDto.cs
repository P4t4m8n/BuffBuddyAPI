using System.ComponentModel.DataAnnotations;
namespace BuffBuddyAPI;

public class ExerciseDto
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
    public string? Type { get; set; }
    [Required(ErrorMessage = "required")]
    public string? Equipment { get; set; }
    [Required(ErrorMessage = "required")]
    public string? TargetMuscle { get; set; }
    [Required(ErrorMessage = "required")]
    public string? ImgUrl { get; set; }

}
