
using Android.Hardware;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using SZT.Test.Models;
using SZT.Test.Services;

namespace SZT.Test.ViewMdoels;

public partial class DataSelectViewModel : ObservableObject
{
    private IDataSaveStorage _dataSaveStorage;

    public DataSelectViewModel(IDataSaveStorage dataSaveStorage)
    {
        _dataSaveStorage = dataSaveStorage;
    }

    // 用于存储显示的数据
    public  ObservableCollection<Data> Datas { get; } = new ObservableCollection<Data>();


    [RelayCommand]
    private async Task ClearAllDataCommand() => await ClearAllDataFunctionAsync();

    //[RelayCommand]
    //private async Task DeleteData() => await _dataSaveStorage.RemoveData();

    [RelayCommand]
    private async Task ShowData() => await LoadDataAsync();

    public  async Task LoadDataAsync()
    {
        await _dataSaveStorage.InitializeAsync();
        // 从数据存储中获取数据
        var dataList = await _dataSaveStorage.GetDataAsync();
        
        foreach (var data in dataList)
        {
            Datas.Add(data);
        }
    }



    private async Task ClearAllDataFunctionAsync()
    {
        await _dataSaveStorage.InitializeAsync();

        await _dataSaveStorage.ClearAllDataAsync();

        Datas.Clear();
        
    }

}
