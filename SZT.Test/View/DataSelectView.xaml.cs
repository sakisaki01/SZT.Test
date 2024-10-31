using AndroidX.Lifecycle;
using SZT.Test.ViewMdoels;

namespace SZT.Test.View;

public partial class DataSelectView : ContentPage
{
	public DataSelectView(DataSelectViewModel vm)
	{
        InitializeComponent();

        BindingContext = vm;
    }
}