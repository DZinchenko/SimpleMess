using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleMess.Data.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; }
        public int ChatId { get; set; }
        public DateTime Time { get; set; }
        public List<UserSeenMessage> UserSeenMessages { get; set; }
    }

    public class PictureMessage : Message
    {
        public byte[] Picture { get; set; }
    }

    public class VideoMessage : Message
    {
        public byte[] Video { get; set; }
    }
}
