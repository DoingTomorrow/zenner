// Decompiled with JetBrains decompiler
// Type: MSSArchive.Data.Mappings.Meters.ArchiveMeterRadioDetailsMap
// Assembly: MSSArchive.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C71A41A-539A-4545-909E-692571DC7265
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSArchive.Data.dll

using FluentNHibernate.Mapping;
using MSSArchive.Core.Model.Archiving;
using MSSArchive.Core.Model.Meters;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSSArchive.Data.Mappings.Meters
{
  public sealed class ArchiveMeterRadioDetailsMap : ClassMap<ArchiveMeterRadioDetails>
  {
    public ArchiveMeterRadioDetailsMap()
    {
      this.Table("t_MeterRadioDetails");
      this.Id((Expression<Func<ArchiveMeterRadioDetails, object>>) (t => (object) t.ArchiveEntityId));
      this.Map((Expression<Func<ArchiveMeterRadioDetails, object>>) (t => (object) t.Id));
      this.Map((Expression<Func<ArchiveMeterRadioDetails, object>>) (m => m.dgMessbereich)).Length(20);
      this.Map((Expression<Func<ArchiveMeterRadioDetails, object>>) (m => m.dgRealErfasser)).Length(10);
      this.Map((Expression<Func<ArchiveMeterRadioDetails, object>>) (m => m.dgReg1DakonSerNr)).Length(12);
      this.Map((Expression<Func<ArchiveMeterRadioDetails, object>>) (m => m.dgReg1Flag)).Length(5);
      this.Map((Expression<Func<ArchiveMeterRadioDetails, object>>) (m => m.dgReg1Mode)).Length(20);
      this.Map((Expression<Func<ArchiveMeterRadioDetails, object>>) (m => m.dgReg1Signal)).Length(3);
      this.Map((Expression<Func<ArchiveMeterRadioDetails, object>>) (m => m.dgReg2DakonSerNr)).Length(12);
      this.Map((Expression<Func<ArchiveMeterRadioDetails, object>>) (m => m.dgReg2Flag)).Length(5);
      this.Map((Expression<Func<ArchiveMeterRadioDetails, object>>) (m => m.dgReg2Mode)).Length(20);
      this.Map((Expression<Func<ArchiveMeterRadioDetails, object>>) (m => m.dgReg2Signal)).Length(3);
      this.Map((Expression<Func<ArchiveMeterRadioDetails, object>>) (m => m.dgReg3DakonSernr)).Length(12);
      this.Map((Expression<Func<ArchiveMeterRadioDetails, object>>) (m => m.dgReg3Flag)).Length(5);
      this.Map((Expression<Func<ArchiveMeterRadioDetails, object>>) (m => m.dgReg3Mode)).Length(20);
      this.Map((Expression<Func<ArchiveMeterRadioDetails, object>>) (m => m.dgReg3Signal)).Length(3);
      this.Map((Expression<Func<ArchiveMeterRadioDetails, object>>) (m => m.dgZaehlerNr)).Length(16);
      this.Map((Expression<Func<ArchiveMeterRadioDetails, object>>) (m => m.Street)).Length(320);
      this.Map((Expression<Func<ArchiveMeterRadioDetails, object>>) (m => m.GemSerialNumber));
      this.Map((Expression<Func<ArchiveMeterRadioDetails, object>>) (m => m.Scenario));
      this.Map((Expression<Func<ArchiveMeterRadioDetails, object>>) (m => (object) m.MeterId));
      this.References<ArchiveInformation>((Expression<Func<ArchiveMeterRadioDetails, ArchiveInformation>>) (m => m.ArchiveInformation)).Column("t_MeterRadioDetails_ArchiveInformationId").Nullable().Not.LazyLoad();
    }
  }
}
