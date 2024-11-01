using SQLite;
namespace SZT.Test.Models;

public class Data
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    //public List<int>? Value { get; set; }

    //public List<int>? Count { get; set; }

    public string DataValue { get; set; }

    public string DataCount { get; set; }
}
