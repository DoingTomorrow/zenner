// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.L2CapIf
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal sealed class L2CapIf : IRfCommIf
  {
    private IntPtr _pIf;

    internal L2CapIf()
    {
    }

    public IntPtr PObject => this._pIf;

    public void Create()
    {
      WidcommL2CapClient.NativeMethods.L2CapIf_Create(out this._pIf);
      if (this._pIf == IntPtr.Zero)
        throw new InvalidOperationException("Native object creation failed.");
    }

    public void Destroy(bool disposing)
    {
      MiscUtils.Trace_WriteLine("L2CapIf.Destroy()");
      if (!(this._pIf != IntPtr.Zero))
        return;
      WidcommL2CapClient.NativeMethods.L2CapIf_Deregister(this._pIf);
      WidcommL2CapClient.NativeMethods.L2CapIf_Destroy(this._pIf);
      this._pIf = IntPtr.Zero;
    }

    public bool ClientAssignScnValue(Guid serviceGuid, int scn)
    {
      ushort psm = checked ((ushort) scn);
      bool flag1 = WidcommL2CapClient.NativeMethods.L2CapIf_AssignPsmValue(this._pIf, ref serviceGuid, psm);
      MiscUtils.Trace_WriteLine("L2CapIf_AssignPsmValue ret: {0} <- psm: {1}=0x{1:X}", (object) flag1, (object) psm);
      if (!flag1)
        return false;
      MiscUtils.Trace_WriteLine("L2CapIf_GetPsm PSM: {0}=0x{0:X}", (object) WidcommL2CapClient.NativeMethods.L2CapIf_GetPsm(this._pIf));
      bool flag2 = WidcommL2CapClient.NativeMethods.L2CapIf_Register(this._pIf);
      MiscUtils.Trace_WriteLine("L2CapIf_Register ret :{0}", (object) flag2);
      return flag2;
    }

    public bool SetSecurityLevel(byte[] serviceName, BTM_SEC securityLevel, bool isServer)
    {
      return WidcommL2CapClient.NativeMethods.L2CapIf_SetSecurityLevel(this._pIf, "foo", securityLevel, isServer);
    }

    public int GetScn() => (int) WidcommL2CapClient.NativeMethods.L2CapIf_GetPsm(this._pIf);
  }
}
