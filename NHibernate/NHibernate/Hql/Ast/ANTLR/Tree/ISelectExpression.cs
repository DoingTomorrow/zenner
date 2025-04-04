// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.ISelectExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public interface ISelectExpression
  {
    IType DataType { get; }

    void SetScalarColumnText(int i);

    FromElement FromElement { get; }

    bool IsConstructor { get; }

    bool IsReturnableEntity { get; }

    string Text { set; }

    bool IsScalar { get; }

    string Alias { get; set; }
  }
}
