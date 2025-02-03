using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Entities
{
    public class ChatMessage : BaseEntity<int>
    {
        [Required]
        public string SenderUserId { get; set; } = default!;

        [Required]
        public string ReceiverUserId { get; set; } = default!;

        [Required]
        public string Content { get; set; } = default!;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; }

        public DateTime? EditedAt { get; set; }
    }
}
