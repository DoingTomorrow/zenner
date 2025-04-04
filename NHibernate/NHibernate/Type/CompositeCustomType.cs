// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.CompositeCustomType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Xml;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class CompositeCustomType : AbstractType, IAbstractComponentType, IType, ICacheAssembler
  {
    private readonly ICompositeUserType userType;
    private readonly string name;

    public ICompositeUserType UserType => this.userType;

    public CompositeCustomType(System.Type userTypeClass, IDictionary<string, string> parameters)
    {
      this.name = userTypeClass.FullName;
      try
      {
        this.userType = (ICompositeUserType) NHibernate.Cfg.Environment.BytecodeProvider.ObjectsFactory.CreateInstance(userTypeClass);
      }
      catch (MethodAccessException ex)
      {
        throw new MappingException("MethodAccessException trying to instantiate custom type: " + this.name, (Exception) ex);
      }
      catch (TargetInvocationException ex)
      {
        throw new MappingException("TargetInvocationException trying to instantiate custom type: " + this.name, (Exception) ex);
      }
      catch (ArgumentException ex)
      {
        throw new MappingException("ArgumentException trying to instantiate custom type: " + this.name, (Exception) ex);
      }
      catch (InvalidCastException ex)
      {
        throw new MappingException(this.name + " must implement NHibernate.UserTypes.ICompositeUserType", (Exception) ex);
      }
      TypeFactory.InjectParameters((object) this.userType, parameters);
      if (!this.userType.ReturnedClass.IsSerializable)
        LoggerProvider.LoggerFor(typeof (CustomType)).WarnFormat("the custom composite class '{0}' handled by '{1}' is not Serializable: ", (object) this.userType.ReturnedClass, (object) userTypeClass);
      if (this.userType.PropertyTypes == null)
        throw new InvalidOperationException(string.Format("ICompositeUserType {0} returned a null value for 'PropertyTypes'.", (object) this.userType.GetType()));
      if (this.userType.PropertyNames == null)
        throw new InvalidOperationException(string.Format("ICompositeUserType {0} returned a null value for 'PropertyNames'.", (object) this.userType.GetType()));
    }

    public virtual IType[] Subtypes => this.userType.PropertyTypes;

    public virtual string[] PropertyNames => this.userType.PropertyNames;

    public virtual object[] GetPropertyValues(object component, ISessionImplementor session)
    {
      return this.GetPropertyValues(component, session.EntityMode);
    }

    public virtual object[] GetPropertyValues(object component, EntityMode entityMode)
    {
      int length = this.Subtypes.Length;
      object[] propertyValues = new object[length];
      for (int i = 0; i < length; ++i)
        propertyValues[i] = this.GetPropertyValue(component, i);
      return propertyValues;
    }

    public virtual void SetPropertyValues(object component, object[] values, EntityMode entityMode)
    {
      for (int property = 0; property < values.Length; ++property)
        this.userType.SetPropertyValue(component, property, values[property]);
    }

    public virtual object GetPropertyValue(object component, int i, ISessionImplementor session)
    {
      return this.GetPropertyValue(component, i);
    }

    public object GetPropertyValue(object component, int i)
    {
      return this.userType.GetPropertyValue(component, i);
    }

    public virtual CascadeStyle GetCascadeStyle(int i) => CascadeStyle.None;

    public virtual FetchMode GetFetchMode(int i) => FetchMode.Default;

    public bool IsEmbedded => false;

    public override bool IsComponentType => true;

    public override object Assemble(object cached, ISessionImplementor session, object owner)
    {
      return this.userType.Assemble(cached, session, owner);
    }

    public override object DeepCopy(
      object value,
      EntityMode entityMode,
      ISessionFactoryImplementor factory)
    {
      return this.userType.DeepCopy(value);
    }

    public override object Disassemble(object value, ISessionImplementor session, object owner)
    {
      return this.userType.Disassemble(value, session);
    }

    public override int GetColumnSpan(IMapping mapping)
    {
      IType[] propertyTypes = this.userType.PropertyTypes;
      int columnSpan = 0;
      for (int index = 0; index < propertyTypes.Length; ++index)
        columnSpan += propertyTypes[index].GetColumnSpan(mapping);
      return columnSpan;
    }

    public override string Name => this.name;

    public override System.Type ReturnedClass => this.userType.ReturnedClass;

    public override bool IsMutable => this.userType.IsMutable;

    public override object NullSafeGet(
      IDataReader rs,
      string name,
      ISessionImplementor session,
      object owner)
    {
      return this.userType.NullSafeGet(rs, new string[1]
      {
        name
      }, session, owner);
    }

    public override object NullSafeGet(
      IDataReader rs,
      string[] names,
      ISessionImplementor session,
      object owner)
    {
      return this.userType.NullSafeGet(rs, names, session, owner);
    }

    public override void NullSafeSet(
      IDbCommand st,
      object value,
      int index,
      bool[] settable,
      ISessionImplementor session)
    {
      this.userType.NullSafeSet(st, value, index, settable, session);
    }

    public override void NullSafeSet(
      IDbCommand cmd,
      object value,
      int index,
      ISessionImplementor session)
    {
      bool[] array = Enumerable.Repeat<bool>(true, this.GetColumnSpan((IMapping) session.Factory)).ToArray<bool>();
      this.userType.NullSafeSet(cmd, value, index, array, session);
    }

    public override SqlType[] SqlTypes(IMapping mapping)
    {
      IType[] propertyTypes = this.userType.PropertyTypes;
      SqlType[] sqlTypeArray = new SqlType[this.GetColumnSpan(mapping)];
      int num = 0;
      for (int index = 0; index < propertyTypes.Length; ++index)
      {
        foreach (SqlType sqlType in propertyTypes[index].SqlTypes(mapping))
          sqlTypeArray[num++] = sqlType;
      }
      return sqlTypeArray;
    }

    public override string ToLoggableString(object value, ISessionFactoryImplementor factory)
    {
      return value != null ? value.ToString() : "null";
    }

    public override bool Equals(object obj)
    {
      return base.Equals(obj) && ((CompositeCustomType) obj).userType.GetType() == this.userType.GetType();
    }

    public override int GetHashCode() => this.userType.GetHashCode();

    public override bool IsDirty(
      object old,
      object current,
      bool[] checkable,
      ISessionImplementor session)
    {
      return this.IsDirty(old, current, session);
    }

    public bool[] PropertyNullability => (bool[]) null;

    public override object Replace(
      object original,
      object current,
      ISessionImplementor session,
      object owner,
      IDictionary copiedAlready)
    {
      return this.userType.Replace(original, current, session, owner);
    }

    public override object FromXMLNode(XmlNode xml, IMapping factory) => (object) xml;

    public override bool IsEqual(object x, object y, EntityMode entityMode)
    {
      return this.userType.Equals(x, y);
    }

    public virtual bool IsMethodOf(MethodBase method) => false;

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
      IType[] subtypes = this.Subtypes;
      for (int index = 0; index < subtypes.Length; ++index)
      {
        bool[] columnNullness = subtypes[index].ToColumnNullness(propertyValues[index], mapping);
        Array.Copy((Array) columnNullness, 0, (Array) destinationArray, destinationIndex, columnNullness.Length);
        destinationIndex += columnNullness.Length;
      }
      return destinationArray;
    }
  }
}
