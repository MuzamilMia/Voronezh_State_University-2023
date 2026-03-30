using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.DataAccess.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Phone { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public int UserRoleId { get; set; }
        public UserRole? UserRole { get; set; }
        public ICollection<Book>? Books { get; set; } 
        public ICollection<Recipt>? Recipts { get; set; }
    }
}
