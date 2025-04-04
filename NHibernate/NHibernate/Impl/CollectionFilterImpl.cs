// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.CollectionFilterImpl
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Type;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Impl
{
  public class CollectionFilterImpl : QueryImpl
  {
    private readonly object collection;

    public CollectionFilterImpl(
      string queryString,
      object collection,
      ISessionImplementor session,
      ParameterMetadata parameterMetadata)
      : base(queryString, session, parameterMetadata)
    {
      this.collection = collection;
    }

    public override IEnumerable Enumerable()
    {
      this.VerifyParameters();
      IDictionary<string, TypedValue> namedParams = this.NamedParams;
      return this.Session.EnumerableFilter(this.collection, this.ExpandParameterLists(namedParams), this.GetQueryParameters(namedParams));
    }

    public override IEnumerable<T> Enumerable<T>()
    {
      this.VerifyParameters();
      IDictionary<string, TypedValue> namedParams = this.NamedParams;
      return this.Session.EnumerableFilter<T>(this.collection, this.ExpandParameterLists(namedParams), this.GetQueryParameters(namedParams));
    }

    public override IList List()
    {
      this.VerifyParameters();
      IDictionary<string, TypedValue> namedParams = this.NamedParams;
      return this.Session.ListFilter(this.collection, this.ExpandParameterLists(namedParams), this.GetQueryParameters(namedParams));
    }

    public override IList<T> List<T>()
    {
      this.VerifyParameters();
      IDictionary<string, TypedValue> namedParams = this.NamedParams;
      return this.Session.ListFilter<T>(this.collection, this.ExpandParameterLists(namedParams), this.GetQueryParameters(namedParams));
    }

    public override IType[] TypeArray()
    {
      IList<IType> types = this.Types;
      int count = types.Count;
      IType[] typeArray = new IType[count + 1];
      for (int index = 0; index < count; ++index)
        typeArray[index + 1] = types[index];
      return typeArray;
    }

    public override object[] ValueArray()
    {
      IList values = this.Values;
      int count = values.Count;
      object[] objArray = new object[count + 1];
      for (int index = 0; index < count; ++index)
        objArray[index + 1] = values[index];
      return objArray;
    }
  }
}
