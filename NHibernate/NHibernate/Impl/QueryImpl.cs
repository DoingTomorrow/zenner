// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.QueryImpl
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Engine.Query;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Impl
{
  public class QueryImpl(
    string queryString,
    FlushMode flushMode,
    ISessionImplementor session,
    ParameterMetadata parameterMetadata) : AbstractQueryImpl(queryString, flushMode, session, parameterMetadata)
  {
    private readonly Dictionary<string, LockMode> lockModes = new Dictionary<string, LockMode>(2);

    public QueryImpl(
      string queryString,
      ISessionImplementor session,
      ParameterMetadata parameterMetadata)
      : this(queryString, FlushMode.Unspecified, session, parameterMetadata)
    {
    }

    public override IEnumerable Enumerable()
    {
      this.VerifyParameters();
      IDictionary<string, TypedValue> namedParams = this.NamedParams;
      this.Before();
      try
      {
        return this.Session.Enumerable(this.ExpandParameterLists(namedParams), this.GetQueryParameters(namedParams));
      }
      finally
      {
        this.After();
      }
    }

    public override IEnumerable<T> Enumerable<T>()
    {
      this.VerifyParameters();
      IDictionary<string, TypedValue> namedParams = this.NamedParams;
      this.Before();
      try
      {
        return this.Session.Enumerable<T>(this.ExpandParameterLists(namedParams), this.GetQueryParameters(namedParams));
      }
      finally
      {
        this.After();
      }
    }

    public override IList List()
    {
      this.VerifyParameters();
      IDictionary<string, TypedValue> namedParams = this.NamedParams;
      this.Before();
      try
      {
        return this.Session.List(this.ExpandParameterLists(namedParams), this.GetQueryParameters(namedParams));
      }
      finally
      {
        this.After();
      }
    }

    public override void List(IList results)
    {
      this.VerifyParameters();
      IDictionary<string, TypedValue> namedParams = this.NamedParams;
      this.Before();
      try
      {
        this.Session.List(this.ExpandParameterLists(namedParams), this.GetQueryParameters(namedParams), results);
      }
      finally
      {
        this.After();
      }
    }

    public override IList<T> List<T>()
    {
      this.VerifyParameters();
      IDictionary<string, TypedValue> namedParams = this.NamedParams;
      this.Before();
      try
      {
        return this.Session.List<T>(this.ExpandParameterLists(namedParams), this.GetQueryParameters(namedParams));
      }
      finally
      {
        this.After();
      }
    }

    public override IQuery SetLockMode(string alias, LockMode lockMode)
    {
      this.lockModes[alias] = lockMode;
      return (IQuery) this;
    }

    protected internal override IDictionary<string, LockMode> LockModes
    {
      get => (IDictionary<string, LockMode>) this.lockModes;
    }

    public override int ExecuteUpdate()
    {
      this.VerifyParameters();
      IDictionary<string, TypedValue> namedParams = this.NamedParams;
      this.Before();
      try
      {
        return this.Session.ExecuteUpdate(this.ExpandParameterLists(namedParams), this.GetQueryParameters(namedParams));
      }
      finally
      {
        this.After();
      }
    }
  }
}
