using SZT.Test.ViewMdoels;

namespace SZT.Test.View;

public partial class MainView : ContentPage
{
	public MainView(MainViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}