using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuffBuddyAPI;

public class CoreSet : IGuid
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public int Reps { get; set; }
    public int Weight { get; set; } // in kg
    public int RestTime { get; set; } // in seconds
    public int Order { get; set; } // Order of the set in the exercise
    public bool IsWarmup { get; set; } = false; // Indicates if the set was a warmup set
    public int RepsInReserve { get; set; } = 0; // Indicates if the reps are in reserve
    // Foreign keys
    [Required(ErrorMessage = "required")]
    public Guid ProgramExerciseId { get; set; } // Foreign key to ProgramExercise
    [ForeignKey("ProgramExerciseId")]
    public ProgramExercise? ProgramExercise { get; set; } // Navigation property to ProgramExercise

}
