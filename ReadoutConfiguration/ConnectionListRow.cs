// Decompiled with JetBrains decompiler
// Type: ReadoutConfiguration.ConnectionListRow
// Assembly: ReadoutConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 1BD19DC4-A290-473A-8451-94ED3EF61361
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ReadoutConfiguration.dll

#nullable disable
namespace ReadoutConfiguration
{
  public class ConnectionListRow
  {
    public int DeviceGroupID;
    public int DeviceModelID;
    public int EquipmentGroupID;
    public int EquipmentModelID;
    public int ProfileTypeGroupID;
    public int ProfileTypeID;

    public int ID { get; set; }

    public string Mark { get; set; }

    public string DeviceGroup { get; set; }

    public int D_ID => this.DeviceModelID;

    public string DeviceModel { get; set; }

    public string EquipmentGroup { get; set; }

    public int E_ID => this.EquipmentModelID;

    public string EquipmentModel { get; set; }

    public string ProfileTypeGroup { get; set; }

    public int T_ID => this.ProfileTypeID;

    public string ProfileType { get; set; }

    public int SettingsId { get; set; }

    public string SettingsName { get; set; }

    public string Parameters { get; set; }
  }
}
