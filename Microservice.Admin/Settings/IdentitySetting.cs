namespace Microservice.Admin.Settings
{
    public class IdentitySetting
    {
        public const string SectionName = "Identity";
        public required string Address { get; set; }
        public required string BaseAddress { get; set; }
        public required IdentitySettingItems Admin { get; set; } = null!;
        public required IdentitySettingItems Web { get; set; } = null!;
    }
}
