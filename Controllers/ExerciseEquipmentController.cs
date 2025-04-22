
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace BuffBuddyAPI;
[Route("api/v1/exercise-equipment")]
[ApiController]
public class ExerciseEquipmentController : BaseController
{
    private const string cacheKey = "exercise-equipment-cache";
    private const string container = "exercise-equipment-container";
    public ExerciseEquipmentController(IOutputCacheStore outputCacheStore, ApplicationDbContext context,
                                  IMapper mapper, IFileStorage fileStorage)
    : base(context, mapper, outputCacheStore, cacheKey, fileStorage, container) { }
    [HttpGet]
    [OutputCache(Tags = [cacheKey])]
    public async Task<List<ExerciseEquipmentDTO>> Get([FromQuery] PaginationDTO pagination)
    {
        return await Get<ExerciseEquipment, ExerciseEquipmentDTO>(pagination, x => x.Name);
    }

    [HttpGet("{id}", Name = "GetExerciseEquipmentById")]
    [OutputCache]
    public async Task<ActionResult<ExerciseEquipmentDTO>> Get(string id)
    {
        return await Get<ExerciseEquipment, ExerciseEquipmentDTO>(id);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] ExerciseEquipmentEditDTO dto)
    {
        return await PostWithFile<ExerciseEquipmentEditDTO, ExerciseEquipment, ExerciseEquipmentDTO>(
             dto,
             "GetExerciseEquipmentById",
             edit => edit.File,
             (entity, url) => entity.ImgUrl = url
         );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromForm] ExerciseEquipmentEditDTO dto)
    {
        return await PutWithFile<ExerciseEquipmentEditDTO, ExerciseEquipment, ExerciseEquipmentDTO>(
         id,
         dto,
         edit => edit.File,
         container
     );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        return await DeleteWithFile<ExerciseEquipment>(id, entity => entity.ImgUrl);
    }

}
