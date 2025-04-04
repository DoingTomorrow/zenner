// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.IParameterContainer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Param;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  public interface IParameterContainer
  {
    string Text { set; }

    void AddEmbeddedParameter(IParameterSpecification specification);

    bool HasEmbeddedParameters { get; }

    IParameterSpecification[] GetEmbeddedParameters();
  }
}
