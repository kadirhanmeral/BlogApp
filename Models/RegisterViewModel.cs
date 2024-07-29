using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class RegisterViewModel
    {

        [Required]
        [Display(Name = "Username")]
        public string? UserName { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string? Name { get; set; }
        [Required]
        [Display(Name = "Surname")]
        public string? Surname { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Mail")]
        public string? Email { get; set; }


        [Required]
        [StringLength(10, ErrorMessage = "{0} space must be at least 10 character length.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]

        public string? Password { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "{0} space must be at least 10 character length.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat password")]
        [Compare("Password", ErrorMessage = "Passwords doesn't match")]

        public string? PasswordConfirm { get; set; }

    }
}