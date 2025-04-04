// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.ResultTransformer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Transform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Linq
{
  [Serializable]
  public class ResultTransformer : IResultTransformer
  {
    private readonly Delegate _itemTransformation;
    private readonly Delegate _listTransformation;

    public ResultTransformer(Delegate itemTransformation, Delegate listTransformation)
    {
      this._itemTransformation = itemTransformation;
      this._listTransformation = listTransformation;
    }

    public object TransformTuple(object[] tuple, string[] aliases)
    {
      if ((object) this._itemTransformation == null)
        return (object) tuple;
      return this._itemTransformation.DynamicInvoke((object) tuple);
    }

    public IList TransformList(IList collection)
    {
      if ((object) this._listTransformation == null)
        return collection;
      object obj = this._listTransformation.DynamicInvoke((object) ResultTransformer.GetToTransform(collection));
      if (obj is IList list)
        return list;
      return (IList) new List<object>() { obj };
    }

    private static IEnumerable<object> GetToTransform(IList collection)
    {
      return collection.Count > 0 && collection[0] is object[] objArray && objArray.Length == 1 ? collection.Cast<object[]>().Select<object[], object>((Func<object[], object>) (o => o[0])) : collection.Cast<object>();
    }

    public bool Equals(ResultTransformer other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return object.Equals((object) other._listTransformation, (object) this._listTransformation) && object.Equals((object) other._itemTransformation, (object) this._itemTransformation);
    }

    public override bool Equals(object obj) => this.Equals(obj as ResultTransformer);

    public override int GetHashCode()
    {
      return ((object) this._listTransformation != null ? this._listTransformation.GetHashCode() : 0) * 397 ^ ((object) this._itemTransformation != null ? this._itemTransformation.GetHashCode() : 0) * 17;
    }
  }
}
