
using LiquidMobile.Services;

using Maixin.Auto.Services;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Maixin.Auto.Services
{
    public class SerialPort : IDisposable
    {
        private readonly Queue<byte> Buffer = new Queue<byte>();




        private string Path { get; set; }

        private int Baudrate { get; set; }
        private Parity Parity { get; set; }


        private bool _IsOpen = false;
        public bool IsOpen
        {
            get
            {
                return _IsOpen;
            }
        }

        public SerialPort(string path, int baudrate, Parity parity = Parity.None)
        {
            Path = path;
            Baudrate = baudrate;
            this.Parity = parity;

        }
        public int BytesToRead
        {
            get
            {
                return Buffer.Count;
            }
        }
        private ISerialPort_MyControl ISerialPort_MyControl_Ins;
        public ManualResetEvent Waiter { get; set; } = new ManualResetEvent(true);
        public int count = 0;
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <returns></returns>
        public bool Open()
        {
            ISerialPort_MyControl_Ins = new SerialPort_MyControl();

            ISerialPort_MyControl_Ins.Open(Path, Baudrate, Parity);

            _IsOpen = true;
            ISerialPort_MyControl_Ins.DataReceEvent += new EventHandler<SerialPortDataReceEventArgs>((s, e) =>
            {
                count++;
                if(count>=100)
                {
                    count = 0;
                    Console.WriteLine($"MCREOP1:{DateTime.Now.Minute}:{DateTime.Now.Second}:{DateTime.Now.Millisecond}");
                }

                //Console.WriteLine($"MCREOP2:{DateTime.Now.Minute}:{DateTime.Now.Second}:{DateTime.Now.Millisecond}");
                DataReceived?.Invoke(this, e);
                //Console.WriteLine($"MCREOP3:{DateTime.Now.Minute}:{DateTime.Now.Second}:{DateTime.Now.Millisecond}");
            });
            return true;
        }
        public void TrimBuffer()
        {
            Buffer.Clear();
        }


        /// <summary>
        /// 关闭串口
        /// </summary>
        public void Close()
        {
            _IsOpen = false;
            ISerialPort_MyControl_Ins.Close();
        }

        /// <summary>
        /// 串口接收事件
        /// </summary>
        public event EventHandler<SerialPortDataReceEventArgs> DataReceived;

        private void Send(byte[] data, bool Force = false)
        {
            if (!Force)
                Waiter.WaitOne();
            lock (ISerialPort_MyControl_Ins.OutputStream)
            {
                try
                {
                    ISerialPort_MyControl_Ins.OutputStream.Write(data, 0, data.Length);
                }
                catch (IOException e)
                {
                    Debug.Print(e.ToString());
                }
                catch (Exception)
                {

                }
            }
        }
        public void Write(byte Data)
        {
            byte[] wb = { Data };
            Send(wb);
        }
        public void WriteWith1R(string Data, bool Force = false)
        {
            Write($"/1{Data}\r", Force);
        }
        public void Write(string Data, bool Force = false)
        {
            byte[] DataInBytes = new byte[Data.Length];
            for (int i = 0; i < Data.Length; i++)
            {
                DataInBytes[i] = Convert.ToByte(Data[i]);
            }
            Send(DataInBytes, Force);
        }
        public void Write(byte[] Data, int start, int length)
        {
            byte[] array = new byte[length];
            Array.Copy(Data, array, length);
            Send(array);
        }
        public void Dispose()
        {
            Close();
        }
        public int TimeOut { get; set; } = 1000;
        public void Read(byte[] Buff, int StartIndex, int EndIndex)
        {
            List<byte> returns = new List<byte>();
            DateTime StartTime = DateTime.Now;
            while (true)
            {
                if (DateTime.Now - StartTime > TimeSpan.FromMilliseconds(TimeOut)) break;
                if (Buffer.Count > 0)
                {
                    returns.Add(Buffer.Dequeue());
                }
                if (returns.Count >= EndIndex) break;
            }
            Array.Copy(returns.ToArray(), Buff, EndIndex);
        }
        public byte[] ReadToChar()
        {
            List<byte> Reutrns = new List<byte>();
            DateTime StartTime = DateTime.Now;
            while (true)
            {
                //if (DateTime.Now - StartTime > TimeSpan.FromMilliseconds(TimeOut)) break;
                byte Pick;
                try
                {
                    Pick = Buffer.Dequeue();
                }
                catch
                {
                    continue;
                }
                if (Pick == '\r')
                {

                    if (Buffer.Count > 0 && Buffer.Peek() == '\n')
                    {
                        Buffer.Dequeue();
                        break;
                    }
                    break;
                }
                else if (Pick == '\n')
                {

                    break;
                }
                Reutrns.Add(Pick);

            }
            return Reutrns.ToArray();
        }
        public string ReadLine()
        {
            string Reutrns = string.Empty;
            DateTime StartTime = DateTime.Now;
            while (true)
            {
                if (DateTime.Now - StartTime > TimeSpan.FromMilliseconds(TimeOut)) break;
                char Pick;
                try
                {
                    Pick = Convert.ToChar(Buffer.Dequeue());
                }
                catch
                {
                    continue;
                }
                if (Pick == '\r')
                {

                    if (Buffer.Count > 0 && Buffer.Peek() == '\n')
                    {
                        Buffer.Dequeue();
                        break;
                    }
                    break;
                }
                else if (Pick == '\n')
                {

                    break;
                }
                Reutrns += Pick;

            }
            return Reutrns;
        }

        internal void WriteLine(string res, bool Force = false)
        {
            Write($"{res}\r\n", Force);
        }
    }
}

