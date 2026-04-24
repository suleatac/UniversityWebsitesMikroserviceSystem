namespace Microservice.Admin.Settings
{
    public class IdentitySetting
    {
        public const string SectionName = "Identity";
        public required string AdminUserAddress { get; set; }
        public required string Address { get; set; }
        public required string BaseAddress { get; set; }
        public required IdentitySettingItems Admin { get; set; } = null!;
        public required IdentitySettingItems WebAdmin { get; set; } = null!;
    }
}
