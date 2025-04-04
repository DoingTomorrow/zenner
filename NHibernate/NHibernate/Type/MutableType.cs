// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.MutableType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlTypes;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public abstract class MutableType(SqlType sqlType) : NullableType(sqlType)
  {
    public override sealed bool IsMutable => true;

    public override object Replace(
      object original,
      object target,
      ISessionImplementor session,
      object owner,
      IDictionary copiedAlready)
    {
      return this.IsEqual(original, target, session.EntityMode) ? original : this.DeepCopy(original, session.EntityMode, session.Factory);
    }

    public abstract object DeepCopyNotNull(object value);

    public override object DeepCopy(
      object value,
      EntityMode entityMode,
      ISessionFactoryImplementor factory)
    {
      return value != null ? this.DeepCopyNotNull(value) : (object) null;
    }
  }
}
