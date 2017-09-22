using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sprotify.Web.Controllers
{
    public class LogoutController : Controller
    {
        public async Task Logout()
        {
            await HttpContext.Authentication.SignOutAsync("Cookies");
            await HttpContext.Authentication.SignOutAsync("oidc");
        }
    }
}