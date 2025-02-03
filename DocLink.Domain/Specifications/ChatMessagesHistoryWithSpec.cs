using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocLink.Domain.Entities;

namespace DocLink.Domain.Specifications
{
    public class ChatMessagesHistoryWithSpec : BaseSpecification<ChatMessage, int>
    {
        public ChatMessagesHistoryWithSpec(string userId, string otherUserId) : base(
            m => (m.SenderUserId == userId && m.ReceiverUserId == otherUserId) ||
                 (m.SenderUserId == otherUserId && m.ReceiverUserId == userId))
        {
            AddOrder(x => x.Timestamp);
        }
    }
}
