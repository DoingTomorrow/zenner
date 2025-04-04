// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.CustomType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
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
  public class CustomType : 
    AbstractType,
    IDiscriminatorType,
    IIdentifierType,
    ILiteralType,
    IVersionType,
    IType,
    ICacheAssembler
  {
    private readonly IUserType userType;
    private readonly string name;
    private readonly SqlType[] sqlTypes;

    public IUserType UserType => this.userType;

    public CustomType(System.Type userTypeClass, IDictionary<string, string> parameters)
    {
      this.name = userTypeClass.Name;
      try
      {
        this.userType = (IUserType) NHibernate.Cfg.Environment.BytecodeProvider.ObjectsFactory.CreateInstance(userTypeClass);
      }
      catch (ArgumentNullException ex)
      {
        throw new MappingException("Argument is a null reference.", (Exception) ex);
      }
      catch (ArgumentException ex)
      {
        throw new MappingException("Argument " + userTypeClass.Name + " is not a RuntimeType", (Exception) ex);
      }
      catch (TargetInvocationException ex)
      {
        throw new MappingException("The constructor being called throws an exception.", (Exception) ex);
      }
      catch (MethodAccessException ex)
      {
        throw new MappingException("The caller does not have permission to call this constructor.", (Exception) ex);
      }
      catch (MissingMethodException ex)
      {
        throw new MappingException("No matching constructor was found.", (Exception) ex);
      }
      catch (InvalidCastException ex)
      {
        throw new MappingException(userTypeClass.Name + " must implement NHibernate.UserTypes.IUserType", (Exception) ex);
      }
      TypeFactory.InjectParameters((object) this.userType, parameters);
      this.sqlTypes = this.userType.SqlTypes;
      if (this.userType.ReturnedType.IsSerializable)
        return;
      LoggerProvider.LoggerFor(typeof (CustomType)).WarnFormat("the custom type '{0}' handled by '{1}' is not Serializable: ", (object) this.userType.ReturnedType, (object) userTypeClass);
    }

    public override SqlType[] SqlTypes(IMapping mapping) => this.sqlTypes;

    public override int GetColumnSpan(IMapping session) => this.sqlTypes.Length;

    public override System.Type ReturnedClass => this.userType.ReturnedType;

    public override object NullSafeGet(
      IDataReader rs,
      string[] names,
      ISessionImplementor session,
      object owner)
    {
      return this.userType.NullSafeGet(rs, names, owner);
    }

    public override object NullSafeGet(
      IDataReader rs,
      string name,
      ISessionImplementor session,
      object owner)
    {
      return this.NullSafeGet(rs, new string[1]{ name }, session, owner);
    }

    public override void NullSafeSet(
      IDbCommand st,
      object value,
      int index,
      bool[] settable,
      ISessionImplementor session)
    {
      if (!settable[0])
        return;
      this.userType.NullSafeSet(st, value, index);
    }

    public override void NullSafeSet(
      IDbCommand cmd,
      object value,
      int index,
      ISessionImplementor session)
    {
      this.userType.NullSafeSet(cmd, value, index);
    }

    public override string ToLoggableString(object value, ISessionFactoryImplementor factory)
    {
      return value == null ? "null" : this.ToXMLString(value, factory);
    }

    public override string Name => this.name;

    public override object DeepCopy(
      object value,
      EntityMode entityMode,
      ISessionFactoryImplementor factory)
    {
      return this.userType.DeepCopy(value);
    }

    public override bool IsMutable => this.userType.IsMutable;

    public override bool Equals(object obj)
    {
      return base.Equals(obj) && ((CustomType) obj).userType.GetType() == this.userType.GetType();
    }

    public override int GetHashCode() => this.userType.GetType().GetHashCode();

    public override int GetHashCode(object x, EntityMode entityMode)
    {
      return this.userType.GetHashCode(x);
    }

    public override bool IsDirty(
      object old,
      object current,
      bool[] checkable,
      ISessionImplementor session)
    {
      return checkable[0] && this.IsDirty(old, current, session);
    }

    public object StringToObject(string xml)
    {
      return ((IEnhancedUserType) this.userType).FromXMLString(xml);
    }

    public object FromStringValue(string xml)
    {
      return ((IEnhancedUserType) this.userType).FromXMLString(xml);
    }

    public virtual string ObjectToSQLString(object value, NHibernate.Dialect.Dialect dialect)
    {
      return ((IEnhancedUserType) this.userType).ObjectToSQLString(value);
    }

    public object Next(object current, ISessionImplementor session)
    {
      return ((IUserVersionType) this.userType).Next(current, session);
    }

    public object Seed(ISessionImplementor session)
    {
      return ((IUserVersionType) this.userType).Seed(session);
    }

    public IComparer Comparator => (IComparer) this.userType;

    public override object Replace(
      object original,
      object current,
      ISessionImplementor session,
      object owner,
      IDictionary copiedAlready)
    {
      return this.userType.Replace(original, current, owner);
    }

    public override object Assemble(object cached, ISessionImplementor session, object owner)
    {
      return this.userType.Assemble(cached, owner);
    }

    public override object Disassemble(object value, ISessionImplementor session, object owner)
    {
      return this.userType.Disassemble(value);
    }

    public override object FromXMLNode(XmlNode xml, IMapping factory)
    {
      return this.FromXMLString(xml.Value, factory);
    }

    public virtual object FromXMLString(string xml, IMapping factory)
    {
      return ((IEnhancedUserType) this.userType).FromXMLString(xml);
    }

    public virtual bool IsEqual(object x, object y) => this.userType.Equals(x, y);

    public override bool IsEqual(object x, object y, EntityMode entityMode) => this.IsEqual(x, y);

    public override void SetToXMLNode(
      XmlNode node,
      object value,
      ISessionFactoryImplementor factory)
    {
      node.Value = this.ToXMLString(value, factory);
    }

    public override bool[] ToColumnNullness(object value, IMapping mapping)
    {
      bool[] array = new bool[this.GetColumnSpan(mapping)];
      if (value != null)
        ArrayHelper.Fill<bool>(array, true);
      return array;
    }

    public virtual string ToXMLString(object value, ISessionFactoryImplementor factory)
    {
      if (value == null)
        return (string) null;
      return this.userType is IEnhancedUserType userType ? userType.ToXMLString(value) : value.ToString();
    }
  }
}
