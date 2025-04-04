// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.ServiceAttribute
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Diagnostics;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public sealed class ServiceAttribute
  {
    private readonly ServiceAttributeId m_id;
    private readonly ServiceElement m_element;

    public ServiceAttribute(ServiceAttributeId id, ServiceElement value)
    {
      this.m_id = id;
      this.m_element = value;
    }

    [CLSCompliant(false)]
    public ServiceAttribute(ushort id, ServiceElement value)
      : this((ServiceAttributeId) id, value)
    {
    }

    public ServiceAttributeId Id
    {
      [DebuggerStepThrough] get => this.m_id;
    }

    internal long IdAsOrdinalNumber => (long) (uint) this.m_id;

    public ServiceElement Value
    {
      [DebuggerStepThrough] get => this.m_element;
    }
  }
}
