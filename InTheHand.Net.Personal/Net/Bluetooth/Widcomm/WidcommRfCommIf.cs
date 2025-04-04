// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommRfCommIf
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal sealed class WidcommRfCommIf : IRfCommIf
  {
    private IntPtr m_pRfCommIf;

    public IntPtr PObject => throw new NotSupportedException();

    public void Create()
    {
      WidcommRfCommIf.NativeMethods.RfCommIf_Create(out this.m_pRfCommIf);
      if (this.m_pRfCommIf == IntPtr.Zero)
        throw new InvalidOperationException("Native object creation failed.");
    }

    public void Destroy(bool disposing)
    {
      if (!(this.m_pRfCommIf != IntPtr.Zero))
        return;
      WidcommRfCommIf.NativeMethods.RfCommIf_Destroy(this.m_pRfCommIf);
      this.m_pRfCommIf = IntPtr.Zero;
    }

    public bool ClientAssignScnValue(Guid serviceGuid, int scn)
    {
      byte scn1 = checked ((byte) scn);
      return WidcommRfCommIf.NativeMethods.RfCommIf_Client_AssignScnValue(this.m_pRfCommIf, ref serviceGuid, scn1);
    }

    public bool SetSecurityLevel(byte[] serviceName, BTM_SEC securityLevel, bool isServer)
    {
      return WidcommRfCommIf.NativeMethods.RfCommIf_SetSecurityLevel(this.m_pRfCommIf, serviceName, securityLevel, isServer);
    }

    public int GetScn() => WidcommRfCommIf.NativeMethods.RfCommIf_GetScn(this.m_pRfCommIf);

    private static class NativeMethods
    {
      [DllImport("32feetWidcomm")]
      internal static extern IntPtr RfCommIf_Create(out IntPtr ppRfCommIf);

      [DllImport("32feetWidcomm")]
      internal static extern void RfCommIf_Destroy(IntPtr pRfCommPort);

      [DllImport("32feetWidcomm")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool RfCommIf_Client_AssignScnValue(
        IntPtr pRfCommPort,
        ref Guid serviceGuid,
        byte scn);

      [DllImport("32feetWidcomm")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool RfCommIf_SetSecurityLevel(
        IntPtr pRfCommPort,
        byte[] serviceName,
        BTM_SEC securityLevel,
        [MarshalAs(UnmanagedType.Bool)] bool isServer);

      [DllImport("32feetWidcomm")]
      internal static extern int RfCommIf_GetScn(IntPtr pRfCommPort);
    }
  }
}
