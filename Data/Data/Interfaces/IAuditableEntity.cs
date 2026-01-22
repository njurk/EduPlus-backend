namespace Data.Data.Interfaces
{
    public interface IAuditableEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
        int? ModifiedByUserId { get; set; }
    }
}
