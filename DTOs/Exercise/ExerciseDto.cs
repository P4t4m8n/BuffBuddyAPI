namespace BuffBuddyAPI;

public class ExerciseDTO : IID
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? YoutubeUrl { get; set; }
    public ExerciseTypeDTO? Type { get; set; }
    public ExerciseEquipmentDTO? Equipment { get; set; }
    public ExerciseMuscleDTO? Muscle { get; set; }

}
