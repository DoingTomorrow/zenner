// Decompiled with JetBrains decompiler
// Type: GMMToMSSMigrator.GMMNodeTypeEnum
// Assembly: GMMToMSSMigrator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3ACF3C29-B99D-4830-8DFE-AD2278C0306B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMMToMSSMigrator.dll

#nullable disable
namespace GMMToMSSMigrator
{
  public enum GMMNodeTypeEnum
  {
    Meter = 1,
    UserDevice = 2,
    Country = 3,
    City = 4,
    CityArea = 5,
    Street = 6,
    House = 7,
    Floor = 8,
    DeviceGroup = 9,
    Flat = 10, // 0x0000000A
    UserGroup = 14, // 0x0000000E
    Repeater = 21, // 0x00000015
    Manifold = 22, // 0x00000016
    COMServer = 23, // 0x00000017
    Converter = 24, // 0x00000018
  }
}
