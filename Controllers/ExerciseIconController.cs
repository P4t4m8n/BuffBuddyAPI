
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;


namespace BuffBuddyAPI;

[Route("api/v1/exercise-icons")]
[ApiController]
public class ExerciseIconController : ControllerBase
{
    private readonly IOutputCacheStore outputCacheStore;
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;
    private readonly IFileStorage fileStorage;
    private const string cacheKey = "exercise-icon";
    private readonly string container = "exercise-icons";

    public ExerciseIconController(IOutputCacheStore outputCacheStore, ApplicationDbContext context, IMapper mapper, IFileStorage fileStorage)
    {
        this.outputCacheStore = outputCacheStore;
        this.context = context;
        this.mapper = mapper;
        this.fileStorage = fileStorage;
    }
    [HttpGet]
    [OutputCache(Tags = [cacheKey])]
    public async Task<List<ExerciseIconDTO>> Get([FromQuery] PaginationDTO pagination)
    {
        var queryable = context.ExerciseIcons;
        await HttpContext.InsertPageInHeader(queryable);
        return await queryable
        .OrderBy(x => x.Name)
        .Paginate(pagination)
        .ProjectTo<ExerciseIconDTO>(mapper.ConfigurationProvider)
        .ToListAsync();

    }

    [HttpGet("{id}", Name = "GetExerciseIconById")]
    [OutputCache]
    public async Task<ActionResult<ExerciseIconDTO>> Get(string id)
    {
        var exerciseIcon = await context.ExerciseIcons
        .ProjectTo<ExerciseIconDTO>(mapper.ConfigurationProvider)
        .FirstOrDefaultAsync(x => x.Id == id);
        if (exerciseIcon is null)
        {
            return NotFound();
        }

        return exerciseIcon;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] ExerciseIconEditDTO dto)
    {
        var exerciseIcon = mapper.Map<ExerciseIconEditDTO, ExerciseIcon>(dto);

        if (dto.File is not null)
        {
            var url = await fileStorage.Store(container, dto.File);

            exerciseIcon.ImgUrl = url;

        }

        context.Add(exerciseIcon);
        await context.SaveChangesAsync();
        await outputCacheStore.EvictByTagAsync(cacheKey, default);
        return new CreatedAtRouteResult("GetExerciseIconById", new { id = exerciseIcon.Id }, exerciseIcon);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromForm] ExerciseIconEditDTO dto)
    {
        var guidId = Guid.Parse(id);
        var exerciseIconExists = await context.ExerciseIcons.AnyAsync(x => x.Id == guidId);
        if (!exerciseIconExists)
        {
            return NotFound();
        }

        var exerciseIcon = mapper.Map<ExerciseIconEditDTO, ExerciseIcon>(dto);

        if (dto.File is not null)
        {
            if (exerciseIcon.ImgUrl is not null)
            {
                await fileStorage.Delete(exerciseIcon.ImgUrl, container);
            }
            var url = await fileStorage.Store(container, dto.File);

            exerciseIcon.ImgUrl = url;

        }
        exerciseIcon.Id = guidId;
        context.Update(exerciseIcon);
        await context.SaveChangesAsync();
        await outputCacheStore.EvictByTagAsync(cacheKey, default);

        var returnDto = mapper.Map<ExerciseIconDTO>(exerciseIcon);
        return Ok(returnDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {

        var guidId = Guid.Parse(id);
        var recordToDelete = await context.ExerciseIcons.FirstOrDefaultAsync(x => x.Id == guidId);
        if (recordToDelete is null)
        {
            return NotFound();
        }
        await fileStorage.Delete(recordToDelete.ImgUrl, container);

        var deletedRecord = await context.ExerciseIcons.Where(x => x.Id == guidId).ExecuteDeleteAsync();
        if (deletedRecord == 0)
        {
            return NotFound();
        }


        await outputCacheStore.EvictByTagAsync(cacheKey, default);
        return NoContent();
    }

}
