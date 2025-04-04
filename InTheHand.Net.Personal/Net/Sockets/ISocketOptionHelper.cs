// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Sockets.ISocketOptionHelper
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

#nullable disable
namespace InTheHand.Net.Sockets
{
  internal interface ISocketOptionHelper
  {
    bool Authenticate { get; set; }

    bool Encrypt { get; set; }

    void SetPin(BluetoothAddress device, string pin);
  }
}
