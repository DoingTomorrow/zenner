// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommRfcommInterface
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.IO;
using Utils;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal sealed class WidcommRfcommInterface : IDisposable
  {
    private IRfCommIf m_RfCommIf;

    internal WidcommRfcommInterface(IRfCommIf rfCommIf) => this.m_RfCommIf = rfCommIf;

    internal void SetScnForPeerServer(Guid serviceGuid, int scn)
    {
      if (!this.m_RfCommIf.ClientAssignScnValue(serviceGuid, scn))
        throw new IOException("IOError on socket.", (Exception) WidcommSocketExceptions.Create_NoResultCode(-1, nameof (SetScnForPeerServer)));
    }

    internal void SetSecurityLevelClient(BTM_SEC securityLevel)
    {
      if (!this.m_RfCommIf.SetSecurityLevel(WidcommUtils.SetSecurityLevel_Client_ServiceName, securityLevel, false))
        throw new IOException("IOError on socket.", (Exception) WidcommSocketExceptions.Create_NoResultCode(-1, "SetSecurityLevel"));
    }

    internal int SetScnForLocalServer(Guid serviceGuid, int scn)
    {
      if (!this.m_RfCommIf.ClientAssignScnValue(serviceGuid, scn))
        throw new IOException("IOError on socket.", (Exception) WidcommSocketExceptions.Create_NoResultCode(-1, nameof (SetScnForLocalServer)));
      int scn1 = this.m_RfCommIf.GetScn();
      MiscUtils.Trace_WriteLine("Server GetScn returned port: {0}", (object) scn1);
      return scn1;
    }

    internal void SetSecurityLevelServer(BTM_SEC securityLevel, byte[] serviceName)
    {
      if (!this.m_RfCommIf.SetSecurityLevel(serviceName, securityLevel, true))
        throw new IOException("IOError on socket.", (Exception) WidcommSocketExceptions.Create_NoResultCode(-1, "SetSecurityLevel"));
    }

    public void Dispose() => this.Dispose(true);

    public void Dispose(bool disposing) => this.m_RfCommIf.Destroy(disposing);
  }
}
