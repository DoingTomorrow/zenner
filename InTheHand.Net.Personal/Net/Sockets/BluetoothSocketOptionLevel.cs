// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Sockets.BluetoothSocketOptionLevel
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Sockets
{
  public static class BluetoothSocketOptionLevel
  {
    public const SocketOptionLevel RFComm = (SocketOptionLevel) 3;
    public const SocketOptionLevel L2Cap = (SocketOptionLevel) 256;
    public const SocketOptionLevel Sdp = (SocketOptionLevel) 257;
  }
}
