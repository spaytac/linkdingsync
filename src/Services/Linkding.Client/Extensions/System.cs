namespace System;

public static class System
{
    public static DateTime CreateDateTime(this long ticks)
    {
        var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(ticks);

        return dateTimeOffset.UtcDateTime;
    }
    
    public static DateTime CreateDateTime(this string ticksString)
    {
        if (long.TryParse(ticksString, out var dateAddedUnixEpoch))
        {
           return dateAddedUnixEpoch.CreateDateTime();
        }

        return default;
    }

    public static string GetYear(this DateTime date)
    {
        return date.ToString("yyyy");
    }

    public static string GetYear(this long ticks)
    {
        var dateTime = ticks.CreateDateTime();
        return dateTime.GetYear();
    }

    public static string GetYear(this string ticksString)
    {
        var dateTime = ticksString.CreateDateTime();
        return dateTime.GetYear();
    }
}