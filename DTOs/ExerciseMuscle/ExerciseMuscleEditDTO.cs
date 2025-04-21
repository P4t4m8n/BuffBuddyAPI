using System.ComponentModel.DataAnnotations;

namespace BuffBuddyAPI;

public class ExerciseMuscleEditDTO : IIDEdit
{
    public string? Id { get; set; }
    [Required(ErrorMessage = "required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Must be between {2} and {1} characters long")]
    [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Only alphanumeric characters and spaces are allowed.")]
    public string? Name { get; set; }
    [Required(ErrorMessage = "required")]
    public IFormFile? File { get; set; }
    public string? ImgUrl { get; set; }
}
