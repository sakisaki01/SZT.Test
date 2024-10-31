using SQLite;
namespace SZT.Test.Models;

public class Data
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    //public List<int>? Value { get; set; }

    //public List<int>? Count { get; set; }

    public int DataValue { get; set; }

    public int DataCount { get; set; }
}
