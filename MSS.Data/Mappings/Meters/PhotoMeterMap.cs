// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Meters.PhotoMeterMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Structures;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Meters
{
  public sealed class PhotoMeterMap : ClassMap<PhotoMeter>
  {
    public PhotoMeterMap()
    {
      this.Table("t_PhotoMeter");
      this.Id((Expression<Func<PhotoMeter, object>>) (_ => (object) _.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<PhotoMeter, object>>) (_ => _.Name)).Nullable();
      this.Map((Expression<Func<PhotoMeter, object>>) (_ => _.Description)).Nullable();
      this.Map((Expression<Func<PhotoMeter, object>>) (_ => _.Payload));
      this.References<StructureNode>((Expression<Func<PhotoMeter, StructureNode>>) (m => m.StructureNode), "PhotoId").Cascade.All();
    }
  }
}
