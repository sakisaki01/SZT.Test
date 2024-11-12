using SZT.Test.Services;
using SZT.Test.ViewModels;

namespace SZT.Test.View
{
    public partial class ChartView : ContentPage
    {
        // 无参数构造函数
        public ChartView()
        {
            InitializeComponent();
        }

        // 可选：如果手动创建页面实例时需要使用带参数构造函数
        public ChartView(ChartViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }


    }
}
