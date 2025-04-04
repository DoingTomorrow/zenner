// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.IRfcommPort
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal interface IRfcommPort
  {
    void SetParentStream(WidcommRfcommStreamBase parent);

    void Create();

    void Destroy();

    PORT_RETURN_CODE OpenClient(int scn, byte[] address);

    PORT_RETURN_CODE OpenServer(int scn);

    PORT_RETURN_CODE Write(byte[] p_data, ushort len_to_write, out ushort p_len_written);

    PORT_RETURN_CODE Close();

    bool IsConnected(out BluetoothAddress p_remote_bdaddr);

    string DebugId { get; }
  }
}
