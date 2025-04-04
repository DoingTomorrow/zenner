// Decompiled with JetBrains decompiler
// Type: NHibernate.NonUniqueObjectException
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
  public class NonUniqueObjectException : HibernateException
  {
    private readonly object identifier;
    private readonly string entityName;

    public NonUniqueObjectException(string message, object id, string entityName)
      : base(message)
    {
      this.entityName = entityName;
      this.identifier = id;
    }

    public NonUniqueObjectException(object id, string entityName)
      : this("a different object with the same identifier value was already associated with the session", id, entityName)
    {
    }

    public object Identifier => this.identifier;

    public override string Message
    {
      get => base.Message + ": " + this.identifier + ", of entity: " + this.entityName;
    }

    public string EntityName => this.entityName;

    protected NonUniqueObjectException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.identifier = info.GetValue(nameof (identifier), typeof (object));
      this.entityName = info.GetValue(nameof (entityName), typeof (string)) as string;
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("identifier", this.identifier, typeof (object));
      info.AddValue("entityName", (object) this.entityName, typeof (string));
    }
  }
}
