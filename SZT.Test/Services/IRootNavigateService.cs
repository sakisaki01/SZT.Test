
namespace SZT.Test.Services;

public interface IRootNavigateService
{
     Task NavigateToAsync(string pagekey);
     Task NavigateToAsync(string pagekey , object parameter);
}
