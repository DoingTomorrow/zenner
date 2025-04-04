// Decompiled with JetBrains decompiler
// Type: NHibernate.UserTypes.ICompositeUserType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Type;
using System.Data;

#nullable disable
namespace NHibernate.UserTypes
{
  public interface ICompositeUserType
  {
    string[] PropertyNames { get; }

    IType[] PropertyTypes { get; }

    object GetPropertyValue(object component, int property);

    void SetPropertyValue(object component, int property, object value);

    System.Type ReturnedClass { get; }

    bool Equals(object x, object y);

    int GetHashCode(object x);

    object NullSafeGet(IDataReader dr, string[] names, ISessionImplementor session, object owner);

    void NullSafeSet(
      IDbCommand cmd,
      object value,
      int index,
      bool[] settable,
      ISessionImplementor session);

    object DeepCopy(object value);

    bool IsMutable { get; }

    object Disassemble(object value, ISessionImplementor session);

    object Assemble(object cached, ISessionImplementor session, object owner);

    object Replace(object original, object target, ISessionImplementor session, object owner);
  }
}
