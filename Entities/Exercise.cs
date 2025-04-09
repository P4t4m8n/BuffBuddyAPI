using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BuffBuddyAPI;

public class Exercise
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [StringLength(100, ErrorMessage = "{0} cannot be longer than 100 characters")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [Url(ErrorMessage = "{0} is not a valid URL")]
    public string? YoutubeUrl { get; set; }
    public string? Type { get; set; }
    public string? Equipment { get; set; }
    public string? TargetMuscle { get; set; }
    public string? ImgUrl { get; set; }

}
