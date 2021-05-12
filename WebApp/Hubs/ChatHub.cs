using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApp.Hubs
{
    public class ChatHub : Hub
    {
        public static Queue<Letter> Letters = new Queue<Letter>();


        public async Task Send(string author, char letter)
        {
            Letters.Enqueue(new Letter(author, letter));
            await Clients.Others.SendAsync("Send", letter);
        }

        public async Task GetStartedString()
        {
            if (Letters.Count > 0)
                await Clients.Caller.SendAsync("GetStartedString", new string(Letters.Select(s => s.Symbol).ToArray()));
        }

        public async Task CheckSymbols()
        {
            var count = 0;
            while (Letters.Count > 0 && Letters.Peek().NeedDelete())
            {
                Letters.Dequeue();
                count++;
            }
            if (count > 0)
                await Clients.All.SendAsync("DeleteSymbols", count);
        }
        
        /// <summary>
        /// Отправить всю инфу о символе (не тестил)
        /// </summary>
        public async Task SendFullLetter(Letter letter)
        {
            Letters.Enqueue(letter);
            await Clients.Others.SendAsync(nameof(SendFullLetter), letter);
        }
    }

    

    public class Letter  // не забыть перенести в отдельный файл
    {
        public string Author { get; set; }

        public char Symbol { get; set; }

        private DateTime ShelfLife;

        public Letter(string author, char symbol)
        {
            Author = author;
            Symbol = symbol;
            ShelfLife = DateTime.Now.AddSeconds(30);
        }

        public bool NeedDelete() => ShelfLife <= DateTime.Now;
    }

    
}


    



    
