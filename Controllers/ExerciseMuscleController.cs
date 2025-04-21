
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;


namespace BuffBuddyAPI;

[Route("api/v1/exercise-muscle")]
[ApiController]
public class ExerciseMuscleController : BaseController
{
    private readonly IOutputCacheStore outputCacheStore;
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;
    private readonly IFileStorage fileStorage;
    private const string cacheKey = "exercise-muscle-cache";
    private readonly string container = "exercise-muscle-container";

    public ExerciseMuscleController(IOutputCacheStore outputCacheStore, ApplicationDbContext context, IMapper mapper, IFileStorage fileStorage)
    : base(context, mapper, outputCacheStore, cacheKey)
    {
        this.outputCacheStore = outputCacheStore;
        this.context = context;
        this.mapper = mapper;
        this.fileStorage = fileStorage;
    }
    [HttpGet]
    [OutputCache(Tags = [cacheKey])]
    public async Task<List<ExerciseMuscleDTO>> Get([FromQuery] PaginationDTO pagination)
    {
        return await Get<ExerciseMuscle, ExerciseMuscleDTO>(pagination, x => x.Name);
    }

    [HttpGet("{id}", Name = "GetExerciseMuscleById")]
    [OutputCache]
    public async Task<ActionResult<ExerciseMuscleDTO>> Get(string id)
    {
        return await Get<ExerciseMuscle, ExerciseMuscleDTO>(id);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] ExerciseMuscleEditDTO dto)
    {
        var exerciseMuscle = mapper.Map<ExerciseMuscleEditDTO, ExerciseMuscle>(dto);

        if (dto.File is not null)
        {
            var url = await fileStorage.Store(container, dto.File);

            exerciseMuscle.ImgUrl = url;

        }

        context.Add(exerciseMuscle);
        await context.SaveChangesAsync();
        await outputCacheStore.EvictByTagAsync(cacheKey, default);
        return new CreatedAtRouteResult("GetExerciseMuscleById", new { id = exerciseMuscle.Id }, exerciseMuscle);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromForm] ExerciseMuscleEditDTO dto)
    {
        var guidId = Guid.Parse(id);
        var exerciseMuscleExists = await context.ExerciseMuscles.AnyAsync(x => x.Id == guidId);
        if (!exerciseMuscleExists)
        {
            return NotFound();
        }

        var exerciseMuscle = mapper.Map<ExerciseMuscleEditDTO, ExerciseMuscle>(dto);

        if (dto.File is not null)
        {
            if (exerciseMuscle.ImgUrl is not null)
            {
                await fileStorage.Delete(exerciseMuscle.ImgUrl, container);
            }
            var url = await fileStorage.Store(container, dto.File);

            exerciseMuscle.ImgUrl = url;

        }
        exerciseMuscle.Id = guidId;
        context.Update(exerciseMuscle);
        await context.SaveChangesAsync();
        await outputCacheStore.EvictByTagAsync(cacheKey, default);

        var returnDto = mapper.Map<ExerciseMuscleDTO>(exerciseMuscle);
        return Ok(returnDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {

        var guidId = Guid.Parse(id);
        var recordToDelete = await context.ExerciseMuscles.FirstOrDefaultAsync(x => x.Id == guidId);
        if (recordToDelete is null)
        {
            return NotFound();
        }
        await fileStorage.Delete(recordToDelete.ImgUrl, container);

        var deletedRecord = await context.ExerciseMuscles.Where(x => x.Id == guidId).ExecuteDeleteAsync();
        if (deletedRecord == 0)
        {
            return NotFound();
        }


        await outputCacheStore.EvictByTagAsync(cacheKey, default);
        return NoContent();
    }

}
