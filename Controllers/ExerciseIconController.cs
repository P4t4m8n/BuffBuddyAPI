
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
    // [HttpGet]
    // [OutputCache(Tags = [cacheKey])]
    // public async Task<List<ExerciseIconDTO>> Get([FromQuery] PaginationDTO pagination)
    // {
    //     throw new NotImplementedException("ExerciseIconController.Get() is not implemented yet.");

    // }

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
            Console.WriteLine(url);

            exerciseIcon.ImgUrl = url;

        }
        string json = System.Text.Json.JsonSerializer.Serialize(exerciseIcon, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true
        });
        Console.WriteLine(json);

        context.Add(exerciseIcon);
        await context.SaveChangesAsync();
        await outputCacheStore.EvictByTagAsync(cacheKey, default);
        return new CreatedAtRouteResult("GetExerciseIconById", new { id = exerciseIcon.Id }, exerciseIcon);
        // return Ok();

    }


}
