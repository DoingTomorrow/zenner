// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommPlaying
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  public class WidcommPlaying
  {
    private WidcommBluetoothFactory m_factory;

    public WidcommPlaying()
    {
      WidcommBluetoothFactory factoryOfTypeOrDefault = BluetoothFactory.GetTheFactoryOfTypeOrDefault<WidcommBluetoothFactory>();
      foreach (BluetoothFactory factory in (IEnumerable<BluetoothFactory>) BluetoothFactory.Factories)
      {
        if (factory is WidcommBluetoothFactory bluetoothFactory)
        {
          this.m_factory = bluetoothFactory;
          break;
        }
      }
      Trace.Assert(factoryOfTypeOrDefault == this.m_factory);
      Trace.Assert(object.ReferenceEquals((object) factoryOfTypeOrDefault, (object) this.m_factory));
      if (this.m_factory == null)
        throw new InvalidOperationException("Widcomm stack not present.");
    }

    internal WidcommPlaying(WidcommBluetoothFactory factory) => this.m_factory = factory;

    public RemoteDeviceState FindIfPresentOrConnected(BluetoothAddress addr)
    {
      return this.FindIfPresentOrConnected(WidcommUtils.FromBluetoothAddress(addr));
    }

    public RemoteDeviceState FindIfPresentOrConnected(byte[] bda)
    {
      WidcommBtInterface widcommBtInterface = this.m_factory.GetWidcommBtInterface();
      int tickCount1 = Environment.TickCount;
      MiscUtils.Trace_WriteLine("FiPoC: gonna IsRemoteDeviceConnected");
      bool flag = widcommBtInterface.IsRemoteDeviceConnected(bda);
      int tickCount2 = Environment.TickCount;
      tickCount1 = Environment.TickCount;
      MiscUtils.Trace_WriteLine("FiPoC: gonna IsRemoteDevicePresent");
      SDK_RETURN_CODE sdkReturnCode = widcommBtInterface.IsRemoteDevicePresent(bda);
      int tickCount3 = Environment.TickCount;
      RemoteDeviceState presentOrConnected;
      if (flag)
      {
        presentOrConnected = RemoteDeviceState.Connected;
      }
      else
      {
        switch (sdkReturnCode)
        {
          case SDK_RETURN_CODE.Success:
            presentOrConnected = RemoteDeviceState.Present;
            break;
          case SDK_RETURN_CODE.NotSupported:
            presentOrConnected = RemoteDeviceState.Unknown;
            break;
          default:
            presentOrConnected = RemoteDeviceState.NotPresent;
            break;
        }
      }
      MiscUtils.Trace_WriteLine("FindIfPresentOrConnected: c={0} + p={1} => {2}", (object) flag, (object) sdkReturnCode, (object) presentOrConnected);
      return presentOrConnected;
    }

    public bool DoPowerDownUpReset
    {
      get => this.m_factory.DoPowerDownUpReset;
      set => this.m_factory.DoPowerDownUpReset = value;
    }
  }
}
