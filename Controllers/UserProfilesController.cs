using LearningBackgroundServices.UserProfiles;
using Microsoft.Extensions.Caching.Memory;

namespace LearningBackgroundServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfilesController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;

        public UserProfilesController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public ActionResult GetUserProfiles()
        {
            var response = _memoryCache.Get<List<UserProfile>>("profiles");
            return Ok(response);
        }
    }
}
