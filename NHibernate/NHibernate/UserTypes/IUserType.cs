// Decompiled with JetBrains decompiler
// Type: NHibernate.UserTypes.IUserType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlTypes;
using System;
using System.Data;

#nullable disable
namespace NHibernate.UserTypes
{
  public interface IUserType
  {
    SqlType[] SqlTypes { get; }

    Type ReturnedType { get; }

    bool Equals(object x, object y);

    int GetHashCode(object x);

    object NullSafeGet(IDataReader rs, string[] names, object owner);

    void NullSafeSet(IDbCommand cmd, object value, int index);

    object DeepCopy(object value);

    bool IsMutable { get; }

    object Replace(object original, object target, object owner);

    object Assemble(object cached, object owner);

    object Disassemble(object value);
  }
}
