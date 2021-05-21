using System.Collections.Generic;
using WebApp.Dto;
using WebApp.Hubs.Classes;
using WebApp.Services.Interfaces;

namespace WebApp.Services
{
    /// <inheritdoc />
    public class SymbolInMemoryStorageService : ISymbolInMemoryStorageService
    {
        /// <inheritdoc />
        public Dictionary<string, HubUser> ConnectedUsers { get; set; } = new();

        /// <inheritdoc />
        public Dictionary<string, Dictionary<string, Queue<SymbolDto>>> ScreenNamesToSymbols { get; set; } = new();

        /// <inheritdoc />
        public void SetChatSymbols(string screenName1, string screenName2, Queue<SymbolDto> symbolDtos)
        {
            ScreenNamesToSymbols.TryAdd(screenName1, new Dictionary<string, Queue<SymbolDto>>());
            ScreenNamesToSymbols.TryAdd(screenName2, new Dictionary<string, Queue<SymbolDto>>());
            if (!ScreenNamesToSymbols[screenName1].TryAdd(screenName2, symbolDtos))
            {
                ScreenNamesToSymbols[screenName1][screenName2] = symbolDtos;
            }

            if (!ScreenNamesToSymbols[screenName2].TryAdd(screenName1, symbolDtos))
            {
                ScreenNamesToSymbols[screenName2][screenName1] = symbolDtos;
            }
        }

        /// <inheritdoc />
        public Queue<SymbolDto> GetChatSymbols(string screenName1, string screenName2)
        {
            if (!ScreenNamesToSymbols.TryGetValue(screenName1, out var row))
            {
                var createdQueue1 = new Queue<SymbolDto>();
                SetChatSymbols(screenName1, screenName2, createdQueue1);
                return createdQueue1;
            }

            if (!row.TryGetValue(screenName2, out _))
            {
                var createdQueue2 = new Queue<SymbolDto>();
                SetChatSymbols(screenName2, screenName1, createdQueue2);
                return createdQueue2;
            }

            return ScreenNamesToSymbols[screenName1][screenName2];
        }
    }
}