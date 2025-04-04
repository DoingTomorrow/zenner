// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.WidcommSppSocketExceptions
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal static class WidcommSppSocketExceptions
  {
    private const int SocketError_Fault10014 = 10014;
    internal const int SocketError_Misc = 10014;

    internal static SocketException Create(WidcommSppClient.SPP_STATE_CODE result, string location)
    {
      return (SocketException) new SPP_STATE_CODE_WidcommSocketException(10014, result, location);
    }

    internal static SocketException Create(
      WidcommSppClient.SPP_CLIENT_RETURN_CODE result,
      string location)
    {
      return (SocketException) new SPP_CLIENT_RETURN_CODE_WidcommSocketException(10014, result, location);
    }
  }
}
