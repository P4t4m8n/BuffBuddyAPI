using AutoMapper;

namespace BuffBuddyAPI;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        ConfigureExercises();
        ConfigureExerciseInfo();
        ConfigurePrograms();
        ConfigureProgramExercises();
        ConfigureSets();
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
    private void ConfigurePrograms()
    {
        CreateMap<Program, ProgramDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
                src.Id == Guid.Empty ? null : src.Id.ToString()));

        CreateMap<ProgramEditDTO, Program>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Id) ? Guid.Empty : Guid.Parse(src.Id)))
            // Don't map ProgramExercises directly from DTO to avoid overwriting existing relationships
            .ForMember(dest => dest.ProgramExercises, opt => opt.Ignore());
    }

    private void ConfigureProgramExercises()
    {
        CreateMap<ProgramExercise, ProgramExerciseDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
                src.Id == Guid.Empty ? null : src.Id.ToString()))
            .ForMember(dest => dest.Exercise, opt => opt.MapFrom(src => src.Exercise));

        CreateMap<ProgramExerciseEditDTO, ProgramExercise>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Id) ? Guid.Empty : Guid.Parse(src.Id)))
            .ForMember(dest => dest.ProgramId, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.ProgramId) ? Guid.Empty : Guid.Parse(src.ProgramId)))
            .ForMember(dest => dest.ExerciseId, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.ExerciseId) ? Guid.Empty : Guid.Parse(src.ExerciseId)))
            // Don't map Sets directly from DTO to avoid overwriting existing relationships
            .ForMember(dest => dest.CoreSets, opt => opt.Ignore())
            // Don't map navigation properties from DTO
            .ForMember(dest => dest.Program, opt => opt.Ignore())
            .ForMember(dest => dest.Exercise, opt => opt.Ignore());
    }

    private void ConfigureSets()
    {
        CreateMap<CoreSet, CoreSetDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
                src.Id == Guid.Empty ? null : src.Id.ToString()));

        CreateMap<SetEditDTO, CoreSet>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Id) ? Guid.Empty : Guid.Parse(src.Id)))
            .ForMember(dest => dest.ProgramExerciseId, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.ProgramExerciseId) ? Guid.Empty : Guid.Parse(src.ProgramExerciseId)))
            // Don't map navigation properties from DTO
            .ForMember(dest => dest.ProgramExercise, opt => opt.Ignore());
    }
}

