using System.ComponentModel.DataAnnotations;

namespace BuffBuddyAPI;
public abstract class BaseExerciseInfo
{

    [Required(ErrorMessage = "required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Must be between {2} and {1} characters long")]
    [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Only alphanumeric characters and spaces are allowed.")]
    public required string Name { get; set; }
    public virtual string? ImgUrl { get; set; }

}
