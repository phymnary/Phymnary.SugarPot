using Phymnary.SugarPot.DependencyInjection;

namespace Phymnary.SugarPot.AspNetCore.IntegrationTests;

interface IEmailService
{
}

[Service(Lifetime.Singleton)]
internal class EmailService : IEmailService
{
}
