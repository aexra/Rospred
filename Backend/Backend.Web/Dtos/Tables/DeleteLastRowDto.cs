namespace Backend.Web.Dtos.Tables;

public class DeleteLastRowDto
{
    public string TableName { get; set; }
    public string OrderByColumn { get; set; } // PRIMARY KEY COLUMN NAME
}
