using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using WebApp.Helpers;

namespace WebApp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
        public IList<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public bool Status { get; set; }

        [NotMapped]
        public UserType UserType
        {
            get
            {
                if (UserRoles != null && UserRoles.Count() > 0)
                {
                    Role role = UserRoles.FirstOrDefault().Role;
                    return (UserType)Enum.Parse(typeof(UserType), role.Name);
                }

                return UserType.None;
            }
        }

        [NotMapped]
        public string FullName
        {
            get
            {
                var fullName = $"{FirstName} {LastName}";
                return fullName.Trim();
            }
        }
    }
}