// Decompiled with JetBrains decompiler
// Type: HandlerLib.MapManagement.MAP_EXCEPTION_HANDLE
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib.MapManagement
{
  public enum MAP_EXCEPTION_HANDLE
  {
    MAPFILE_NOT_FOUND = 0,
    MAPFILE_NOT_READABLE = 1,
    MAPFILE_WRONG_VERSION = 3,
    MAPFILE_FIRMWARE_NOT_SUPPORTED = 4,
    MAP_ILLEGAL_PARAMETER = 11, // 0x0000000B
    MAP_PARAMETER_NOT_FOUND = 12, // 0x0000000C
    MAP_PARAMETER_IS_NULL = 13, // 0x0000000D
    MAP_PARAMETER_LIST_IS_NULL = 14, // 0x0000000E
    MAP_PARAMETER_ADDRESS_ERROR = 15, // 0x0000000F
    MAP_CLASS_ILLEGAL_FORMAT = 21, // 0x00000015
    MAP_CLASS_UNKNOWN_SECTIONNAME = 31, // 0x0000001F
    MAP_PATH_EMPTY = 91, // 0x0000005B
  }
}
