using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebApp.Hubs
{
    /// <summary>
    /// хаб, от которого надо создавать все остальные, требующие авторизацию
    /// </summary>
    [Authorize]
    public class BaseAuthorizedHub : Hub
    {
        /// <summary>
        /// guid пользователя из идентити
        /// </summary>
        protected string UserId => Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}