
using System.ComponentModel.DataAnnotations;
namespace BuffBuddyAPI;

public class EquipmentDTO
{
    public string? Id { get; set; }
    [Required(ErrorMessage = "required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Must be between {2} and {1} characters long")]
    public string? Name { get; set; }
}