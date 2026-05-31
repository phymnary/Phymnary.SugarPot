namespace Phymnary.SugarPot.AspNetCore.Api.ExceptionHandler;

public interface IAspErrorMessageProvider
{
    ValueTask<string> GetAsync(string code, CancellationToken cancellationToken = default);
}
