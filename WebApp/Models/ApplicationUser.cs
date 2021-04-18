using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// свойство для создания связи один ко многим
        /// </summary>
        public List<UserToContact> UserToContactUsers { get; set; }
        
        /// <summary>
        /// свойство для создания связи один ко многим
        /// </summary>
        public List<UserToContact> UserToContactContacts { get; set; }
        
        /// <summary>
        /// никнейм/id/имя пользователя
        /// </summary>
        public string ScreenName { get; set; }
        
        /// <summary>
        /// ссылка на аватарку
        /// </summary>
        public string LinkToAvatar { get; set; }
    }
}
