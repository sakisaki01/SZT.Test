
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
        DataList = new ObservableCollection<Data>();
    }

    // 用于存储显示的数据
    [ObservableProperty]
    private ObservableCollection<Data> dataList;

    [RelayCommand]
    private async Task ShowDataCommand() => await ShowData();

    private async Task ShowData()
    {
        // 从数据存储中获取数据
        var list = await _dataSaveStorage.GetDataAsync();

        // 清空现有数据，并添加新数据
        DataList.Clear();
        foreach (var data in list)
        {
            DataList.Add(data);
        }
    }

}
