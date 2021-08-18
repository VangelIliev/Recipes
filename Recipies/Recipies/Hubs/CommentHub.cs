using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipies.Hubs
{
    public class CommentHub : Hub
    {
        public async Task SendMessage(string user, string message, string recipeName)
        {
            await Clients.Others.SendAsync("ReceiveMessage", user, message, recipeName);
        }
    }
}
