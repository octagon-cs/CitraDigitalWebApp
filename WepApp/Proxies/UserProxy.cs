using WebApp.Helpers;
using WebApp.Models;
using WebApp.Proxy.Domains;
using WebApp.Services;

namespace WebApp.Proxy
{
    public  class UserProxy{
        public static IAdministrator GetAdministratorProxy(IUserService userService){
                     return new Administrator(userService);
        }

        public static ICompanyAdministrator GetCompanyAdministratorProxy(){
                    return new CompanyAdministrator();
        }

        public static IApproval GetApprovalProxy(User userLogin){

                    return new Approval(userLogin);
        }

    }
}