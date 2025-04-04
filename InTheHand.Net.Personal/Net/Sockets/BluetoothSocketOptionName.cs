// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Sockets.BluetoothSocketOptionName
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Sockets
{
  public static class BluetoothSocketOptionName
  {
    public const SocketOptionName Authenticate = SocketOptionName.Debug;
    public const SocketOptionName XPAuthenticate = (SocketOptionName) -2147483647;
    public const SocketOptionName Encrypt = SocketOptionName.AcceptConnection;
    public const SocketOptionName SetPin = SocketOptionName.TypeOfService;
    public const SocketOptionName SetLink = SocketOptionName.ReuseAddress;
    public const SocketOptionName GetLink = SocketOptionName.Debug | SocketOptionName.ReuseAddress;
    public const SocketOptionName XPMtu = (SocketOptionName) -2147483641;
    public const SocketOptionName SetMtu = SocketOptionName.AcceptConnection | SocketOptionName.ReuseAddress;
    public const SocketOptionName GetMtu = SocketOptionName.TypeOfService | SocketOptionName.ReuseAddress;
    public const SocketOptionName XPMtuMaximum = (SocketOptionName) -2147483640;
    public const SocketOptionName SetMtuMaximum = SocketOptionName.KeepAlive;
    public const SocketOptionName GetMtuMaximum = SocketOptionName.MulticastInterface;
    public const SocketOptionName XPMtuMinimum = (SocketOptionName) -2147483638;
    public const SocketOptionName SetMtuMinimum = SocketOptionName.MulticastTimeToLive;
    public const SocketOptionName GetMtuMinimum = SocketOptionName.MulticastLoopback;
    public const SocketOptionName SetXOnLimit = SocketOptionName.AddMembership;
    public const SocketOptionName GetXOnLimit = SocketOptionName.DropMembership;
    public const SocketOptionName SetXOffLimit = SocketOptionName.DontFragment;
    public const SocketOptionName GetXOffLimit = SocketOptionName.AddSourceMembership;
    public const SocketOptionName SetSendBuffer = SocketOptionName.DontRoute;
    public const SocketOptionName GetSendBuffer = SocketOptionName.BlockSource;
    public const SocketOptionName SetReceiveBuffer = SocketOptionName.UnblockSource;
    public const SocketOptionName GetReceiveBuffer = SocketOptionName.PacketInformation;
    public const SocketOptionName GetV24Break = SocketOptionName.ChecksumCoverage;
    public const SocketOptionName GetRls = SocketOptionName.HopLimit;
    public const SocketOptionName SendMsc = SocketOptionName.UnblockSource | SocketOptionName.ReuseAddress;
    public const SocketOptionName SendRls = SocketOptionName.PacketInformation | SocketOptionName.ReuseAddress;
    public const SocketOptionName GetFlowType = SocketOptionName.KeepAlive | SocketOptionName.DontRoute;
    public const SocketOptionName SetPageTimeout = SocketOptionName.MulticastInterface | SocketOptionName.DontRoute;
    public const SocketOptionName GetPageTimeout = SocketOptionName.MulticastTimeToLive | SocketOptionName.DontRoute;
    public const SocketOptionName SetScan = SocketOptionName.MulticastLoopback | SocketOptionName.DontRoute;
    public const SocketOptionName GetScan = SocketOptionName.AddMembership | SocketOptionName.DontRoute;
    public const SocketOptionName SetCod = SocketOptionName.DropMembership | SocketOptionName.DontRoute;
    public const SocketOptionName GetCod = SocketOptionName.DontFragment | SocketOptionName.DontRoute;
    public const SocketOptionName GetLocalVersion = SocketOptionName.AddSourceMembership | SocketOptionName.DontRoute;
    public const SocketOptionName GetRemoteVersion = SocketOptionName.Broadcast;
    public const SocketOptionName GetAuthenticationEnabled = SocketOptionName.Debug | SocketOptionName.Broadcast;
    public const SocketOptionName SetAuthenticationEnabled = SocketOptionName.AcceptConnection | SocketOptionName.Broadcast;
    public const SocketOptionName ReadRemoteName = SocketOptionName.TypeOfService | SocketOptionName.Broadcast;
    public const SocketOptionName GetLinkPolicy = SocketOptionName.ReuseAddress | SocketOptionName.Broadcast;
    public const SocketOptionName SetLinkPolicy = SocketOptionName.Debug | SocketOptionName.ReuseAddress | SocketOptionName.Broadcast;
    public const SocketOptionName EnterHoldMode = SocketOptionName.AcceptConnection | SocketOptionName.ReuseAddress | SocketOptionName.Broadcast;
    public const SocketOptionName EnterSniffMode = SocketOptionName.TypeOfService | SocketOptionName.ReuseAddress | SocketOptionName.Broadcast;
    public const SocketOptionName ExitSniffMode = SocketOptionName.KeepAlive | SocketOptionName.Broadcast;
    public const SocketOptionName EnterParkMode = SocketOptionName.MulticastInterface | SocketOptionName.Broadcast;
    public const SocketOptionName ExitParkMode = SocketOptionName.MulticastTimeToLive | SocketOptionName.Broadcast;
    public const SocketOptionName GetMode = SocketOptionName.MulticastLoopback | SocketOptionName.Broadcast;
  }
}
