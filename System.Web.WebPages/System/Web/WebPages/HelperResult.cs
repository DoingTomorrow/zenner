// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.HelperResult
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Globalization;
using System.IO;

#nullable disable
namespace System.Web.WebPages
{
  public class HelperResult : IHtmlString
  {
    private readonly Action<TextWriter> _action;

    public HelperResult(Action<TextWriter> action)
    {
      this._action = action != null ? action : throw new ArgumentNullException(nameof (action));
    }

    public string ToHtmlString() => this.ToString();

    public override string ToString()
    {
      using (StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture))
      {
        this._action((TextWriter) stringWriter);
        return stringWriter.ToString();
      }
    }

    public void WriteTo(TextWriter writer) => this._action(writer);
  }
}
