// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommBluetoothSecurity
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Factory;
using System;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal sealed class WidcommBluetoothSecurity : IBluetoothSecurity
  {
    private readonly WidcommBluetoothFactoryBase _factory;

    internal WidcommBluetoothSecurity(WidcommBluetoothFactoryBase factory)
    {
      this._factory = factory;
    }

    public bool PairRequest(BluetoothAddress device, string pin)
    {
      BOND_RETURN_CODE bondReturnCode = pin != null ? this.Bond_(device, pin) : throw new ArgumentNullException(nameof (pin));
      return bondReturnCode == BOND_RETURN_CODE.SUCCESS || bondReturnCode == BOND_RETURN_CODE.ALREADY_BONDED;
    }

    internal BOND_RETURN_CODE Bond_(BluetoothAddress device, string pin)
    {
      MiscUtils.Trace_WriteLine("Calling CBtIf:Bond...");
      BOND_RETURN_CODE bondReturnCode = this._factory.GetWidcommBtInterface().Bond(device, pin);
      MiscUtils.Trace_WriteLine("Bond returned: {0} = 0x{1:X}", (object) bondReturnCode, (object) (int) bondReturnCode);
      return bondReturnCode;
    }

    public bool RemoveDevice(BluetoothAddress device)
    {
      bool flag = this._factory.GetWidcommBtInterface().UnBond(device);
      return true ? WidcommBtInterface.DeleteKnownDevice(device) : flag;
    }

    public bool SetPin(BluetoothAddress device, string pin)
    {
      throw new NotImplementedException("The method or operation is not implemented.");
    }

    public bool RevokePin(BluetoothAddress device)
    {
      throw new NotImplementedException("The method or operation is not implemented.");
    }

    public BluetoothAddress GetPinRequest() => throw new NotSupportedException();

    public bool RefusePinRequest(BluetoothAddress device) => throw new NotSupportedException();

    public bool SetLinkKey(BluetoothAddress a, Guid linkkey) => throw new NotSupportedException();
  }
}
