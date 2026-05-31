using Microsoft.AspNetCore.WebUtilities;

namespace AspNetCore.Boilerplate.Utils;

public class UrlBuilder(string url)
{
    private Dictionary<string, string>? _queries;

    public UrlBuilder WithQueries(Dictionary<string, string> queries)
    {
        _queries = queries;
        return this;
    }

    public string Build()
    {
        if (_queries is null)
        {
            return url;
        }

        return QueryHelpers.AddQueryString(
            url,
            _queries
                .AsEnumerable()
                .Select(item => new KeyValuePair<string, string?>(item.Key, item.Value))
        );
    }
}
