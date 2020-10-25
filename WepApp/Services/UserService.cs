using Dapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApp.DataStores;
using WebApp.Helpers;
using WebApp.Models;
using WebApp.Proxy;

namespace WebApp.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);

        Task<User> Register(User user);
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }


        public async Task<User> Register(User user)
        {
            try
            {
                user.Password = MD5Hash.ToMD5Hash(user.Password);
                IDataStores<User> store = new UserDataStore();
                var result = await store.InsertAndGetLastId(user);
                if (result != null)
                    return result;
                throw new SystemException("Registration Invalid ...!");

            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            await Task.Delay(1);
            var dataStore = new UserDataStore();
            var userRoleStrore = new UserRoleDataStore();
            var users = await dataStore.Get();
            var user = users.Where(x => x.UserName == model.UserName && x.Password == MD5Hash.ToMD5Hash(model.Password)).FirstOrDefault();
           if(users==null || users.Count()<=0)
           {
                await RegisterAdmin();
           }
           
            if (user == null)
            {
                return null;
            }
            user.Roles = await user.GetRoles();
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        private async Task RegisterAdmin()
        {
            var model = new User { UserName = "Administrator", Password = "Admin123", 
            FirstName="Administrator", Status=true };
            var userDataStore = new UserDataStore();
            model.Password = MD5Hash.ToMD5Hash(model.Password);
            var data = await userDataStore.InsertAndGetLastId(model);
            if (data.Id > 0)
            {
                var userinRole = new UserRoleDataStore();
                var result = await userinRole.AddUserRole(data.Id,"administrator");
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var dataStore = new UserDataStore();
            var users = await dataStore.Get();
            return users;
        }

        public async Task<User> GetById(int id)
        {
            var dataStore = new UserDataStore();
            var result = await dataStore.GetById(id);
            return result;

        }

        // helper methods

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}