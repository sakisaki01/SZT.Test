using Bumptech.Glide.Load.Resource.Bitmap;
using DevExpress.Maui;
using DevExpress.Maui.Charts;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using SZT.Test.ViewModels;
using SZT.Test.Services;
using SZT.Test.View;
using ChartView = SZT.Test.View.ChartView;
using SZT.Test.ViewMdoels;

namespace SZT.Test
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>().UseMauiCommunityToolkit().ConfigureFonts()
                .UseDevExpress()
                .UseDevExpressCharts()
                .ConfigureFonts(fonts => {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

#if DEBUG
            builder.Logging.AddDebug();
            builder.Services.AddScoped<ChartView ,ChartViewModel>();
            builder.Services.AddSingleton<DataShowStorage>(); // 注册 DataShowStorage 为单例服务
            builder.Services.AddSingleton<IDataShowStorage , DataShowStorage>();

            builder.Services.AddSingleton<RootNavigateService>();
            builder.Services.AddSingleton<IRootNavigateService , RootNavigateService>();

            builder.Services.AddSingleton<DataSelectView>();
            builder.Services.AddSingleton<DataSelectViewModel>();
            builder.Services.AddSingleton<DataSaveStorage>();
            builder.Services.AddSingleton<IDataSaveStorage , DataSaveStorage>();

            builder.Services.AddSingleton<MainView>();
            builder.Services.AddSingleton<MainViewModel>();

#endif

            return builder.Build();
        }
    }
}
