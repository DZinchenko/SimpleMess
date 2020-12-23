using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleMess.Data.Entities
{
    public class UserSeenMessage
    {
        public int UserId { get; set; }
        public int MessageId { get; set; }
        public Message Message { get; set; }
    }
}
