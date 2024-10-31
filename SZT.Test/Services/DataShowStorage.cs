
using System.Text;
using System.Timers;
using SZT.Test.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Maixin.Auto.Services;
using Xamarin.KotlinX.Coroutines;


namespace SZT.Test.Services;

public class DataShowStorage : ObservableObject ,IDataShowStorage
{
    private readonly SerialPort _serialPort;

    // 定义一个事件，用于将数据传递给订阅者
    public event Action<int> DataReceived;

    public SerialPort stmcSerialPort { get; set; } = new SerialPort("/dev/ttys4", 115200);

    public DataShowStorage()
    {
        stmcSerialPort.Open();
    }

    public void OnDataReceived(object sender, ElapsedEventArgs e)
    {
        string rawData = _serialPort.ReadLine();
        int.TryParse(rawData,  out int data);
        DataReceived?.Invoke(data);  // 直接触发事件
    }

}
