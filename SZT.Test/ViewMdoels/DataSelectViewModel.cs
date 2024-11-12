
using Android.Runtime;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        // 数据初始化后，订阅选择状态
        foreach (var data in Datas)
        {
            data.SelectionChanged += ToggleSelection;
        }
    }

    // 用于存储显示的数据
    public  ObservableCollection<Data> Datas { get; } = new ObservableCollection<Data>();

    public ObservableCollection<Data> PaginatedDatas { get; } = new ObservableCollection<Data>();

    // 用于存储选中的 Id
    public HashSet<int> SelectedIds { get; private set; } = new HashSet<int>();

    [ObservableProperty]
    int currentPage = 1;

    [ObservableProperty]
    int itemsPerPage = 10;

    [ObservableProperty]
    bool select;

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
    private async Task SaveData() => await SaveSelectedData();


    private  async Task LoadDataAsync()
    {
        Datas.Clear();
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

    public async Task SaveSelectedData()
    {
        
        // 在数据加载后手动触发刷新
        OnPropertyChanged(nameof(Datas));

        var selectedData = Datas.Where(data => data.IsSelected).ToList();

        // 如果没有选中的数据，弹出提示并退出方法
        if (!selectedData.Any())
        {
            await Shell.Current.DisplayAlert("提示", "请选择数据！", "确定");
            return;
        }

        try
        {
            // 批量保存到 Excel
            foreach (var data in selectedData)
            {
                await _dataSaveStorage.ExportDataRowToExcelAsync(data.Uid);
                Debug.WriteLine(data.Uid);
            }

            // 保存完后更新数据库中的选中状态
            await UpdateDataSelectionStatus(selectedData);

            await Shell.Current.DisplayAlert("提示", "已保存到 Excel！", "确定");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"保存出错: {ex.Message}");
            await Shell.Current.DisplayAlert("错误", "保存过程中发生错误，请重试。", "确定");
        }
        finally
        {
            // 重置所有选中数据的 IsSelected 为 false 并批量触发 UI 刷新
            foreach (var data in selectedData)
            {
                data.IsSelected = false;
            }
            OnPropertyChanged(nameof(Datas));  // 一次性触发所有数据变化通知
        }
    }





    ////更新数据库中选中状态的代码
    //public async Task SaveSelectedData()
    //{
    //    // 根据 SelectedIds 过滤出选中的数据
    //    var selectedData = Datas.Where(data => SelectedIds.Contains(data.Id)).ToList();

    //    // 如果没有选中的数据，弹出提示并退出方法
    //    if (!selectedData.Any())
    //    {
    //        await Shell.Current.DisplayAlert("提示", "请选择数据！", "确定");
    //        return;
    //    }

    //    // 根据勾选的项保存到 Excel
    //    foreach (var data in selectedData)
    //    {
    //        await _dataSaveStorage.ExportDataRowToExcelAsync(data.Uid);
    //        Debug.WriteLine(data.Uid);
    //    }

    //    // 提示数据保存完成
    //    await Shell.Current.DisplayAlert("提示", "已保存到 Excel！", "确定");

    //    // 保存完数据后，清空 SelectedIds 集合，同时触发 UI 更新
    //    SelectedIds.Clear();
    //    foreach (var data in Datas)
    //    {
    //        if (data.IsSelected)
    //            data.IsSelected = false; // 触发 OnPropertyChanged，UI 会自动刷新
    //    }

    //    // 通知界面更新
    //    OnPropertyChanged(nameof(SelectedIds));
    //}


    private async Task UpdateDataSelectionStatus(List<Data> selectedData)
    {
        foreach (var data in selectedData)
        {
            
            await _dataSaveStorage.UpdateDataSelectionStatus(data);
        }
    }

    // 更新选中状态的方法
    private void ToggleSelection(int id, bool isSelected)
    {
        if (isSelected)
            SelectedIds.Add(id);
        else
            SelectedIds.Remove(id);
    }



    /// <summary>
    /// 选择勾选的保存
    /// </summary>
    //private async void SaveSelectedData()
    //{
    //    // 获取所有选中数据的 Uid 列表
    //    var selectedUids = Datas
    //        .Where(data => data.IsSelected) // 筛选出选中的数据项
    //        .Select(data => data.Uid) // 提取选中的数据项的 Uid
    //        .ToList(); // 转换为列表

    //    // 遍历所有选中的 Uid，并调用保存方法
    //    foreach (var uid in selectedUids)
    //    {
    //        await _dataSaveStorage.ExportDataRowToExcelAsync(uid); // 使用 Uid 来调用保存方法
    //    }
    //    await Shell.Current.DisplayAlert("提示", "已保存到Excel！", "确定");
    //}
}
