// Decompiled with JetBrains decompiler
// Type: RestSharp.Authenticators.OAuth.OAuthTools
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using RestSharp.Authenticators.OAuth.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace RestSharp.Authenticators.OAuth
{
  [Serializable]
  internal static class OAuthTools
  {
    private const string AlphaNumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
    private const string Digit = "1234567890";
    private const string Lower = "abcdefghijklmnopqrstuvwxyz";
    private const string Unreserved = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-._~";
    private const string Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static readonly Random _random;
    private static readonly object _randomLock = new object();
    private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();
    private static readonly Encoding _encoding = Encoding.UTF8;
    private static readonly string[] UriRfc3986CharsToEscape = new string[5]
    {
      "!",
      "*",
      "'",
      "(",
      ")"
    };
    private static readonly string[] UriRfc3968EscapedHex = new string[5]
    {
      "%21",
      "%2A",
      "%27",
      "%28",
      "%29"
    };

    static OAuthTools()
    {
      byte[] data = new byte[4];
      OAuthTools._rng.GetNonZeroBytes(data);
      OAuthTools._random = new Random(BitConverter.ToInt32(data, 0));
    }

    public static string GetNonce()
    {
      char[] chArray = new char[16];
      lock (OAuthTools._randomLock)
      {
        for (int index = 0; index < chArray.Length; ++index)
          chArray[index] = "abcdefghijklmnopqrstuvwxyz1234567890"[OAuthTools._random.Next(0, "abcdefghijklmnopqrstuvwxyz1234567890".Length)];
      }
      return new string(chArray);
    }

    public static string GetTimestamp() => OAuthTools.GetTimestamp(DateTime.UtcNow);

    public static string GetTimestamp(DateTime dateTime) => dateTime.ToUnixTime().ToString();

    public static string UrlEncodeRelaxed(string value)
    {
      StringBuilder stringBuilder = new StringBuilder(Uri.EscapeDataString(value));
      for (int index = 0; index < OAuthTools.UriRfc3986CharsToEscape.Length; ++index)
      {
        string oldValue = OAuthTools.UriRfc3986CharsToEscape[index];
        stringBuilder.Replace(oldValue, OAuthTools.UriRfc3968EscapedHex[index]);
      }
      return stringBuilder.ToString();
    }

    public static string UrlEncodeStrict(string value)
    {
      return value.Where<char>((Func<char, bool>) (c => !"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-._~".Contains<char>(c) && c != '%')).Aggregate<char, string>(value, (Func<string, char, string>) ((current, c) => current.Replace(c.ToString(), c.ToString().PercentEncode()))).Replace("%%", "%25%");
    }

    public static string NormalizeRequestParameters(WebParameterCollection parameters)
    {
      return OAuthTools.SortParametersExcludingSignature(parameters).Concatenate("=", "&");
    }

    public static WebParameterCollection SortParametersExcludingSignature(
      WebParameterCollection parameters)
    {
      WebParameterCollection parameterCollection = new WebParameterCollection((IEnumerable<WebPair>) parameters);
      IEnumerable<WebPair> parameters1 = parameterCollection.Where<WebPair>((Func<WebPair, bool>) (n => n.Name.EqualsIgnoreCase("oauth_signature")));
      parameterCollection.RemoveAll(parameters1);
      parameterCollection.ForEach<WebPair>((Action<WebPair>) (p =>
      {
        p.Name = OAuthTools.UrlEncodeStrict(p.Name);
        p.Value = OAuthTools.UrlEncodeStrict(p.Value);
      }));
      parameterCollection.Sort((Comparison<WebPair>) ((x, y) => string.CompareOrdinal(x.Name, y.Name) == 0 ? string.CompareOrdinal(x.Value, y.Value) : string.CompareOrdinal(x.Name, y.Name)));
      return parameterCollection;
    }

    public static string ConstructRequestUrl(Uri url)
    {
      if (url == (Uri) null)
        throw new ArgumentNullException(nameof (url));
      StringBuilder stringBuilder = new StringBuilder();
      string str1 = "{0}://{1}".FormatWith((object) url.Scheme, (object) url.Host);
      string str2 = ":{0}".FormatWith((object) url.Port);
      bool flag1 = url.Scheme == "http" && url.Port == 80;
      bool flag2 = url.Scheme == "https" && url.Port == 443;
      stringBuilder.Append(str1);
      stringBuilder.Append(flag1 || flag2 ? "" : str2);
      stringBuilder.Append(url.AbsolutePath);
      return stringBuilder.ToString();
    }

    public static string ConcatenateRequestElements(
      string method,
      string url,
      WebParameterCollection parameters)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string str1 = method.ToUpper().Then("&");
      string str2 = OAuthTools.UrlEncodeRelaxed(OAuthTools.ConstructRequestUrl(url.AsUri())).Then("&");
      string str3 = OAuthTools.UrlEncodeRelaxed(OAuthTools.NormalizeRequestParameters(parameters));
      stringBuilder.Append(str1);
      stringBuilder.Append(str2);
      stringBuilder.Append(str3);
      return stringBuilder.ToString();
    }

    public static string GetSignature(
      OAuthSignatureMethod signatureMethod,
      string signatureBase,
      string consumerSecret)
    {
      return OAuthTools.GetSignature(signatureMethod, OAuthSignatureTreatment.Escaped, signatureBase, consumerSecret, (string) null);
    }

    public static string GetSignature(
      OAuthSignatureMethod signatureMethod,
      OAuthSignatureTreatment signatureTreatment,
      string signatureBase,
      string consumerSecret)
    {
      return OAuthTools.GetSignature(signatureMethod, signatureTreatment, signatureBase, consumerSecret, (string) null);
    }

    public static string GetSignature(
      OAuthSignatureMethod signatureMethod,
      string signatureBase,
      string consumerSecret,
      string tokenSecret)
    {
      return OAuthTools.GetSignature(signatureMethod, OAuthSignatureTreatment.Escaped, consumerSecret, tokenSecret);
    }

    public static string GetSignature(
      OAuthSignatureMethod signatureMethod,
      OAuthSignatureTreatment signatureTreatment,
      string signatureBase,
      string consumerSecret,
      string tokenSecret)
    {
      if (tokenSecret.IsNullOrBlank())
        tokenSecret = string.Empty;
      consumerSecret = OAuthTools.UrlEncodeRelaxed(consumerSecret);
      tokenSecret = OAuthTools.UrlEncodeRelaxed(tokenSecret);
      string str;
      switch (signatureMethod)
      {
        case OAuthSignatureMethod.HmacSha1:
          HMACSHA1 algorithm = new HMACSHA1();
          string s = "{0}&{1}".FormatWith((object) consumerSecret, (object) tokenSecret);
          algorithm.Key = OAuthTools._encoding.GetBytes(s);
          str = signatureBase.HashWith((HashAlgorithm) algorithm);
          break;
        case OAuthSignatureMethod.PlainText:
          str = "{0}&{1}".FormatWith((object) consumerSecret, (object) tokenSecret);
          break;
        default:
          throw new NotImplementedException("Only HMAC-SHA1 is currently supported.");
      }
      return signatureTreatment == OAuthSignatureTreatment.Escaped ? OAuthTools.UrlEncodeRelaxed(str) : str;
    }
  }
}
