using Microsoft.AspNetCore.Mvc;
using Sprotify.Web.Models.User;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Sprotify.Web.Controllers
{
    public class UserController : Controller
    {
        [Authorize]
        [Route("user-info/{owner:guid}")]
        public IActionResult Info([FromRoute]Guid owner)
        {
            return View(new UserInfo { FullName = "Wesley Cabus", Id = owner });
        }
    }
}
