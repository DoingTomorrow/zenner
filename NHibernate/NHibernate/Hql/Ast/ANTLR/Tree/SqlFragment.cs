// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.SqlFragment
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using NHibernate.Engine;
using NHibernate.Param;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class SqlFragment(IToken token) : SqlNode(token), IParameterContainer
  {
    private JoinFragment _joinFragment;
    private FromElement _fromElement;
    private List<IParameterSpecification> _embeddedParameters;

    public void SetJoinFragment(JoinFragment joinFragment) => this._joinFragment = joinFragment;

    public bool HasFilterCondition => this._joinFragment.HasFilterCondition;

    public FromElement FromElement
    {
      get => this._fromElement;
      set => this._fromElement = value;
    }

    public override SqlString RenderText(ISessionFactoryImplementor sessionFactory)
    {
      SqlString sqlString = SqlString.Parse(this.Text);
      if (this.HasEmbeddedParameters)
      {
        Parameter[] array = sqlString.GetParameters().ToArray<Parameter>();
        int num = 0;
        foreach (string str in this._embeddedParameters.SelectMany<IParameterSpecification, string>((Func<IParameterSpecification, IEnumerable<string>>) (specification => specification.GetIdsForBackTrack((IMapping) sessionFactory))))
          array[num++].BackTrack = (object) str;
      }
      return sqlString;
    }

    public void AddEmbeddedParameter(IParameterSpecification specification)
    {
      if (this._embeddedParameters == null)
        this._embeddedParameters = new List<IParameterSpecification>();
      this._embeddedParameters.Add(specification);
    }

    public bool HasEmbeddedParameters
    {
      get => this._embeddedParameters != null && this._embeddedParameters.Count != 0;
    }

    public IParameterSpecification[] GetEmbeddedParameters() => this._embeddedParameters.ToArray();
  }
}
