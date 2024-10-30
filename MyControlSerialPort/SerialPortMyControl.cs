using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;


using System.IO;
using Android_serialport_api;
using Java.Interop;
using Device = Android_serialport_api.Device;

namespace LiquidMobile.Services
{
    class SerialPort_MyControl : ISerialPort_MyControl
    {
        
        private SerialPortManager SerialPortMannagerG { get; set; }
        private Stream _OutputStream;
        public Stream OutputStream { get => _OutputStream; }
        private Stream _InputStream;

        public event EventHandler<SerialPortDataReceEventArgs> DataReceEvent;

        public Stream InputStream { get => _InputStream; }

        public string Path { get; set; }


        public void Open(string Path, int Speed, Parity Parity)
        {
            Device device = new Device();
            this.Path = Path;
            device.Path = Path;
            device.Speed = Speed;
            device.Block = true;
            device.Parity = Parity == Parity.None ? 'n' : Parity == Parity.Even ? 'e' : Parity == Parity.Odd ? 'o' : 'n';
            SerialPortMannagerG = new SerialPortManager(device);
            _OutputStream = SerialPortMannagerG.MOutputStream;
            _InputStream = SerialPortMannagerG.MInputStream;
            SerialPortMannagerG.DataReceive += SerialPortMannagerG_DataReceive;
        }

        private void SerialPortMannagerG_DataReceive(object sender, SerialPortManager.DataReceiveEventArgs e)
        {
            var a = e.P0;
            var b = e.P1;
            DataReceEvent.Invoke(this as ISerialPort_MyControl, new SerialPortDataReceEventArgs(a, b));
        }

        public void Close()
        {
            SerialPortMannagerG.CloseSerialPort();
        }
    }
}