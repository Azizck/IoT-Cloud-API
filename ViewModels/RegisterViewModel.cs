using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace packagesentinel.ViewModels
{
    public class RegisterViewModel
    {

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string PasswordConfirm { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid Serial Number")]
        public string SerialNumber { get; set; }

    }
}
