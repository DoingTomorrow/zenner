// Decompiled with JetBrains decompiler
// Type: NHibernate.UnresolvableObjectException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Impl;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

#nullable disable
namespace NHibernate
{
  [Serializable]
  public class UnresolvableObjectException : HibernateException
  {
    private readonly object identifier;
    private readonly Type clazz;
    private readonly string entityName;

    public UnresolvableObjectException(object identifier, Type clazz)
      : this("No row with the given identifier exists", identifier, clazz)
    {
    }

    public UnresolvableObjectException(object identifier, string entityName)
      : this("No row with the given identifier exists", identifier, entityName)
    {
    }

    public UnresolvableObjectException(string message, object identifier, Type clazz)
      : base(message)
    {
      this.identifier = identifier;
      this.clazz = clazz;
    }

    public UnresolvableObjectException(string message, object identifier, string entityName)
      : base(message)
    {
      this.identifier = identifier;
      this.entityName = entityName;
    }

    public object Identifier => this.identifier;

    public override string Message
    {
      get => base.Message + MessageHelper.InfoString(this.EntityName, this.identifier);
    }

    public Type PersistentClass => this.clazz;

    public string EntityName => this.clazz != null ? this.clazz.FullName : this.entityName;

    public static void ThrowIfNull(object o, object id, Type clazz)
    {
      if (o == null)
        throw new UnresolvableObjectException(id, clazz);
    }

    public static void ThrowIfNull(object o, object id, string entityName)
    {
      if (o == null)
        throw new UnresolvableObjectException(id, entityName);
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("identifier", this.identifier);
      info.AddValue("clazz", (object) this.clazz);
      info.AddValue("entityName", (object) this.entityName);
    }

    protected UnresolvableObjectException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.identifier = info.GetValue(nameof (identifier), typeof (object));
      this.clazz = info.GetValue(nameof (clazz), typeof (Type)) as Type;
      this.entityName = info.GetString(nameof (entityName));
    }
  }
}
