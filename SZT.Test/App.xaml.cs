using Microsoft.Extensions.DependencyInjection;
using SZT.Test.Services;
using SZT.Test.ViewModels;

namespace SZT.Test
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; }

        public App()
        {
            InitializeComponent();

            // 创建服务集合
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // 构建服务提供器
            Services = serviceCollection.BuildServiceProvider();

            MainPage = new AppShell();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // 注册 ViewModel
            services.AddSingleton<ResultViewModel>();

            // 注册其他服务（如 DataShowStorage），根据需要添加
            services.AddSingleton<DataShowStorage>();
        }
    }
}

