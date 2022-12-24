using SqlServerDateTime2PrecisionApp.Classes;

namespace SqlServerDateTime2PrecisionApp
{
    internal partial class Program
    {
        static void Main(string[] args)
        {
            // main code sample use DataReader, DataTable and EF Core 7
            DateTime2Operations.GetCreatedColumnDateTime();

            Console.WriteLine();

            // example for Extensions.GetMilliseconds
            DateTime2Operations.RawMocked();

            SpectreConsoleHelpers.ExitPrompt();
        }
    }
}