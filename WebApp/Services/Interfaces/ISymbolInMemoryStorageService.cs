using System.Collections.Generic;
using WebApp.Dto;
using WebApp.Hubs;
using WebApp.Hubs.Classes;

namespace WebApp.Services.Interfaces
{
    /// <summary>
    /// хранилище символов и юзеров для <see cref="ChatHub"/>.
    /// благодаря подключению как синглтон не теряет свои данные с каждым новым запросом 
    /// </summary>
    public interface ISymbolInMemoryStorageService
    {
        /// <summary>
        /// подключенные к хабу пользователи, ScreenName в качестве ключа
        /// </summary>
        public Dictionary<string, HubUser> ConnectedUsers { get; set; }
        
        /// <summary>
        /// символы чата, UserId в качестве ключей
        /// </summary>
        public Dictionary<string, Dictionary<string, Queue<SymbolDto>>> ScreenNamesToSymbols { get; set; }

        /// <summary>
        /// сопоставляет двух общающихся людей и символы в их чате.
        /// userIds в любом порядке
        /// </summary>
        public void SetChatSymbols(string userId1, string userId2, Queue<SymbolDto> symbolDtos);

        /// <summary>
        /// возвращает символы в чате двух общающихся людей, создает, если объекта нет
        /// userIds в любом порядке
        /// </summary>
        public Queue<SymbolDto> GetChatSymbols(string userId1, string userId2);
    }
}