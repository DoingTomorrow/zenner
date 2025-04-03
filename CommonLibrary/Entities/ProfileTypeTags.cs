// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Entities.ProfileTypeTags
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;

#nullable disable
namespace ZENNER.CommonLibrary.Entities
{
  [Flags]
  public enum ProfileTypeTags : ulong
  {
    None = 0,
    Undefined = 1,
    Scanning = 2,
    JobManager = 4,
    All = JobManager | Scanning | Undefined, // 0x0000000000000007
  }
}
