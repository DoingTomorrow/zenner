// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Meters.UserDeviceTypeSettingsMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Meters;
using MSS.Core.Model.UsersManagement;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.Meters
{
  public class UserDeviceTypeSettingsMap : ClassMap<UserDeviceTypeSettings>
  {
    public UserDeviceTypeSettingsMap()
    {
      this.Table("t_UserDeviceTypeSettings");
      this.Id((Expression<Func<UserDeviceTypeSettings, object>>) (n => (object) n.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<UserDeviceTypeSettings, object>>) (m => (object) m.DeviceType));
      this.References<MeasureUnit>((Expression<Func<UserDeviceTypeSettings, MeasureUnit>>) (m => m.DisplayUnit), "DisplayUnitId");
      this.Map((Expression<Func<UserDeviceTypeSettings, object>>) (m => (object) m.DecimalPlaces));
      this.References<User>((Expression<Func<UserDeviceTypeSettings, User>>) (m => m.User), "UserId");
    }
  }
}
