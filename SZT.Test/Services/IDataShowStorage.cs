
using LiquidMobile.Services;
using System.Timers;

namespace SZT.Test.Services;

public interface IDataShowStorage
{
    void OnDataReceived(object sender , SerialPortDataReceEventArgs e);
}
