// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.ExpressionQueryImpl
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Type;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Impl
{
  internal class ExpressionQueryImpl : AbstractQueryImpl
  {
    private readonly Dictionary<string, LockMode> _lockModes = new Dictionary<string, LockMode>(2);

    public ExpressionQueryImpl(
      IQueryExpression queryExpression,
      ISessionImplementor session,
      ParameterMetadata parameterMetadata)
      : base(queryExpression.Key, FlushMode.Unspecified, session, parameterMetadata)
    {
      this.QueryExpression = queryExpression;
    }

    public IQueryExpression QueryExpression { get; private set; }

    protected internal override IDictionary<string, LockMode> LockModes
    {
      get => (IDictionary<string, LockMode>) this._lockModes;
    }

    public override IQuery SetLockMode(string alias, LockMode lockMode)
    {
      this._lockModes[alias] = lockMode;
      return (IQuery) this;
    }

    public override int ExecuteUpdate() => throw new NotImplementedException();

    public override IEnumerable Enumerable() => throw new NotImplementedException();

    public override IEnumerable<T> Enumerable<T>() => throw new NotImplementedException();

    public override IList List()
    {
      this.VerifyParameters();
      IDictionary<string, TypedValue> namedParams = this.NamedParams;
      this.Before();
      try
      {
        return this.Session.List(this.ExpandParameters(namedParams), this.GetQueryParameters(namedParams));
      }
      finally
      {
        this.After();
      }
    }

    protected internal IQueryExpression ExpandParameters(
      IDictionary<string, TypedValue> namedParamsCopy)
    {
      if (this.namedParameterLists.Count == 0)
        return this.QueryExpression;
      Dictionary<string, System.Collections.Generic.List<string>> dictionary = new Dictionary<string, System.Collections.Generic.List<string>>();
      foreach (KeyValuePair<string, TypedValue> namedParameterList in this.namedParameterLists)
      {
        string key1 = namedParameterList.Key;
        ICollection collection = (ICollection) namedParameterList.Value.Value;
        IType type = namedParameterList.Value.Type;
        if (collection.Count == 1)
        {
          IEnumerator enumerator = collection.GetEnumerator();
          enumerator.MoveNext();
          namedParamsCopy[key1] = new TypedValue(type, enumerator.Current, this.Session.EntityMode);
        }
        else
        {
          System.Collections.Generic.List<string> stringList = new System.Collections.Generic.List<string>();
          int num = 0;
          bool jpaStyle = this.parameterMetadata.GetNamedParameterDescriptor(key1).JpaStyle;
          foreach (object obj in (IEnumerable) collection)
          {
            string key2 = (jpaStyle ? (object) ('x'.ToString() + key1) : (object) (key1 + (object) '_')).ToString() + (object) num++ + (object) '_';
            namedParamsCopy[key2] = new TypedValue(type, obj, this.Session.EntityMode);
            stringList.Add(key2);
          }
          dictionary.Add(key1, stringList);
        }
      }
      IASTNode tree = ParameterExpander.Expand(this.QueryExpression.Translate(this.Session.Factory), dictionary);
      StringBuilder seed = new StringBuilder(this.QueryExpression.Key);
      dictionary.Aggregate<KeyValuePair<string, System.Collections.Generic.List<string>>, StringBuilder>(seed, (Func<StringBuilder, KeyValuePair<string, System.Collections.Generic.List<string>>, StringBuilder>) ((sb, kvp) =>
      {
        sb.Append(' ');
        sb.Append(kvp.Key);
        sb.Append(':');
        kvp.Value.Aggregate<string, StringBuilder>(sb, (Func<StringBuilder, string, StringBuilder>) ((sb2, str) => sb2.Append(str)));
        return sb;
      }));
      return (IQueryExpression) new ExpandedQueryExpression(this.QueryExpression, tree, seed.ToString());
    }

    public override void List(IList results) => throw new NotImplementedException();

    public override IList<T> List<T>()
    {
      this.VerifyParameters();
      IDictionary<string, TypedValue> namedParams = this.NamedParams;
      this.Before();
      try
      {
        return (IList<T>) this.Session.List(this.ExpandParameters(namedParams), this.GetQueryParameters(namedParams));
      }
      finally
      {
        this.After();
      }
    }
  }
}
