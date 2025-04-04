// Decompiled with JetBrains decompiler
// Type: NHibernate.WrongClassException
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
  public class WrongClassException : HibernateException, ISerializable
  {
    private readonly object identifier;
    private readonly string entityName;

    public WrongClassException(string message, object identifier, string entityName)
      : base(message)
    {
      this.identifier = identifier;
      this.entityName = entityName;
    }

    public object Identifier => this.identifier;

    public string EntityName => this.entityName;

    public override string Message
    {
      get
      {
        return string.Format("Object with id: {0} was not of the specified subclass: {1} ({2})", this.identifier, (object) this.entityName, (object) base.Message);
      }
    }

    protected WrongClassException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.entityName = info.GetValue(nameof (entityName), typeof (string)) as string;
      this.identifier = info.GetValue(nameof (identifier), typeof (object));
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("entityName", (object) this.entityName, typeof (string));
      info.AddValue("identifier", this.identifier, typeof (object));
    }
  }
}
