using SQLite;
using System.ComponentModel;
namespace SZT.Test.Models;

[Table("Data")]
public class Data : INotifyPropertyChanged
{
    private bool _isSelected;

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Uid {  get; set; }

    public DateTime LogTime { get; set; }/* = DateTime.Now.ToNearbySecond();*/

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));

                // 触发事件来更新 ViewModel 中的选择列表
                SelectionChanged?.Invoke(Id, _isSelected);
            }
        }
    }

    public event Action<int, bool> SelectionChanged;

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


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





