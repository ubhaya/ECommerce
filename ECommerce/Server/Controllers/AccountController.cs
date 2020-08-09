using ECommerce.Server.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ECommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("GetUserInfo")]
        public async Task<ActionResult<ApplicationUser>> GetUserInfo(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            user.PasswordHash = null;

            return user;
        }

        
    }
}