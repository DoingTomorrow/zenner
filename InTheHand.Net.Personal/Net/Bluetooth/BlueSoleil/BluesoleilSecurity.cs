// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BlueSoleil.BluesoleilSecurity
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

#nullable disable
namespace InTheHand.Net.Bluetooth.BlueSoleil
{
  internal class BluesoleilSecurity : IBluetoothSecurity
  {
    private readonly BluesoleilFactory _factory;
    private readonly object _lock = new object();
    private readonly Dictionary<uint, byte[]> _pinResponses = new Dictionary<uint, byte[]>();

    internal BluesoleilSecurity(BluesoleilFactory fcty) => this._factory = fcty;

    internal StackConsts.CallbackResult HandlePinReqInd(uint dev_hdl)
    {
      byte[] pinResponse;
      lock (this._lock)
        pinResponse = !this._pinResponses.ContainsKey(dev_hdl) ? (byte[]) null : this._pinResponses[dev_hdl];
      if (pinResponse == null)
        return StackConsts.CallbackResult.NotHandled;
      ThreadPool.QueueUserWorkItem(new WaitCallback(this.PinReply_Runner), (object) new BluesoleilSecurity.DeviceAndPin()
      {
        Device = dev_hdl,
        Pin = pinResponse
      });
      return StackConsts.CallbackResult.Handled;
    }

    private void PinReply_Runner(object state)
    {
      BluesoleilSecurity.DeviceAndPin deviceAndPin = (BluesoleilSecurity.DeviceAndPin) state;
      int num = (int) this._factory.Api.Btsdk_PinCodeReply(deviceAndPin.Device, deviceAndPin.Pin, (ushort) deviceAndPin.Pin.Length);
    }

    bool IBluetoothSecurity.PairRequest(BluetoothAddress device, string pin)
    {
      uint handle = BluesoleilDeviceInfo.CreateFromGivenAddress(device, this._factory).Handle;
      this._factory.RegisterCallbacksOnce();
      try
      {
        if (pin != null)
          this.SetPin(handle, pin);
        return this._factory.Api.Btsdk_PairDevice(handle) == BtSdkError.OK;
      }
      finally
      {
        if (pin != null)
          this.RevokePin(handle);
      }
    }

    bool IBluetoothSecurity.RemoveDevice(BluetoothAddress device)
    {
      return this._factory.Api.Btsdk_DeleteRemoteDeviceByHandle(BluesoleilDeviceInfo.CreateFromGivenAddress(device, this._factory).Handle) == BtSdkError.OK;
    }

    private void SetPin(uint hDev, string pin)
    {
      lock (this._lock)
      {
        byte[] bytes = Encoding.UTF8.GetBytes(pin);
        this._pinResponses.Add(hDev, bytes);
      }
    }

    private void RevokePin(uint hDev)
    {
      lock (this._lock)
        this._pinResponses.Remove(hDev);
    }

    bool IBluetoothSecurity.SetPin(BluetoothAddress device, string pin)
    {
      throw new NotImplementedException();
    }

    bool IBluetoothSecurity.RevokePin(BluetoothAddress device)
    {
      throw new NotImplementedException();
    }

    BluetoothAddress IBluetoothSecurity.GetPinRequest() => throw new NotSupportedException();

    bool IBluetoothSecurity.RefusePinRequest(BluetoothAddress device)
    {
      throw new NotSupportedException();
    }

    bool IBluetoothSecurity.SetLinkKey(BluetoothAddress device, Guid linkKey)
    {
      throw new NotImplementedException();
    }

    private class DeviceAndPin
    {
      public uint Device { get; set; }

      public byte[] Pin { get; set; }
    }
  }
}
