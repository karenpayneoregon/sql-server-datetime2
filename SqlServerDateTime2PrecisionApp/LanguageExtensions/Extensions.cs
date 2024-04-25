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

    /// <summary>
    /// Get milliseconds from <see cref="DateTime"/>
    /// </summary>
    /// <param name="sender"></param>
    /// <returns>
    /// Does not give precision of 7 if value of sender does not have 7 places while <see cref="GetMilliseconds7"/> does
    /// </returns>
    public static int GetMilliseconds(this DateTime sender) 
        => Convert.ToInt32(sender.TimeOfDay.Milliseconds.ToString() + sender.TimeOfDay.Microseconds);

    /// <summary>
    /// Ensures precision is 7
    /// </summary>
    public static int GetMilliseconds7(this DateTime sender) 
        => Convert.ToInt32((sender.TimeOfDay.Milliseconds.ToString() + sender.TimeOfDay.Microseconds)
            .PadRight(7, '0'));

}