using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Dto;

namespace WebApp.Controllers
{
    [Route("user")]
    public class UserController : BaseAuthorizedController
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UserController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        
        /// <summary>
        /// инфа о данном пользователе с идентити (кроме системной и чувствительной)
        /// </summary>
        [HttpGet("my-data")]
        public async Task<IActionResult> GetMyData()
        {
            var myUser = await _applicationDbContext.Users.FindAsync(UserId);
            return Ok(new ContactDto
            {
                ScreenName = myUser.ScreenName,
                LinkToAvatar = myUser.LinkToAvatar,
            });
        }
    }
}