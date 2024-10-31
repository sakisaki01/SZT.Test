
using SQLite;

namespace SZT.Test.Models;

public  class Data
{
    [PrimaryKey ,AutoIncrement]
    public string Id { get; set; }

    public List<int> Value { get; set; }

    public List<int> Count { get; set; }
}
