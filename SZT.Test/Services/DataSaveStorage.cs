using SQLite;
using SZT.Test.Models;
using Android;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using OfficeOpenXml;
using Bumptech.Glide.Util;
using System.Diagnostics;
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

    /// <summary>
    /// 初始化数据库
    /// </summary>
    /// <returns></returns>
    public async Task InitializeAsync()
    {
        //await Connection.DropTableAsync<Data>(); // 删除旧表结构（如果已存在）
        await Connection.CreateTableAsync<Data>();
        await Connection.CreateTableAsync<DataNumber>();
    }


    /// <summary>
    /// 加入一组数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="numbers"></param>
    /// <returns></returns>
    public async Task AddDataAsync(Data data,List<int> numbers) {
        if (_connection == null)
            await InitializeAsync();
        await Connection.InsertAsync(data);
        await Task.Delay(100); // 等待 SQLite 异步操作确保 Id 生成（可调整

        // 检查 data.Id 是否已经成功生成
        if (data.Id <= 0)
        {
            throw new InvalidOperationException("Data ID 生成失败，无法关联 DataNumber 表。");
        }

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
        await  CloseDataAsync();
    }


    // 获取所有 Data 及其关联的 List<int> 数据
    public async Task<List<Data>> GetDataAsync()
    {
        string root = USBDiskRoute;
        Debug.WriteLine(root);

        if (_connection == null)
            await InitializeAsync();
        // 获取所有 Data 项
        var dataList = await Connection.Table<Data>().ToListAsync();

        // 为每个 Data 获取关联的 List<int>
        foreach (var data in dataList)
        {
            data.Numbers = await GetNumbersAsync(data.Id);
        }

        await CloseDataAsync();  // 确保连接关闭在 return 之前
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

    private async Task<SQLiteAsyncConnection> GetDatabaseAsync()
    {
        if (_connection == null)
        {
            _connection = new SQLiteAsyncConnection(DataDbPath);
            await _connection.CreateTableAsync<Data>(); // 初始化创建表结构
            await _connection.CreateTableAsync<DataNumber>();
        }
        return _connection;
    }

    /// <summary>
    /// 更新数据库
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public async Task UpdateDataSelectionStatus(Data data)
    {
        var db = await GetDatabaseAsync(); // 获取数据库实例
        await db.UpdateAsync(data); // 更新 
    }


    //关闭数据库
    private async Task CloseDataAsync()
    {
        await Connection.CloseAsync();
    }


    /// <summary>
    /// 移除一个数据
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public async Task RemoveData(Data data)
    {
        await Connection.DeleteAsync(data); // 删除 Data 对象
        await Connection.ExecuteAsync("DELETE FROM DataNumber WHERE DataId = ?", data.Id); // 删除关联的 List<int> 数据
    }


    /// <summary>
    /// 清除所有数据
    /// </summary>
    /// <returns></returns>
    public async Task ClearAllDataAsync() {
        await Connection.ExecuteAsync("DELETE FROM Data");
        // 重置自增序列，让下一个插入的数据从 ID 1 开始
        await Connection.ExecuteAsync("DELETE FROM sqlite_sequence WHERE name='Data'");
        await Connection.ExecuteAsync("DELETE FROM DataNumber");
        await CloseDataAsync();
    }


    /// <summary>
        /// 申请访问安卓地址
        /// </summary>
        /// <returns></returns>
    public async Task RequestStoragePermissionAsync()
    {
        if (ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.WriteExternalStorage) != Android.Content.PM.Permission.Granted)
        {
            ActivityCompat.RequestPermissions(Platform.CurrentActivity, new string[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage }, 0);
        }
    }

    public static string USBDiskRoute
    {
        get
        {
            try
            {
                var directories = Directory.GetDirectories("/storage");
                foreach (string driver in directories)
                {
                    if (driver.Contains("udiskh"))
                    {
                        var subDirs = Directory.GetDirectories(driver);
                        if (subDirs.Length > 0)
                        {
                            return subDirs[0]; // 返回第一个子目录路径
                        }
                        return driver; // 返回根 USB_DISK 路径
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing USB storage: {ex.Message}");
            }

            return null;
        }
    }



    /// <summary>
    /// 保存为excle
    /// </summary>
    /// <param name="Uid"></param>
    /// <returns></returns>
    public async Task ExportDataRowToExcelAsync(string Uid)
    {
        string root = USBDiskRoute;
        // 请求存储权限
        await RequestStoragePermissionAsync();

        // 查询指定 ID 的数据行
        var dataRow = await Connection.Table<Data>().FirstOrDefaultAsync(d => d.Uid == Uid);
        // 加载关联的 Numbers 数据
        dataRow.Numbers = await GetNumbersAsync(dataRow.Id);
        if (dataRow == null)
            return;

        // 定义文件保存路径
        //var downloadsPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
        
        // 定义根目录下的 SZTData 文件夹路径
        var rootPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
        var downloadsPath = Path.Combine(rootPath, "SZTData");

        // 检查并创建 SZTData 文件夹（如果不存在）
        if (!Directory.Exists(downloadsPath))
        {
            Directory.CreateDirectory(downloadsPath);
        }
        var logTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

        var filePath = Path.Combine(downloadsPath, $"SZT_{Uid}_{logTime}.xlsx");

        // 创建 Excel 文件并打开工作簿
        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            // 创建第一个工作表，显示 ID、UID 和 LogTime
            var worksheet1 = package.Workbook.Worksheets.Add("DataInfo");
            worksheet1.Cells[1, 1].Value = "ID";
            worksheet1.Cells[1, 2].Value = "UID";
            worksheet1.Cells[1, 3].Value = "LogTime";

            // 填充数据到第一个工作表
            worksheet1.Cells[2, 1].Value = dataRow.Id;
            worksheet1.Cells[2, 2].Value = dataRow.Uid;
            worksheet1.Cells[2, 3].Value = dataRow.LogTime.ToString("G");

            // 创建第二个工作表，显示 Numbers 数据
            var worksheet2 = package.Workbook.Worksheets.Add("numbers");
            worksheet2.Cells[1, 1].Value = "numbers";

            // 填充 Numbers 数据到第二个工作表
            var rowIndex = 2;
            foreach (var number in dataRow.Numbers)
            {
                worksheet2.Cells[rowIndex++, 1].Value = number;
            }

            // 保存 Excel 文件
            await package.SaveAsync();
        }
    }
}

