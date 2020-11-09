using System;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Helpers;
using WebApp.Models;
using WebApp.Proxy.Domains;
using WebApp.Services;

namespace WebApp.Proxy
{
    public class UserProxy
    {


        public static IAdministrator GetAdministratorProxy(IUserService userService)
        {
            var context = GetServiceProvider.Instance.GetRequiredService<DataContext>();
            return new Administrator(userService, context);
        }

        public static ICompanyAdministrator GetCompanyAdministratorProxy()
        {
            return new CompanyAdministrator();
        }

        internal static IGateAdministrator GetGateProxy()
        {
            return new GateAdministrator();
        }

        public static IApproval GetApprovalProxy(User userLogin)
        {

            return new Approval(userLogin);
        }

    }
}