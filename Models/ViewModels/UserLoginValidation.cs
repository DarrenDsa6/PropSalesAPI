using System.ComponentModel.DataAnnotations;

namespace PropertySales.Models.ViewModels
{
    public class UserLoginValidation
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
