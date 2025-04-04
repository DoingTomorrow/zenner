// Decompiled with JetBrains decompiler
// Type: RestSharp.Authenticators.OAuth.Extensions.TimeExtensions
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System;

#nullable disable
namespace RestSharp.Authenticators.OAuth.Extensions
{
  internal static class TimeExtensions
  {
    public static DateTime FromNow(this TimeSpan value)
    {
      return new DateTime((DateTime.Now + value).Ticks);
    }

    public static DateTime FromUnixTime(this long seconds)
    {
      DateTime dateTime = new DateTime(1970, 1, 1);
      dateTime = dateTime.AddSeconds((double) seconds);
      return dateTime.ToLocalTime();
    }

    public static long ToUnixTime(this DateTime dateTime)
    {
      return (long) (dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
    }
  }
}
