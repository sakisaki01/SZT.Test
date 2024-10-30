using SZT.Test.ViewModels;

namespace SZT.Test;

public partial class ResultView : ContentPage
{
	public ResultView(ResultViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

    }
}