// Decompiled with JetBrains decompiler
// Type: NHibernate.InvalidProxyTypeException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

#nullable disable
namespace NHibernate
{
  [Serializable]
  public class InvalidProxyTypeException : MappingException
  {
    public InvalidProxyTypeException(ICollection<string> errors)
      : base(InvalidProxyTypeException.FormatMessage((IEnumerable<string>) errors))
    {
      this.Errors = errors;
    }

    public ICollection<string> Errors { get; private set; }

    private static string FormatMessage(IEnumerable<string> errors)
    {
      StringBuilder stringBuilder = new StringBuilder("The following types may not be used as proxies:");
      foreach (string error in errors)
        stringBuilder.Append('\n').Append(error);
      return stringBuilder.ToString();
    }

    public InvalidProxyTypeException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.Errors = (ICollection<string>) info.GetValue("errors", typeof (ICollection));
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("errors", (object) this.Errors, typeof (ICollection));
    }
  }
}
