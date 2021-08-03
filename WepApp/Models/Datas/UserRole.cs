using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
    }

}