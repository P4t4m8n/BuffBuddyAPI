using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace BuffBuddyAPI;

[Route("api/v1/equipment")]
[ApiController]
public class EquipmentController : ControllerBase
{
    private readonly IOutputCacheStore outputCacheStore;
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;
    private const string cacheKey = "equipment";

    public EquipmentController(IOutputCacheStore outputCacheStore, ApplicationDbContext context, IMapper mapper)
    {

        this.outputCacheStore = outputCacheStore;
        this.context = context;
        this.mapper = mapper;
    }

    [HttpGet]
    [OutputCache(Tags = [cacheKey])]
    public async Task<List<EquipmentDTO>> Get([FromQuery] PaginationDTO pagination)
    {
        throw new NotImplementedException("This method is not implemented yet.");


    }
    [HttpGet("{id}", Name = "GetEquipmentById")]
    [OutputCache]
    public async Task<ActionResult<EquipmentDTO>> Get(string id)
    {
        throw new NotImplementedException("This method is not implemented yet.");

    }

    [HttpPost]
    public async Task<CreatedAtRouteResult> Post([FromBody] EquipmentDTO dto)
    {
        throw new NotImplementedException("This method is not implemented yet.");

    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] EquipmentDTO dto)
    {
        throw new NotImplementedException("This method is not implemented yet.");


    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {

        throw new NotImplementedException("This method is not implemented yet.");


    }
}