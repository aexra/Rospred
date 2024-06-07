namespace Backend.Web.Dtos.Tables;

public class UpdateCellDto
{
    public string TableName { get; set; }
    public string ColumnName { get; set; }
    public string NewValue { get; set; }
    public Dictionary<string, string> Criteria { get; set; }
}
