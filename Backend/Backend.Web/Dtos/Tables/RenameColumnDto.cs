namespace Backend.Web.Dtos.Tables;

public class RenameColumnDto
{
    public string TableName { get; set; }
    public string OldColumnName { get; set; }
    public string NewColumnName { get; set; }
}
