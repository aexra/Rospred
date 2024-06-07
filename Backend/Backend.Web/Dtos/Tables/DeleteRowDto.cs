namespace Backend.Web.Dtos.Tables;

public class DeleteRowDto
{
    public string TableName { get; set; }
    public Dictionary<string, string> Criteria { get; set; }
}
