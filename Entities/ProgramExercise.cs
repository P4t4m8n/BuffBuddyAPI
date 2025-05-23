﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuffBuddyAPI;

public class ProgramExercise : IGuid, INotes
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    // Foreign keys
    [Required(ErrorMessage = "required")]
    public Guid ProgramId { get; set; }
    [Required(ErrorMessage = "required")]
    public Guid ExerciseId { get; set; }
    // Navigation properties
    [ForeignKey("ProgramId")]
    public Program? Program { get; set; }
    [ForeignKey("ExerciseId")]
    public Exercise? Exercise { get; set; }
    public int Order { get; set; }
    public ICollection<CoreSet>? CoreSets { get; set; }
    public ICollection<DaysOfWeek>? DaysOfWeek { get; set; } // Days of the week when the exercise is scheduled
    public string? Note { get; set; } = string.Empty; // Note for the exercise in the program
}
