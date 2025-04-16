using System.ComponentModel.DataAnnotations;
namespace BuffBuddyAPI;

public class ExerciseDTO
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? YoutubeUrl { get; set; }
    public string? Type { get; set; }
    public string? Equipment { get; set; }
    public string? TargetMuscle { get; set; }
    public string? ImgUrl { get; set; }

}
