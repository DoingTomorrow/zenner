// Decompiled with JetBrains decompiler
// Type: NHibernate.FlushMode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate
{
  [Serializable]
  public enum FlushMode
  {
    Unspecified = -1, // 0xFFFFFFFF
    Never = 0,
    Commit = 5,
    Auto = 10, // 0x0000000A
    Always = 20, // 0x00000014
  }
}
