using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiquidMobile.Services
{
    public interface ISerialPort_MyControl
    {
        /// <summary>
        /// 发送数据流
        /// </summary>
        Stream OutputStream { get; }
        /// <summary>
        /// 接收数据流
        /// </summary>
        Stream InputStream { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path">设备路径</param>
        /// <param name="Speed">波特率</param>
        void Open(string Path, int Speed, Parity Parity);
        void Close();
        event EventHandler<SerialPortDataReceEventArgs> DataReceEvent;
    }
    public class SerialPortDataReceEventArgs : EventArgs
    {
        public SerialPortDataReceEventArgs(byte[] Data, int Length)
        {
            this.Data = Data;
            this.Length = Length;
        }
        public byte[] Data { get; set; }
        public int Length { get; set; }
    }

    public enum Parity
    {
        /// <summary>
        /// 无校验
        /// </summary>
        None,
        /// <summary>
        /// 偶校验
        /// </summary>
        Even,
        /// <summary>
        /// 奇校验
        /// </summary>
        Odd
    }
}
