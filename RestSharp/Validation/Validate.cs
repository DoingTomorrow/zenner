// Decompiled with JetBrains decompiler
// Type: RestSharp.Validation.Validate
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System;

#nullable disable
namespace RestSharp.Validation
{
  public class Validate
  {
    public static void IsBetween(int value, int min, int max)
    {
      if (value < min || value > max)
        throw new ArgumentException(string.Format("Value ({0}) is not between {1} and {2}.", (object) value, (object) min, (object) max));
    }

    public static void IsValidLength(string value, int maxSize)
    {
      if (value != null && value.Length > maxSize)
        throw new ArgumentException(string.Format("String is longer than max allowed size ({0}).", (object) maxSize));
    }
  }
}
