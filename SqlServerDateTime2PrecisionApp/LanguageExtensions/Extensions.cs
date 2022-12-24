using System.Globalization;

namespace SqlServerDateTime2PrecisionApp.LanguageExtensions;

public static class Extensions
{
    /// <summary>
    /// Get milliseconds from <see cref="DateTime.TimeOfDay"/>
    /// </summary>
    /// <remarks>
    /// It's easy to get the fraction as a decimal but not an int
    /// </remarks>
    public static int GetMilliseconds(this double sender)
        => Convert.ToInt32(Convert.ToString(Convert.ToDecimal(sender) % 1.0m,
            CultureInfo.InvariantCulture).Replace("0.", ""));

    public static int GetMilliseconds(this DateTime sender) 
        => Convert.ToInt32(sender.TimeOfDay.Milliseconds.ToString() + sender.TimeOfDay.Microseconds.ToString());

    // think this might work, no as it do addition
    //public static int GetMilliseconds(this DateTime sender) 
    //    => Convert.ToInt32(sender.TimeOfDay.Milliseconds + sender.TimeOfDay.Microseconds);
}