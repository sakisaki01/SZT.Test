


namespace SZT.Test.Services;

public class RootNavigateService : IRootNavigateService
{
    //private IRouteService _routeService;
    //public RootNavigateService(IRouteService routeService)
    //{
    //    _routeService = routeService;   
    //}
    public async Task NavigateToAsync(string pagekey)
    {
        await Shell.Current.GoToAsync(pagekey);
        //await Shell.Current.GoToAsync($"//{_routeService.GetRoute(pagekey)}");
    }

    public Task NavigateToAsync(string pagekey, object parameter)
    {
        throw new NotImplementedException();
    }
}
