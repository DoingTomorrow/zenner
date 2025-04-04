// Decompiled with JetBrains decompiler
// Type: NHibernate.InstantiationException
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
  public class InstantiationException : HibernateException
  {
    private readonly Type type;

    public InstantiationException(string message, Type type)
      : base(message)
    {
      this.type = type != null ? type : throw new ArgumentNullException(nameof (type));
    }

    public InstantiationException(string message, Exception innerException, Type type)
      : base(message, innerException)
    {
      this.type = type != null ? type : throw new ArgumentNullException(nameof (type));
    }

    public Type PersistentType => this.type;

    public override string Message => base.Message + (this.type == null ? "" : this.type.FullName);

    protected InstantiationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.type = info.GetValue(nameof (type), typeof (Type)) as Type;
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("type", (object) this.type, typeof (Type));
    }
  }
}
