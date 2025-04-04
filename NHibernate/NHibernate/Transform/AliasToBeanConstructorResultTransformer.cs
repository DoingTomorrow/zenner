// Decompiled with JetBrains decompiler
// Type: NHibernate.Transform.AliasToBeanConstructorResultTransformer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Reflection;

#nullable disable
namespace NHibernate.Transform
{
  [Serializable]
  public class AliasToBeanConstructorResultTransformer : IResultTransformer
  {
    private readonly ConstructorInfo constructor;

    public AliasToBeanConstructorResultTransformer(ConstructorInfo constructor)
    {
      this.constructor = constructor != null ? constructor : throw new ArgumentNullException(nameof (constructor));
    }

    public object TransformTuple(object[] tuple, string[] aliases)
    {
      try
      {
        return this.constructor.Invoke(tuple);
      }
      catch (Exception ex)
      {
        throw new QueryException("could not instantiate: " + this.constructor.DeclaringType.FullName, ex);
      }
    }

    public IList TransformList(IList collection) => collection;

    public bool Equals(AliasToBeanConstructorResultTransformer other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      return object.ReferenceEquals((object) this, (object) other) || object.Equals((object) other.constructor, (object) this.constructor);
    }

    public override bool Equals(object obj)
    {
      return this.Equals(obj as AliasToBeanConstructorResultTransformer);
    }

    public override int GetHashCode() => this.constructor.GetHashCode();
  }
}
