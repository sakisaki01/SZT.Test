

using SZT.Test.Models;

namespace SZT.Test.Services;

public  interface IDataSaveStorage
{
    Task InitializeAsync();

    Task AddDataAsync(Data data);

    void RemoveData(Data data);

    Task<IEnumerable<Data>> GetDataAsync();
}
