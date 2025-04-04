// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Sockets.IrDASocketOptionName
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Sockets
{
  public static class IrDASocketOptionName
  {
    public const SocketOptionName EnumDevice = SocketOptionName.DontRoute;
    public const SocketOptionName IasSet = SocketOptionName.BlockSource;
    public const SocketOptionName IasQuery = SocketOptionName.UnblockSource;
    public const SocketOptionName SendPduLength = SocketOptionName.PacketInformation;
    public const SocketOptionName ExclusiveMode = SocketOptionName.ChecksumCoverage;
    public const SocketOptionName IrLptMode = SocketOptionName.HopLimit;
    public const SocketOptionName NineWireMode = SocketOptionName.UnblockSource | SocketOptionName.ReuseAddress;
    public const SocketOptionName SharpMode = SocketOptionName.Broadcast;
  }
}
