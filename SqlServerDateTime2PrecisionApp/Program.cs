using SqlServerDateTime2PrecisionApp.Classes;

namespace SqlServerDateTime2PrecisionApp
{
    internal partial class Program
    {
        static void Main(string[] args)
        {
            DateTime2Operations.GetCreatedColumnDateTime();
            Console.WriteLine();
            DateTime2Operations.RawMocked();
            SpectreConsoleHelpers.ExitPrompt();
        }
    }
}