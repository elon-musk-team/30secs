using System;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Dto;
using WebApp.Hubs.Classes;
using WebApp.Services.Interfaces;

namespace WebApp.Hubs
{
    public class ChatHub : BaseAuthorizedHub
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ISymbolInMemoryStorageService _symbolStorageService;

        public ChatHub(ApplicationDbContext dbContext,
            ISymbolInMemoryStorageService symbolStorageService)
        {
            _dbContext = dbContext;
            _symbolStorageService = symbolStorageService;
        }

        public override async Task OnConnectedAsync()
        {
            var connectedUser = await _dbContext.Users.FindAsync(UserId);
            var userContacts = await _dbContext.UsersToContacts
                .Where(x => x.UserId == UserId)
                .Select(x => x.Contact)
                .ToListAsync();
            _symbolStorageService.ConnectedUsers.Add(connectedUser.ScreenName,
                new HubUser(Context.ConnectionId, UserId, connectedUser.ScreenName, userContacts));
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectedUser = await _dbContext.Users.FindAsync(UserId);
            _symbolStorageService.ConnectedUsers.Remove(connectedUser.ScreenName);
            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Отправить символ с инфой
        /// </summary>
        public async Task Send(SymbolDto letter)
        {
            if (!_symbolStorageService.ConnectedUsers.TryGetValue(letter.Receiver.ScreenName, out var receiverUser))
            {
                // не смогли получить юзера из коллекции, либо ошибка в коде, либо нас пытаются зломать
                return;
            }

            var symbols = _symbolStorageService.GetChatSymbols(UserId, receiverUser.UserId);
            symbols.Enqueue(letter);
            if (letter.Receiver.IsPrivate)
            {
                var receiverHubUser = _symbolStorageService.ConnectedUsers[letter.Receiver.ScreenName];
                if (receiverHubUser != null)
                {
                    await Clients.Client(receiverHubUser.ConnectionId).SendAsync(nameof(Send), letter);
                }
            }
            else
            {
                // беседа, не сделано
                await Clients.Others.SendAsync(nameof(Send), letter);
            }
        }

        /// <summary>
        /// макар прокомментируй
        /// </summary>
        public async Task GetStartedString(SymbolReceiverDto receiverDto)
        {
            if (!_symbolStorageService.ConnectedUsers.TryGetValue(receiverDto.ScreenName, out var receiverUser))
            {
                // не смогли получить юзера из коллекции, либо ошибка в коде, либо нас пытаются зломать
                return;
            }

            var symbols = _symbolStorageService.GetChatSymbols(UserId, receiverUser.UserId);
            if (symbols.Count > 0)
                await Clients.Caller.SendAsync("GetStartedString", new string(symbols.Select(s => s.Symbol).ToArray()));
        }

        public async Task CheckSymbols(SymbolReceiverDto receiverDto)
        {
            var count = 0;
            if (string.IsNullOrWhiteSpace(receiverDto.ScreenName))
            {
                // так чуть быстрее
                return;
            }
            if (!_symbolStorageService.ConnectedUsers.TryGetValue(receiverDto.ScreenName, out var receiverUser))
            {
                // не смогли получить юзера из коллекции, либо ошибка в коде, либо нас пытаются зломать
                return;
            }
            
            var symbols = _symbolStorageService.GetChatSymbols(UserId, receiverUser.UserId);
            while (symbols.Count > 0 && symbols.Peek().NeedDelete())
            {
                symbols.Dequeue();
                count++;
            }

            if (count > 0)
                await Clients.All.SendAsync("DeleteSymbols", count);
        }
    }
}