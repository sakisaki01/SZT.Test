
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using SZT.Test.Models;
using SZT.Test.Services;
using SZT.Test.View;

namespace SZT.Test.ViewMdoels;

public partial class DataSelectViewModel : ObservableObject
{
    private IDataSaveStorage _dataSaveStorage;
    private IRootNavigateService _rootNavigateService;

    public DataSelectViewModel(IDataSaveStorage dataSaveStorage , IRootNavigateService rootNavigateService)
    {
        _dataSaveStorage = dataSaveStorage;
        _rootNavigateService = rootNavigateService;
    }

    // 用于存储显示的数据
    public  ObservableCollection<Data> Datas { get; } = new ObservableCollection<Data>();


    [RelayCommand]
    private async Task ClearAllDataCommand() => await ClearAllDataFunctionAsync();

    [RelayCommand]
    private async Task TurnMenuCommand() =>
        await _rootNavigateService.NavigateToAsync("///" + nameof(MainView));

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
