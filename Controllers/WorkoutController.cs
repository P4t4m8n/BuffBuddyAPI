using AutoMapper;
using BuffBuddyAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace BuffBuddyAPI
{
    [Route("api/v1/workout")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private const string cacheKey = "workout-cache";
        private const string container = "workout-container";

        private readonly IOutputCacheStore outputCacheStore;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public WorkoutController(IOutputCacheStore outputCacheStore, ApplicationDbContext context, IMapper mapper)
        {
            this.outputCacheStore = outputCacheStore;
            this.context = context;
            this.mapper = mapper;
        }

        [Route("user-sets")]
        [HttpPost]
        public async Task<IActionResult> SaveUserSets([FromBody] UserSetEditDTO[] dto)
        {
            if (dto == null || dto.Length == 0)
            {
                return BadRequest("No user sets provided.");
            }

            var userSets = dto.Select(set =>
            {
                var userSet = mapper.Map<UserSet>(set);
                userSet.Id = string.IsNullOrEmpty(set.Id) ? Guid.NewGuid() : Guid.Parse(set.Id);
                return userSet;
            }).ToList();
            context.UserSets.AddRange(userSets);
            await context.SaveChangesAsync();

            await outputCacheStore.EvictByTagAsync(cacheKey, default);

            return Ok("User sets saved successfully.");

        }

    }
}
