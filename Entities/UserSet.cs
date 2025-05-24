using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuffBuddyAPI;

public class UserSet : IGuid
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public int Reps { get; set; }
    public int Weight { get; set; } // in kg
    public int RestTime { get; set; } // in seconds
    public bool IsCompleted { get; set; } = false; // Indicates if the set was completed
    public bool IsFinished { get; set; } = false; // Indicates if the set was finished (for logging in the front)
    public bool IsMuscleFailure { get; set; } = false; // Indicates if the set was done to muscle failure
    public bool IsJointPain { get; set; } = false; // Indicates if the user experienced joint pain during the set
    // Foreign keys
    [Required(ErrorMessage = "required")]
    public Guid ProgramExerciseId { get; set; } // Foreign key to ProgramExercise
    [ForeignKey("ProgramExerciseId")]
    public ProgramExercise? ProgramExercise { get; set; } // Navigation property to ProgramExercise\
    [Required(ErrorMessage = "required")]
    public Guid CoreSetId { get; set; } // Foreign key to CoreSet
    [ForeignKey("CoreSetId")]
    public CoreSet? CoreSet { get; set; } // Navigation property to CoreSet


}
