using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApp;
using WebApp.Helpers;
using WebApp.Models;

namespace WepApp.Helpers
{
    public static class EmailHelper
    {

        public static string templateHeader = @"<!DOCTYPE html>
        <html>
        <head>
            <title></title>
            <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
            <meta name='viewport' content='width=device-width, initial-scale=1'>
            <meta http-equiv='X-UA-Compatible' content='IE=edge' />
            <style type='text/css'>
                @media screen {
                    @font-face {
                        font-family: 'Lato';
                        font-style: normal;
                        font-weight: 400;
                        src: local('Lato Regular'), local('Lato-Regular'), url(https://fonts.gstatic.com/s/lato/v11/qIIYRU-oROkIk8vfvxw6QvesZW2xOQ-xsNqO47m55DA.woff) format('woff');
                    }
                }

                /* CLIENT-SPECIFIC STYLES */
                body,
                table,
                td,
                a {
                    -webkit-text-size-adjust: 100%;
                    -ms-text-size-adjust: 100%;
                }

                img {
                    -ms-interpolation-mode: bicubic;
                }

                /* RESET STYLES */
                img {
                    border: 0;
                    height: auto;
                    line-height: 100%;
                    outline: none;
                    text-decoration: none;
                }
                body {
                    height: 100vh !important;
                    font-family: 'Lato';
                    margin: 0 !important;
                    width: 100% !important;
                }

                /* iOS BLUE LINKS */
                a[x-apple-data-detectors] {
                    color: inherit !important;
                    text-decoration: none !important;
                    font-size: inherit !important;
                    font-family: inherit !important;
                    font-weight: inherit !important;
                    line-height: inherit !important;
                }

                /* MOBILE STYLES */
                @media screen and (max-width:600px) {
                    h1 {
                        font-size: 32px !important;
                        line-height: 32px !important;
                    }
                }

                /* ANDROID CENTER FIX */
                div[style*='margin: 16px 0;'] {
                    margin: 0 !important;
                }

                .mail-body {
                    width: 600px;
                    margin: auto;
                    margin-top: 20px;
                    padding: 30px;
                    border-radius: 20px;
                    background-color: white;
                }


                .inputData {
                    width: 100%;
                    line-height: 0px;
                    border-bottom: 0.5px silver solid;
                    margin-bottom: 30px;
                }

                .inputData>label {
                    font-size: smaller;
                }

                .btn {
                    width: 50%;
                    background: #acc42a;
                    font-size: 17px;
                    color: #ffffff;
                    text-decoration: none;
                    padding: 7px 25px;
                    border-radius: 30px;
                    border: 1px solid #00a8e6;
                    display: inline-block;
                    margin: auto;
                    text-align: center;
                }

                .logo {
                    width: 40%;
                }

                .footer {
                    text-align: center;
                    width: 100%;
                    margin: auto;
                }
            </style>
        </head>

        <body style='background-color: #00a8e6; padding:30px'>
            <div class='mail-body'>
                <div class='header'>
                    <img src='https://kimprt.com/assets/images/Logo.png' class='logo' />
                   ";
        
        private static string footerTemplate= @"
                </div>
                <div class='footer'>
                    <div>
                        <a href='[confirm]' target='_blank' class='btn'>[btnTitle] </a>
                    </div>
                    <div><h3>PERTAMINA JAYAPURA </h3></div>
                </div>
            </div>
        </body>
        </html>";


        public static string GetUserCreatedTemplate(string url, User user, string password)
        {
            var template= templateHeader+ @" <h2>[judul]</h2>
            <div class='inputData'>
                <label>Nama</label>
                <h4>[nama]</h4>
            </div>
            <div class='inputData'>
                <label>User Name</label>
                <h4>[username]</h4>
            </div>
            <div class='inputData'>
                <label>Email</label>
                <h4>[email]</h4>
            </div>
            <div class='inputData'>
                <label>Password</label>
                <h4>[password]</h4>
            </div>"
            +footerTemplate;

            template = template
                .Replace("[nama]", user.FullName)
                .Replace("[username]", user.UserName)
                .Replace("[email]", user.Email)
                .Replace("[password]", password)
                .Replace("[judul]", "Confirm Account !")
                .Replace("[confirm]", url)
                .Replace("[btnTitle]", "Confirm Email !");
            return template;
        }




        public static string CreateNotification(string message)
        {
            var template = templateHeader +message
            + @"
                </div>
                <div class='footer'>
                    <div><h3>PERTAMINA JAYAPURA </h3></div>
                </div>
            </div>
        </body>
        </html>";
            return template;
        }







        internal static string ConfirmEmailTemplate(string url, User user, string token)
        {
            throw new NotImplementedException();
        }

        public static Task<bool> ValidateToken(string token, AppSettings _appSettings, DataContext db)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                if (userId > 0)
                {
                    var user = db.Users.Where(x => x.Id == userId);
                    if (user != null)
                    {
                        return Task.FromResult(true);
                    }
                }

                return Task.FromResult(false);


            }
            catch
            {
                return Task.FromResult(false);
            }
        }


        public static string GenerateJwtToken(User user, AppSettings _appSettings)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public static string ChangePasswordTemplate(string url)
        {

          var template=  templateHeader + @" <h2>[judul]</h2>
            <h4>Silahkan Reset Password Anda !</h4>"
            + footerTemplate ;
            template = template.Replace("[judul]","Reset Password !").Replace("[confirm]", url).Replace("[btnTitle]","Reset Password !");
            return template;

        }

    }

}
