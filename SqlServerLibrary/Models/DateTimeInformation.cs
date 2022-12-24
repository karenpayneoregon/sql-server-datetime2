namespace SqlServerLibrary.Models;

public class DateTimeInformation
{
    public string TableName { get; set; }
    public string ColumnName { get; set; }
    public int Precision { get; set; }
    public override string ToString() => $"{ColumnName}: {Precision}";

}