
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace BuffBuddyAPI;
[Route("api/v1/exercise-type")]
[ApiController]
public class ExerciseTypeController : BaseController
{
    private const string cacheKey = "exercise-type-cache";
    private const string container = "exercise-type-container";
    public ExerciseTypeController(IOutputCacheStore outputCacheStore, ApplicationDbContext context,
                                  IMapper mapper, IFileStorage fileStorage)
    : base(context, mapper, outputCacheStore, cacheKey, fileStorage, container) { }
    [HttpGet]
    [OutputCache(Tags = [cacheKey])]
    public async Task<List<ExerciseTypeDTO>> Get([FromQuery] PaginationDTO pagination)
    {
        return await Get<ExerciseType, ExerciseTypeDTO>(pagination, x => x.Name);
    }

    [HttpGet("{id}", Name = "GetExerciseTypeById")]
    [OutputCache]
    public async Task<ActionResult<ExerciseTypeDTO>> Get(string id)
    {
        return await Get<ExerciseType, ExerciseTypeDTO>(id);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] ExerciseTypeEditDTO dto)
    {
        return await PostWithFile<ExerciseTypeEditDTO, ExerciseType, ExerciseTypeDTO>(
             dto,
             "GetExerciseTypeById",
             edit => edit.File,
             (entity, url) => entity.ImgUrl = url
         );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromForm] ExerciseTypeEditDTO dto)
    {
        return await PutWithFile<ExerciseTypeEditDTO, ExerciseType, ExerciseTypeDTO>(
         id,
         dto,
         edit => edit.File,
         container
     );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        return await DeleteWithFile<ExerciseType>(id, entity => entity.ImgUrl);
    }

}
