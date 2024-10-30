using Bumptech.Glide.Load.Resource.Bitmap;
using DevExpress.Maui;
using DevExpress.Maui.Charts;
using Microsoft.Extensions.Logging;
using SZT.Test.ViewModels;

namespace SZT.Test
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseDevExpress()
                .UseDevExpressCharts()
                .ConfigureFonts(fonts => {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

#if DEBUG
            builder.Logging.AddDebug();
            builder.Services.AddSingleton<ChartView>();
            builder.Services.AddSingleton<ResultViewModel>();
#endif

            return builder.Build();
        }
    }
}
