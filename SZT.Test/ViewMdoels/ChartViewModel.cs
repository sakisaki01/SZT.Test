﻿using Android.Provider;
using Android.Runtime;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers; // 使用 System.Timers.Timer
using SZT.Test.Models;
using SZT.Test.Services;
using SZT.Test.View;

namespace SZT.Test.ViewModels;

#region 10.30 调试
//public partial class ChartViewModel : ObservableObject
//{
//    private readonly DataShowStorage _dataShowStorage;
//    private readonly RootNavigateService _rootNavigateService;
//    private readonly IPeakStorage _peakStorage;
//    private readonly IDataSaveStorage _dataSaveStorage;

//    public ChartViewModel
//        (DataShowStorage dataShowStorage, RootNavigateService rootNavigateService, 
//        DataSaveStorage dataSaveStorage,IPeakStorage peakStorage)
//    {
//        _dataShowStorage = dataShowStorage;
//        _rootNavigateService = rootNavigateService;
//        _dataSaveStorage = dataSaveStorage;
//        _peakStorage = peakStorage;

//        // 初始化定时器
//        _dataTimer = new System.Timers.Timer(500); // 每 1 秒触发一次
//        _dataTimer.Elapsed += GenerateRandomData; // 绑定定时生成数据的方法
//    }
//    private System.Timers.Timer _dataTimer; // 定时器，用于生成数据

//    public ObservableCollection<DataPoint> DataPoints { get; } = new();
//    public ObservableCollection<string> Userid { get; set; } = new();

//    List<int> CountList = [1, 2];
//    List<int> ValueList = [];
//    public ObservableCollection<Data> Datas { get; } = new();

//    [ObservableProperty]
//    string light = "信号";

//    [ObservableProperty]
//    string newUid = string.Empty;

//    private int[] peakdatas;

//    [ObservableProperty]
//    double amplitudeThreshold = 7;

//    [ObservableProperty]
//    int minDistance = 10;

//    [ObservableProperty]
//    int tPeak;

//    [ObservableProperty]
//    int firstPeak;

//    [ObservableProperty]
//    int secondPeak;

//    [ObservableProperty]
//    int thirdPeak;

//    [ObservableProperty]
//    double tPeakArea;

//    [ObservableProperty]
//    double firstPeakArea;

//    // 控制实时数据更新的属性
//    public bool AllowLiveDataUpdates { get; set; }

//    private int _pointCounter = 1;  // 计数器，从 1 开始

//    private bool ispointCounter = true;


//    private RelayCommand _startDataCommand;
//    public RelayCommand StartDataCommand =>
//        _startDataCommand ??= new RelayCommand(StartRandomDataGeneration);

//    private RelayCommand _clearCommand;
//    public RelayCommand ClearCommand =>
//        _clearCommand ??= new RelayCommand(StopRandomDataGeneration);

//    [RelayCommand]
//    private async Task TurnMenuCommand() => 
//        await _rootNavigateService.NavigateToAsync("///"+nameof(MainView));

//    //保存数据
//    [RelayCommand]
//    private async Task SaveDataCommand() => await SaveDataGeneration();

//    [RelayCommand]
//    private async Task PeakSearch() => await PeakSearchFunc();

//    private async Task SaveDataGeneration()
//    {
//        await _dataSaveStorage.InitializeAsync();

//        if (string.IsNullOrWhiteSpace(NewUid))
//        {
//            await Shell.Current.DisplayAlert("提示", "请输入用户ID！", "确定");
//        }
//        else
//        {
//            var data = new Data{
//                Uid = NewUid,
//                LogTime = DateTime.Now
//            };
//            try{
//                await _dataSaveStorage.AddDataAsync(data, ValueList);
//                await Shell.Current.DisplayAlert("提示", "数据已保存！", "确定");
//                ValueList.Clear();
//            }
//            catch (Exception ex) {
//                await Shell.Current.DisplayAlert("错误", $"保存数据失败：{ex.Message}", "确定");
//            }
//        }
//    }
//    //寻峰
//    private async Task PeakSearchFunc()
//    {
//        if(peakdatas == null)
//            return;
//        int[] peaks = _peakStorage.FindPeak(peakdatas, AmplitudeThreshold, MinDistance);
//        try
//        {
//            peaks[0] = TPeak;
//            peaks[1] = FirstPeak;
//            peaks[2] = SecondPeak;
//            peaks[3] = ThirdPeak;
//        }
//        catch (Exception ex)
//        {
//            await Shell.Current.DisplayAlert("错误", $"无法识别波峰！\n{ex.Message}", "确定");
//        }

//    }


//    // 启动随机数据生成
//    private void StartRandomDataGeneration()
//    {
//        AllowLiveDataUpdates = true; // 允许更新数据
//        _dataTimer.Start(); // 启动定时器
//    }

//    // 停止随机数据生成
//    private void StopRandomDataGeneration()
//    {
//        AllowLiveDataUpdates = false; // 禁止更新数据
//        _dataTimer.Stop(); // 停止定时器
//        DataPoints.Clear();
//        peakdatas = new int[ValueList.Count];
//        ispointCounter = false;

//    }

//    // 每秒生成一个随机数据的事件处理方法
//    private void GenerateRandomData(object sender, ElapsedEventArgs e)
//    {
//        if (AllowLiveDataUpdates)
//        {
//            if (ispointCounter == false)
//            {
//                //清空
//                _pointCounter = 0;
//            }
//            ispointCounter = true;

//            Random random = new Random();
//            int randomData = random.Next(1, 100); // 生成随机数据，范围是 0 到 99
//            DataPoints.Add(new DataPoint { Count = _pointCounter++, Value = randomData });

//            //CountList.Add(_pointCounter);
//            ValueList.Add(randomData);

//            // 通知UI更新
//            OnPropertyChanged(nameof(DataPoints));
//        }
//    }
//}
#endregion



#region 串口通信数据
public partial class ChartViewModel : ObservableObject
{
    private readonly DataShowStorage _dataShowStorage;
    private readonly RootNavigateService _rootNavigateService;
    private readonly IPeakStorage _peakStorage;
    private readonly IDataSaveStorage _dataSaveStorage;

    public DataShowStorage DataShowStorage_Ins { get; set; }

    [ObservableProperty]
    string light = "信号";

    [ObservableProperty]
    string newUid = string.Empty;

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

    // 控制实时数据更新的属性
    public bool AllowLiveDataUpdates { get; set; }

    private bool ispointCounter = true;
    
    private int ReceivedData { get; set; }

    List<int> ValueList = [];

    // 用于图表的数据集合
    public ObservableCollection<DataPoint> DataPoints { get; set; } 

    public ChartViewModel
        (DataShowStorage dataShowStorage, RootNavigateService rootNavigateService,
        DataSaveStorage dataSaveStorage, IPeakStorage peakStorage)
    {
        _dataShowStorage = dataShowStorage;
        _rootNavigateService = rootNavigateService;
        _dataSaveStorage = dataSaveStorage;
        _peakStorage = peakStorage;

        _dataShowStorage = DataShowStorage.Instance;

        DataPoints = _dataShowStorage.DataPoints;
    }


    private RelayCommand _startDataCommand;
    public RelayCommand StartDataCommand =>
        _startDataCommand ??= new RelayCommand(() => {});

    private RelayCommand _clearCommand;
    public RelayCommand ClearCommand =>
        _clearCommand ??= new RelayCommand(StopDataGeneration);

    [RelayCommand]
    private async Task TurnMenuCommand() =>
        await _rootNavigateService.NavigateToAsync("///" + nameof(MainView));

    //保存数据
    [RelayCommand]
    private async Task SaveDataCommand() => await SaveDataGeneration();
    //寻峰
    [RelayCommand]
    private async Task PeakSearch() => await PeakSearchFunc();

    /// <summary>
    /// 将数据保存到数据库
    /// </summary>
    /// <returns></returns>
    private async Task SaveDataGeneration()
    {
        await _dataSaveStorage.InitializeAsync();
        ValueList = _dataShowStorage.ValueList;
        //保存到峰数据
        peakdatas = new int[ValueList.Count];
        if (string.IsNullOrWhiteSpace(NewUid))
        {
            await Shell.Current.DisplayAlert("提示", "请输入用户ID！", "确定");
        }
        else
        {
            var data = new Data
            {
                Uid = NewUid,
                LogTime = DateTime.Now
            };
            try
            {
                await _dataSaveStorage.AddDataAsync(data, ValueList);
                await Shell.Current.DisplayAlert("提示", "数据已保存！", "确定");
                ValueList.Clear();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("错误", $"保存数据失败：{ex.Message}", "确定");
            }
        }
    }


    //清空数据
    private void StopDataGeneration()
    {
        //AllowLiveDataUpdates = false; // 禁止更新数据
        DataPoints.Clear();
        _dataShowStorage.DataCount = 1;
        peakdatas = new int[ValueList.Count];
        ispointCounter = false;
    }

    //寻峰
    private async Task PeakSearchFunc()
    {
        if (peakdatas == null)
            return;
        int[] peaks = _peakStorage.FindPeak(peakdatas, AmplitudeThreshold, MinDistance);
        try
        {
            peaks[0] = TPeak;
            peaks[1] = FirstPeak;
            peaks[2] = SecondPeak;
            peaks[3] = ThirdPeak;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("错误", $"无法识别波峰！\n{ex.Message}", "确定");
        }

    }

    // 取消订阅事件
    //~ChartViewModel()
    //{
    //    _dataShowStorage.DataReceived -= OnDataReceived;
    //}
}
#endregion