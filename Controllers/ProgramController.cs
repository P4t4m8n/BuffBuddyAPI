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
        Console.WriteLine("*************");
        var program = mapper.Map<Program>(dto);
        Console.WriteLine($"Program: {program.Name}");

        context.Add(program);

        if (dto.ProgramExercises != null && dto.ProgramExercises.Any())
        {
            var programExercises = new List<ProgramExercise>();

            foreach (var peDto in dto.ProgramExercises)
            {
                var programExercise = mapper.Map<ProgramExercise>(peDto);

                programExercise.ProgramId = program.Id;


                if (peDto.Sets != null && peDto.Sets.Any())
                {
                    programExercise.Sets = new List<Set>();

                    foreach (var setDto in peDto.Sets)
                    {
                        var set = mapper.Map<Set>(setDto);

                        programExercise.Sets.Add(set);
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
                .ThenInclude(pe => pe.Sets)
            .FirstOrDefaultAsync(p => p.Id == program.Id);


        var returnDto = mapper.Map<ProgramDTO>(completeProgram);
        return CreatedAtRoute("GetProgramById", new { id = returnDto.Id }, returnDto);
    }

}

