using SQLite;
using SZT.Test.Models;

namespace SZT.Test.Services;

public class DataSaveStorage : IDataSaveStorage
{
    //创建数据库地址
    public const string DbFileName = "SZT.sqlite3";
    public static readonly string DataDbPath =
        Path.Combine(
        Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData), DbFileName);

    //打开数据库连接
    private SQLiteAsyncConnection? _connection;
    public SQLiteAsyncConnection Connection =>
        _connection ??= new SQLiteAsyncConnection(DataDbPath);


    public async Task InitializeAsync()
    {
        //await Connection.DropTableAsync<Data>(); // 删除旧表结构（如果已存在）
        await Connection.CreateTableAsync<Data>();
        await Connection.CreateTableAsync<DataNumber>();
    }

    public async Task AddDataAsync(Data data,List<int> numbers) { 
    
        await Connection.InsertAsync(data);
        // 保存 List<int> 的每个值
        foreach (var number in numbers)
        {
            var dataNumber = new DataNumber
            {
                DataId = data.Id,  // 使用 Data 的 Id 作为外键
                Number = number
            };
            await Connection.InsertAsync(dataNumber);
        }
    }

    // 获取所有 Data 及其关联的 List<int> 数据
    public async Task<List<Data>> GetDataAsync()
    {
        // 获取所有 Data 项
        var dataList = await Connection.Table<Data>().ToListAsync();

        // 为每个 Data 获取关联的 List<int>
        foreach (var data in dataList)
        {
            data.Numbers = await GetNumbersAsync(data.Id);
        }
        return dataList;
    }

    // 根据 Data 的 Id 获取关联的 List<int>
    private async Task<List<int>> GetNumbersAsync(int dataId)
    {
        var numbers = await Connection.Table<DataNumber>()
                            .Where(dn => dn.DataId == dataId)
                            .ToListAsync();
        return numbers.Select(dn => dn.Number).ToList();
    }


    public async Task RemoveData(Data data)
    {
        await Connection.DeleteAsync(data); // 删除 Data 对象
        await Connection.ExecuteAsync("DELETE FROM DataNumber WHERE DataId = ?", data.Id); // 删除关联的 List<int> 数据
    }

    public async Task ClearAllDataAsync() {
        await Connection.ExecuteAsync("DELETE FROM Data");
        // 重置自增序列，让下一个插入的数据从 ID 1 开始
        await Connection.ExecuteAsync("DELETE FROM sqlite_sequence WHERE name='Data'");
        await Connection.ExecuteAsync("DELETE FROM DataNumber");
    }
}

