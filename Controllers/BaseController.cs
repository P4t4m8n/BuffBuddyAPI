using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace BuffBuddyAPI;

public class BaseController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;
    private readonly IOutputCacheStore outputCacheStore;
    private readonly string cacheKey;

    public BaseController(ApplicationDbContext context, IMapper mapper, IOutputCacheStore outputCacheStore, string cacheKey)
    {
        this.context = context;
        this.mapper = mapper;
        this.outputCacheStore = outputCacheStore;
        this.cacheKey = cacheKey;
    }


    protected async Task<List<TDTO>> Get<T, TDTO>(PaginationDTO pagination,
        Expression<Func<T, object>> orderBy)
        where T : class
    {
        var queryable = context.Set<T>().AsQueryable();
        await HttpContext.InsertPageInHeader(queryable);
        return await queryable
            .OrderBy(orderBy)
            .Paginate(pagination)
            .ProjectTo<TDTO>(mapper.ConfigurationProvider)
            .ToListAsync();

    }

    protected async Task<ActionResult<TDTO>> Get<T, TDTO>(string id)
        where T : class
        where TDTO : IID
    {
        var entity = await context.Set<T>()
                            .ProjectTo<TDTO>(mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync(e => e.Id == id);

        if (entity is null)
        {
            return NotFound();
        }

        return entity;
    }

    protected async Task<CreatedAtRouteResult> Post<TEdit, T, TDTO>(TEdit creationDTO,
        string routeName)
        where T : class
        where TDTO : IID
    {
        var entity = mapper.Map<T>(creationDTO);
        context.Add(entity);
        await context.SaveChangesAsync();
        await outputCacheStore.EvictByTagAsync(cacheKey, default);
        var entityDTO = mapper.Map<TDTO>(entity);
        return CreatedAtRoute(routeName, new { id = entityDTO.Id }, entityDTO);
    }

    protected async Task<IActionResult> Put<TEdit, T>(string id, TEdit creationDTO)
        where T : class, IID
    {
        var entityExists = await context.Set<T>().AnyAsync(e => e.Id == id);

        if (!entityExists)
        {
            return NotFound();
        }

        var entity = mapper.Map<T>(creationDTO);
        entity.Id = id;

        context.Update(entity);
        await context.SaveChangesAsync();
        await outputCacheStore.EvictByTagAsync(cacheKey, default);

        return NoContent();
    }

    protected async Task<IActionResult> Delete<T>(string id)
        where T : class, IGuid
    {
        var guidId = Guid.Parse(id);
        var deletedRecords = await context.Set<T>()
                         .Where(g => g.Id == guidId)
                         .ExecuteDeleteAsync();

        if (deletedRecords == 0)
        {
            return NotFound();
        }

        await outputCacheStore.EvictByTagAsync(cacheKey, default);
        return NoContent();
    }
}


