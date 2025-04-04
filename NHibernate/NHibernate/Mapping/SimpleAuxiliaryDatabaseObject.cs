// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.SimpleAuxiliaryDatabaseObject
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Util;
using System;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class SimpleAuxiliaryDatabaseObject : AbstractAuxiliaryDatabaseObject
  {
    private readonly string sqlCreateString;
    private readonly string sqlDropString;

    public SimpleAuxiliaryDatabaseObject(string sqlCreateString, string sqlDropString)
    {
      this.sqlCreateString = sqlCreateString;
      this.sqlDropString = sqlDropString;
    }

    public SimpleAuxiliaryDatabaseObject(
      string sqlCreateString,
      string sqlDropString,
      HashedSet<string> dialectScopes)
      : base(dialectScopes)
    {
      this.sqlCreateString = sqlCreateString;
      this.sqlDropString = sqlDropString;
    }

    public override string SqlCreateString(
      NHibernate.Dialect.Dialect dialect,
      IMapping p,
      string defaultCatalog,
      string defaultSchema)
    {
      return SimpleAuxiliaryDatabaseObject.InjectCatalogAndSchema(this.sqlCreateString, defaultCatalog, defaultSchema);
    }

    public override string SqlDropString(
      NHibernate.Dialect.Dialect dialect,
      string defaultCatalog,
      string defaultSchema)
    {
      return SimpleAuxiliaryDatabaseObject.InjectCatalogAndSchema(this.sqlDropString, defaultCatalog, defaultSchema);
    }

    private static string InjectCatalogAndSchema(
      string ddlString,
      string defaultCatalog,
      string defaultSchema)
    {
      return StringHelper.Replace(StringHelper.Replace(ddlString, "${catalog}", defaultCatalog), "${schema}", defaultSchema);
    }
  }
}
