using System.Collections.Generic;
using System.Linq;

namespace WebApp.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }

        public AuthenticateResponse(User user, string token)
        {
            var roles = user.UserRoles.SelectMany(x => x.Role.Name);

            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserName = user.UserName;
            Email = user.Email;
            Roles = user.UserRoles != null ? user.UserRoles.Select(x => x.Role.Name) : null;
            Token = token;
        }
    }
}