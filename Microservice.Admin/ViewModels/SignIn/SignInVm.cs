using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.SignIn

{
    public record SignInVm
    {

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; init; }


        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; init; }

       
    }
}
