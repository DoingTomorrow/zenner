// Decompiled with JetBrains decompiler
// Type: NHibernate.PropertyValueException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

#nullable disable
namespace NHibernate
{
  [Serializable]
  public class PropertyValueException : HibernateException
  {
    private readonly string entityName;
    private readonly string propertyName;

    public PropertyValueException(string message, string entityName, string propertyName)
      : base(message)
    {
      this.entityName = entityName;
      this.propertyName = propertyName;
    }

    public PropertyValueException(
      string message,
      string entityName,
      string propertyName,
      Exception innerException)
      : base(message, innerException)
    {
      this.entityName = entityName;
      this.propertyName = propertyName;
    }

    public string EntityName => this.entityName;

    public string PropertyName => this.propertyName;

    public override string Message
    {
      get => base.Message + " " + StringHelper.Qualify(this.entityName, this.propertyName);
    }

    protected PropertyValueException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.entityName = info.GetValue(nameof (entityName), typeof (string)) as string;
      this.propertyName = info.GetString(nameof (propertyName));
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("entityName", (object) this.entityName);
      info.AddValue("propertyName", (object) this.propertyName);
    }
  }
}
