
using System.Timers;

namespace SZT.Test.Services;

public interface IDataShowStorage
{
    void OnDataReceived(object sender , ElapsedEventArgs e);
}
