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

            // 从服务容器中获取 ResultViewModel 实例并设置为 BindingContext
            BindingContext = App.Services.GetService<ResultViewModel>();
            BindingContext = new ResultViewModel(new DataShowStorage()); // 确保这里创建了 ViewModel 的实例
        }

        // 可选：如果手动创建页面实例时需要使用带参数构造函数
        public ChartView(ResultViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
