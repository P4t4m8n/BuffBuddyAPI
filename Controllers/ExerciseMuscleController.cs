
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace BuffBuddyAPI;
[Route("api/v1/exercise-muscle")]
[ApiController]
public class ExerciseMuscleController : BaseController
{
    private const string cacheKey = "exercise-muscle-cache";
    private const string container = "exercise-muscle-container";
    public ExerciseMuscleController(IOutputCacheStore outputCacheStore, ApplicationDbContext context,
                                  IMapper mapper, IFileStorage fileStorage)
    : base(context, mapper, outputCacheStore, cacheKey, fileStorage, container) { }
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
        return await PostWithFile<ExerciseMuscleEditDTO, ExerciseMuscle, ExerciseMuscleDTO>(
             dto,
             "GetExerciseMuscleById",
             edit => edit.File,
             (entity, url) => entity.ImgUrl = url
         );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromForm] ExerciseMuscleEditDTO dto)
    {
        return await PutWithFile<ExerciseMuscleEditDTO, ExerciseMuscle, ExerciseMuscleDTO>(
         id,
         dto,
         edit => edit.File,
         container
     );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        return await DeleteWithFile<ExerciseMuscle>(id, entity => entity.ImgUrl);
    }

}
