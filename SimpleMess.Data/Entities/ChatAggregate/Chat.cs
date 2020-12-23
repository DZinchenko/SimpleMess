using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleMess.Data.Entities
{
    public class Chat
    {
        public int Id { get; set; }
        public List<UserChat> UserChats { get; set; }
    }

    public class GroupChat : Chat
    {
        public string Name { get; set; }
        public byte[] Picture { get; set; }
    }
}
