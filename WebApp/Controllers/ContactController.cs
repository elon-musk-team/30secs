using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    [ProducesResponseType(401)]
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ContactController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        /// <summary>
        /// возвращает все контакты текущего пользователя
        /// </summary>
        [HttpGet("ThisUserContacts")]
        public async Task<IActionResult> GetThisUserContacts()
        {
            var userId = User.Identity?.Name;
            var contacts = await _applicationDbContext.UsersToContacts
                .Where(x => x.UserId == userId)
                .Select(x => x.Contact)
                .ToListAsync();
            return Ok(contacts);
        }
        
        /// <summary>
        /// добавляет указанного пользователя этому пользователю в контакты (не завершено)
        /// </summary>
        [HttpPost("ThisUserContacts")]
        public async Task<IActionResult> AddContactToThisUser(string chatId)
        {
            var userId = User.Identity?.Name;
            await _applicationDbContext.UsersToContacts
                .AddAsync(new UserToContact
                {
                    
                });
            return Ok();
        }
    }
}