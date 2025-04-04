// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ISqlCustomizable
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;

#nullable disable
namespace NHibernate.Mapping
{
  public interface ISqlCustomizable
  {
    void SetCustomSQLInsert(string sql, bool callable, ExecuteUpdateResultCheckStyle checkStyle);

    void SetCustomSQLUpdate(string sql, bool callable, ExecuteUpdateResultCheckStyle checkStyle);

    void SetCustomSQLDelete(string sql, bool callable, ExecuteUpdateResultCheckStyle checkStyle);
  }
}
