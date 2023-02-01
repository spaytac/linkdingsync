// ReSharper disable once CheckNamespace
namespace System.Collections.Generic;

public static class SystemCollectionGenericExtesions
{
    public static IEnumerable<T> Add<T>(this IEnumerable<T> e, T value) {
        foreach ( var cur in e) {
            yield return cur;
        }
        yield return value;
    }
}