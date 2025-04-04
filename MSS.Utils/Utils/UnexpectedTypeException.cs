// Decompiled with JetBrains decompiler
// Type: MSS.Utils.Utils.UnexpectedTypeException
// Assembly: MSS.Utils, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8365EDE-890D-4A42-AEA4-3B8FCE5E7B93
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Utils.dll

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

#nullable disable
namespace MSS.Utils.Utils
{
  public class UnexpectedTypeException : Exception
  {
    public UnexpectedTypeException()
    {
    }

    public UnexpectedTypeException(string message)
      : base(message)
    {
    }

    public UnexpectedTypeException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected UnexpectedTypeException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public UnexpectedTypeException(string message, object fromTarget, Type toType)
      : this(message, fromTarget.IfNotNull<object, Type>((Func<object, Type>) (_ => _.GetType()), typeof (void)), toType)
    {
    }

    public UnexpectedTypeException(string message, Type fromType, Type toType)
      : base(message)
    {
      this.FromType = fromType;
      this.ToType = toType;
    }

    public Type FromType { get; private set; }

    public Type ToType { get; private set; }

    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
    }
  }
}
