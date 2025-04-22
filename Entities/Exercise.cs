using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BuffBuddyAPI;

public class Exercise
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Must be between {2} and {1} characters long")]
    [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Only alphanumeric characters and spaces are allowed.")]
    public string? Name { get; set; }
    [Required(ErrorMessage = "required")]
    [Url(ErrorMessage = "Not a valid URL")]
    public string? YoutubeUrl { get; set; }
    // Foreign keys
    [Required(ErrorMessage = "required")]
    public Guid ExerciseMuscleId { get; set; }
    [Required(ErrorMessage = "required")]
    public Guid ExerciseTypeId { get; set; }
    [Required(ErrorMessage = "required")]
    public Guid EquipmentId { get; set; }
    // Navigation properties
    [ForeignKey("ExerciseIconId")]
    public ExerciseMuscle? ExerciseIMuscle { get; set; }
    [ForeignKey("ExerciseTypeId")]
    public ExerciseType? ExerciseType { get; set; }
    [ForeignKey("EquipmentId")]
    public ExerciseEquipment? Equipment { get; set; }


}