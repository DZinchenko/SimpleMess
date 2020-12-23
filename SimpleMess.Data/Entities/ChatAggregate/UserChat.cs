using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleMess.Data.Entities
{
    public class UserChat
    {
        public int UserId { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}
