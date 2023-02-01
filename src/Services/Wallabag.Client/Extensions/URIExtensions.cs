// ReSharper disable once CheckNamespace
namespace System;

public static class URIExtensions
{
    public static string AppendToURL(this string uri1, string uri2)
    {
        return AppendToUrlInternal(uri1, uri2);
    }
    
    public static string AppendToURL(this string baseURL, params string[] segments)
    {
        return AppendToUrlInternal(baseURL, segments);
    }

    private static string AppendToUrlInternal(this string baseURL, params string[] segments)
    {
        return string.Join("/", new[] { baseURL.TrimEnd('/') }
            .Concat(segments.Select(s => s.Trim('/'))));
    }
    
    public static Uri Append(this Uri uri, params string[] paths)
    {
        return new Uri(paths.Aggregate(uri.AbsoluteUri, (current, path) => string.Format("{0}/{1}", current.TrimEnd('/'), path.TrimStart('/'))));
    }
}