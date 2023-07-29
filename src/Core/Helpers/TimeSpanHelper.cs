namespace Core.Helpers;

public static class TimeSpanHelper
{
    public static TimeSpan FromMinutesNotNull(int? minutesToAdd, int defaultValue)
    {
        return TimeSpan.FromMinutes(minutesToAdd ?? defaultValue);
    }
}