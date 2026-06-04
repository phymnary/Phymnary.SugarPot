namespace Phymnary.SugarPot.AspNetCore;

internal class HttpContextAbortedProvider : IAbortedToken
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
