using AutoMapper;

namespace BuffBuddyAPI;

public class AutoMapperProfiles : Profile
{

    public AutoMapperProfiles()
    {
        ConfigureExercises();
    }

    private void ConfigureExercises()
    {
        CreateMap<ExerciseDto, Exercise>()
     .ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
         string.IsNullOrEmpty(src.Id) ? Guid.Empty : Guid.Parse(src.Id)));

        CreateMap<Exercise, ExerciseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
                src.Id == Guid.Empty ? null : src.Id.ToString()));

    }
}
