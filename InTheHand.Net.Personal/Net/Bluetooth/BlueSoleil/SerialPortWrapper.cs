// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BlueSoleil.SerialPortWrapper
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System.IO;
using System.IO.Ports;

#nullable disable
namespace InTheHand.Net.Bluetooth.BlueSoleil
{
  internal class SerialPortWrapper : ISerialPortWrapper
  {
    private readonly SerialPort _child;

    internal SerialPortWrapper(SerialPort port) => this._child = port;

    void ISerialPortWrapper.Close() => this._child.Close();

    void ISerialPortWrapper.Open() => this._child.Open();

    Stream ISerialPortWrapper.BaseStream => this._child.BaseStream;

    int ISerialPortWrapper.BytesToRead => this._child.BytesToRead;

    Handshake ISerialPortWrapper.Handshake
    {
      get => this._child.Handshake;
      set => this._child.Handshake = value;
    }

    string ISerialPortWrapper.PortName
    {
      get => this._child.PortName;
      set => this._child.PortName = value;
    }

    int ISerialPortWrapper.ReadBufferSize
    {
      get => this._child.ReadBufferSize;
      set => this._child.ReadBufferSize = value;
    }

    int ISerialPortWrapper.WriteBufferSize
    {
      get => this._child.WriteBufferSize;
      set => this._child.WriteBufferSize = value;
    }

    event SerialDataReceivedEventHandler ISerialPortWrapper.DataReceived
    {
      add => this._child.DataReceived += value;
      remove => this._child.DataReceived -= value;
    }
  }
}
