using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using SZT.Test.Services;
using SZT.Test.ViewModels;

namespace SZT.Test
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

    }
}

