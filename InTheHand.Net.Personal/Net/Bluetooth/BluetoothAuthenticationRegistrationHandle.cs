// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BluetoothAuthenticationRegistrationHandle
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Msft;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  internal sealed class BluetoothAuthenticationRegistrationHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    private object m_objectToKeepAlive;
    private object m_objectToKeepAlive2;

    public BluetoothAuthenticationRegistrationHandle()
      : base(true)
    {
    }

    protected override bool ReleaseHandle()
    {
      bool flag = NativeMethods.BluetoothUnregisterAuthentication(this.handle);
      Marshal.GetLastWin32Error();
      return flag;
    }

    internal void SetObjectToKeepAlive(object objectToKeepAlive, object objectToKeepAlive2)
    {
      this.m_objectToKeepAlive = objectToKeepAlive;
      this.m_objectToKeepAlive2 = objectToKeepAlive2;
    }
  }
}
