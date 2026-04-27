namespace Microservice.Admin.Settings
{
    public class MicroservicesSetting
    {
        public const string SectionName = "Microservices";
        public required MikroserviceSettingItem Site { get; set; }
        public required MikroserviceSettingItem Personel { get; set; }
        public required MikroserviceSettingItem Ogrenci { get; set; }
    }
    public class MikroserviceSettingItem
    {
        public required string BaseUrl { get; set; }

    }
}
