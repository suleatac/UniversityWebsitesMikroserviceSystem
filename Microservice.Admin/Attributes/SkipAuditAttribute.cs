namespace Microservice.Admin.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SkipAuditAttribute : Attribute
    {
    }
}
