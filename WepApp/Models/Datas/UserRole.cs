using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }

}