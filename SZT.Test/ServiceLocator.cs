

using Android.SE.Omapi;
using Android.Service.QuickSettings;
using SZT.Test.Services;
using SZT.Test.ViewMdoels;
using SZT.Test.ViewModels;

namespace SZT.Test;

public class ServiceLocator
{
    private IServiceProvider? _serviceProvider;

    public ChartViewModel ChartViewModel => //自动依赖注入
        _serviceProvider.GetService<ChartViewModel>();

    public DataSelectViewModel DataSelectViewModel =>
        _serviceProvider.GetService<DataSelectViewModel>();

    public MainViewModel MainViewModel =>
        _serviceProvider.GetService<MainViewModel>();

    public ServiceLocator()
    {
        var serviceCollection = new ServiceCollection();

        // 注册服务和 ViewModel
        serviceCollection.AddTransient<IDataSaveStorage ,DataSaveStorage>();
        serviceCollection.AddTransient<IDataShowStorage,DataShowStorage>();
        serviceCollection.AddTransient<IRootNavigateService,RootNavigateService>();
        serviceCollection.AddTransient<IPeakStorage, PeakStorage>();


        serviceCollection.AddTransient<DataSelectViewModel>();
        serviceCollection.AddTransient<ChartViewModel>();
        serviceCollection.AddTransient<MainViewModel>();

        // 构建服务提供器
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
}
