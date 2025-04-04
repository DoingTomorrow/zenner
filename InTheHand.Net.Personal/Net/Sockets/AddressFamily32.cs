// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Sockets.AddressFamily32
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System.Net.Sockets;

#nullable disable
namespace InTheHand.Net.Sockets
{
  public static class AddressFamily32
  {
    public const AddressFamily Bluetooth = (AddressFamily) 32;
    public const AddressFamily Irda = AddressFamily.Atm;
    internal const AddressFamily BluetoothOnLinuxBlueZ = AddressFamily.HyperChannel | AddressFamily.AppleTalk;
  }
}
