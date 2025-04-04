// Decompiled with JetBrains decompiler
// Type: RestSharp.Validation.Require
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System;

#nullable disable
namespace RestSharp.Validation
{
  public class Require
  {
    public static void Argument(string argumentName, object argumentValue)
    {
      if (argumentValue == null)
        throw new ArgumentException("Argument cannot be null.", argumentName);
    }
  }
}
