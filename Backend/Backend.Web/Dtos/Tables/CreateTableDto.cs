namespace Backend.Web.Dtos.Tables;

public class CreateTableDto
{
    public string TableName { get; set; }
    public Dictionary<string, string> Columns { get; set; }
}
