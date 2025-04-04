// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.Formula
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Function;
using NHibernate.SqlCommand;
using System;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class Formula : ISelectable
  {
    private static int formulaUniqueInteger;
    private string formula;
    private readonly int uniqueInteger;

    public Formula() => this.uniqueInteger = Formula.formulaUniqueInteger++;

    public string GetTemplate(NHibernate.Dialect.Dialect dialect, SQLFunctionRegistry functionRegistry)
    {
      return Template.RenderWhereStringTemplate(this.formula, dialect, functionRegistry);
    }

    public string FormulaString
    {
      get => this.formula;
      set => this.formula = value;
    }

    public string GetText(NHibernate.Dialect.Dialect dialect) => this.FormulaString;

    public string Text => this.FormulaString;

    public string GetAlias(NHibernate.Dialect.Dialect dialect)
    {
      return string.Format("formula{0}{1}", (object) this.uniqueInteger, (object) '_');
    }

    public string GetAlias(NHibernate.Dialect.Dialect dialect, Table table)
    {
      return this.GetAlias(dialect);
    }

    public bool IsFormula => true;

    public override string ToString() => this.GetType().FullName + "( " + this.formula + " )";
  }
}
