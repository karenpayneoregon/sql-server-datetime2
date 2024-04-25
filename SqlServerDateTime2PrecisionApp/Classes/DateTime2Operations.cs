using System.Data;
using System.Globalization;
using ConfigurationLibrary.Classes;
using Microsoft.Data.SqlClient;
using SqlServerDateTime2PrecisionApp.Data;
using SqlServerDateTime2PrecisionApp.LanguageExtensions;
using SqlServerDateTime2PrecisionApp.Models;
using SqlServerLibrary;
// ReSharper disable PossibleInvalidOperationException

namespace SqlServerDateTime2PrecisionApp.Classes;

internal class DateTime2Operations
{

    /// <summary>
    /// A raw example which returns 567123 for milliseconds similar to SQL-Server
    /// </summary>
    public static void RawMocked()
    {
        var table = CreateTableMocked();

        AuditLog auditLog = new() { Created = new DateTime(2022,12,1,13,1,0)};
        auditLog.Created = auditLog.Created.Value.AddMilliseconds(567);
        auditLog.Created = auditLog.Created.Value.AddMicroseconds(123);

        var milliseconds = auditLog.Created.Value.GetMilliseconds();
        table.AddRow(milliseconds.ToString());
        
        using var context = new Context();
        auditLog = context.AuditLog.FirstOrDefault();
        milliseconds = auditLog!.Created.Value.GetMilliseconds();
        table.AddRow("",milliseconds.ToString());
        AnsiConsole.Write(table);
    }

    /// <summary>
    /// In this sample if precision is less than 7 we use <see cref="Extensions.GetMilliseconds7"/> to ensure
    /// this by padding zeros to the right of the value.
    /// </summary>
    public static void PaddingOutToSevenPlaced()
    {
        AuditLog auditLog = new() { Created = new DateTime(2022, 12, 1, 13, 1, 0) };
        auditLog.Created = auditLog.Created.Value.AddMilliseconds(567);
        auditLog.Created = auditLog.Created.Value.AddMicroseconds(1230);
        var milliseconds = auditLog.Created.Value.GetMilliseconds7();

        Console.WriteLine($"{auditLog.Created,-25}{milliseconds}");
        Console.WriteLine($"{auditLog.Created.Value:MM/dd/yyyy hh:mm:ss.fffffff}");

    }
    public static void GetCreatedColumnDateTime()
    {
        var table1 = CreateDataReaderTable();

        using SqlConnection cn = new (ConfigurationHelper.ConnectionString());
        using SqlCommand cmd = new() { Connection = cn, CommandText = "SELECT Created FROM dbo.AuditLog" };

        cn.Open();
        
        var reader = cmd.ExecuteReader();
        var millisecondsFormat = GetMillisecondPrecision();
        
        while (reader.Read())
        {
            DateTime created = reader.GetDateTime(0);
            var formatted1 = created.ToString($"MM/dd/yyyy hh:mm:ss.{millisecondsFormat}");
            var formatted2 = created.ToString($"MM/dd/yyyy hh:mm:ss.fff");
            var formatted3 = created.ToString($"MM/dd/yyyy hh:mm:ss.ff");
            
            table1.AddRow("Date time value", created.ToString(CultureInfo.CurrentCulture), "", $"[white on red]{created.Millisecond}[/]", $"[white on blue]{created.TimeOfDay.ToString()}[/]");
            table1.AddRow("Date time value formatted", formatted1, millisecondsFormat, $"[white on red]{created.Millisecond}[/]");
            table1.AddRow("Date time value formatted", formatted2, "fff");
            table1.AddRow("Date time value formatted", formatted3, "ff");
            table1.AddRow("milliseconds", $"[white on blue]{created.GetMilliseconds()}[/]", "");

            table1.AddEmptyRow();
        }

        reader.Close();
        
        AnsiConsole.Write(table1);


        //-------------------------------------------------------------\\
        DataTable dataTable = new DataTable();
        dataTable.Load(cmd.ExecuteReader());

        var table2 = CreateTable("DataTable");

        foreach (DataRow row in dataTable.Rows)
        {
            table2.AddRow(
                row.Field<DateTime>("Created").ToString(CultureInfo.InvariantCulture),
                row.Field<DateTime>("Created").ToString($"MM/dd/yyyy hh:mm:ss.{millisecondsFormat}"));
        }

        AnsiConsole.Write(table2);

        //-------------------------------------------------------------\\
        var table3 = CreateTable();

        using var context = new Context();
        var data = context.AuditLog.ToList();
        foreach (var log in data)
        {
            table3.AddRow(log.Created.ToString(), log.Created?.ToString($"MM/dd/yyyy hh:mm:ss.{millisecondsFormat}")!);
        }

        AnsiConsole.Write(table3);

    }

    /// <summary>
    /// Get precision for DateTime2 for AuditLog using, in this case catalog defined in appsettings.json
    /// Defaults to fff
    /// </summary>
    /// <returns>DateTime2 precision</returns>
    /// <remarks>See overload below</remarks>
    public static string GetMillisecondPrecision()
    {
        var (list, hasColumns) = DataHelpers.GetDateTimeInformation(ConfigurationHelper.ConnectionString(), "AuditLog");
        return hasColumns ? new string('f', list.FirstOrDefault()!.Precision) : "fff";
    }

    /// <summary>
    /// Get precision for DateTime2 using, in this case catalog defined in appsettings.json
    /// Defaults to fff
    /// </summary>
    /// <param name="tableName">Existing table name</param>
    /// <param name="columnName">Column to get precision</param>
    /// <returns>DateTime2 precision</returns>
    public static string GetMillisecondPrecision(string tableName, string columnName)
    {
        var (list, _) = DataHelpers.GetDateTimeInformation(ConfigurationHelper.ConnectionString(), tableName);
        
        var column = list.FirstOrDefault(x => 
            string.Equals(x.ColumnName, columnName, StringComparison.CurrentCultureIgnoreCase));

        return column != null ? new string('f', column.Precision) : "ff";
        
    }

    /// <summary>
    /// Create Spectre.Console table
    /// </summary>
    public static Table CreateDataReaderTable()
    {
        var table = new Table()
            .RoundedBorder()
            .AddColumn("[cyan]Description[/]")
            .AddColumn("[cyan]Value[/]")
            .AddColumn("[cyan]Format[/]")
            .AddColumn("[cyan]Milliseconds[/]")
            .AddColumn("[cyan]TimeOfDay[/]")
            .Alignment(Justify.Center)
            .BorderColor(Color.LightSlateGrey)
            .Title("[LightGreen]DataReader[/]");
        return table;
    }
    public static Table CreateTable(string title = "EF Core")
    {
        var table = new Table()
            .RoundedBorder()
            .AddColumn("[cyan]Actual[/]")
            .AddColumn("[cyan]Formatted[/]")
            .Alignment(Justify.Center)
            .BorderColor(Color.LightSlateGrey)
            .Title($"[LightGreen]{title}[/]");
        return table;
    }
    public static Table CreateTableMocked(string title = "Mocked")
    {
        var table = new Table()
            .RoundedBorder()
            .AddColumn("[cyan]DataTable[/]")
            .AddColumn("[cyan]EF Core[/]")
            .Alignment(Justify.Center)
            .BorderColor(Color.LightSlateGrey)
            .Title($"[LightGreen]{title}[/]");
        return table;
    }
}