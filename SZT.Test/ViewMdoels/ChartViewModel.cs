using Android.Provider;
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
public partial class ChartViewModel : ObservableObject
{
    private readonly DataShowStorage _dataShowStorage;
    private readonly RootNavigateService _rootNavigateService;
    private IDataSaveStorage _dataSaveStorage;

    private System.Timers.Timer _dataTimer; // 定时器，用于生成数据

    public ObservableCollection<DataPoint> DataPoints { get; } = new();

    List<int> CountList = [1, 2];
    List<int> ValueList = [1, 2, 3];
    public ObservableCollection<Data> Datas { get; } = new();

    [ObservableProperty]
    string light = "信号";

    // 控制实时数据更新的属性
    public bool AllowLiveDataUpdates { get; set; }

    private int _pointCounter = 1;  // 计数器，从 1 开始

    private bool ispointCounter = true;


    public ChartViewModel
        (DataShowStorage dataShowStorage ,RootNavigateService rootNavigateService ,DataSaveStorage dataSaveStorage)
    {
        _dataShowStorage = dataShowStorage;
        _rootNavigateService = rootNavigateService;
        _dataSaveStorage = dataSaveStorage;

        // 初始化定时器
        _dataTimer = new System.Timers.Timer(500); // 每 1 秒触发一次
        _dataTimer.Elapsed += GenerateRandomData; // 绑定定时生成数据的方法
    }

    private RelayCommand _startDataCommand;
    public RelayCommand StartDataCommand =>
        _startDataCommand ??= new RelayCommand(StartRandomDataGeneration);

    private RelayCommand _clearCommand;
    public RelayCommand ClearCommand =>
        _clearCommand ??= new RelayCommand(StopRandomDataGeneration);

    [RelayCommand]
    private async Task TurnMenuCommand() => 
        await _rootNavigateService.NavigateToAsync(nameof(DataSelectView));

    //保存数据
    [RelayCommand]
    private async Task SaveDataCommand() =>
        await SaveDataGeneration();

    private async Task SaveDataGeneration()
    {
        await _dataSaveStorage.InitializeAsync();
        await _dataSaveStorage.AddDataAsync(new Data
        {
            DataValue = 11,
            DataCount = 22,
        });
    }


    // 启动随机数据生成
    private void StartRandomDataGeneration()
    {
        AllowLiveDataUpdates = true; // 允许更新数据
        _dataTimer.Start(); // 启动定时器
    }

    // 停止随机数据生成
    private void StopRandomDataGeneration()
    {
        AllowLiveDataUpdates = false; // 禁止更新数据
        _dataTimer.Stop(); // 停止定时器
        //DataPoints.Clear();
        ispointCounter = false;

    }

    // 每秒生成一个随机数据的事件处理方法
    private void GenerateRandomData(object sender, ElapsedEventArgs e)
    {
        if (AllowLiveDataUpdates)
        {
            if (ispointCounter == false)
            {
                //清空
                _pointCounter = 0;
            }
            ispointCounter = true;

            Random random = new Random();
            int randomData = random.Next(1, 100); // 生成随机数据，范围是 0 到 99
            DataPoints.Add(new DataPoint { Count = _pointCounter++, Value = randomData });

            //CountList.Add(_pointCounter);
            //ValueList.Add(randomData);

            // 通知UI更新
            OnPropertyChanged(nameof(DataPoints));
        }
    }
}
#endregion

#region 串口通信数据
//public partial class ResultViewModel : ObservableObject
//{
//    private readonly DataShowStorage _dataShowStorage;

//    [ObservableProperty]
//    string light = "信号"; // 修正拼写错误

//    // 控制实时数据更新的属性
//    public bool AllowLiveDataUpdates { get; set; }

//    private int _pointCounter = 1;  // 计数器，从 1 开始

//    private bool ispointCounter = true;

//    // 用于图表的数据集合
//    public ObservableCollection<DataPoint> DataPoints { get; } = new ObservableCollection<DataPoint>();

//    public ResultViewModel(DataShowStorage dataShowStorage)
//    {
//        _dataShowStorage = dataShowStorage;

//        // 订阅数据接收事件
//        _dataShowStorage.DataReceived += OnDataReceived;
//    }

//    private RelayCommand _startDataCommand;
//    public RelayCommand StartDataCommand =>
//        _startDataCommand ??= new RelayCommand(() => { });

//    private RelayCommand _clearCommand;
//    public RelayCommand ClearCommand =>
//        _clearCommand ??= new RelayCommand(OnChatCommandExecuted);

//    // 接收到数据时触发
//    private void OnDataReceived(int data)
//    {
//        if (AllowLiveDataUpdates)
//        {
//            if (ispointCounter == false){
//                _pointCounter = 0;//清空
//            }
//            ispointCounter = true;

//            //使用计数器作为 x 轴，接收到的数据作为 y 轴
//            DataPoints.Add(new DataPoint { Count = _pointCounter++, Value = data });
//            // 通知UI更新
//            OnPropertyChanged(nameof(DataPoints));
//        }
//    }

//    // 重置图表的方法
//    public void OnChatCommandExecuted()
//    {
//        AllowLiveDataUpdates = false; // 禁止更新数据
//        // 清空数据点集合
//        DataPoints.Clear();
//        ispointCounter = false;
//}

//    // 取消订阅事件
//    ~ResultViewModel()
//    {
//        _dataShowStorage.DataReceived -= OnDataReceived;
//    }
//}
#endregion