// Decompiled with JetBrains decompiler
// Type: NHibernate.Param.DynamicFilterParameterSpecification
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;

#nullable disable
namespace NHibernate.Param
{
  public class DynamicFilterParameterSpecification : IParameterSpecification
  {
    private const string DynamicFilterParameterIdTemplate = "<dfnh-{0}_span{1}>";
    private readonly IType expectedDefinedType;
    private readonly string filterParameterFullName;
    private IType elementType;

    public DynamicFilterParameterSpecification(
      string filterName,
      string parameterName,
      IType expectedDefinedType,
      int? collectionSpan)
    {
      this.elementType = expectedDefinedType;
      this.expectedDefinedType = collectionSpan.HasValue ? (IType) new DynamicFilterParameterSpecification.CollectionOfValuesType(expectedDefinedType, collectionSpan.Value) : expectedDefinedType;
      this.filterParameterFullName = filterName + (object) '.' + parameterName;
    }

    public void Bind(
      IDbCommand command,
      IList<Parameter> sqlQueryParametersList,
      QueryParameters queryParameters,
      ISessionImplementor session)
    {
      this.Bind(command, sqlQueryParametersList, 0, sqlQueryParametersList, queryParameters, session);
    }

    public void Bind(
      IDbCommand command,
      IList<Parameter> multiSqlQueryParametersList,
      int singleSqlParametersOffset,
      IList<Parameter> sqlQueryParametersList,
      QueryParameters queryParameters,
      ISessionImplementor session)
    {
      string backTrackId = this.GetIdsForBackTrack((IMapping) session.Factory).First<string>();
      object filterParameterValue = session.GetFilterParameterValue(this.filterParameterFullName);
      foreach (int parameterLocation in multiSqlQueryParametersList.GetEffectiveParameterLocations(backTrackId))
        this.ExpectedType.NullSafeSet(command, filterParameterValue, parameterLocation, session);
    }

    public string FilterParameterFullName => this.filterParameterFullName;

    public IType ElementType => this.elementType;

    public IType ExpectedType
    {
      get => this.expectedDefinedType;
      set => throw new InvalidOperationException();
    }

    public string RenderDisplayInfo() => "dynamic-filter={" + this.filterParameterFullName + "}";

    public IEnumerable<string> GetIdsForBackTrack(IMapping sessionFactory)
    {
      int columnSpan = sessionFactory != null ? this.ExpectedType.GetColumnSpan(sessionFactory) : throw new ArgumentNullException(nameof (sessionFactory));
      for (int i = 0; i < columnSpan; ++i)
        yield return string.Format("<dfnh-{0}_span{1}>", (object) this.filterParameterFullName, (object) i);
    }

    public override bool Equals(object obj)
    {
      return this.Equals(obj as DynamicFilterParameterSpecification);
    }

    public bool Equals(DynamicFilterParameterSpecification other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      return object.ReferenceEquals((object) this, (object) other) || object.Equals((object) other.filterParameterFullName, (object) this.filterParameterFullName);
    }

    public override int GetHashCode() => this.filterParameterFullName.GetHashCode() ^ 53;

    [Serializable]
    private class CollectionOfValuesType : IType, ICacheAssembler
    {
      private readonly IType elementType;
      private readonly int valueSpan;

      public CollectionOfValuesType(IType elementType, int valueSpan)
      {
        this.elementType = elementType;
        this.valueSpan = valueSpan;
      }

      public object Disassemble(object value, ISessionImplementor session, object owner)
      {
        throw new InvalidOperationException();
      }

      public object Assemble(object cached, ISessionImplementor session, object owner)
      {
        throw new InvalidOperationException();
      }

      public void BeforeAssemble(object cached, ISessionImplementor session)
      {
      }

      public string Name => "DynamicFilterCollectionOfValues";

      public System.Type ReturnedClass => typeof (IEnumerable);

      public bool IsMutable => false;

      public bool IsAssociationType => false;

      public bool IsXMLElement => false;

      public bool IsCollectionType => false;

      public bool IsComponentType => false;

      public bool IsEntityType => false;

      public bool IsAnyType => false;

      public SqlType[] SqlTypes(IMapping mapping)
      {
        List<SqlType> sqlTypeList = new List<SqlType>(20);
        for (int index = 0; index < this.valueSpan; ++index)
          sqlTypeList.AddRange((IEnumerable<SqlType>) this.elementType.SqlTypes(mapping));
        return sqlTypeList.ToArray();
      }

      public int GetColumnSpan(IMapping mapping)
      {
        return this.elementType.GetColumnSpan(mapping) * this.valueSpan;
      }

      public bool IsDirty(object old, object current, ISessionImplementor session) => false;

      public bool IsDirty(
        object old,
        object current,
        bool[] checkable,
        ISessionImplementor session)
      {
        return false;
      }

      public bool IsModified(
        object oldHydratedState,
        object currentState,
        bool[] checkable,
        ISessionImplementor session)
      {
        return false;
      }

      public object NullSafeGet(
        IDataReader rs,
        string[] names,
        ISessionImplementor session,
        object owner)
      {
        throw new InvalidOperationException();
      }

      public object NullSafeGet(
        IDataReader rs,
        string name,
        ISessionImplementor session,
        object owner)
      {
        throw new InvalidOperationException();
      }

      public void NullSafeSet(
        IDbCommand st,
        object value,
        int index,
        bool[] settable,
        ISessionImplementor session)
      {
        throw new InvalidOperationException();
      }

      public void NullSafeSet(IDbCommand st, object value, int index, ISessionImplementor session)
      {
        int num1 = index;
        int num2 = 0;
        int columnSpan = this.elementType.GetColumnSpan((IMapping) session.Factory);
        foreach (object obj in (IEnumerable) value)
        {
          this.elementType.NullSafeSet(st, obj, num1 + num2, session);
          num2 += columnSpan;
        }
      }

      public string ToLoggableString(object value, ISessionFactoryImplementor factory)
      {
        throw new InvalidOperationException();
      }

      public object DeepCopy(object val, EntityMode entityMode, ISessionFactoryImplementor factory)
      {
        throw new InvalidOperationException();
      }

      public object Hydrate(
        IDataReader rs,
        string[] names,
        ISessionImplementor session,
        object owner)
      {
        throw new InvalidOperationException();
      }

      public object ResolveIdentifier(object value, ISessionImplementor session, object owner)
      {
        throw new InvalidOperationException();
      }

      public object SemiResolve(object value, ISessionImplementor session, object owner)
      {
        throw new InvalidOperationException();
      }

      public object Replace(
        object original,
        object target,
        ISessionImplementor session,
        object owner,
        IDictionary copiedAlready)
      {
        throw new InvalidOperationException();
      }

      public object Replace(
        object original,
        object target,
        ISessionImplementor session,
        object owner,
        IDictionary copyCache,
        ForeignKeyDirection foreignKeyDirection)
      {
        throw new InvalidOperationException();
      }

      public bool IsSame(object x, object y, EntityMode entityMode) => false;

      public bool IsEqual(object x, object y, EntityMode entityMode) => false;

      public bool IsEqual(
        object x,
        object y,
        EntityMode entityMode,
        ISessionFactoryImplementor factory)
      {
        return false;
      }

      public int GetHashCode(object x, EntityMode entityMode) => this.GetHashCode();

      public int GetHashCode(object x, EntityMode entityMode, ISessionFactoryImplementor factory)
      {
        return this.GetHashCode();
      }

      public int Compare(object x, object y, EntityMode? entityMode) => 1;

      public IType GetSemiResolvedType(ISessionFactoryImplementor factory)
      {
        throw new InvalidOperationException();
      }

      public void SetToXMLNode(XmlNode node, object value, ISessionFactoryImplementor factory)
      {
        throw new InvalidOperationException();
      }

      public object FromXMLNode(XmlNode xml, IMapping factory)
      {
        throw new InvalidOperationException();
      }

      public bool[] ToColumnNullness(object value, IMapping mapping)
      {
        throw new InvalidOperationException();
      }
    }
  }
}
