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
        }

        // ��ѡ������ֶ�����ҳ��ʵ��ʱ��Ҫʹ�ô��������캯��
        public ChartView(ChartViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }


    }
}
