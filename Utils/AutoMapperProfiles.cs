using AutoMapper;

namespace BuffBuddyAPI;

public class AutoMapperProfiles : Profile
{

    public AutoMapperProfiles()
    {
        ConfigureExercises();
        // ConfigureExerciseMuscles();
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
                    .MapFrom(src => string.IsNullOrEmpty(src.EquipmentId) ? Guid.Empty : Guid.Parse(src.EquipmentId)))
                .ForMember(dest => dest.ExerciseMuscleId, opt => opt
                    .MapFrom(src => string.IsNullOrEmpty(src.ExerciseMuscleId) ? Guid.Empty : Guid.Parse(src.ExerciseMuscleId)));

        CreateMap<Exercise, ExerciseDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
                src.Id == Guid.Empty ? null : src.Id.ToString()));

    }

    // private void ConfigureExerciseMuscles()
    // {
    //     CreateMap<ExerciseMuscleEditDTO, ExerciseMuscle>()
    //   .ForMember(dest => dest.Id, opt => opt
    //       .MapFrom(src => string.IsNullOrEmpty(src.Id) ? Guid.Empty : Guid.Parse(src.Id)))
    //   .ForMember(dest => dest.ImgUrl, opt => opt
    //       .MapFrom(src => src.ImgUrl));


    //     CreateMap<ExerciseMuscle, ExerciseMuscleDTO>()
    //         .ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
    //             src.Id == Guid.Empty ? null : src.Id.ToString()));
    // }

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
