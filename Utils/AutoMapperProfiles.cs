using AutoMapper;

namespace BuffBuddyAPI;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        ConfigureExercises();
        ConfigureExerciseInfo();
    }

    private void ConfigureExercises()
    {
        CreateMap<ExerciseEditDTO, Exercise>()
                .ForMember(dest => dest.Id, opt => opt
                    .MapFrom(src => string.IsNullOrEmpty(src.Id) ? Guid.Empty : Guid.Parse(src.Id)))
                .ForMember(dest => dest.ExerciseTypeId, opt => opt
                    .MapFrom(src => string.IsNullOrEmpty(src.ExerciseTypeId) ? Guid.Empty : Guid.Parse(src.ExerciseTypeId)))
                .ForMember(dest => dest.ExerciseEquipmentId, opt => opt
                    .MapFrom(src => string.IsNullOrEmpty(src.ExerciseEquipmentId) ? Guid.Empty : Guid.Parse(src.ExerciseEquipmentId)))
                .ForMember(dest => dest.ExerciseMuscleId, opt => opt
                    .MapFrom(src => string.IsNullOrEmpty(src.ExerciseMuscleId) ? Guid.Empty : Guid.Parse(src.ExerciseMuscleId)));

        CreateMap<Exercise, ExerciseDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
                src.Id == Guid.Empty ? null : src.Id.ToString()))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.ExerciseType))
            .ForMember(dest => dest.Equipment, opt => opt.MapFrom(src => src.ExerciseEquipment))
            .ForMember(dest => dest.Muscle, opt => opt.MapFrom(src => src.ExerciseMuscle)); ;
    }
    private void ConfigureExerciseInfo()
    {
        CreateMap(typeof(BaseExerciseInfoEditDTO), typeof(BaseExerciseInfoEntity))
            .ForMember("Id", opt => opt
                .MapFrom(src => string.IsNullOrEmpty(((IIDEdit)src).Id) ?
                        Guid.Empty : Guid.Parse(((IIDEdit)src).Id!)))
            .ForMember("ImgUrl", opt => opt
                .MapFrom(src => ((BaseExerciseInfoEditDTO)src).ImgUrl));

        CreateMap<ExerciseMuscleEditDTO, ExerciseMuscle>();
        CreateMap<ExerciseEquipmentEditDTO, ExerciseEquipment>();
        CreateMap<ExerciseTypeEditDTO, ExerciseType>();

        CreateMap(typeof(BaseExerciseInfoEntity), typeof(BaseExerciseInfoDTO))
            .ForMember("Id", opt => opt.MapFrom(src =>
                ((BaseExerciseInfoEntity)src).Id == Guid.Empty ?
                null : ((BaseExerciseInfoEntity)src).Id.ToString()));

        CreateMap<ExerciseMuscle, ExerciseMuscleDTO>();
        CreateMap<ExerciseEquipment, ExerciseEquipmentDTO>();
        CreateMap<ExerciseType, ExerciseTypeDTO>();
    }
}
