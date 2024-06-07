namespace Backend.Web.Dtos.Tables;

public class AddColumnDto
{
    public string TableName { get; set; }
    public string ColumnName { get; set; }
    public string ColumnType { get; set; }
}
