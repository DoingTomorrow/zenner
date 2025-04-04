// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Orders.CelestaReadingDeviceTypesMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Orders;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.Orders
{
  public class CelestaReadingDeviceTypesMap : ClassMap<CelestaReadingDeviceTypes>
  {
    public CelestaReadingDeviceTypesMap()
    {
      this.Table("t_CelestaReadingDeviceTypes");
      this.Id((Expression<Func<CelestaReadingDeviceTypes, object>>) (o => (object) o.Id)).GeneratedBy.Increment();
      this.Map((Expression<Func<CelestaReadingDeviceTypes, object>>) (o => (object) o.Type)).Not.Nullable();
      this.Map((Expression<Func<CelestaReadingDeviceTypes, object>>) (o => o.Messbereich)).Nullable();
      this.Map((Expression<Func<CelestaReadingDeviceTypes, object>>) (o => o.CelestaId)).Not.Nullable();
    }
  }
}
