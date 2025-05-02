

using System.ComponentModel.DataAnnotations;
using BuffBuddyAPI.Validations;
namespace BuffBuddyAPI;
[FileOrUrlRequired]
public abstract class BaseExerciseInfoEditDTO : BaseExerciseInfo, IIDEdit
{
    public string? Id { get; set; }
    public IFormFile? File { get; set; }
    [Url(ErrorMessage = "Not a valid URL")]
    public override string? ImgUrl { get; set; }

}
