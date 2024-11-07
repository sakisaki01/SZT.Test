
using SZT.Test.Models;
namespace SZT.Test.Services;

public  interface IDataSaveStorage
{
    Task InitializeAsync();

    Task AddDataAsync(Data data, List<int> numbers);

    Task RemoveData(Data data);

    Task ClearAllDataAsync();

    Task<List<Data>> GetDataAsync();

    Task ExportDataRowToExcelAsync(string Uid);

}
