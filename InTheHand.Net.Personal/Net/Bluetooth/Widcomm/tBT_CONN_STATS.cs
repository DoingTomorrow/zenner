// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.tBT_CONN_STATS
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal struct tBT_CONN_STATS
  {
    public readonly uint bIsConnected;
    public readonly int rssi;
    public readonly uint bytesSent;
    public readonly uint bytesRcvd;
    public readonly uint duration;
  }
}
