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
        // Your existing ID parsing and existence check
        var guidId = Guid.Parse(id);
        var programExists = await context.Programs.AnyAsync(x => x.Id == guidId);
        if (!programExists)
        {
            return NotFound();
        }

        // Load the existing program to apply updates
        var programToUpdate = await context.Programs
            .Include(p => p.ProgramExercises!)
                .ThenInclude(pe => pe.CoreSets)
            .FirstOrDefaultAsync(p => p.Id == guidId);

        // This check is for the unlikely scenario where AnyAsync was true, but the entity couldn't be fetched.
        if (programToUpdate == null)
        {
            return StatusCode(500, "Internal server error: Program found by AnyAsync but not by FirstOrDefaultAsync.");
        }

        // Update scalar properties of the loaded Program entity
        mapper.Map(dto, programToUpdate); // Maps Name, Note, StartDate, EndDate, IsActive

        programToUpdate.ProgramExercises ??= new List<ProgramExercise>();

        // Handle DELETED ProgramExercises
        if (dto.DeleteProgramExercises != null && dto.DeleteProgramExercises.Any())
        {
            foreach (var peDtoToDelete in dto.DeleteProgramExercises)
            {
                if (string.IsNullOrEmpty(peDtoToDelete.Id) || !Guid.TryParse(peDtoToDelete.Id, out var peIdToDelete))
                {
                    continue; // Skip if ID is invalid or missing
                }
                var exerciseToRemove = programToUpdate.ProgramExercises.FirstOrDefault(pe => pe.Id == peIdToDelete);
                if (exerciseToRemove != null)
                {
                    // EF Core will handle deletion of Sets if cascade delete is configured.
                    // If not, you might need to remove sets explicitly: context.Sets.RemoveRange(exerciseToRemove.Sets);
                    context.ProgramExercises.Remove(exerciseToRemove);
                }
            }
        }

        // Handle UPDATED ProgramExercises
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
                    // existingProgramExercise.DaysOfWeek should be mapped by AutoMapper

                    // Manage Sets for existingProgramExercise (add, update, delete sets)
                    existingProgramExercise.CoreSets ??= new List<CoreSet>();
                    var currentSetsInDb = existingProgramExercise.CoreSets.ToList(); // Sets currently in DB for this exercise
                    var dtoSets = peDtoToUpdate.Sets?.ToList() ?? new List<SetEditDTO>(); // Sets from DTO

                    // Identify Set IDs from DTO that are valid Guids
                    var dtoSetIdsWithGuid = dtoSets
                        .Where(ds => !string.IsNullOrEmpty(ds.Id) && Guid.TryParse(ds.Id, out _))
                        .Select(ds => Guid.Parse(ds.Id!))
                        .ToList();

                    // Delete sets that are in DB but not in DTO's list of existing sets
                    var setsToDelete = currentSetsInDb.Where(s => !dtoSetIdsWithGuid.Contains(s.Id)).ToList();
                    foreach (var setToDelete in setsToDelete)
                    {
                        context.CoreSets.Remove(setToDelete);
                    }

                    // Update existing sets or add new ones
                    foreach (var setDto in dtoSets)
                    {
                        if (!string.IsNullOrEmpty(setDto.Id) && Guid.TryParse(setDto.Id, out var setIdToUpdate))
                        {
                            // This is an existing set, try to update it
                            var existingSet = currentSetsInDb.FirstOrDefault(s => s.Id == setIdToUpdate);
                            if (existingSet != null)
                            {
                                mapper.Map(setDto, existingSet);
                                existingSet.ProgramExerciseId = existingProgramExercise.Id; // Ensure FK
                            }
                            // If existingSet is null here, it means DTO provided an ID for a set that doesn't belong to this exercise.
                            // Decide how to handle: ignore, error, or treat as new (if ID is to be ignored for new items).
                        }
                        else
                        {
                            // This is a new set, add it
                            var newSet = mapper.Map<CoreSet>(setDto);
                            newSet.ProgramExerciseId = existingProgramExercise.Id; // Ensure FK
                            // newSet.Id will be generated by DB if DatabaseGeneratedOption.Identity is set
                            existingProgramExercise.CoreSets.Add(newSet);
                        }
                    }
                }
                // If existingProgramExercise is null, DTO provided an ID for an exercise that doesn't belong to this program.
                // Decide how to handle: ignore, error, or treat as new (if ID is to be ignored for new items).
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

