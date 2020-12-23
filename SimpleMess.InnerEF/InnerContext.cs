using Microsoft.EntityFrameworkCore;
using System;
using SimpleMess.Data.Entities;

namespace SimpleMess.InnerEF
{
    public class InnerContext : DbContext 
    {
        private IDatabaseConfiguration _databaseConfig = new AndroidDatabaseConfiguration();
        
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserChat> UserChats { get; set; }
        public DbSet<GroupChat> GroupChats { get; set; }
        public DbSet<CurrUserInfo> CurrUserInfo { get; set; }

        public InnerContext()
        {
            if (!this.Database.EnsureCreated()) { }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_databaseConfig.GetDatabasePath()}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserChat>().HasKey(uc => new { uc.UserId, uc.ChatId });
            modelBuilder.Entity<UserChat>().HasOne(uc => uc.Chat).WithMany(c => c.UserChats).HasForeignKey(uc => uc.ChatId);

            modelBuilder.Entity<UserSeenMessage>().HasKey(usm => new { usm.UserId, usm.MessageId});
            modelBuilder.Entity<UserSeenMessage>().HasOne(usm => usm.Message).WithMany(u => u.UserSeenMessages).HasForeignKey(usm => usm.MessageId);

            modelBuilder.Entity<User>().Ignore(u => u.Password);
        }
    }
}