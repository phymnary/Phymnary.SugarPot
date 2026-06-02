using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Phymnary.SugarPot.AspNetCore.Domain.Auditing;
using Phymnary.SugarPot.AspNetCore.Domain.Auditings;
using Phymnary.SugarPot.AspNetCore.Domain.Entities;
using Phymnary.SugarPot.AspNetCore.EntityFrameworkCore.Interceptors;
using Phymnary.SugarPot.AspNetCore.EntityFrameworkCore.Interceptors.Trackers;

namespace Phymnary.SugarPot.AspNetCore.EntityFrameworkCore;

public class EfServicesConfigurator<TDbContext>
    where TDbContext : DbContext
{
    private readonly IServiceCollection _services;

    private bool _canAuditPropertyChange;

    private readonly AuditingMetadata _auditingMetadata = new();

    internal EfServicesConfigurator(IServiceCollection services)
    {
        services.AddScoped<SoftDeleteInterceptor>();

        //if (EfFeatureFlags.IsMultiTenantEnable)
        //    services.AddScoped<IInterceptor, SetTenantOnSavingInterceptor>();

        //if (EfFeatureFlags.IsAuditingEnable)
        //    services.AddScoped<IInterceptor, AuditOnSavingInterceptor>();

        _services = services;
    }

    public EfServicesConfigurator<TDbContext> ConfigureAuditing(Action<AuditingMetadata> configure)
    {
        configure(_auditingMetadata);
        return this;
    }

    public EfServicesConfigurator<TDbContext> AddPropertyChangeAudit<TAuditDbContext, TAudit>(
        Func<PropertyChangeAudit, TAudit> mapper
    )
        where TAuditDbContext : DbContext
        where TAudit : PropertyChangeAudit, IEntity
    {
        _canAuditPropertyChange = true;
        _services
            .AddScoped<
                IEntityPropertyChangeTracker,
                EntityPropertyChangeTracker<TAuditDbContext, TAudit>
            >()
            .AddSingleton(new AuditingEntityMapper<PropertyChangeAudit, TAudit> { Map = mapper });

        _auditingMetadata.HasDifferentDbContextForAuditChanges =
            typeof(TAuditDbContext) != typeof(TDbContext);

        return this;
    }

    internal IServiceCollection Build()
    {
        if (!_canAuditPropertyChange)
            _services.AddSingleton<IEntityPropertyChangeTracker>(
                new EmptyEntityPropertyChangeTracker()
            );

        _services.AddSingleton(_auditingMetadata);

        return _services;
    }
}
