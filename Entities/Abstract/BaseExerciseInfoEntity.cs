using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuffBuddyAPI;
public abstract class BaseExerciseInfoEntity : BaseExerciseInfo, IGuid, ICreateAt
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "required")]
    [Url(ErrorMessage = "Not a valid URL")]
    public override string? ImgUrl { get; set; }
    public DateTime CreateAt { get; set; }
}
