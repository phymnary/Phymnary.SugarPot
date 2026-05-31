namespace AspNetCore.Boilerplate.Domain;

public interface ISoftDelete
{
    Guid? DeletedById { get; set; }

    DateTimeOffset? DeletedAt { get; set; }

    public void UndoDelete()
    {
        DeletedAt = null;
        DeletedById = null;
    }

    public void SoftDeleted()
    {
        DeletedAt = DateTimeOffset.MaxValue;
    }
}
