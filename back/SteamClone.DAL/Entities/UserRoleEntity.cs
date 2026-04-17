using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace SteamClone.DAL.Entities
{
    public class UserRoleEntity : IdentityUserRole<string>
    {
        public UserEntity? User { get; set; }
        public RoleEntity? Role { get; set; }
    }
}
