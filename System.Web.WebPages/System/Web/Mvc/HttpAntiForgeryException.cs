// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.HttpAntiForgeryException
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Web.WebPages.Resources;

#nullable disable
namespace System.Web.Mvc
{
  [TypeForwardedFrom("System.Web.Mvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
  [Serializable]
  public sealed class HttpAntiForgeryException : HttpException
  {
    public HttpAntiForgeryException()
    {
    }

    private HttpAntiForgeryException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public HttpAntiForgeryException(string message)
      : base(message)
    {
    }

    private HttpAntiForgeryException(string message, params object[] args)
      : this(string.Format((IFormatProvider) CultureInfo.CurrentCulture, message, args))
    {
    }

    public HttpAntiForgeryException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    internal static HttpAntiForgeryException CreateAdditionalDataCheckFailedException()
    {
      return new HttpAntiForgeryException(WebPageResources.AntiForgeryToken_AdditionalDataCheckFailed);
    }

    internal static HttpAntiForgeryException CreateClaimUidMismatchException()
    {
      return new HttpAntiForgeryException(WebPageResources.AntiForgeryToken_ClaimUidMismatch);
    }

    internal static HttpAntiForgeryException CreateCookieMissingException(string cookieName)
    {
      return new HttpAntiForgeryException(WebPageResources.AntiForgeryToken_CookieMissing, new object[1]
      {
        (object) cookieName
      });
    }

    internal static HttpAntiForgeryException CreateDeserializationFailedException()
    {
      return new HttpAntiForgeryException(WebPageResources.AntiForgeryToken_DeserializationFailed);
    }

    internal static HttpAntiForgeryException CreateFormFieldMissingException(string formFieldName)
    {
      return new HttpAntiForgeryException(WebPageResources.AntiForgeryToken_FormFieldMissing, new object[1]
      {
        (object) formFieldName
      });
    }

    internal static HttpAntiForgeryException CreateSecurityTokenMismatchException()
    {
      return new HttpAntiForgeryException(WebPageResources.AntiForgeryToken_SecurityTokenMismatch);
    }

    internal static HttpAntiForgeryException CreateTokensSwappedException(
      string cookieName,
      string formFieldName)
    {
      return new HttpAntiForgeryException(WebPageResources.AntiForgeryToken_TokensSwapped, new object[2]
      {
        (object) cookieName,
        (object) formFieldName
      });
    }

    internal static HttpAntiForgeryException CreateUsernameMismatchException(
      string usernameInToken,
      string currentUsername)
    {
      return new HttpAntiForgeryException(WebPageResources.AntiForgeryToken_UsernameMismatch, new object[2]
      {
        (object) usernameInToken,
        (object) currentUsername
      });
    }
  }
}
