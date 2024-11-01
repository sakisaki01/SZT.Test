﻿
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




    private RelayCommand _ShowDataCommand;
    public RelayCommand ShowDataCommand =>
        _ShowDataCommand ??= new RelayCommand(async () => { await ShowData(); });



    [RelayCommand]
    private async Task ClearAllDataCommand() => await ClearAllDataFunctionAsync();

    public  async Task ShowData()
    {
        //await _dataSaveStorage.InitializeAsync();
        // 从数据存储中获取数据
        var list = await _dataSaveStorage.GetDataAsync();

        
        foreach (var data in list)
        {
            Datas.Add(data);
        }
    }

    private async Task ClearAllDataFunctionAsync()
    {
        await _dataSaveStorage.InitializeAsync();

        await _dataSaveStorage.ClearAllDataAsync();
    }

}
