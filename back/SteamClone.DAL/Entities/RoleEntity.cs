using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SteamClone.DAL.Entities
{
    public class RoleEntity : IdentityRole
    {
        public List<UserRoleEntity> UserRoles { get; set; } = [];
    }
}
