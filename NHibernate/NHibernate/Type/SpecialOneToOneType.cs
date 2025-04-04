// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.SpecialOneToOneType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlTypes;
using System;
using System.Data;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class SpecialOneToOneType(
    string referencedEntityName,
    ForeignKeyDirection foreignKeyType,
    string uniqueKeyPropertyName,
    bool lazy,
    bool unwrapProxy,
    string entityName,
    string propertyName) : OneToOneType(referencedEntityName, foreignKeyType, uniqueKeyPropertyName, lazy, unwrapProxy, true, entityName, propertyName)
  {
    public override int GetColumnSpan(IMapping mapping)
    {
      return this.GetIdentifierOrUniqueKeyType(mapping).GetColumnSpan(mapping);
    }

    public override SqlType[] SqlTypes(IMapping mapping)
    {
      return this.GetIdentifierOrUniqueKeyType(mapping).SqlTypes(mapping);
    }

    public override bool UseLHSPrimaryKey => false;

    public override object Hydrate(
      IDataReader rs,
      string[] names,
      ISessionImplementor session,
      object owner)
    {
      return this.GetIdentifierOrUniqueKeyType((IMapping) session.Factory).NullSafeGet(rs, names, session, owner);
    }
  }
}
