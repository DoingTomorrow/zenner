// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.NullableType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.Util;
using System;
using System.Data;
using System.Xml;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public abstract class NullableType : AbstractType
  {
    private static readonly bool IsDebugEnabled = LoggerProvider.LoggerFor(typeof (IType).Namespace).IsDebugEnabled;
    private readonly SqlType _sqlType;

    private IInternalLogger Log => LoggerProvider.LoggerFor(this.GetType());

    protected NullableType(SqlType sqlType) => this._sqlType = sqlType;

    public abstract void Set(IDbCommand cmd, object value, int index);

    public abstract object Get(IDataReader rs, int index);

    public abstract object Get(IDataReader rs, string name);

    public abstract string ToString(object val);

    public override sealed string ToLoggableString(object value, ISessionFactoryImplementor factory)
    {
      return value != null ? this.ToString(value) : (string) null;
    }

    public abstract object FromStringValue(string xml);

    public override void NullSafeSet(
      IDbCommand st,
      object value,
      int index,
      bool[] settable,
      ISessionImplementor session)
    {
      if (!settable[0])
        return;
      this.NullSafeSet(st, value, index);
    }

    public override sealed void NullSafeSet(
      IDbCommand st,
      object value,
      int index,
      ISessionImplementor session)
    {
      this.NullSafeSet(st, value, index);
    }

    public void NullSafeSet(IDbCommand cmd, object value, int index)
    {
      if (value == null)
      {
        if (NullableType.IsDebugEnabled)
          this.Log.Debug((object) ("binding null to parameter: " + (object) index));
        ((IDataParameter) cmd.Parameters[index]).Value = (object) DBNull.Value;
      }
      else
      {
        if (NullableType.IsDebugEnabled)
          this.Log.Debug((object) ("binding '" + this.ToString(value) + "' to parameter: " + (object) index));
        this.Set(cmd, value, index);
      }
    }

    public override sealed object NullSafeGet(
      IDataReader rs,
      string[] names,
      ISessionImplementor session,
      object owner)
    {
      return this.NullSafeGet(rs, names[0]);
    }

    public virtual object NullSafeGet(IDataReader rs, string[] names)
    {
      return this.NullSafeGet(rs, names[0]);
    }

    public virtual object NullSafeGet(IDataReader rs, string name)
    {
      int ordinal = rs.GetOrdinal(name);
      if (rs.IsDBNull(ordinal))
      {
        if (NullableType.IsDebugEnabled)
          this.Log.Debug((object) ("returning null as column: " + name));
        return (object) null;
      }
      object val;
      try
      {
        val = this.Get(rs, ordinal);
      }
      catch (InvalidCastException ex)
      {
        throw new ADOException(string.Format("Could not cast the value in field {0} of type {1} to the Type {2}.  Please check to make sure that the mapping is correct and that your DataProvider supports this Data Type.", (object) name, (object) rs[ordinal].GetType().Name, (object) this.GetType().Name), (Exception) ex);
      }
      if (NullableType.IsDebugEnabled)
        this.Log.Debug((object) ("returning '" + this.ToString(val) + "' as column: " + name));
      return val;
    }

    public override sealed object NullSafeGet(
      IDataReader rs,
      string name,
      ISessionImplementor session,
      object owner)
    {
      return this.NullSafeGet(rs, name);
    }

    public virtual SqlType SqlType => this._sqlType;

    public override sealed SqlType[] SqlTypes(IMapping mapping)
    {
      return new SqlType[1]{ this.SqlType };
    }

    public override sealed int GetColumnSpan(IMapping session) => 1;

    public override bool IsDirty(
      object old,
      object current,
      bool[] checkable,
      ISessionImplementor session)
    {
      return checkable[0] && this.IsDirty(old, current, session);
    }

    public override bool[] ToColumnNullness(object value, IMapping mapping)
    {
      return value != null ? ArrayHelper.True : ArrayHelper.False;
    }

    public override object FromXMLNode(XmlNode xml, IMapping factory)
    {
      return this.FromXMLString(xml.InnerText, factory);
    }

    public override void SetToXMLNode(
      XmlNode xml,
      object value,
      ISessionFactoryImplementor factory)
    {
      xml.InnerText = this.ToXMLString(value, factory);
    }

    public string ToXMLString(object value, ISessionFactoryImplementor pc) => this.ToString(value);

    public object FromXMLString(string xml, IMapping factory)
    {
      return !string.IsNullOrEmpty(xml) ? this.FromStringValue(xml) : (object) null;
    }

    public override bool IsEqual(object x, object y, EntityMode entityMode) => this.IsEqual(x, y);

    public virtual bool IsEqual(object x, object y) => EqualsHelper.Equals(x, y);

    public override bool Equals(object obj)
    {
      return this == obj || obj is NullableType nullableType && this.Name.Equals(nullableType.Name) && this.SqlType.Equals(nullableType.SqlType);
    }

    public override int GetHashCode()
    {
      return this.SqlType.GetHashCode() / 2 + this.Name.GetHashCode() / 2;
    }
  }
}
