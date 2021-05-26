using System.Collections.Generic;
using WebApp.Dto;
using WebApp.Models;

namespace WebApp.Hubs.Classes
{
    /// <summary>
    /// устанавливает связь между id в signalr и публичным id
    /// </summary>
    public class HubUser
    {
        /// <param name="connectionId">id подключения в signalr</param>
        /// <param name="userId">id в идентити</param>
        /// <param name="screenName">публичный id</param>
        /// <param name="contacts">контакты</param>
        public HubUser(string connectionId, string userId, string screenName/*, List<ApplicationUser> contacts*/)
        {
            ConnectionId = connectionId;
            UserId = userId;
            ScreenName = screenName;
            // Contacts = contacts;
        }

        /// <summary>
        /// id подключения в signalr
        /// </summary>
        public string ConnectionId { get; init; }

        /// <summary>
        /// id в идентити
        /// </summary>
        public string UserId { get; init; }

        /// <summary>
        /// публичный id
        /// </summary>
        public string ScreenName { get; init; }

        /// <summary>
        /// контакты
        /// </summary>
        // public List<ApplicationUser> Contacts { get; init; }
    };
}