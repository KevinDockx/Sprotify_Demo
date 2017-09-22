using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Sprotify.Web.Models.User;
using System.Threading.Tasks;

namespace Sprotify.Web.ViewComponents
{
    public class LoginPanel : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var ci = User.Identity as ClaimsIdentity;
            var name = $"{ci.Claims.FirstOrDefault(x => x.Type == "given_name")?.Value} {ci.Claims.FirstOrDefault(x => x.Type == "family_name")?.Value}";
            
            return View(new LoginPanelModel
            {
                Name = name
            });
        }
    }
}
