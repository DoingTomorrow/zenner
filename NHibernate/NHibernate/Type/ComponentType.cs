// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.ComponentType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.Tuple;
using NHibernate.Tuple.Component;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Xml;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class ComponentType : AbstractType, IAbstractComponentType, IType, ICacheAssembler
  {
    private readonly IType[] propertyTypes;
    private readonly string[] propertyNames;
    private readonly bool[] propertyNullability;
    private readonly int propertySpan;
    private readonly CascadeStyle[] cascade;
    private readonly FetchMode?[] joinedFetch;
    private readonly bool isKey;
    protected internal EntityModeToTuplizerMapping tuplizerMapping;
    private bool overridesGetHashCode;

    public override SqlType[] SqlTypes(IMapping mapping)
    {
      SqlType[] sqlTypeArray = new SqlType[this.GetColumnSpan(mapping)];
      int num = 0;
      for (int index = 0; index < this.propertySpan; ++index)
      {
        foreach (SqlType sqlType in this.propertyTypes[index].SqlTypes(mapping))
          sqlTypeArray[num++] = sqlType;
      }
      return sqlTypeArray;
    }

    public override int GetColumnSpan(IMapping mapping)
    {
      int columnSpan = 0;
      for (int index = 0; index < this.propertySpan; ++index)
        columnSpan += this.propertyTypes[index].GetColumnSpan(mapping);
      return columnSpan;
    }

    public ComponentType(ComponentMetamodel metamodel)
    {
      this.isKey = metamodel.IsKey;
      this.propertySpan = metamodel.PropertySpan;
      this.propertyNames = new string[this.propertySpan];
      this.propertyTypes = new IType[this.propertySpan];
      this.propertyNullability = new bool[this.propertySpan];
      this.cascade = new CascadeStyle[this.propertySpan];
      this.joinedFetch = new FetchMode?[this.propertySpan];
      for (int index = 0; index < this.propertySpan; ++index)
      {
        StandardProperty property = metamodel.GetProperty(index);
        this.propertyNames[index] = property.Name;
        this.propertyTypes[index] = property.Type;
        this.propertyNullability[index] = property.IsNullable;
        this.cascade[index] = property.CascadeStyle;
        this.joinedFetch[index] = property.FetchMode;
      }
      this.tuplizerMapping = (EntityModeToTuplizerMapping) metamodel.TuplizerMapping;
      ITuplizer tuplizerOrNull = this.tuplizerMapping.GetTuplizerOrNull(EntityMode.Poco);
      if (tuplizerOrNull == null)
        return;
      this.overridesGetHashCode = ReflectHelper.OverridesGetHashCode(tuplizerOrNull.MappedClass);
    }

    public override bool IsCollectionType => false;

    public override bool IsComponentType => true;

    public override bool IsEntityType => false;

    public override System.Type ReturnedClass
    {
      get => this.tuplizerMapping.GetTuplizer(EntityMode.Poco).MappedClass;
    }

    public override int GetHashCode(
      object x,
      EntityMode entityMode,
      ISessionFactoryImplementor factory)
    {
      return this.overridesGetHashCode ? x.GetHashCode() : this.GetHashCode(x, entityMode);
    }

    public override int GetHashCode(object x, EntityMode entityMode)
    {
      int hashCode = 17;
      object[] propertyValues = this.GetPropertyValues(x, entityMode);
      for (int index = 0; index < this.propertySpan; ++index)
      {
        object x1 = propertyValues[index];
        hashCode *= 37;
        if (x1 != null)
          hashCode += this.propertyTypes[index].GetHashCode(x1, entityMode);
      }
      return hashCode;
    }

    public override bool IsDirty(object x, object y, ISessionImplementor session)
    {
      if (x == y)
        return false;
      EntityMode entityMode = session.EntityMode;
      if (entityMode != EntityMode.Poco && (x == null || y == null))
        return true;
      object[] propertyValues1 = this.GetPropertyValues(x, entityMode);
      object[] propertyValues2 = this.GetPropertyValues(y, entityMode);
      for (int index = 0; index < propertyValues1.Length; ++index)
      {
        if (this.propertyTypes[index].IsDirty(propertyValues1[index], propertyValues2[index], session))
          return true;
      }
      return false;
    }

    public override bool IsDirty(
      object x,
      object y,
      bool[] checkable,
      ISessionImplementor session)
    {
      if (x == y)
        return false;
      EntityMode entityMode = session.EntityMode;
      if (entityMode != EntityMode.Poco && (x == null || y == null))
        return true;
      object[] propertyValues1 = this.GetPropertyValues(x, entityMode);
      object[] propertyValues2 = this.GetPropertyValues(y, entityMode);
      int sourceIndex = 0;
      for (int index = 0; index < propertyValues1.Length; ++index)
      {
        int columnSpan = this.propertyTypes[index].GetColumnSpan((IMapping) session.Factory);
        if (columnSpan <= 1)
        {
          if ((columnSpan == 0 || checkable[sourceIndex]) && this.propertyTypes[index].IsDirty(propertyValues1[index], propertyValues2[index], session))
            return true;
        }
        else
        {
          bool[] flagArray = new bool[columnSpan];
          Array.Copy((Array) checkable, sourceIndex, (Array) flagArray, 0, columnSpan);
          if (this.propertyTypes[index].IsDirty(propertyValues1[index], propertyValues2[index], flagArray, session))
            return true;
        }
        sourceIndex += columnSpan;
      }
      return false;
    }

    public override object NullSafeGet(
      IDataReader rs,
      string[] names,
      ISessionImplementor session,
      object owner)
    {
      return this.ResolveIdentifier(this.Hydrate(rs, names, session, owner), session, owner);
    }

    public override void NullSafeSet(
      IDbCommand st,
      object value,
      int begin,
      ISessionImplementor session)
    {
      object[] values = this.NullSafeGetValues(value, session.EntityMode);
      for (int index = 0; index < this.propertySpan; ++index)
      {
        this.propertyTypes[index].NullSafeSet(st, values[index], begin, session);
        begin += this.propertyTypes[index].GetColumnSpan((IMapping) session.Factory);
      }
    }

    public override void NullSafeSet(
      IDbCommand st,
      object value,
      int begin,
      bool[] settable,
      ISessionImplementor session)
    {
      object[] values = this.NullSafeGetValues(value, session.EntityMode);
      int sourceIndex = 0;
      for (int index = 0; index < this.propertySpan; ++index)
      {
        int columnSpan = this.propertyTypes[index].GetColumnSpan((IMapping) session.Factory);
        switch (columnSpan)
        {
          case 0:
            sourceIndex += columnSpan;
            continue;
          case 1:
            if (settable[sourceIndex])
            {
              this.propertyTypes[index].NullSafeSet(st, values[index], begin, session);
              ++begin;
              goto case 0;
            }
            else
              goto case 0;
          default:
            bool[] flagArray = new bool[columnSpan];
            Array.Copy((Array) settable, sourceIndex, (Array) flagArray, 0, columnSpan);
            this.propertyTypes[index].NullSafeSet(st, values[index], begin, flagArray, session);
            begin += ArrayHelper.CountTrue(flagArray);
            goto case 0;
        }
      }
    }

    private object[] NullSafeGetValues(object value, EntityMode entityMode)
    {
      return value == null ? new object[this.propertySpan] : this.GetPropertyValues(value, entityMode);
    }

    public override object NullSafeGet(
      IDataReader rs,
      string name,
      ISessionImplementor session,
      object owner)
    {
      return this.NullSafeGet(rs, new string[1]{ name }, session, owner);
    }

    public object GetPropertyValue(object component, int i, EntityMode entityMode)
    {
      return this.tuplizerMapping.GetTuplizer(entityMode).GetPropertyValue(component, i);
    }

    public object GetPropertyValue(object component, int i, ISessionImplementor session)
    {
      return this.GetPropertyValue(component, i, session.EntityMode);
    }

    public object[] GetPropertyValues(object component, EntityMode entityMode)
    {
      return this.tuplizerMapping.GetTuplizer(entityMode).GetPropertyValues(component);
    }

    public object[] GetPropertyValues(object component, ISessionImplementor session)
    {
      return this.GetPropertyValues(component, session.EntityMode);
    }

    public virtual void SetPropertyValues(object component, object[] values, EntityMode entityMode)
    {
      this.tuplizerMapping.GetTuplizer(entityMode).SetPropertyValues(component, values);
    }

    public IType[] Subtypes => this.propertyTypes;

    public override string Name
    {
      get => "component" + ArrayHelper.ToString((object[]) this.propertyNames);
    }

    public override string ToLoggableString(object value, ISessionFactoryImplementor factory)
    {
      if (value == null)
        return "null";
      IDictionary<string, string> dictionary = (IDictionary<string, string>) new Dictionary<string, string>();
      object[] propertyValues = this.GetPropertyValues(value, (this.tuplizerMapping.GuessEntityMode(value) ?? throw new InvalidCastException(value.GetType().FullName)).Value);
      for (int index = 0; index < this.propertyTypes.Length; ++index)
        dictionary[this.propertyNames[index]] = this.propertyTypes[index].ToLoggableString(propertyValues[index], factory);
      return StringHelper.Unqualify(this.Name) + CollectionPrinter.ToString(dictionary);
    }

    public string[] PropertyNames => this.propertyNames;

    public override object DeepCopy(
      object component,
      EntityMode entityMode,
      ISessionFactoryImplementor factory)
    {
      if (component == null)
        return (object) null;
      object[] propertyValues = this.GetPropertyValues(component, entityMode);
      for (int index = 0; index < this.propertySpan; ++index)
        propertyValues[index] = this.propertyTypes[index].DeepCopy(propertyValues[index], entityMode, factory);
      object component1 = this.Instantiate(entityMode);
      this.SetPropertyValues(component1, propertyValues, entityMode);
      IComponentTuplizer tuplizer = (IComponentTuplizer) this.tuplizerMapping.GetTuplizer(entityMode);
      if (tuplizer.HasParentProperty)
        tuplizer.SetParent(component1, tuplizer.GetParent(component), factory);
      return component1;
    }

    public override object Replace(
      object original,
      object target,
      ISessionImplementor session,
      object owner,
      IDictionary copiedAlready)
    {
      if (original == null)
        return (object) null;
      object component = target ?? this.Instantiate(owner, session);
      EntityMode entityMode = session.EntityMode;
      object[] values = TypeHelper.Replace(this.GetPropertyValues(original, entityMode), this.GetPropertyValues(component, entityMode), this.propertyTypes, session, owner, copiedAlready);
      this.SetPropertyValues(component, values, entityMode);
      return component;
    }

    public override object Replace(
      object original,
      object target,
      ISessionImplementor session,
      object owner,
      IDictionary copyCache,
      ForeignKeyDirection foreignKeyDirection)
    {
      if (original == null)
        return (object) null;
      object component = target ?? this.Instantiate(owner, session);
      EntityMode entityMode = session.EntityMode;
      object[] values = TypeHelper.Replace(this.GetPropertyValues(original, entityMode), this.GetPropertyValues(component, entityMode), this.propertyTypes, session, owner, copyCache, foreignKeyDirection);
      this.SetPropertyValues(component, values, entityMode);
      return component;
    }

    public object Instantiate(EntityMode entityMode)
    {
      return this.tuplizerMapping.GetTuplizer(entityMode).Instantiate();
    }

    public virtual object Instantiate(object parent, ISessionImplementor session)
    {
      object component = this.Instantiate(session.EntityMode);
      IComponentTuplizer tuplizer = (IComponentTuplizer) this.tuplizerMapping.GetTuplizer(session.EntityMode);
      if (tuplizer.HasParentProperty && parent != null)
        tuplizer.SetParent(component, session.PersistenceContext.ProxyFor(parent), session.Factory);
      return component;
    }

    public CascadeStyle GetCascadeStyle(int i) => this.cascade[i];

    public override bool IsMutable => true;

    public override object Disassemble(object value, ISessionImplementor session, object owner)
    {
      if (value == null)
        return (object) null;
      object[] propertyValues = this.GetPropertyValues(value, session.EntityMode);
      for (int index = 0; index < this.propertyTypes.Length; ++index)
        propertyValues[index] = this.propertyTypes[index].Disassemble(propertyValues[index], session, owner);
      return (object) propertyValues;
    }

    public override object Assemble(object obj, ISessionImplementor session, object owner)
    {
      if (obj == null)
        return (object) null;
      object[] objArray = (object[]) obj;
      object[] values = new object[objArray.Length];
      for (int index = 0; index < this.propertyTypes.Length; ++index)
        values[index] = this.propertyTypes[index].Assemble(objArray[index], session, owner);
      object component = this.Instantiate(owner, session);
      this.SetPropertyValues(component, values, session.EntityMode);
      return component;
    }

    public FetchMode GetFetchMode(int i) => this.joinedFetch[i].GetValueOrDefault();

    public virtual bool IsEmbedded => false;

    public override object Hydrate(
      IDataReader rs,
      string[] names,
      ISessionImplementor session,
      object owner)
    {
      int begin = 0;
      bool flag = false;
      object[] objArray = new object[this.propertySpan];
      for (int index = 0; index < this.propertySpan; ++index)
      {
        int columnSpan = this.propertyTypes[index].GetColumnSpan((IMapping) session.Factory);
        string[] names1 = ArrayHelper.Slice(names, begin, columnSpan);
        object obj = this.propertyTypes[index].Hydrate(rs, names1, session, owner);
        if (obj == null)
        {
          if (this.isKey)
            return (object) null;
        }
        else
          flag = true;
        objArray[index] = obj;
        begin += columnSpan;
      }
      if (this.ReturnedClass.IsValueType)
        return (object) objArray;
      return !flag ? (object) null : (object) objArray;
    }

    public override object ResolveIdentifier(
      object value,
      ISessionImplementor session,
      object owner)
    {
      if (value == null)
        return (object) null;
      object component = this.Instantiate(owner, session);
      object[] objArray = (object[]) value;
      object[] values = new object[objArray.Length];
      for (int index = 0; index < objArray.Length; ++index)
        values[index] = this.propertyTypes[index].ResolveIdentifier(objArray[index], session, owner);
      this.SetPropertyValues(component, values, session.EntityMode);
      return component;
    }

    public override object SemiResolve(object value, ISessionImplementor session, object owner)
    {
      return this.ResolveIdentifier(value, session, owner);
    }

    public override bool IsModified(
      object old,
      object current,
      bool[] checkable,
      ISessionImplementor session)
    {
      if (current == null)
        return old != null;
      if (old == null)
        return current != null;
      object[] propertyValues = this.GetPropertyValues(current, session);
      object[] objArray = (object[]) old;
      int sourceIndex = 0;
      for (int index = 0; index < propertyValues.Length; ++index)
      {
        int columnSpan = this.propertyTypes[index].GetColumnSpan((IMapping) session.Factory);
        bool[] flagArray = new bool[columnSpan];
        Array.Copy((Array) checkable, sourceIndex, (Array) flagArray, 0, columnSpan);
        if (this.propertyTypes[index].IsModified(objArray[index], propertyValues[index], flagArray, session))
          return true;
        sourceIndex += columnSpan;
      }
      return false;
    }

    public bool[] PropertyNullability => this.propertyNullability;

    public override int Compare(object x, object y, EntityMode? entityMode)
    {
      if (x == y)
        return 0;
      object[] propertyValues1 = this.GetPropertyValues(x, entityMode.GetValueOrDefault());
      object[] propertyValues2 = this.GetPropertyValues(y, entityMode.GetValueOrDefault());
      for (int index = 0; index < this.propertySpan; ++index)
      {
        int num = this.propertyTypes[index].Compare(propertyValues1[index], propertyValues2[index], entityMode);
        if (num != 0)
          return num;
      }
      return 0;
    }

    public override object FromXMLNode(XmlNode xml, IMapping factory) => (object) xml;

    public override bool IsEqual(object x, object y, EntityMode entityMode)
    {
      if (x == y)
        return true;
      if (x == null || y == null)
        return false;
      object[] propertyValues1 = this.GetPropertyValues(x, entityMode);
      object[] propertyValues2 = this.GetPropertyValues(y, entityMode);
      for (int index = 0; index < this.propertySpan; ++index)
      {
        if (!this.propertyTypes[index].IsEqual(propertyValues1[index], propertyValues2[index], entityMode))
          return false;
      }
      return true;
    }

    public override bool IsEqual(
      object x,
      object y,
      EntityMode entityMode,
      ISessionFactoryImplementor factory)
    {
      if (x == y)
        return true;
      if (x == null || y == null)
        return false;
      object[] propertyValues1 = this.GetPropertyValues(x, entityMode);
      object[] propertyValues2 = this.GetPropertyValues(y, entityMode);
      for (int index = 0; index < this.propertySpan; ++index)
      {
        if (!this.propertyTypes[index].IsEqual(propertyValues1[index], propertyValues2[index], entityMode, factory))
          return false;
      }
      return true;
    }

    public virtual bool IsMethodOf(MethodBase method) => false;

    public override bool IsSame(object x, object y, EntityMode entityMode)
    {
      if (x == y)
        return true;
      if (x == null || y == null)
        return false;
      object[] propertyValues1 = this.GetPropertyValues(x, entityMode);
      object[] propertyValues2 = this.GetPropertyValues(y, entityMode);
      for (int index = 0; index < this.propertySpan; ++index)
      {
        if (!this.propertyTypes[index].IsSame(propertyValues1[index], propertyValues2[index], entityMode))
          return false;
      }
      return true;
    }

    public override void SetToXMLNode(
      XmlNode node,
      object value,
      ISessionFactoryImplementor factory)
    {
      AbstractType.ReplaceNode(node, (XmlNode) value);
    }

    public override bool[] ToColumnNullness(object value, IMapping mapping)
    {
      bool[] destinationArray = new bool[this.GetColumnSpan(mapping)];
      if (value == null)
        return destinationArray;
      object[] propertyValues = this.GetPropertyValues(value, EntityMode.Poco);
      int destinationIndex = 0;
      for (int index = 0; index < this.propertyTypes.Length; ++index)
      {
        bool[] columnNullness = this.propertyTypes[index].ToColumnNullness(propertyValues[index], mapping);
        Array.Copy((Array) columnNullness, 0, (Array) destinationArray, destinationIndex, columnNullness.Length);
        destinationIndex += columnNullness.Length;
      }
      return destinationArray;
    }

    public override bool IsXMLElement => true;

    public int GetPropertyIndex(string name)
    {
      string[] propertyNames = this.PropertyNames;
      for (int propertyIndex = 0; propertyIndex < propertyNames.Length; ++propertyIndex)
      {
        if (propertyNames[propertyIndex].Equals(name))
          return propertyIndex;
      }
      throw new PropertyNotFoundException(this.ReturnedClass, name);
    }
  }
}
