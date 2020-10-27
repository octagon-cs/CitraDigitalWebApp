using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApp.Helpers;
using WebApp.Models;

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
        private readonly DataContext context;

        public UserService(IOptions<AppSettings> appSettings, DataContext _context)
        {
            _appSettings = appSettings.Value;
            context = _context;
        }


        public async Task<User> Register(User user)
        {
            try
            {
                user.Password = MD5Hash.ToMD5Hash(user.Password);
                context.Users.Add(user);
                await context.SaveChangesAsync();
                return user;
            }
            catch (System.Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {

            try
            {
                var users = context.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role).ToList();
                var user = users.Where(x => x.UserName == model.UserName && x.Password == MD5Hash.ToMD5Hash(model.Password)).FirstOrDefault();

                if (user == null)
                {
                    throw new SystemException("User Name Or Password Invalid !");
                }
                var token = generateJwtToken(user);

                return Task.FromResult(new AuthenticateResponse(user, token));
            }
            catch (System.Exception ex)
            {

                throw new SystemException(ex.Message);
            }
        }


        public Task<IEnumerable<User>> GetAll()
        {
            var users = context.Users.ToList();
            return Task.FromResult(users.AsEnumerable());
        }

        public Task<User> GetById(int id)
        {
            var result = context.Users.Where(x => x.Id == id).Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefault();
            return Task.FromResult(result);

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