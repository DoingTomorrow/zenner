// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.IType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlTypes;
using System.Collections;
using System.Data;
using System.Xml;

#nullable disable
namespace NHibernate.Type
{
  public interface IType : ICacheAssembler
  {
    string Name { get; }

    System.Type ReturnedClass { get; }

    bool IsMutable { get; }

    bool IsAssociationType { get; }

    bool IsXMLElement { get; }

    bool IsCollectionType { get; }

    bool IsComponentType { get; }

    bool IsEntityType { get; }

    bool IsAnyType { get; }

    SqlType[] SqlTypes(IMapping mapping);

    int GetColumnSpan(IMapping mapping);

    bool IsDirty(object old, object current, ISessionImplementor session);

    bool IsDirty(object old, object current, bool[] checkable, ISessionImplementor session);

    bool IsModified(
      object oldHydratedState,
      object currentState,
      bool[] checkable,
      ISessionImplementor session);

    object NullSafeGet(IDataReader rs, string[] names, ISessionImplementor session, object owner);

    object NullSafeGet(IDataReader rs, string name, ISessionImplementor session, object owner);

    void NullSafeSet(
      IDbCommand st,
      object value,
      int index,
      bool[] settable,
      ISessionImplementor session);

    void NullSafeSet(IDbCommand st, object value, int index, ISessionImplementor session);

    string ToLoggableString(object value, ISessionFactoryImplementor factory);

    object DeepCopy(object val, EntityMode entityMode, ISessionFactoryImplementor factory);

    object Hydrate(IDataReader rs, string[] names, ISessionImplementor session, object owner);

    object ResolveIdentifier(object value, ISessionImplementor session, object owner);

    object SemiResolve(object value, ISessionImplementor session, object owner);

    object Replace(
      object original,
      object target,
      ISessionImplementor session,
      object owner,
      IDictionary copiedAlready);

    object Replace(
      object original,
      object target,
      ISessionImplementor session,
      object owner,
      IDictionary copyCache,
      ForeignKeyDirection foreignKeyDirection);

    bool IsSame(object x, object y, EntityMode entityMode);

    bool IsEqual(object x, object y, EntityMode entityMode);

    bool IsEqual(object x, object y, EntityMode entityMode, ISessionFactoryImplementor factory);

    int GetHashCode(object x, EntityMode entityMode);

    int GetHashCode(object x, EntityMode entityMode, ISessionFactoryImplementor factory);

    int Compare(object x, object y, EntityMode? entityMode);

    IType GetSemiResolvedType(ISessionFactoryImplementor factory);

    void SetToXMLNode(XmlNode node, object value, ISessionFactoryImplementor factory);

    object FromXMLNode(XmlNode xml, IMapping factory);

    bool[] ToColumnNullness(object value, IMapping mapping);
  }
}
