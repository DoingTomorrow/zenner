// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Users.UserDeviceTypeSettingsDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.Meters;
using MSS.Core.Model.UsersManagement;
using System;

#nullable disable
namespace MSS.DTO.Users
{
  public class UserDeviceTypeSettingsDTO
  {
    public virtual Guid Id { get; set; }

    public virtual DeviceTypeEnum DeviceType { get; set; }

    public virtual double? DecimalPlaces { get; set; }

    public virtual Guid DisplayUnitId { get; set; }

    public virtual User User { get; set; }
  }
}
