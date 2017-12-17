using System.ComponentModel.DataAnnotations;

namespace BillZip.Dtos
{
    public class NewUserDto
    {
        [EmailAddress, Required]
        public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, Compare("Password", ErrorMessage = "Passwords must match.")]
        public string ConfirmPassword { get; set; }
    }
}
