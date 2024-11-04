using SQLite;
namespace SZT.Test.Models;

[Table("Data")]
public class Data
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Uid {  get; set; }

    public DateTime LogTime { get; set; }/* = DateTime.Now.ToNearbySecond();*/

    [Ignore]
    public List<int> Numbers { get; set; } = new List<int>();
    public override string ToString()
    {
        return string.Join(", ", Numbers); // 以逗号分隔的字符串
    }
}
[Table("DataNumber")]
public class DataNumber
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int DataId { get; set; }  // 关联到 Data 的外键
    public int Number { get; set; }  // List<int> 中的单个数值
}



