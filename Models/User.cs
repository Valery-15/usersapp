using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UsersApp.Models
{
    public class User : IdentityUser
    {
        public DateTime? LastLoginTime { get; set; }

        [Required]
        public DateTime ReqistrationTime { get; set; }

        [Required]
        [MaxLength(10)]
        public String Status { get; set; }
    }
}
