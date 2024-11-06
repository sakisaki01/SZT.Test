
using Android.Runtime;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
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

    public ObservableCollection<Data> PaginatedDatas { get; } = new ObservableCollection<Data>();

    [ObservableProperty]
    int currentPage = 1;

    [ObservableProperty]
    int itemsPerPage = 10;

    //每页显示的数据数量
    private const int PageSize = 10;

    [RelayCommand]
    private async Task ClearAllDataCommand() => await ClearAllDataFunctionAsync();

    [RelayCommand]
    private async Task TurnMenuCommand() =>
        await _rootNavigateService.NavigateToAsync("///" + nameof(MainView));

    //[RelayCommand]
    //private async Task DeleteData() => await _dataSaveStorage.RemoveData();

    [RelayCommand]
    private async Task ShowData() => await LoadDataAsync();



    [RelayCommand]
    private void SaveData() => SaveSelectedData();


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



    #region 翻页



    //[RelayCommand]
    //private void LoadPretPage() => PreviousPage();

    //[RelayCommand]
    //private void LoadNextPage() => NextPage();


    //// 更新显示的分页数据
    //private void UpdatePaginatedDatas()
    //{
    //    PaginatedDatas.Clear();
    //    var pagedData = Datas.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage);

    //    foreach (var data in pagedData)
    //    {
    //        PaginatedDatas.Add(data);
    //    }
    //}

    //public List<Data> GetPagedData(int currentPage, int pageSize)
    //{
    //    // 计算当前页数据的起始位置
    //    int startIndex = (currentPage - 1) * pageSize;

    //    // 获取该页的记录
    //    return Datas.Skip(startIndex).Take(pageSize).ToList();
    //}

    //private void LoadData()
    //{
    //    // 假设 GetPagedData 是您获取特定页面数据的方法
    //    var pagedData = GetPagedData(CurrentPage, PageSize);
    //    Datas.Clear();
    //    foreach (var data in pagedData)
    //    {
    //        Datas.Add(data);
    //    }
    //}

    ////下一页
    //private void NextPage()
    //{
    //    CurrentPage++;
    //    LoadData();
    //}

    ////上一页
    //private void PreviousPage()
    //{
    //    if (CurrentPage > 1)
    //    {
    //        CurrentPage--;
    //        UpdatePaginatedDatas();
    //    }
    //}
    #endregion    


    /// <summary>
    /// 选择勾选的保存
    /// </summary>
    private async void SaveSelectedData()
    {
        // 获取所有选中数据的 Uid 列表
        var selectedUids = Datas
            .Where(data => data.IsSelected) // 筛选出选中的数据项
            .Select(data => data.Uid) // 提取选中的数据项的 Uid
            .ToList(); // 转换为列表

        // 遍历所有选中的 Uid，并调用保存方法
        foreach (var uid in selectedUids)
        {
            await _dataSaveStorage.ExportDataRowToExcelAsync(uid); // 使用 Uid 来调用保存方法
        }
        await Shell.Current.DisplayAlert("提示", "已保存到Excel！", "确定");
    }
}
