﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace BuffBuddyAPI;

[Route("api/v1/exercise")]
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
    public async Task<List<ExerciseDTO>> Get([FromQuery] PaginationDTO pagination)
    {
        var queryable = context.Exercises;


        await HttpContext.InsertPageInHeader(queryable);
        return await queryable
        .OrderBy(x => x.Name)
        .Paginate(pagination)
        .ProjectTo<ExerciseDTO>(mapper.ConfigurationProvider)
        .ToListAsync();

    }
    [HttpGet("{id}", Name = "GetExerciseById")]
    [OutputCache]
    public async Task<ActionResult<ExerciseDTO>> Get(string id)
    {
        var exercise = await context.Exercises

        .ProjectTo<ExerciseDTO>(mapper.ConfigurationProvider)
        .FirstOrDefaultAsync(x => x.Id == id);

        if (exercise is null)
        {
            return NotFound();
        }

        return exercise;

    }

    [HttpPost]
    public async Task<CreatedAtRouteResult> Post([FromBody] ExerciseEditDTO dto)
    {
        var exerciseToSave = mapper.Map<Exercise>(dto);
        context.Add(exerciseToSave);
        await context.SaveChangesAsync();
        await outputCacheStore.EvictByTagAsync(cacheKey, default);

        var completeExercise = await context.Exercises
            .Include(x => x.ExerciseType)
            .Include(x => x.ExerciseEquipment)
            .Include(x => x.ExerciseMuscle)
          .FirstOrDefaultAsync(x => x.Id == exerciseToSave.Id);
        var returnDto = mapper.Map<ExerciseDTO>(completeExercise);
        return CreatedAtRoute("GetExerciseById", new { id = returnDto.Id }, returnDto);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] ExerciseEditDTO dto)
    {
        var guidId = Guid.Parse(id);
        var exerciseExists = await context.Exercises.AnyAsync(x => x.Id == guidId);
        if (!exerciseExists)
        {
            return NotFound();
        }

        var exerciseToSave = mapper.Map<Exercise>(dto);
        exerciseToSave.Id = guidId;
        context.Update(exerciseToSave);
        await context.SaveChangesAsync();
        await outputCacheStore.EvictByTagAsync(cacheKey, default);

        var completeExercise = await context.Exercises
            .Include(x => x.ExerciseType)
            .Include(x => x.ExerciseEquipment)
            .Include(x => x.ExerciseMuscle)
            .FirstOrDefaultAsync(x => x.Id == exerciseToSave.Id);
        var returnDto = mapper.Map<ExerciseDTO>(completeExercise);
        return Ok(returnDto);

    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {

        var guidId = Guid.Parse(id);
        var deletedRecord = await context.Exercises.Where(x => x.Id == guidId).ExecuteDeleteAsync();
        if (deletedRecord == 0)
        {
            return NotFound();
        }


        await outputCacheStore.EvictByTagAsync(cacheKey, default);
        return NoContent();
    }
}