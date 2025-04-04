// Decompiled with JetBrains decompiler
// Type: RestSharp.Authenticators.OAuth.Extensions.OAuthExtensions
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace RestSharp.Authenticators.OAuth.Extensions
{
  internal static class OAuthExtensions
  {
    public static string ToRequestValue(this OAuthSignatureMethod signatureMethod)
    {
      string upper = signatureMethod.ToString().ToUpper();
      int startIndex = upper.IndexOf("SHA1");
      return startIndex <= -1 ? upper : upper.Insert(startIndex, "-");
    }

    public static OAuthSignatureMethod FromRequestValue(this string signatureMethod)
    {
      switch (signatureMethod)
      {
        case "HMAC-SHA1":
          return OAuthSignatureMethod.HmacSha1;
        case "RSA-SHA1":
          return OAuthSignatureMethod.RsaSha1;
        default:
          return OAuthSignatureMethod.PlainText;
      }
    }

    public static string HashWith(this string input, HashAlgorithm algorithm)
    {
      byte[] bytes = Encoding.UTF8.GetBytes(input);
      return Convert.ToBase64String(algorithm.ComputeHash(bytes));
    }
  }
}
