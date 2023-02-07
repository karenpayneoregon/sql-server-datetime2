using Microsoft.Data.SqlClient;
using System.Data;
using SqlServerLibrary.Models;

namespace SqlServerLibrary;

public class DataHelpers
{
    public static bool TablesArePopulated(string connectionString)
    {
        using var cn = new SqlConnection(connectionString);
        using var cmd = new SqlCommand(
            """
                       SELECT T.name TableName,i.Rows NumberOfRows 
                       FROM sys.tables T JOIN sys.sysindexes I ON T.OBJECT_ID = I.ID 
                       WHERE indid IN (0,1) 
                       ORDER BY i.Rows DESC,T.name
                   """, cn);

        DataTable table = new DataTable();

        cn.Open();

        table.Load(cmd.ExecuteReader());
        return table.AsEnumerable()
            .All(row => row.Field<int>("NumberOfRows") > 0);

    }
        
    public static bool ExpressDatabaseExists(string databaseName)
    {
        using var cn = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=master;integrated security=True;Encrypt=False");
        using var cmd = new SqlCommand($"SELECT DB_ID('{databaseName}'); ", cn);

        cn.Open();
        return cmd.ExecuteScalar() != DBNull.Value;

    }
    public static bool LocalDbDatabaseExists(string databaseName)
    {
        using var cn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;integrated security=True;Encrypt=False");
        using var cmd = new SqlCommand($"SELECT DB_ID('{databaseName}'); ", cn);

        cn.Open();
        return cmd.ExecuteScalar() != DBNull.Value;

    }

    public static (List<DateTimeInformation> list, bool hasColumns) GetDateTimeInformation(string connectionString, string tableName)
    {
        List<DateTimeInformation> dateTimeInfoList = new();
        var sql =
            "SELECT TABLE_NAME,COLUMN_NAME,DATETIME_PRECISION " + 
            "FROM INFORMATION_SCHEMA.COLUMNS WHERE DATA_TYPE = 'datetime2' AND TABLE_NAME = @TableName;";

        using var cn = new SqlConnection(connectionString);
        using var cmd = new SqlCommand(sql, cn);
        cmd.Parameters.Add("@TableName", SqlDbType.NChar).Value = tableName;

        cn.Open();

        var reader = cmd.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                dateTimeInfoList.Add(new DateTimeInformation()
                {
                    TableName = reader.GetString(0), 
                    ColumnName = reader.GetString(1), 
                    Precision = reader.GetInt16(2)
                });
            }

            return (dateTimeInfoList, true);
        }
        else
        {
            return (null, false)!;
        }
    }
}