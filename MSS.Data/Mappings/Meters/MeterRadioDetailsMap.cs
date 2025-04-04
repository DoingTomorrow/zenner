// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Meters.MeterRadioDetailsMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Meters;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Meters
{
  public sealed class MeterRadioDetailsMap : ClassMap<MeterRadioDetails>
  {
    public MeterRadioDetailsMap()
    {
      this.Table("t_MeterRadioDetails");
      this.Id((Expression<Func<MeterRadioDetails, object>>) (n => (object) n.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<MeterRadioDetails, object>>) (m => m.dgMessbereich)).Length(20);
      this.Map((Expression<Func<MeterRadioDetails, object>>) (m => m.dgRealErfasser)).Length(10);
      this.Map((Expression<Func<MeterRadioDetails, object>>) (m => m.dgReg1DakonSerNr)).Length(12);
      this.Map((Expression<Func<MeterRadioDetails, object>>) (m => m.dgReg1Flag)).Length(5);
      this.Map((Expression<Func<MeterRadioDetails, object>>) (m => m.dgReg1Mode)).Length(20);
      this.Map((Expression<Func<MeterRadioDetails, object>>) (m => m.dgReg1Signal)).Length(3);
      this.Map((Expression<Func<MeterRadioDetails, object>>) (m => m.dgReg2DakonSerNr)).Length(12);
      this.Map((Expression<Func<MeterRadioDetails, object>>) (m => m.dgReg2Flag)).Length(5);
      this.Map((Expression<Func<MeterRadioDetails, object>>) (m => m.dgReg2Mode)).Length(20);
      this.Map((Expression<Func<MeterRadioDetails, object>>) (m => m.dgReg2Signal)).Length(3);
      this.Map((Expression<Func<MeterRadioDetails, object>>) (m => m.dgReg3DakonSernr)).Length(12);
      this.Map((Expression<Func<MeterRadioDetails, object>>) (m => m.dgReg3Flag)).Length(5);
      this.Map((Expression<Func<MeterRadioDetails, object>>) (m => m.dgReg3Mode)).Length(20);
      this.Map((Expression<Func<MeterRadioDetails, object>>) (m => m.dgReg3Signal)).Length(3);
      this.Map((Expression<Func<MeterRadioDetails, object>>) (m => m.dgZaehlerNr)).Length(16);
      this.Map((Expression<Func<MeterRadioDetails, object>>) (m => m.Street)).Length(320);
      this.Map((Expression<Func<MeterRadioDetails, object>>) (m => m.GemSerialNumber));
      this.Map((Expression<Func<MeterRadioDetails, object>>) (m => m.Scenario));
      this.Map((Expression<Func<MeterRadioDetails, object>>) (m => (object) m.LastChangedOn)).Nullable();
      this.References<Meter>((Expression<Func<MeterRadioDetails, Meter>>) (m => m.Meter), "MeterId");
    }
  }
}
