
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using SZT.Test.Services;

namespace SZT.Test.ViewMdoels;

public partial class PeakTestViewModle : ObservableObject
{
    private IPeakStorage _peakStorage;
    private IDataSaveStorage _dataSaveStorage;

    public PeakTestViewModle(IPeakStorage peakStorage, IDataSaveStorage dataSaveStorage)
    {
        _peakStorage = peakStorage;
        _dataSaveStorage = dataSaveStorage;
    }

    public ObservableCollection<double[]> PeakDatas { get; set; } = new();

    private int[] peakdatas;

    [ObservableProperty]
    double amplitudeThreshold = 7;

    [ObservableProperty]
    int minDistance = 10;

    [ObservableProperty]
    int tPeak;

    [ObservableProperty]
    int firstPeak;

    [ObservableProperty]
    int secondPeak;

    [ObservableProperty]
    int thirdPeak;

    [ObservableProperty]
    double tPeakArea;

    [ObservableProperty]
    double firstPeakArea;

    [RelayCommand]
    private void PeakSearch() => PeakSearchFunc();

    private void PeakSearchFunc()
    {

        int[] peaks = _peakStorage.FindPeak(peakdatas, AmplitudeThreshold, MinDistance);

        peaks[0] = TPeak;
        peaks[1] = FirstPeak;
        peaks[2] = SecondPeak;
        peaks[3] = ThirdPeak;
    }

    private async Task LoadDataAsync()
    {
        await _dataSaveStorage.InitializeAsync();
        // 从数据存储中获取数据
        var dataList = await _dataSaveStorage.GetDataAsync();
    }

}
