using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Dto;

namespace WebApp.Controllers
{
    /// <summary>
    /// контроллер, от которого надо создавать все остальные, требующие авторизацию
    /// </summary>
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(List<ErrorDto>), 400)]
    [Authorize]
    [ApiController]
    public class BaseAuthorizedController : ControllerBase
    {
        /// <summary>
        /// guid пользователя из идентити
        /// </summary>
        protected string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}