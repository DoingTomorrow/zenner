// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Entities.DeviceModelTags
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;

#nullable disable
namespace ZENNER.CommonLibrary.Entities
{
  [Flags]
  public enum DeviceModelTags : ulong
  {
    None = 0,
    Undefined = 1,
    Radio2 = 2,
    Radio3 = 4,
    SystemDevice = 8,
    MBus = 16, // 0x0000000000000010
    wMBus = 32, // 0x0000000000000020
    LoRa = 64, // 0x0000000000000040
    All = LoRa | wMBus | MBus | SystemDevice | Radio3 | Radio2 | Undefined, // 0x000000000000007F
  }
}
