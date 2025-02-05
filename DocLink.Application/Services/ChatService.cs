using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocLink.Domain.Entities;
using DocLink.Domain.Interfaces.Repositories;
using DocLink.Domain.Interfaces.Services;
using DocLink.Domain.Responses.Genaric;
using DocLink.Domain.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DocLink.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public ChatService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<BaseResponse<ChatMessage>> CreateMessageAsync(string senderUserId, string receiverUserId, string content)
        {
            var message = new ChatMessage
            {
                SenderUserId = senderUserId,
                ReceiverUserId = receiverUserId,
                Content = content
            };

            _unitOfWork.Repository<ChatMessage, int>().AddAsync(message);
            await _unitOfWork.SaveAsync();
            return new BaseResponse<ChatMessage>(message, "Message sent!");
        }

        public async Task<BaseResponse<bool>> DeleteMessageAsync(int messageId, string userId)
        {
            var message = await _unitOfWork.Repository<ChatMessage, int>().GetByIdAsync(messageId);
            if (message == null || message.SenderUserId != userId)
            {
                return new BaseResponse<bool> (false, "User hasn't permision to delete", null, StatusCodes.Status403Forbidden); // Only the sender can delete.
            }

            _unitOfWork.Repository<ChatMessage, int>().Remove(message);
            await _unitOfWork.SaveAsync();
            return new BaseResponse<bool>(true, "Message deleted!");
        }

        public async Task<BaseResponse<ChatMessage?>> EditMessageAsync(int messageId, string senderUserId, string newContent)
        {
            var message = await _unitOfWork.Repository<ChatMessage, int>().GetByIdAsync(messageId);
            if (message == null || message.SenderUserId != senderUserId)
            {
                return new BaseResponse<ChatMessage?>("Error has been occured during edit this message", StatusCodes.Status400BadRequest);
            }

            message.Content = newContent;
            message.EditedAt = DateTime.UtcNow;
            await _unitOfWork.SaveAsync();
            return new BaseResponse<ChatMessage?>(message, "Message edited!");
        }

        public async Task<BaseResponse<IEnumerable<ChatMessage>>> GetChatHistoryAsync(string userId, string otherUserId)
        {
            var spec = new ChatMessagesHistoryWithSpec(userId, otherUserId);
            var chat = await _unitOfWork.Repository<ChatMessage, int>().GetAllWithSpecAsync(spec);

            return new BaseResponse<IEnumerable<ChatMessage>>(chat);
        }

        public async Task<BaseResponse<ChatMessage?>> GetMessageAsync(int messageId)
        {
            var message = await _unitOfWork.Repository<ChatMessage, int>().GetByIdAsync(messageId);
            return new BaseResponse<ChatMessage?>(message);
        }

        public async Task<BaseResponse<ChatMessage?>> MarkMessageAsReadAsync(int messageId, string receiverUserId)
        {
            var message = await _unitOfWork.Repository<ChatMessage, int>().GetByIdAsync(messageId);
            if (message == null || message.ReceiverUserId != receiverUserId)
            {
                return new BaseResponse<ChatMessage?>("Error has been occured while marked the message", StatusCodes.Status400BadRequest);
            }

            message.IsRead = true;
            await _unitOfWork.SaveAsync();
            return new BaseResponse<ChatMessage?>(message, "Message edited!");
        }
    }
}
