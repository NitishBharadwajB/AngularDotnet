using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {
        // public int Id { get; set; }
        // public string UserName { get; set; }
        // public byte[] PasswordHash { get; set; }
        // public byte[] PasswordSalt { get; set; }
        
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        //Alias Name 
        public string KnownAs { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        
        public string Intrests { get; set; }
        public string City { get; set; }
        
        public string Country { get; set; }
        
        public ICollection<Photo> Photos { get; set; }

        public ICollection<UserLike> LikeByUsers { get; set; }
        public ICollection<UserLike> LikedUsers { get; set; }

        public ICollection<Message> MessagesSent { get; set; }
        
        public ICollection<Message> MessagesRecieved { get; set; }
        
        public ICollection<AppUserRole> AppUserRoles { get; set; }
    }
}
