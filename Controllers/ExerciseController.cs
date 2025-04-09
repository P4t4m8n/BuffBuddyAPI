using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace BuffBuddyAPI;

[Route("api/v1/exercises")]
[ApiController]
public class ExerciseController : ControllerBase
{
    private readonly IOutputCacheStore outputCacheStore;
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;
    private const string cacheKey = "exercise";

    public ExerciseController(IOutputCacheStore outputCacheStore, ApplicationDbContext context, IMapper mapper)
    {
        this.outputCacheStore = outputCacheStore;
        this.context = context;
        this.mapper = mapper;
    }

    [HttpGet]
    [OutputCache(Tags = [cacheKey])]
    public async Task<List<ExerciseDto>> Get()
    {
        return await context.Exercises
        .ProjectTo<ExerciseDto>(mapper.ConfigurationProvider)
        .ToListAsync();

    }
    [HttpGet("{id}", Name = "GetExerciseById")]
    [OutputCache]
    public async Task<ActionResult<Exercise>> Get(string id)
    {
        throw new NotImplementedException();

    }

    [HttpPost]
    public async Task<CreatedAtRouteResult> Post([FromBody] ExerciseDto dto)
    {
        var exercise = mapper.Map<Exercise>(dto);
        context.Add(exercise);
        await context.SaveChangesAsync();
        await outputCacheStore.EvictByTagAsync(cacheKey, default);
        // var returnDto = mapper.Map<ExerciseDto>(exercise);
        return CreatedAtRoute("GetExerciseById", new { id = exercise.Id }, exercise);
    }


    [HttpPut]
    public void Put()
    {

    }
    [HttpDelete]
    public void Delete()
    {

    }
}
