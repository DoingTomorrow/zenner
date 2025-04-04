// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.TypedValue
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Engine
{
  [Serializable]
  public sealed class TypedValue
  {
    private readonly IType type;
    private readonly object value;
    private readonly IEqualityComparer<TypedValue> comparer;

    public TypedValue(IType type, object value, EntityMode entityMode)
    {
      this.type = type;
      this.value = value;
      ICollection collection = value as ICollection;
      if (!type.IsCollectionType && collection != null && !type.ReturnedClass.IsArray)
        this.comparer = (IEqualityComparer<TypedValue>) new TypedValue.ParameterListComparer(entityMode);
      else
        this.comparer = (IEqualityComparer<TypedValue>) new TypedValue.DefaultComparer(entityMode);
    }

    public object Value => this.value;

    public IType Type => this.type;

    public IEqualityComparer<TypedValue> Comparer => this.comparer;

    public override int GetHashCode() => this.comparer.GetHashCode(this);

    public override bool Equals(object obj) => this.comparer.Equals(this, obj as TypedValue);

    public override string ToString() => this.value != null ? this.value.ToString() : "null";

    [Serializable]
    public class ParameterListComparer : IEqualityComparer<TypedValue>
    {
      private readonly EntityMode entityMode;

      public ParameterListComparer(EntityMode entityMode) => this.entityMode = entityMode;

      public bool Equals(TypedValue x, TypedValue y)
      {
        return y != null && x.type.ReturnedClass.Equals(y.type.ReturnedClass) && this.IsEquals(x.type, x.value as ICollection, y.value as ICollection);
      }

      public int GetHashCode(TypedValue obj)
      {
        return this.GetHashCode(obj.type, obj.value as ICollection);
      }

      private int GetHashCode(IType type, ICollection values)
      {
        if (values == null)
          return 0;
        int hashCode = 0;
        foreach (object x in (IEnumerable) values)
          hashCode += x == null ? 0 : type.GetHashCode(x, this.entityMode);
        return hashCode;
      }

      private bool IsEquals(IType type, ICollection x, ICollection y)
      {
        if (x == y)
          return true;
        if (x == null || y == null || x.Count != y.Count)
          return false;
        IEnumerator enumerator1 = x.GetEnumerator();
        IEnumerator enumerator2 = y.GetEnumerator();
        while (enumerator1.MoveNext())
        {
          enumerator2.MoveNext();
          if (!type.IsEqual(enumerator1.Current, enumerator2.Current, this.entityMode))
            return false;
        }
        return true;
      }
    }

    [Serializable]
    public class DefaultComparer : IEqualityComparer<TypedValue>
    {
      private readonly EntityMode entityMode;

      public DefaultComparer(EntityMode entityMode) => this.entityMode = entityMode;

      public bool Equals(TypedValue x, TypedValue y)
      {
        return y != null && x.type.ReturnedClass.Equals(y.type.ReturnedClass) && x.type.IsEqual(y.value, x.value, this.entityMode);
      }

      public int GetHashCode(TypedValue obj)
      {
        return obj.value != null ? obj.type.GetHashCode(obj.value, this.entityMode) : 0;
      }
    }
  }
}
