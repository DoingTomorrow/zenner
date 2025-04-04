// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BlueSoleil.SerialPortNetworkStream
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using System;
using System.IO.Ports;

#nullable disable
namespace InTheHand.Net.Bluetooth.BlueSoleil
{
  internal abstract class SerialPortNetworkStream : DecoratorNetworkStream
  {
    protected readonly ISerialPortWrapper _port;

    internal SerialPortNetworkStream(SerialPort port, IBluetoothClient cli)
      : this((ISerialPortWrapper) new SerialPortWrapper(port), cli)
    {
    }

    internal SerialPortNetworkStream(ISerialPortWrapper port, IBluetoothClient cli)
      : base(port.BaseStream)
    {
      this._port = port;
    }

    public override bool DataAvailable => this._port.BytesToRead > 0;

    internal int Available => this._port.BytesToRead;

    internal new abstract bool Connected { get; }

    public override void Flush()
    {
      try
      {
        base.Flush();
      }
      catch (ObjectDisposedException ex)
      {
      }
    }

    ~SerialPortNetworkStream() => this.Dispose(false);

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (!disposing)
          return;
        this._port.Close();
      }
      finally
      {
        base.Dispose(disposing);
      }
    }
  }
}
