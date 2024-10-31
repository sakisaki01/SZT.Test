

using SZT.Test.Models;

namespace SZT.Test.Services;

public  interface IDataSaveStorage
{
    Task InitializeAsync();

    Task AddDataAsync(Data data);

    Task RemoveData(Data data);

    Task ClearAllDataAsync();

    Task<IEnumerable<Data>> GetDataAsync();
}
