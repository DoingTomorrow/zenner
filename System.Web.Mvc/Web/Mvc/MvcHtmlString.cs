// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.MvcHtmlString
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public sealed class MvcHtmlString : HtmlString
  {
    public static readonly MvcHtmlString Empty = MvcHtmlString.Create(string.Empty);
    private readonly string _value;

    public MvcHtmlString(string value)
      : base(value ?? string.Empty)
    {
      this._value = value ?? string.Empty;
    }

    public static MvcHtmlString Create(string value) => new MvcHtmlString(value);

    public static bool IsNullOrEmpty(MvcHtmlString value)
    {
      return value == null || value._value.Length == 0;
    }
  }
}
