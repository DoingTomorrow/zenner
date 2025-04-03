// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.DeviceDB_Data
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System.Collections.Generic;

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class DeviceDB_Data
  {
    public readonly string Name;
    public readonly SortedList<string, List<TelegramParameter>> Parameter;
    public readonly int HardwareTypeID;
    public readonly int MapID;

    public DeviceDB_Data(
      int HardwareTypeID,
      int MapID,
      string Name,
      SortedList<string, List<TelegramParameter>> Parameter)
    {
      this.HardwareTypeID = HardwareTypeID;
      this.Name = Name;
      this.Parameter = Parameter;
      this.MapID = MapID;
    }
  }
}
