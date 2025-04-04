// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Meters.MeterRadioDetailsSerializableSync
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.DTO.Sync;
using System;

#nullable disable
namespace MSS.DTO.Meters
{
  public class MeterRadioDetailsSerializableSync : ISerializableObject
  {
    public virtual Guid Id { get; set; }

    public virtual Guid? MeterId { get; set; }

    public virtual string dgReg1Flag { get; set; }

    public virtual string dgReg1Mode { get; set; }

    public virtual string dgReg1DakonSerNr { get; set; }

    public virtual string dgReg1Signal { get; set; }

    public virtual string dgReg2Flag { get; set; }

    public virtual string dgReg2Mode { get; set; }

    public virtual string dgReg2DakonSerNr { get; set; }

    public virtual string dgReg2Signal { get; set; }

    public virtual string dgReg3Flag { get; set; }

    public virtual string dgReg3Mode { get; set; }

    public virtual string dgReg3DakonSernr { get; set; }

    public virtual string dgReg3Signal { get; set; }

    public virtual string dgMessbereich { get; set; }

    public virtual string dgZaehlerNr { get; set; }

    public virtual string dgRealErfasser { get; set; }

    public virtual string Street { get; set; }

    public virtual string GemSerialNumber { get; set; }

    public virtual string Scenario { get; set; }

    public virtual DateTime? LastChangedOn { get; set; }
  }
}
