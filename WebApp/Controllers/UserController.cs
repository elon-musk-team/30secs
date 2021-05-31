using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Dto;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers
{
    [Route("user")]
    public class UserController : BaseAuthorizedController
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IContactLogicService _contactLogicService;

        public UserController(ApplicationDbContext applicationDbContext, IContactLogicService contactLogicService)
        {
            _applicationDbContext = applicationDbContext;
            _contactLogicService = contactLogicService;
        }
        
        /// <summary>
        /// инфа о данном пользователе с идентити (кроме системной и чувствительной)
        /// </summary>
        [HttpGet("my-info")]
        [ProducesResponseType(typeof(MyInfoDto), 200)]
        public async Task<IActionResult> GetMyData()
        {
            var myUser = await _applicationDbContext.Users.FindAsync(UserId);
            var myContacts = await _contactLogicService.GetMyContacts(UserId);
            return Ok(new MyInfoDto
            {
                ScreenName = myUser.ScreenName,
                LinkToAvatar = myUser.LinkToAvatar,
                StatusMessage = myUser.StatusMessage,
                MyContacts = myContacts.ToList(),
            });
        }
    }
}