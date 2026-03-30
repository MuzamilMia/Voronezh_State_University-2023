using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.DataAccess.Entities
{
    public class UserRole
    {
        public int UserRoleId { get; set; }
        public string RoleName { get; set; } = "";
        public string? Description { get; set; }
        public ICollection<User>? Users { get; set; }
    }
}
