namespace Backend.Web.Dtos.Tables;

public class TableDTO
{
    public int Id { get; set; }
    public int SDG { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<string> Headers { get; set; } = [];
    public List<string> Values { get; set; } = [];
}
