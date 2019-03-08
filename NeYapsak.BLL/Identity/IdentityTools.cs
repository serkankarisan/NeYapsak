using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NeYapsak.DAL.Context;
using NeYapsak.Entity.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeYapsak.BLL.Identity
{
        public static class IdentityTools
        {
            public static UserStore<ApplicationUser> NewUserStore() => new UserStore<ApplicationUser>(new NeYapsakContext());
            public static UserManager<ApplicationUser> NewUserManager() => new UserManager<ApplicationUser>(NewUserStore());
            public static RoleStore<ApplicationRole> NewRoleStore() => new RoleStore<ApplicationRole>(new NeYapsakContext());
            public static RoleManager<ApplicationRole> NewRoleManager() => new RoleManager<ApplicationRole>(NewRoleStore());
        }
}
