using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.Profile
{
    public class ProfileVm
    {
        public string Id { get; set; } = default!;
        public string UserName { get; set; } = default!;
        
        [Display(Name = "Ad")]
        [Required(ErrorMessage = "Ad zorunludur")]
        public string FirstName { get; set; } = default!;

        [Display(Name = "Soyad")]
        [Required(ErrorMessage = "Soyad zorunludur")]
        public string LastName { get; set; } = default!;

        [Display(Name = "E-posta")]
        [Required(ErrorMessage = "E-posta zorunludur")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi")]
        public string Email { get; set; } = default!;

        [Display(Name = "Profil Resmi URL")]
        [Url(ErrorMessage = "Geçerli bir URL giriniz")]
        public string? ProfileImageUrl { get; set; }

        public bool Enabled { get; set; }
    }
}