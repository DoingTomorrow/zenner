// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.IRfCommIf
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal interface IRfCommIf
  {
    IntPtr PObject { get; }

    void Create();

    void Destroy(bool disposing);

    bool ClientAssignScnValue(Guid serviceGuid, int scn);

    bool SetSecurityLevel(byte[] p_service_name, BTM_SEC securityLevel, bool isServer);

    int GetScn();
  }
}
