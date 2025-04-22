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
    private readonly IFileStorage? fileStorage;
    private readonly string cacheKey;
    private readonly string? container;

    public BaseController(ApplicationDbContext context, IMapper mapper, IOutputCacheStore outputCacheStore, string cacheKey)
    {
        this.context = context;
        this.mapper = mapper;
        this.outputCacheStore = outputCacheStore;
        this.cacheKey = cacheKey;
    }

    public BaseController(ApplicationDbContext context, IMapper mapper, IOutputCacheStore outputCacheStore, string cacheKey,
                      IFileStorage fileStorage, string container)
    : this(context, mapper, outputCacheStore, cacheKey)
    {
        this.fileStorage = fileStorage;
        this.container = container;
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
    protected async Task<CreatedAtRouteResult> PostWithFile<TEdit, T, TDTO>(TEdit creationDTO,
     string routeName, Func<TEdit, IFormFile?> getFile, Action<T, string> setUrl)
     where T : class
     where TDTO : IID
    {
        var entity = mapper.Map<T>(creationDTO);

        var file = getFile(creationDTO);
        if (file != null && fileStorage != null && container != null)
        {
            var url = await fileStorage.Store(container, file);
            setUrl(entity, url);
        }

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
    protected async Task<IActionResult> PutWithFile<TEdit, T, TDTO>(string id, TEdit editDTO,
        Func<TEdit, IFormFile?> getFile,
        string container)
        where T : BaseExerciseInfo
        where TDTO : class
    {
        var guidId = Guid.Parse(id);

        var entityExists = await context.Set<T>().AnyAsync(e => e.Id == guidId);
        if (!entityExists)
        {
            return NotFound();
        }

        var entity = mapper.Map<T>(editDTO);

        var file = getFile(editDTO);
        if (file != null)
        {
            if (fileStorage == null || container == null)
            {
                throw new InvalidOperationException("File storage and container must be configured for file uploads");
            }
            if (!string.IsNullOrEmpty(entity.ImgUrl))
            {
                await fileStorage.Delete(entity.ImgUrl, container);
            }
            var url = await fileStorage.Store(container, file);
            entity.ImgUrl = url;
        }

        entity.Id = guidId;
        context.Update(entity);
        await context.SaveChangesAsync();
        await outputCacheStore.EvictByTagAsync(cacheKey, default);

        var returnDto = mapper.Map<TDTO>(entity);
        return Ok(returnDto);
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
    protected async Task<IActionResult> DeleteWithFile<T>(string id, Func<T, string?> getFileUrl)
    where T : class, IGuid
    {
        if (fileStorage == null || container == null)
        {
            throw new InvalidOperationException("File storage and container must be configured for file deletions");
        }

        var guidId = Guid.Parse(id);
        var entityToDelete = await context.Set<T>().FirstOrDefaultAsync(x => x.Id == guidId);

        if (entityToDelete == null)
        {
            return NotFound();
        }

        var fileUrl = getFileUrl(entityToDelete);
        if (!string.IsNullOrEmpty(fileUrl))
        {
            await fileStorage.Delete(fileUrl, container);
        }

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

    protected async Task<string> HandleFileUpload(IFormFile file, string? existingUrl = null)
    {
        if (fileStorage == null || container == null)
        {
            throw new InvalidOperationException("File storage and container must be configured for file uploads");
        }

        if (existingUrl != null)
        {
            await fileStorage.Delete(existingUrl, container);
        }
        return await fileStorage.Store(container, file);
    }
}


