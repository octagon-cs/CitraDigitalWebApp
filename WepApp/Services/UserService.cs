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
using WepApp.Helpers;

namespace WebApp.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);
        Task<User> Register(string url, string roleName, User user);
        Task<bool> UpdateUser(int id, User user);
        Task<bool> ChangePassword(User user, ChangePasswordModel email);
        Task<bool> ForgotPassword(string url, string email);
        Task<bool> ConfirmEmail(User user);
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private readonly AppSettings _appSettings;
        private readonly DataContext context;
        private readonly IEmailService _mailService;

        public UserService(IOptions<AppSettings> appSettings, DataContext _context, IEmailService mailService)
        {
            _appSettings = appSettings.Value;
            context = _context;
            _mailService = mailService;
        }


        public async Task<User> Register(string url, string roleName, User user)
        {
            try
            {
                var newPassword = MD5Hash.ToMD5Hash(DateTime.Now.Ticks.ToString()).Substring(1, 6);
                user.Password = MD5Hash.ToMD5Hash(newPassword);
                var role = context.Roles.Where(x => x.Name == roleName).FirstOrDefault();
                user.UserRoles.Add(new UserRole { Role = role });
                user.Status = false;
                context.Users.Add(user);
                await context.SaveChangesAsync();

                var token = EmailHelper.GenerateJwtToken(user, _appSettings);
                var template = EmailHelper.GetUserCreatedTemplate(url+$"/#!/account/confirmemail/{token}", user, newPassword);
                var sended = await _mailService.SendEmail(new MailRequest { ToEmail = user.Email, Subject = "Confirm Account", Body = template });
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
                var hastPassword = MD5Hash.ToMD5Hash(model.Password);
                var user = context.Users.Where(x => x.UserName == model.UserName && x.Password == hastPassword).Include(x => x.UserRoles).ThenInclude(x => x.Role).SingleOrDefault();

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
            var users = context.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role).ToList();
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

        public async Task<bool> UpdateUser(int id, User user)
        {
            try
            {
                var dataUSer = context.Users.Where(x => x.Id == id).SingleOrDefault();
                if (dataUSer == null)
                    throw new SystemException("User Not Found !");

                dataUSer.FirstName = user.FullName;
                dataUSer.LastName = user.LastName;
                dataUSer.Status = user.Status;
                var result = await context.SaveChangesAsync();

                if (result <= 0)
                    throw new SystemException("Data Not Saved .... !");

                var sended = await _mailService.SendEmail(new MailRequest { ToEmail = "ocph23@gmail.com", Subject = "Chage User", Body = "User Change" });

                return true;
            }
            catch (System.Exception ex)
            {

                throw new SystemException(ex.Message);
            }
        }

        public async Task<bool> ChangePassword(User user, ChangePasswordModel email)
        {
            try
            {
                var dataUSer = context.Users.Where(x => x.Id == user.Id).SingleOrDefault();
                if (dataUSer == null)
                    throw new SystemException("User Not Found !");

                if (dataUSer.Password == MD5Hash.ToMD5Hash(email.Password))
                    throw new SystemException("Password yang anda masukkan sudah pernah digunakan!");
                dataUSer.Password = MD5Hash.ToMD5Hash(email.Password);
                var result = await context.SaveChangesAsync();

                if (result <= 0)
                    throw new SystemException("Data Not Saved .... !");
                return true;
            }
            catch (System.Exception ex)
            {

                throw new SystemException(ex.Message);
            }
        }

        public async Task<bool> ForgotPassword(string url, string email)
        {
            try
            {
                var user = context.Users.SingleOrDefault(x => x.Email == email);
                if (user == null)
                    throw new SystemException("Account Anda Tidak Ditemukan !");
                if (!user.Status)
                    throw new SystemException("Account Anda Tidak Aktif, Silahkan hubungi Administrator !");

                var token = EmailHelper.GenerateJwtToken(user, _appSettings);
                var template = EmailHelper.ChangePasswordTemplate($"{url}/#!/account/changepassword/{token}");
                var sendedEmail = await _mailService.SendEmail(new MailRequest { Body = template, Subject = "Change Password", ToEmail = email });
                return true;

            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

         public async Task<bool> ConfirmEmail(User user)
        {
            try
            {
                var dataUSer = context.Users.Where(x => x.Id == user.Id).SingleOrDefault();
                if (dataUSer == null)
                    throw new SystemException("User Tidak Ditemukan !");

                dataUSer.Status = true;
                var result = await context.SaveChangesAsync();

                if (result <= 0)
                    throw new SystemException("Data Tidak Tersimpan .... !");
                return true;
            }
            catch (System.Exception ex)
            {

                throw new SystemException(ex.Message);
            }
        }
    }
}