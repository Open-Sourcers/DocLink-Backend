using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.DTOs.ChatDtos
{
    public class SendMessageDto
    {
        public string ReceiverUserId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
