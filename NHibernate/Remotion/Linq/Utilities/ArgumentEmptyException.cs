// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Utilities.ArgumentEmptyException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using JetBrains.Annotations;
using System;
using System.Runtime.Serialization;

#nullable disable
namespace Remotion.Linq.Utilities
{
  [Serializable]
  public class ArgumentEmptyException : ArgumentException
  {
    public ArgumentEmptyException([InvokerParameterName] string paramName)
      : base(ArgumentEmptyException.FormatMessage(paramName), paramName)
    {
    }

    public ArgumentEmptyException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    private static string FormatMessage(string paramName)
    {
      return string.Format("Parameter '{0}' cannot be empty.", (object) paramName);
    }
  }
}
