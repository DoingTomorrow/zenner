// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Meters.ConnectedDeviceTypeMap
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
  public sealed class ConnectedDeviceTypeMap : ClassMap<ConnectedDeviceType>
  {
    public ConnectedDeviceTypeMap()
    {
      this.Table("t_ConnectedDeviceType");
      this.Id((Expression<Func<ConnectedDeviceType, object>>) (n => (object) n.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<ConnectedDeviceType, object>>) (c => c.Code)).Length(200);
    }
  }
}
