// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommBluetoothFactoryBase
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using System;
using System.Collections.Generic;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal abstract class WidcommBluetoothFactoryBase : BluetoothFactory
  {
    private List<WidcommRfcommStreamBase> _livePorts = new List<WidcommRfcommStreamBase>();

    internal abstract WidcommBtInterface GetWidcommBtInterface();

    internal abstract WidcommRfcommStreamBase GetWidcommRfcommStream();

    internal abstract WidcommRfcommStreamBase GetWidcommRfcommStreamWithoutRfcommIf();

    internal abstract IRfcommPort GetWidcommRfcommPort();

    internal abstract IRfCommIf GetWidcommRfCommIf();

    internal abstract ISdpService GetWidcommSdpService();

    internal abstract void EnsureLoaded();

    internal abstract WidcommPortSingleThreader GetSingleThreader();

    internal abstract bool IsWidcommSingleThread();

    internal void AddPort(WidcommRfcommStreamBase port)
    {
      lock (this._livePorts)
        this._livePorts.Add(port);
    }

    internal void RemovePort(WidcommRfcommStreamBase port)
    {
      lock (this._livePorts)
        this._livePorts.Remove(port);
    }

    internal WidcommRfcommStreamBase[] GetPortList()
    {
      lock (this._livePorts)
        return this._livePorts.ToArray();
    }

    [Obsolete("_untested_")]
    internal virtual void AddThingsToKeepAlive<TObject>(TObject o) where TObject : class
    {
    }
  }
}
