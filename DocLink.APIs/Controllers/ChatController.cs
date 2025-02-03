using System.Security.Claims;
using DocLink.Application.Hubs;
using DocLink.Domain.DTOs.ChatDtos;
using DocLink.Domain.Entities;
using DocLink.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DocLink.APIs.Controllers
{
    public class ChatController : BaseController
    {
        private readonly IChatService _chatService;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(IChatService chatService, IHubContext<ChatHub> hubContext)
        {
            _chatService = chatService;
            _hubContext = hubContext;
        }


        private string GetUserId()
        {
            // مثال باستخدام ClaimTypes.NameIdentifier
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }


        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDto dto)
        {
            var senderUserId = GetUserId();
            if (string.IsNullOrEmpty(senderUserId))
                return Unauthorized("المستخدم غير مصدق.");

            // تخزين الرسالة في قاعدة البيانات
            var response = await _chatService.CreateMessageAsync(senderUserId, dto.ReceiverUserId, dto.Content);
            var message = response.Data;
            // إرسال الرسالة عبر SignalR للطرف المستقبل وللمُرسل أيضاً
            await _hubContext.Clients.User(senderUserId).SendAsync("MessageSent", message);
            await _hubContext.Clients.User(dto.ReceiverUserId).SendAsync("ReceiveMessage", message);

            return Ok(message);
        }


        [HttpGet("history/{otherUserId}")]
        public async Task<IActionResult> GetChatHistory(string otherUserId)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("المستخدم غير مصدق.");

            var history = await _chatService.GetChatHistoryAsync(userId, otherUserId);
            return Ok(history);
        }


        [HttpPut("edit/{messageId}")]
        public async Task<IActionResult> EditMessage(int messageId, [FromBody] EditMessageDto dto)
        {
            var senderUserId = GetUserId();
            if (string.IsNullOrEmpty(senderUserId))
                return Unauthorized("المستخدم غير مصدق.");

            var message = await _chatService.EditMessageAsync(messageId, senderUserId, dto.NewContent);
            var updatedMessage = message.Data;
            if (updatedMessage == null)
                return NotFound("الرسالة غير موجودة أو ليس لديك صلاحية التعديل.");

            // إخطار الطرفين بالتعديل
            await _hubContext.Clients.User(senderUserId).SendAsync("MessageEdited", updatedMessage);
            await _hubContext.Clients.User(updatedMessage.ReceiverUserId).SendAsync("MessageEdited", updatedMessage);

            return Ok(updatedMessage);
        }


        [HttpDelete("{messageId}")]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("المستخدم غير مصدق.");

            // نحاول جلب الرسالة للإخطار
            var messageResponse = await _chatService.GetMessageAsync(messageId);
            var message = messageResponse.Data;
            if (message == null)
                return NotFound("الرسالة غير موجودة.");

            var response = await _chatService.DeleteMessageAsync(messageId, userId);
            var deleted = response.Data;
            if (!deleted)
                return Forbid("ليس لديك صلاحية حذف الرسالة.");

            // إخطار الطرفين بحذف الرسالة
            await _hubContext.Clients.User(userId).SendAsync("MessageDeleted", messageId);
            await _hubContext.Clients.User(message.ReceiverUserId).SendAsync("MessageDeleted", messageId);

            return Ok();
        }


        [HttpPut("markAsRead/{messageId}")]
        public async Task<IActionResult> MarkAsRead(int messageId)
        {
            var receiverUserId = GetUserId();
            if (string.IsNullOrEmpty(receiverUserId))
                return Unauthorized("المستخدم غير مصدق.");

            var message = await _chatService.MarkMessageAsReadAsync(messageId, receiverUserId);
            var updatedMessage = message.Data;
            if (updatedMessage == null)
                return NotFound("الرسالة غير موجودة أو ليس لديك صلاحية تعديلها.");

            // إخطار المرسل بأن الرسالة تمت قراءتها
            await _hubContext.Clients.User(updatedMessage.SenderUserId).SendAsync("MessageRead", updatedMessage);

            return Ok(updatedMessage);
        }

    }
}
