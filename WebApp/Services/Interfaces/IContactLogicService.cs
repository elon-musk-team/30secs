using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Dto;

namespace WebApp.Services.Interfaces
{
    public interface IContactLogicService
    {
        /// <summary>
        /// возвращает все контакты текущего пользователя
        /// </summary>
        public Task<IEnumerable<ContactDto>> GetMyContacts(string userId);
    }
}