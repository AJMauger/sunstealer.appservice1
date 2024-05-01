using System.ComponentModel.DataAnnotations;

namespace AppService1.Models;

public class Table1
{
    [Key]
    public int UUID { get; set; }

    public int Number1 { get; set; }

    public string Text1 { get; set; } = string.Empty;
}
