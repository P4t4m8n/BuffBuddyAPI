using AutoMapper;

namespace BuffBuddyAPI;

public class AutoMapperProfiles : Profile
{

    public AutoMapperProfiles()
    {
        ConfigureExercises();
        ConfigureExerciseIcons();
    }

    private void ConfigureExercises()
    {
        CreateMap<ExerciseEditDTO, Exercise>()
                .ForMember(dest => dest.Id, opt => opt
                    .MapFrom(src => string.IsNullOrEmpty(src.Id) ? Guid.Empty : Guid.Parse(src.Id)))
                .ForMember(dest => dest.ExerciseTypeId, opt => opt
                    .MapFrom(src => string.IsNullOrEmpty(src.ExerciseTypeId) ? Guid.Empty : Guid.Parse(src.ExerciseTypeId)))
                .ForMember(dest => dest.EquipmentId, opt => opt
                    .MapFrom(src => string.IsNullOrEmpty(src.EquipmentId) ? Guid.Empty : Guid.Parse(src.EquipmentId)))
                .ForMember(dest => dest.TargetMuscleId, opt => opt
                    .MapFrom(src => string.IsNullOrEmpty(src.TargetMuscleId) ? Guid.Empty : Guid.Parse(src.TargetMuscleId)));

        CreateMap<Exercise, ExerciseDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
                src.Id == Guid.Empty ? null : src.Id.ToString()));

    }

    private void ConfigureExerciseIcons()
    {
        CreateMap<ExerciseIconEditDTO, ExerciseIcon>()
            .ForMember(dest => dest.Id, opt => opt
            .MapFrom(src => string.IsNullOrEmpty(src.Id) ? Guid.Empty : Guid.Parse(src.Id)))
            .ForMember(dest => dest.ImgUrl, opt => opt.Ignore());

        CreateMap<ExerciseIcon, ExerciseIconDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
                src.Id == Guid.Empty ? null : src.Id.ToString()));
    }
}
