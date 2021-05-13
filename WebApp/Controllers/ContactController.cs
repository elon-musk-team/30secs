using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Constants;
using WebApp.Data;
using WebApp.Dto;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("contact/this-user-contacts")]
    public class ContactController : BaseAuthorizedController
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ContactController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        /// <summary>
        /// возвращает все контакты текущего пользователя
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<ContactDto>), 200)]
        public async Task<IActionResult> GetThisUserContacts()
        {
            var contacts = await _applicationDbContext.UsersToContacts
                .Where(x => x.UserId == UserId)
                .Select(x => x.Contact)
                .ToListAsync();
            var contactDtos = contacts.Select(x => new ContactDto
            {
                ScreenName = x.ScreenName,
                LinkToAvatar = x.LinkToAvatar,
            });
            return Ok(contactDtos);
        }
        
        /// <summary>
        /// добавляет переданного пользователя этому пользователю в контакты
        /// </summary>
        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> AddContactToThisUser([FromQuery] string screenName)
        {
            var contact =  await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.ScreenName == screenName);
            if (contact == null)
            {
                return BadRequest(new ErrorDto
                {
                    ErrorCode = (int)ApiErrors.NoSuchUser,
                    ErrorText = "Пользователь с таким screenName не найден",
                });
            }
            var thisUserContacts = await _applicationDbContext.UsersToContacts
                .Where(x => x.UserId == UserId && x.ContactId == contact.Id)
                .Select(x => x.Contact)
                .ToListAsync();
            if (thisUserContacts.Any())
            {
                return BadRequest(new ErrorDto
                {
                    ErrorCode = (int)ApiErrors.Already,
                    ErrorText = "Этого пользователя уже добавляли",
                });
            }
            await _applicationDbContext.UsersToContacts
                .AddAsync(new UserToContact
                {
                    ContactId = contact.Id,
                    UserId = UserId,
                });
            await _applicationDbContext.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// удаляет переданного юзера из контактов этого
        /// </summary>
        [HttpDelete]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteContact([FromQuery] string screenName)
        {
            var contactUser =  await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.ScreenName == screenName);
            if (contactUser == null)
            {
                return BadRequest(new ErrorDto
                {
                    ErrorCode = (int)ApiErrors.NoSuchUser,
                    ErrorText = "Пользователь с таким screenName не найден",
                });
            }

            var contactTask = _applicationDbContext.UsersToContacts
                .FirstOrDefaultAsync(x => x.UserId == UserId && x.ContactId == contactUser.Id);
            var contact = await contactTask;
            if (contact == null)
            {
                return BadRequest(new ErrorDto
                {
                    ErrorCode = (int)ApiErrors.Already,
                    ErrorText = "Этого пользователя уже удаляли или не добавляли",
                });
            }

            _applicationDbContext.UsersToContacts.Remove(contact);
            await _applicationDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}