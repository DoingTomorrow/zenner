// Decompiled with JetBrains decompiler
// Type: NHibernate.ObjectNotFoundException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate
{
  [Serializable]
  public class ObjectNotFoundException : UnresolvableObjectException
  {
    public ObjectNotFoundException(object identifier, Type type)
      : base(identifier, type)
    {
    }

    public ObjectNotFoundException(object identifier, string entityName)
      : base(identifier, entityName)
    {
    }

    protected ObjectNotFoundException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
