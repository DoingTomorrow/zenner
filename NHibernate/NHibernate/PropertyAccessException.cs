// Decompiled with JetBrains decompiler
// Type: NHibernate.PropertyAccessException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

#nullable disable
namespace NHibernate
{
  [Serializable]
  public class PropertyAccessException : HibernateException, ISerializable
  {
    private readonly Type persistentType;
    private readonly string propertyName;
    private readonly bool wasSetter;

    public PropertyAccessException(
      Exception innerException,
      string message,
      bool wasSetter,
      Type persistentType,
      string propertyName)
      : base(message, innerException)
    {
      this.persistentType = persistentType;
      this.wasSetter = wasSetter;
      this.propertyName = propertyName;
    }

    public PropertyAccessException(
      Exception innerException,
      string message,
      bool wasSetter,
      Type persistentType)
      : base(message, innerException)
    {
      this.persistentType = persistentType;
      this.wasSetter = wasSetter;
    }

    public Type PersistentType => this.persistentType;

    public override string Message
    {
      get
      {
        return base.Message + (this.wasSetter ? " setter of " : " getter of ") + (this.persistentType == null ? "UnknownType" : this.persistentType.FullName) + (string.IsNullOrEmpty(this.propertyName) ? string.Empty : "." + this.propertyName);
      }
    }

    protected PropertyAccessException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.persistentType = info.GetValue(nameof (persistentType), typeof (Type)) as Type;
      this.propertyName = info.GetString(nameof (propertyName));
      this.wasSetter = info.GetBoolean(nameof (wasSetter));
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("persistentType", (object) this.persistentType, typeof (Type));
      info.AddValue("propertyName", (object) this.propertyName);
      info.AddValue("wasSetter", this.wasSetter);
    }
  }
}
