using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Makeup_Web.Controllers
{
    public abstract class BaseController : Controller
    {

        protected bool TryGetUserId(out int userId)
        {
            userId = 0;
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out userId))
            {
                return false;
            }
            return true;
        }
    }
}
