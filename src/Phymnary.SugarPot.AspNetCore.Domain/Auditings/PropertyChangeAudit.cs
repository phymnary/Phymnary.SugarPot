namespace Phymnary.SugarPot.AspNetCore.Domain.Auditings;

public interface IPropertyChangeAudit
{
    string EntityName { get; }

    string PropertyName { get; }

    string TypeName { get; }

    string EntityId { get; }

    string OldValue { get; }

    string NewValue { get; }

    Guid? ModifiedById { get; }

    DateTimeOffset ModifiedAt { get; }

    bool IsDeleted { get; }
}
