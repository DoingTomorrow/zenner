// Decompiled with JetBrains decompiler
// Type: TH_Handler.RadioFlags
// Assembly: TH_Handler, Version=1.3.4.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 02D62764-6653-46F8-9117-1BC5233AD061
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\TH_Handler.dll

using System;

#nullable disable
namespace TH_Handler
{
  [Flags]
  public enum RadioFlags : byte
  {
    LONGHEADER = 1,
    ENCRYPT = 2,
    SYNCHRONOUS = 4,
    INSTALL = 8,
    PACKET_T = 16, // 0x10
    PACKET_RH = 32, // 0x20
  }
}
