namespace Microsoft.Extensions.DependencyInjection;

public static class StringExtensions
{
    public static string NormalizeTag(this string tag, int maxTagLength = 64)
    {
        //in the database only 64 characters are allowed for tags
        if (tag.Length >= maxTagLength)
        {
            tag = tag.Substring(0, (maxTagLength));
        }
        
        return tag;
    }
}