// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ISelectable
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Function;

#nullable disable
namespace NHibernate.Mapping
{
  public interface ISelectable
  {
    string GetAlias(NHibernate.Dialect.Dialect dialect);

    string GetAlias(NHibernate.Dialect.Dialect dialect, Table table);

    bool IsFormula { get; }

    string GetTemplate(NHibernate.Dialect.Dialect dialect, SQLFunctionRegistry functionRegistry);

    string GetText(NHibernate.Dialect.Dialect dialect);

    string Text { get; }
  }
}
