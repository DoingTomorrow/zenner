// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Utilities.ArgumentTypeException
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
  public class ArgumentTypeException : ArgumentException
  {
    public readonly Type ExpectedType;
    public readonly Type ActualType;

    public ArgumentTypeException(
      string message,
      [InvokerParameterName] string argumentName,
      Type expectedType,
      Type actualType)
      : base(message, argumentName)
    {
      this.ExpectedType = expectedType;
      this.ActualType = actualType;
    }

    public ArgumentTypeException(string argumentName, Type expectedType, Type actualType)
      : this(ArgumentTypeException.FormatMessage(argumentName, expectedType, actualType), argumentName, actualType, expectedType)
    {
    }

    public ArgumentTypeException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.ExpectedType = (Type) info.GetValue(nameof (ExpectedType), typeof (Type));
      this.ActualType = (Type) info.GetValue(nameof (ActualType), typeof (Type));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("ExpectedType", (object) this.ExpectedType);
      info.AddValue("ActualType", (object) this.ActualType);
    }

    private static string FormatMessage(string argumentName, Type expectedType, Type actualType)
    {
      string str = actualType != null ? actualType.ToString() : "<null>";
      return expectedType == null ? string.Format("Argument {0} has unexpected type {1}", (object) argumentName, (object) str) : string.Format("Argument {0} has type {2} when type {1} was expected.", (object) argumentName, (object) expectedType, (object) str);
    }
  }
}
