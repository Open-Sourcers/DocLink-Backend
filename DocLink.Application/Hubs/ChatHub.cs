using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocLink.Domain.Entities;
using DocLink.Domain.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;

namespace DocLink.Application.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task SendMessage(string receiverUserId, string content)
        {
            var senderUserId = Context.UserIdentifier;
            if (senderUserId == null)
            {
                throw new HubException("User is not authenticated.");
            }

            // تخزين الرسالة في قاعدة البيانات
            var response = await _chatService.CreateMessageAsync(senderUserId, receiverUserId, content);
            var message = response.Data;

            // إرسال الرسالة للطرفين
            await Clients.User(senderUserId).SendAsync("MessageSent", message);
            await Clients.User(receiverUserId).SendAsync("ReceiveMessage", message);
        }
    }
}
