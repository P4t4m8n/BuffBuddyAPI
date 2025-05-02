using System.ComponentModel.DataAnnotations;

namespace BuffBuddyAPI.Validations; public class FileOrUrlRequiredAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is BaseExerciseInfoEditDTO dto)
        {
            if (dto.File == null && string.IsNullOrWhiteSpace(dto.ImgUrl))
            {
                return new ValidationResult("Either an image file or an image URL must be provided.",
                    [nameof(dto.File), nameof(dto.ImgUrl)]);
            }
        }

        return ValidationResult.Success;
    }
}