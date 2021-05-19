﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Dto;

namespace WebApp.Hubs
{
    public class ChatHub : Hub
    {
        public static Queue<LetterDto> Letters = new Queue<LetterDto>();
        
        /// <summary>
        /// Отправить всю инфу о символе
        /// </summary>
        public async Task Send(LetterDto letter)
        {
            Letters.Enqueue(letter);
            await Clients.Others.SendAsync(nameof(Send), letter);
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
    }
}


    



    
