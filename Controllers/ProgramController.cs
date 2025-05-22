using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace BuffBuddyAPI;

[Route("api/v1/program")]
[ApiController]
public class ProgramController : ControllerBase
{
    private const string cacheKey = "program-cache";
    private const string container = "program-container";

    private readonly IOutputCacheStore outputCacheStore;
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;
    public ProgramController(IOutputCacheStore outputCacheStore, ApplicationDbContext context, IMapper mapper)
    {

        this.outputCacheStore = outputCacheStore;
        this.context = context;
        this.mapper = mapper;
    }
    [HttpGet]
    [OutputCache(Tags = [cacheKey])]
    public async Task<List<ProgramDTO>> Get([FromQuery] PaginationDTO pagination)
    {
        var queryable = context.Programs;


        await HttpContext.InsertPageInHeader(queryable);
        return await queryable
        .OrderBy(x => x.StartDate)
        .Paginate(pagination)
        .ProjectTo<ProgramDTO>(mapper.ConfigurationProvider)
        .ToListAsync();

    }
    [HttpGet("{id}", Name = "GetProgramById")]
    [OutputCache]
    public async Task<ActionResult<ProgramDTO>> Get(string id)
    {
        var program = await context.Programs.ProjectTo<ProgramDTO>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (program is null)
        {
            return NotFound();
        }

        return program;

    }

    [HttpPost]
    public async Task<CreatedAtRouteResult> Post([FromBody] ProgramEditDTO dto)
    {
        var program = mapper.Map<Program>(dto);

        context.Add(program);

        if (dto.NewProgramExercises != null && dto.NewProgramExercises.Any())
        {
            var programExercises = new List<ProgramExercise>();

            foreach (var peDto in dto.NewProgramExercises)
            {
                var programExercise = mapper.Map<ProgramExercise>(peDto);

                programExercise.ProgramId = program.Id;


                if (peDto.Sets != null && peDto.Sets.Any())
                {
                    programExercise.CoreSets = new List<CoreSet>();

                    foreach (var setDto in peDto.Sets)
                    {
                        var set = mapper.Map<CoreSet>(setDto);

                        programExercise.CoreSets.Add(set);
                    }
                }
                programExercises.Add(programExercise);
            }

            program.ProgramExercises = programExercises;
        }

        await context.SaveChangesAsync();

        await outputCacheStore.EvictByTagAsync(cacheKey, default);

        var completeProgram = await context.Programs
            .Include(p => p.ProgramExercises!)
                .ThenInclude(pe => pe.Exercise!)
                    .ThenInclude(e => e.ExerciseType)
            .Include(p => p.ProgramExercises!)
                .ThenInclude(pe => pe.Exercise!)
                    .ThenInclude(e => e.ExerciseEquipment)
            .Include(p => p.ProgramExercises!)
                .ThenInclude(pe => pe.Exercise!)
                    .ThenInclude(e => e.ExerciseMuscle)
            .Include(p => p.ProgramExercises!)
                .ThenInclude(pe => pe.CoreSets)
            .FirstOrDefaultAsync(p => p.Id == program.Id);


        var returnDto = mapper.Map<ProgramDTO>(completeProgram);
        return CreatedAtRoute("GetProgramById", new { id = returnDto.Id }, returnDto);
    }




    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] ProgramEditDTO dto)
    {
        var guidId = Guid.Parse(id);
        var programExists = await context.Programs.AnyAsync(x => x.Id == guidId);
        if (!programExists)
        {
            return NotFound();
        }

        var programToUpdate = await context.Programs
            .Include(p => p.ProgramExercises!)
                .ThenInclude(pe => pe.CoreSets)
            .FirstOrDefaultAsync(p => p.Id == guidId);

        if (programToUpdate == null)
        {
            return StatusCode(500, "Internal server error: Program found by AnyAsync but not by FirstOrDefaultAsync.");
        }

        mapper.Map(dto, programToUpdate); 

        programToUpdate.ProgramExercises ??= new List<ProgramExercise>();

        if (dto.DeleteProgramExercises != null && dto.DeleteProgramExercises.Any())
        {
            foreach (var peDtoToDelete in dto.DeleteProgramExercises)
            {
                if (string.IsNullOrEmpty(peDtoToDelete.Id) || !Guid.TryParse(peDtoToDelete.Id, out var peIdToDelete))
                {
                    continue; 
                }
                var exerciseToRemove = programToUpdate.ProgramExercises.FirstOrDefault(pe => pe.Id == peIdToDelete);
                if (exerciseToRemove != null)
                {
          
                    context.ProgramExercises.Remove(exerciseToRemove);
                }
            }
        }

        if (dto.UpdateProgramExercises != null && dto.UpdateProgramExercises.Any())
        {
            foreach (var peDtoToUpdate in dto.UpdateProgramExercises)
            {
                if (string.IsNullOrEmpty(peDtoToUpdate.Id) || !Guid.TryParse(peDtoToUpdate.Id, out var peIdToUpdate)) continue;

                var existingProgramExercise = programToUpdate.ProgramExercises.FirstOrDefault(pe => pe.Id == peIdToUpdate);
                if (existingProgramExercise != null)
                {
                    mapper.Map(peDtoToUpdate, existingProgramExercise);
                    if (Guid.TryParse(peDtoToUpdate.ExerciseId, out var exId))
                    {
                        existingProgramExercise.ExerciseId = exId;
                    }
   
                    existingProgramExercise.CoreSets ??= new List<CoreSet>();
                    var currentSetsInDb = existingProgramExercise.CoreSets.ToList(); 
                    var dtoSets = peDtoToUpdate.Sets?.ToList() ?? new List<SetEditDTO>(); 

                    var dtoSetIdsWithGuid = dtoSets
                        .Where(ds => !string.IsNullOrEmpty(ds.Id) && Guid.TryParse(ds.Id, out _))
                        .Select(ds => Guid.Parse(ds.Id!))
                        .ToList();

                    var setsToDelete = currentSetsInDb.Where(s => !dtoSetIdsWithGuid.Contains(s.Id)).ToList();
                    foreach (var setToDelete in setsToDelete)
                    {
                        context.CoreSets.Remove(setToDelete);
                    }

                    foreach (var setDto in dtoSets)
                    {
                        if (!string.IsNullOrEmpty(setDto.Id) && Guid.TryParse(setDto.Id, out var setIdToUpdate))
                        {
                            var existingSet = currentSetsInDb.FirstOrDefault(s => s.Id == setIdToUpdate);
                            if (existingSet != null)
                            {
                                mapper.Map(setDto, existingSet);
                                existingSet.ProgramExerciseId = existingProgramExercise.Id; // Ensure FK
                            }
                         
                        }
                        else
                        {
                            var newSet = mapper.Map<CoreSet>(setDto);
                            newSet.ProgramExerciseId = existingProgramExercise.Id; // Ensure FK
                            existingProgramExercise.CoreSets.Add(newSet);
                        }
                    }
                }
             
            }
        }

        // Handle NEW ProgramExercises
        if (dto.NewProgramExercises != null && dto.NewProgramExercises.Any())
        {
            foreach (var peDtoToCreate in dto.NewProgramExercises)
            {
                var newProgramExercise = mapper.Map<ProgramExercise>(peDtoToCreate);
                newProgramExercise.ProgramId = programToUpdate.Id; // Link to parent
                if (Guid.TryParse(peDtoToCreate.ExerciseId, out var exId))
                {
                    newProgramExercise.ExerciseId = exId;
                }
                // newProgramExercise.DaysOfWeek should be mapped by AutoMapper

                if (peDtoToCreate.Sets != null && peDtoToCreate.Sets.Any())
                {
                    newProgramExercise.CoreSets = new List<CoreSet>();
                    foreach (var setDto in peDtoToCreate.Sets)
                    {
                        var newSet = mapper.Map<CoreSet>(setDto);
                        // newSet.ProgramExerciseId will be set by relationship fix-up when newProgramExercise is added to context,
                        // or you can set it explicitly if newProgramExercise.Id is generated client-side (not typical for DB-generated IDs).
                        newProgramExercise.CoreSets.Add(newSet);
                    }
                }
                programToUpdate.ProgramExercises.Add(newProgramExercise);
            }
        }

        await context.SaveChangesAsync();

        await outputCacheStore.EvictByTagAsync(cacheKey, default);

        var completeProgram = await context.Programs
            .AsNoTracking()
            .Include(p => p.ProgramExercises!)
                .ThenInclude(pe => pe.Exercise!)
                    .ThenInclude(e => e.ExerciseType)
            .Include(p => p.ProgramExercises!)
                .ThenInclude(pe => pe.Exercise!)
                    .ThenInclude(e => e.ExerciseEquipment)
            .Include(p => p.ProgramExercises!)
                .ThenInclude(pe => pe.Exercise!)
                    .ThenInclude(e => e.ExerciseMuscle)
            .Include(p => p.ProgramExercises!)
                .ThenInclude(pe => pe.CoreSets)
            .FirstOrDefaultAsync(p => p.Id == programToUpdate.Id); // Use programToUpdate.Id


        var returnDto = mapper.Map<ProgramDTO>(completeProgram);
        return Ok(returnDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {

        var guidId = Guid.Parse(id);
        var deletedRecord = await context.Programs.Where(x => x.Id == guidId).ExecuteDeleteAsync();
        if (deletedRecord == 0)
        {
            return NotFound();
        }


        await outputCacheStore.EvictByTagAsync(cacheKey, default);
        return NoContent();

    }

}

