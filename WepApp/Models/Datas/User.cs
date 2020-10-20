using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using WebApp.Helpers;

namespace WebApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
        public List<Role> Roles { get; set; }
        public bool Status { get; set; }
        public UserType UserType
        {
            get
            {
                if (Roles != null && Roles.Count() > 0)
                {
                    Role role = Roles.FirstOrDefault();
                    return (UserType)Enum.Parse(typeof(UserType), role.Name);
                }

                return UserType.None;
            }
        }


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