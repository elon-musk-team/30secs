using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApp.Data;
using WebApp.Dto;
using WebApp.Services.Interfaces;

namespace WebApp.Services
{
    public class ContactLogicService : IContactLogicService
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _applicationDbContext;

        public ContactLogicService(ILogger logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ContactDto>> GetMyContacts(string userId)
        {
            var contacts = await _applicationDbContext.UsersToContacts
                .Where(x => x.UserId == userId)
                .Select(x => x.Contact)
                .ToListAsync();
            var contactDtos = contacts.Select(x => new ContactDto
            {
                ScreenName = x.ScreenName,
                LinkToAvatar = x.LinkToAvatar,
            });
            return contactDtos;
        }
    }
}