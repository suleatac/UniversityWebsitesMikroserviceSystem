using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Microservice.Admin.ViewModels.User
{
    public record UserAddVm
    {
        [JsonPropertyName("username")]
        [Display(Name = "User Name")]
        [Required(ErrorMessage = "UserName is required")]
        public string? Username { get; init; }

        [JsonPropertyName("enabled")]
        public bool Enabled { get; init; } = true;

        [JsonPropertyName("firstName")]
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Firstname is required")]
        public string? FirstName { get; init; }

        [JsonPropertyName("lastName")]
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Lastname is required")]
        public string? LastName { get; init; }

        [JsonPropertyName("email")]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; init; }

        [Display(Name = "Person Id")]
        [Required(ErrorMessage = "Person Id is required")]
        public int PersonId { get; init; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; init; }

        [Display(Name = "Password Confirm")]
        [Required(ErrorMessage = "PasswordConfirm is required")]
        [Compare(nameof(Password), ErrorMessage = "Password and PasswordConfirm must be the same")]
        [DataType(DataType.Password)]
        public string? PasswordConfirm { get; init; }

        [Display(Name = "Roller")]
        public List<string> SelectedRoles { get; init; } = new();

        public static UserAddVm Empty => new() {
            Username = string.Empty,
            Enabled = true,
            FirstName = string.Empty,
            LastName = string.Empty,
            Email = string.Empty,
            PersonId = 0,
            Password = string.Empty,
            PasswordConfirm = string.Empty,
            SelectedRoles = new List<string>()
        };

        public static UserAddVm GetExampleModel()
        {
            return new UserAddVm {
                Username = "suleatac",
                Enabled = true,
                FirstName = "Şule",
                LastName = "ATAÇ",
                Email = "suleatac@gmail.com",
                PersonId = 12345,
                Password = "123456789**",
                PasswordConfirm = "123456789**",
                SelectedRoles = new List<string>()
            };
        }
    }

    public class KeycloakUserCreateRequest
    {
        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }

        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("attributes")]
        public Dictionary<string, string[]> Attributes { get; set; }
            = new();

        [JsonPropertyName("credentials")]
        public List<KeycloakCredential> Credentials { get; set; }
            = new();
    }

    public class KeycloakCredential
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "password";

        [JsonPropertyName("value")]
        public string? Value { get; set; }

        [JsonPropertyName("temporary")]
        public bool Temporary { get; set; } = false;
    }
}