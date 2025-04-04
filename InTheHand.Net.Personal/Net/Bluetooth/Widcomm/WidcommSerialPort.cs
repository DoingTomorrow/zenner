// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommSerialPort
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Ports;
using System;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  public abstract class WidcommSerialPort : IDisposable
  {
    public static WidcommSerialPort CreateClient(BluetoothAddress device)
    {
      return WidcommSerialPort.CreateClient2(device);
    }

    private static WidcommSerialPort CreateClient2(BluetoothAddress device)
    {
      WidcommSppClient client2 = new WidcommSppClient(WidcommBluetoothFactory.GetWidcommIfExists());
      client2.CreatePort(device);
      return (WidcommSerialPort) client2;
    }

    public abstract string PortName { get; }

    public abstract BluetoothAddress Address { get; }

    public abstract Guid Service { get; }

    public void Close() => this.Dispose();

    protected void OnPortStatusChanged(object server, PortStatusChangedEventArgs e)
    {
      this.StatusChanged(server, e);
    }

    private event EventHandler<PortStatusChangedEventArgs> StatusChanged = delegate { };

    protected abstract void Dispose(bool disposing);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }
  }
}
