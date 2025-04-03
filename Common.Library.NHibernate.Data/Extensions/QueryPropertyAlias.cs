// Decompiled with JetBrains decompiler
// Type: Common.Library.NHibernate.Data.Extensions.QueryPropertyAlias
// Assembly: Common.Library.NHibernate.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8E2D87B3-234F-4936-9817-E8F0EDC71AA1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Common.Library.NHibernate.Data.dll

#nullable disable
namespace Common.Library.NHibernate.Data.Extensions
{
  public class QueryPropertyAlias
  {
    private string alias;
    private string propertyName;

    protected QueryPropertyAlias(string alias, string propertyName)
    {
      this.alias = alias;
      this.propertyName = propertyName;
    }

    public static QueryPropertyAlias MapAliasToProperty(string propertyName, string alias)
    {
      return new QueryPropertyAlias(alias, propertyName);
    }

    public string Alias => this.alias;

    public string PropertyName => this.propertyName;
  }
}
