
using System.Text;
using System.Timers;
using SZT.Test.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Maixin.Auto.Services;
using Xamarin.KotlinX.Coroutines;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Xamarin.Google.Crypto.Tink.Shaded.Protobuf;

namespace SZT.Test.Services;

public class DataShowStorage : ObservableObject , IDataShowStorage
{

    public static DataShowStorage Instance { get; private set; } = new DataShowStorage();

    public ObservableCollection<DataPoint> DataPoints { get; set; } = new ObservableCollection<DataPoint>();

    public SerialPort StmcSerialPort { get; set; } = new SerialPort("/dev/ttyS7", 115200);

    public List<int> ValueList { get; set; } = [];

    public DataShowStorage()
    {
        StmcSerialPort.Open();
        StmcSerialPort.DataReceived += OnDataReceived;
        Debug.WriteLine(StmcSerialPort.Open());
    }

    public int DataCount { get; set; } = 1;

    public void OnDataReceived(object sender, LiquidMobile.Services.SerialPortDataReceEventArgs e)
    {
        try
        {
            if (e.Length <= 0) return;
            string str = Encoding.Default.GetString(e.Data);
            if (!int.TryParse(str, out int a)) return;

            DataPoints.Add(new DataPoint { Count = DataCount++, Value = a });
            ValueList.Add(a);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error reading from serial port: {ex.Message}");
        }
    }

}
