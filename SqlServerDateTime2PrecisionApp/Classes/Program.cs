using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace
namespace SqlServerDateTime2PrecisionApp;

internal partial class Program
{
    [ModuleInitializer]
    public static void Init()
    {
        Console.Title = "Code sample: DateTime2";
        Console.WriteLine();
        WindowUtility.SetConsoleWindowPosition(WindowUtility.AnchorWindow.Center);
    }
}