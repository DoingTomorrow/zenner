// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Meters.UserDeviceTypeSettingsMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Meters;
using MSS.Core.Model.UsersManagement;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Meters
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
