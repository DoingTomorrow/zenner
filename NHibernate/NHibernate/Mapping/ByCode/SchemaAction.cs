// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.SchemaAction
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  [Flags]
  public enum SchemaAction
  {
    None = 0,
    Drop = 1,
    Update = 2,
    Export = 4,
    Validate = 8,
    All = Validate | Export | Update | Drop, // 0x0000000F
  }
}
