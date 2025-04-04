// Decompiled with JetBrains decompiler
// Type: NHibernate.StaleObjectStateException
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
  public class StaleObjectStateException : StaleStateException
  {
    private readonly string entityName;
    private readonly object identifier;

    public StaleObjectStateException(string entityName, object identifier)
      : base("Row was updated or deleted by another transaction (or unsaved-value mapping was incorrect)")
    {
      this.entityName = entityName;
      this.identifier = identifier;
    }

    public string EntityName => this.entityName;

    public object Identifier => this.identifier;

    public override string Message
    {
      get => base.Message + ": " + MessageHelper.InfoString(this.entityName, this.identifier);
    }

    protected StaleObjectStateException(SerializationInfo info, StreamingContext context)
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
