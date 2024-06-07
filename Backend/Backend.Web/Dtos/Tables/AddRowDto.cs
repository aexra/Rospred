namespace Backend.Web.Dtos.Tables;

public class AddRowDto
{
    public string TableName { get; set; }
    public Dictionary<string, string> Values { get; set; }
}
