﻿

using SQLite;
using SZT.Test.Models;

namespace SZT.Test.Services;

public class DataSaveStorage : IDataSaveStorage
{
    //创建数据库地址
    public const string DbFileName = "SZT.sqlite";
    public static readonly string DataDbPath =
        Path.Combine(
        Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData), DbFileName);

    //数据库连接
    private SQLiteAsyncConnection? _connection;
    public SQLiteAsyncConnection Connection =>
        _connection ??= new SQLiteAsyncConnection(DataDbPath);

    public async Task InitializeAsync()
    {
        await Connection.CreateTableAsync<Data>();
    }

    public async Task AddDataAsync(Data data)
    {
        await Connection.InsertAsync(data);
    }

    public async Task<IEnumerable<Data>> GetDataAsync()
    {
        return await Connection.Table<Data>().ToListAsync();
    }


    public void RemoveData(Data data)
    {
        throw new NotImplementedException();
    }
}
