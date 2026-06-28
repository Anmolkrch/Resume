using System.ComponentModel.DataAnnotations;

namespace TastehubModel.ViewModel
{
    /// <summary>
    /// Login View Model
    /// </summary>
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Always stay signed in")]
        public bool RememberMe { get; set; }


        public string PasswordHash { get; set; }
        public long LocationId { get; set; }
    }
}
