// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.ISdpService
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal interface ISdpService : IDisposable
  {
    void AddServiceClassIdList(IList<Guid> serviceClasses);

    void AddServiceClassIdList(Guid serviceClass);

    void AddRFCommProtocolDescriptor(byte scn);

    void AddServiceName(string serviceName);

    void AddAttribute(ushort id, SdpService.DESC_TYPE dt, int valLen, byte[] val);

    void CommitRecord();
  }
}
