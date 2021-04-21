using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApp.Hubs
{
    public class ChatHub : Hub
    {
        public async Task Send(char letter)
        {

            await this.Clients.Others.SendAsync("Send", letter);


        }


    }
}
