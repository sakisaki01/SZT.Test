using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Maui.Charts;
using SZT.Test.Services;
using SZT.Test.View;
using ChartView = SZT.Test.View.ChartView;
using DataSelectView = SZT.Test.View.DataSelectView;
namespace SZT.Test.ViewMdoels;

public partial class MainViewModel : ObservableObject
{
    private IRootNavigateService _rootNavigateService;

    public MainViewModel(IRootNavigateService rootNavigateService)
    {
        _rootNavigateService = rootNavigateService;
    }

    [RelayCommand]
    private async Task ChartRoot() => 
        await _rootNavigateService.NavigateToAsync(nameof(ChartView));

    [RelayCommand]
    private async Task DataSelectRoot() =>
        await _rootNavigateService.NavigateToAsync(nameof(DataSelectView));


}
