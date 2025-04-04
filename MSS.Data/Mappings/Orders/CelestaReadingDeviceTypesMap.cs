// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Orders.CelestaReadingDeviceTypesMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Orders;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Orders
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
