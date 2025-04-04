// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.SybaseSQLAnywhereDotNet4Driver
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Driver
{
  public class SybaseSQLAnywhereDotNet4Driver : ReflectionBasedDriver
  {
    public SybaseSQLAnywhereDotNet4Driver()
      : base("iAnywhere.Data.SQLAnywhere", "iAnywhere.Data.SQLAnywhere.v4.0", "iAnywhere.Data.SQLAnywhere.SAConnection", "iAnywhere.Data.SQLAnywhere.SACommand")
    {
    }

    public override bool UseNamedPrefixInSql => true;

    public override bool UseNamedPrefixInParameter => true;

    public override string NamedPrefix => ":";
  }
}
