// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.IResolvableNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public interface IResolvableNode
  {
    void Resolve(bool generateJoin, bool implicitJoin, string classAlias, IASTNode parent);

    void Resolve(bool generateJoin, bool implicitJoin, string classAlias);

    void Resolve(bool generateJoin, bool implicitJoin);

    void ResolveInFunctionCall(bool generateJoin, bool implicitJoin);

    void ResolveIndex(IASTNode parent);
  }
}
