using SZT.Test.Services;
using SZT.Test.ViewModels;

namespace SZT.Test.View
{
    public partial class ChartView : ContentPage
    {
        // �޲������캯��
        public ChartView()
        {
            InitializeComponent();

            // �ӷ��������л�ȡ ResultViewModel ʵ��������Ϊ BindingContext
            BindingContext = App.Services.GetService<ResultViewModel>();
            BindingContext = new ResultViewModel(new DataShowStorage()); // ȷ�����ﴴ���� ViewModel ��ʵ��
        }

        // ��ѡ������ֶ�����ҳ��ʵ��ʱ��Ҫʹ�ô��������캯��
        public ChartView(ResultViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
