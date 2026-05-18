namespace Microservice.Admin.Attributes
{
    public class AuditLogAttribute : Attribute
    {
        public string Description { get; }
        public AuditLogAttribute(string description) => Description = description;
    }
}
