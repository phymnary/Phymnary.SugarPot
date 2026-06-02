namespace Phymnary.SugarPot.AspNetCore.Api;

internal class HttpContextAbortedProvider : IAbortedProvider
{
    private CancellationToken _token;

    public void Set(CancellationToken token)
    {
        _token = token;
    }

    public CancellationToken Get(CancellationToken cancellationToken)
    {
        return cancellationToken == default ? _token : cancellationToken;
    }
}
