// Decompiled with JetBrains decompiler
// Type: MSS.Business.Utils.Sql2008StructuredForReadingValues
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.Type;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

#nullable disable
namespace MSS.Business.Utils
{
  public class Sql2008StructuredForReadingValues : IType, ICacheAssembler
  {
    private static readonly SqlType[] x = new SqlType[1]
    {
      new SqlType(DbType.Object)
    };

    public SqlType[] SqlTypes(IMapping mapping) => Sql2008StructuredForReadingValues.x;

    public bool IsXMLElement { get; private set; }

    public bool IsCollectionType => true;

    public bool IsComponentType { get; private set; }

    public bool IsEntityType { get; private set; }

    public bool IsAnyType { get; private set; }

    public int GetColumnSpan(IMapping mapping) => 1;

    public bool IsDirty(object old, object current, ISessionImplementor session)
    {
      throw new NotImplementedException();
    }

    public bool IsDirty(object old, object current, bool[] checkable, ISessionImplementor session)
    {
      throw new NotImplementedException();
    }

    public bool IsModified(
      object oldHydratedState,
      object currentState,
      bool[] checkable,
      ISessionImplementor session)
    {
      throw new NotImplementedException();
    }

    public object NullSafeGet(
      IDataReader rs,
      string[] names,
      ISessionImplementor session,
      object owner)
    {
      throw new NotImplementedException();
    }

    public object NullSafeGet(
      IDataReader rs,
      string name,
      ISessionImplementor session,
      object owner)
    {
      throw new NotImplementedException();
    }

    public void NullSafeSet(
      IDbCommand st,
      object value,
      int index,
      bool[] settable,
      ISessionImplementor session)
    {
      throw new NotImplementedException();
    }

    public void NullSafeSet(IDbCommand st, object value, int index, ISessionImplementor session)
    {
      if (!(st is SqlCommand sqlCommand))
        throw new NotImplementedException();
      sqlCommand.Parameters[index].SqlDbType = SqlDbType.Structured;
      sqlCommand.Parameters[index].TypeName = "ReadingValuesToParse_TableType";
      sqlCommand.Parameters[index].Value = value;
    }

    public string ToLoggableString(object value, ISessionFactoryImplementor factory)
    {
      throw new NotImplementedException();
    }

    public object DeepCopy(object val, EntityMode entityMode, ISessionFactoryImplementor factory)
    {
      throw new NotImplementedException();
    }

    public object Hydrate(
      IDataReader rs,
      string[] names,
      ISessionImplementor session,
      object owner)
    {
      throw new NotImplementedException();
    }

    public object ResolveIdentifier(object value, ISessionImplementor session, object owner)
    {
      throw new NotImplementedException();
    }

    public object SemiResolve(object value, ISessionImplementor session, object owner)
    {
      throw new NotImplementedException();
    }

    public object Replace(
      object original,
      object target,
      ISessionImplementor session,
      object owner,
      IDictionary copiedAlready)
    {
      throw new NotImplementedException();
    }

    public object Replace(
      object original,
      object target,
      ISessionImplementor session,
      object owner,
      IDictionary copyCache,
      ForeignKeyDirection foreignKeyDirection)
    {
      throw new NotImplementedException();
    }

    public bool IsSame(object x, object y, EntityMode entityMode)
    {
      throw new NotImplementedException();
    }

    public bool IsEqual(object x, object y, EntityMode entityMode)
    {
      throw new NotImplementedException();
    }

    public bool IsEqual(
      object x,
      object y,
      EntityMode entityMode,
      ISessionFactoryImplementor factory)
    {
      throw new NotImplementedException();
    }

    public int GetHashCode(object x, EntityMode entityMode) => throw new NotImplementedException();

    public int GetHashCode(object x, EntityMode entityMode, ISessionFactoryImplementor factory)
    {
      throw new NotImplementedException();
    }

    public int Compare(object x, object y, EntityMode? entityMode)
    {
      throw new NotImplementedException();
    }

    public IType GetSemiResolvedType(ISessionFactoryImplementor factory)
    {
      throw new NotImplementedException();
    }

    public void SetToXMLNode(XmlNode node, object value, ISessionFactoryImplementor factory)
    {
      throw new NotImplementedException();
    }

    public object FromXMLNode(XmlNode xml, IMapping factory) => throw new NotImplementedException();

    public bool[] ToColumnNullness(object value, IMapping mapping)
    {
      throw new NotImplementedException();
    }

    public string Name { get; private set; }

    public System.Type ReturnedClass { get; private set; }

    public bool IsMutable { get; private set; }

    public bool IsAssociationType { get; private set; }

    public object Disassemble(object value, ISessionImplementor session, object owner)
    {
      throw new NotImplementedException();
    }

    public object Assemble(object cached, ISessionImplementor session, object owner)
    {
      throw new NotImplementedException();
    }

    public void BeforeAssemble(object cached, ISessionImplementor session)
    {
      throw new NotImplementedException();
    }
  }
}
