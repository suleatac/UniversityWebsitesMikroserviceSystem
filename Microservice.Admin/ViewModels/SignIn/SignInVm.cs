using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.SignIn

{
    public record SignInVm
    {

        [Display(Name = "Usernmae")]
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; init; }


        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; init; }

       
    }
}
