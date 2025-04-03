// Decompiled with JetBrains decompiler
// Type: Common.Library.NHibernate.Data.Extensions.HQLOperator
// Assembly: Common.Library.NHibernate.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8E2D87B3-234F-4936-9817-E8F0EDC71AA1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Common.Library.NHibernate.Data.dll

#nullable disable
namespace Common.Library.NHibernate.Data.Extensions
{
  public enum HQLOperator
  {
    IsLessThan = 0,
    IsLessThanOrEqualTo = 1,
    IsEqualTo = 2,
    IsNotEqualTo = 3,
    IsGreaterThanOrEqualTo = 4,
    IsGreaterThan = 5,
    StartsWith = 6,
    EndsWith = 7,
    Contains = 8,
    IsContainedIn = 9,
    IsNull = 10, // 0x0000000A
    IsNotNull = 11, // 0x0000000B
    Wildcards = 13, // 0x0000000D
  }
}
