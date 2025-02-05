using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocLink.Domain.Entities;
using DocLink.Domain.Responses.Genaric;

namespace DocLink.Domain.Interfaces.Services
{
    public interface IChatService
    {
        Task<BaseResponse<ChatMessage>> CreateMessageAsync(string senderUserId, string receiverUserId, string content);
        Task<BaseResponse<bool>> DeleteMessageAsync(int messageId, string userId);
        Task<BaseResponse<ChatMessage?>> EditMessageAsync(int messageId, string senderUserId, string newContent);
        Task<BaseResponse<ChatMessage?>> MarkMessageAsReadAsync(int messageId, string receiverUserId);
        Task<BaseResponse<IEnumerable<ChatMessage>>> GetChatHistoryAsync(string userId, string otherUserId);
        Task<BaseResponse<ChatMessage?>> GetMessageAsync(int messageId);
    }
}
