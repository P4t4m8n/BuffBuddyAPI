using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuffBuddyAPI;

public class ExerciseIcon
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Must be between {2} and {1} characters long")]
    [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Only alphanumeric characters and spaces are allowed.")]
    public required string Name { get; set; }
    [Required(ErrorMessage = "required")]
    [Url(ErrorMessage = "Not a valid URL")]
    public  string? ImgUrl { get; set; }

}
