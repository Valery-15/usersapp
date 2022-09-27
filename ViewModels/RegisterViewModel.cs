using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace UsersApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "e-mail")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Field {0} must have at least {2} and maximum {1} characters.", MinimumLength = 1)]
        [Display(Name = "password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords don't match.")]
        [DataType(DataType.Password)]
        [Display(Name = "confirm password")]
        public string PasswordConfirm { get; set; }
    }
}
