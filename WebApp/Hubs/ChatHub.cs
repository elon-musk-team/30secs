using System;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using WebApp.Data;
using WebApp.Dto;
using WebApp.Hubs.Classes;
using WebApp.Services.Interfaces;

namespace WebApp.Hubs
{
    public class ChatHub : BaseAuthorizedHub
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly ISymbolInMemoryStorageService _symbolStorageService;

        public ChatHub(ApplicationDbContext dbContext,
            ILogger logger,
            ISymbolInMemoryStorageService symbolStorageService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _symbolStorageService = symbolStorageService;
        }

        public override async Task OnConnectedAsync()
        {
            var connectedUser = await _dbContext.Users.FindAsync(UserId);
            var hubUser = new HubUser(Context.ConnectionId, UserId, connectedUser.ScreenName);
            _logger.Information("connecting user {@HubUser}", hubUser);
            _symbolStorageService.ConnectedUsers.Add(connectedUser.ScreenName, hubUser);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectedUser = await _dbContext.Users.FindAsync(UserId);
            var hubUser = _symbolStorageService.ConnectedUsers[connectedUser.ScreenName];
            _logger.Information("disconnecting user {@HubUser}", hubUser);
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
                // не смогли получить юзера из коллекции, либо ошибка в коде, либо нас пытаются взломать
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
        
        public async Task GetStartedString(SymbolReceiverDto receiverDto)
        {
            if (!_symbolStorageService.ConnectedUsers.TryGetValue(receiverDto.ScreenName, out var receiverUser))
            {
                // не смогли получить юзера из коллекции, либо ошибка в коде, либо нас пытаются взломать
                return;
            }

            var symbols = _symbolStorageService.GetChatSymbols(UserId, receiverUser.UserId);
            if (symbols.Count > 0)
                await Clients.Caller.SendAsync("GetStartedString", symbols);
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
                // не смогли получить юзера из коллекции, либо ошибка в коде, либо нас пытаются взломать
                return;
            }
            
            var symbols = _symbolStorageService.GetChatSymbols(UserId, receiverUser.UserId);
            while (symbols.Count > 0 && symbols.Peek().NeedDelete())
            {
                symbols.Dequeue();
                count++;
            }

            if (count > 0)
                await Clients.Clients(receiverUser.ConnectionId, Context.ConnectionId).SendAsync("DeleteSymbols", count);
        }
    }
}