using Maixin.Auto.Services;


namespace SZT.Test.Models;

public class Chart
{
    private readonly SerialPort _serialPort;

    public int Data {  get; set; }

    public Chart(int data)
    {
        Data = data;
    }

    public string Name { get; set; }

    public Chart(string name, int data)
    {
        this.Name = name;
        this.Data = data;
    }
}
