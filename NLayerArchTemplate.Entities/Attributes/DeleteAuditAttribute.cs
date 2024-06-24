namespace NLayerArchTemplate.Entities.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class DeleteAuditAttribute : Attribute
{
    public bool IsSoftDelete { get; }
    public DeleteAuditAttribute(bool isSoftDelete = true)
    {
        IsSoftDelete = isSoftDelete;
    }
}
