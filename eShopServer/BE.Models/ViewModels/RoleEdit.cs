using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using BE.Models.Entities;

namespace BE.Models.ViewModels
{
    public class RoleEdit
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<AppUser> Members { get; set; }
        public IEnumerable<AppUser> NonMembers { get; set; }
    }
}
