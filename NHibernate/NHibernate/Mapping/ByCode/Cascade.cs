// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Cascade
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  [Flags]
  public enum Cascade
  {
    None = 0,
    Persist = 2,
    Refresh = 4,
    Merge = 8,
    Remove = 16, // 0x00000010
    Detach = 32, // 0x00000020
    ReAttach = 64, // 0x00000040
    DeleteOrphans = 128, // 0x00000080
    All = 256, // 0x00000100
  }
}
