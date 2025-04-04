// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.SybaseAsaClientDriver
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Driver
{
  public class SybaseAsaClientDriver : ReflectionBasedDriver
  {
    public SybaseAsaClientDriver()
      : base("iAnywhere.Data.AsaClient", "iAnywhere.Data.AsaClient.AsaConnection", "iAnywhere.Data.AsaClient.AsaCommand")
    {
    }

    public override bool UseNamedPrefixInSql => false;

    public override bool UseNamedPrefixInParameter => false;

    public override string NamedPrefix => string.Empty;
  }
}
