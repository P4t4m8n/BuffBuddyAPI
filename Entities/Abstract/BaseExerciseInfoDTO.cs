

namespace BuffBuddyAPI;
public abstract class BaseExerciseInfoDTO : BaseExerciseInfo, IID
{
    public required string Id { get; set; }

    public override required string? ImgUrl { get; set; }

}
