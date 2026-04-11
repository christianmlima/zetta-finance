namespace Zetta.SharedKernel.Primitives;

public abstract class BaseEntity
{
    protected BaseEntity(Guid id)
    {
        Id = id;
    }
    
    protected BaseEntity() { }

    public Guid Id { get; private set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public bool IsDeleted => DeletedAt.HasValue;

    public void SetUpdatedAt() => UpdatedAt = DateTime.UtcNow;
    public void SoftDelete() => DeletedAt = DateTime.UtcNow;
}
