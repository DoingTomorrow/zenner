// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Sockets.BluetoothProtocolType
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Sockets
{
  public static class BluetoothProtocolType
  {
    public const ProtocolType Sdp = ProtocolType.Icmp;
    public const ProtocolType RFComm = ProtocolType.Ggp;
    public const ProtocolType L2Cap = (ProtocolType) 256;
  }
}
