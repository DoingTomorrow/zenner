// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.TagBuilder
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using Microsoft.Internal.Web.Utils;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web.WebPages.Html;

#nullable disable
namespace System.Web.Mvc
{
  [TypeForwardedFrom("System.Web.Mvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
  public class TagBuilder
  {
    private string _idAttributeDotReplacement;
    private string _innerHtml;

    public TagBuilder(string tagName)
    {
      this.TagName = !string.IsNullOrEmpty(tagName) ? tagName : throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (tagName));
      this.Attributes = (IDictionary<string, string>) new SortedDictionary<string, string>((IComparer<string>) StringComparer.Ordinal);
    }

    public IDictionary<string, string> Attributes { get; private set; }

    public string IdAttributeDotReplacement
    {
      get
      {
        if (string.IsNullOrEmpty(this._idAttributeDotReplacement))
          this._idAttributeDotReplacement = HtmlHelper.IdAttributeDotReplacement;
        return this._idAttributeDotReplacement;
      }
      set => this._idAttributeDotReplacement = value;
    }

    public string InnerHtml
    {
      get => this._innerHtml ?? string.Empty;
      set => this._innerHtml = value;
    }

    public string TagName { get; private set; }

    public void AddCssClass(string value)
    {
      string str;
      if (this.Attributes.TryGetValue("class", out str))
        this.Attributes["class"] = value + " " + str;
      else
        this.Attributes["class"] = value;
    }

    public static string CreateSanitizedId(string originalId)
    {
      return TagBuilder.CreateSanitizedId(originalId, HtmlHelper.IdAttributeDotReplacement);
    }

    public static string CreateSanitizedId(string originalId, string invalidCharReplacement)
    {
      if (string.IsNullOrEmpty(originalId))
        return (string) null;
      if (invalidCharReplacement == null)
        throw new ArgumentNullException(nameof (invalidCharReplacement));
      char c1 = originalId[0];
      if (!TagBuilder.Html401IdUtil.IsLetter(c1))
        return (string) null;
      StringBuilder stringBuilder = new StringBuilder(originalId.Length);
      stringBuilder.Append(c1);
      for (int index = 1; index < originalId.Length; ++index)
      {
        char c2 = originalId[index];
        if (TagBuilder.Html401IdUtil.IsValidIdCharacter(c2))
          stringBuilder.Append(c2);
        else
          stringBuilder.Append(invalidCharReplacement);
      }
      return stringBuilder.ToString();
    }

    public void GenerateId(string name)
    {
      if (this.Attributes.ContainsKey("id"))
        return;
      string sanitizedId = TagBuilder.CreateSanitizedId(name, this.IdAttributeDotReplacement);
      if (string.IsNullOrEmpty(sanitizedId))
        return;
      this.Attributes["id"] = sanitizedId;
    }

    private void AppendAttributes(StringBuilder sb)
    {
      foreach (KeyValuePair<string, string> attribute in (IEnumerable<KeyValuePair<string, string>>) this.Attributes)
      {
        string key = attribute.Key;
        if (!string.Equals(key, "id", StringComparison.Ordinal) || !string.IsNullOrEmpty(attribute.Value))
        {
          string str = HttpUtility.HtmlAttributeEncode(attribute.Value);
          sb.Append(' ').Append(key).Append("=\"").Append(str).Append('"');
        }
      }
    }

    public void MergeAttribute(string key, string value)
    {
      bool replaceExisting = false;
      this.MergeAttribute(key, value, replaceExisting);
    }

    public void MergeAttribute(string key, string value, bool replaceExisting)
    {
      if (string.IsNullOrEmpty(key))
        throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (key));
      if (!replaceExisting && this.Attributes.ContainsKey(key))
        return;
      this.Attributes[key] = value;
    }

    public void MergeAttributes<TKey, TValue>(IDictionary<TKey, TValue> attributes)
    {
      bool replaceExisting = false;
      this.MergeAttributes<TKey, TValue>(attributes, replaceExisting);
    }

    public void MergeAttributes<TKey, TValue>(
      IDictionary<TKey, TValue> attributes,
      bool replaceExisting)
    {
      if (attributes == null)
        return;
      foreach (KeyValuePair<TKey, TValue> attribute in (IEnumerable<KeyValuePair<TKey, TValue>>) attributes)
        this.MergeAttribute(Convert.ToString((object) attribute.Key, (IFormatProvider) CultureInfo.InvariantCulture), Convert.ToString((object) attribute.Value, (IFormatProvider) CultureInfo.InvariantCulture), replaceExisting);
    }

    public void SetInnerText(string innerText)
    {
      this.InnerHtml = HttpUtility.HtmlEncode(innerText);
    }

    internal HtmlString ToHtmlString(TagRenderMode renderMode)
    {
      return new HtmlString(this.ToString(renderMode));
    }

    public override string ToString() => this.ToString(TagRenderMode.Normal);

    public string ToString(TagRenderMode renderMode)
    {
      StringBuilder sb = new StringBuilder();
      switch (renderMode)
      {
        case TagRenderMode.StartTag:
          sb.Append('<').Append(this.TagName);
          this.AppendAttributes(sb);
          sb.Append('>');
          break;
        case TagRenderMode.EndTag:
          sb.Append("</").Append(this.TagName).Append('>');
          break;
        case TagRenderMode.SelfClosing:
          sb.Append('<').Append(this.TagName);
          this.AppendAttributes(sb);
          sb.Append(" />");
          break;
        default:
          sb.Append('<').Append(this.TagName);
          this.AppendAttributes(sb);
          sb.Append('>').Append(this.InnerHtml).Append("</").Append(this.TagName).Append('>');
          break;
      }
      return sb.ToString();
    }

    private static class Html401IdUtil
    {
      private static bool IsAllowableSpecialCharacter(char c)
      {
        switch (c)
        {
          case '-':
          case ':':
          case '_':
            return true;
          default:
            return false;
        }
      }

      private static bool IsDigit(char c) => '0' <= c && c <= '9';

      public static bool IsLetter(char c)
      {
        if ('A' <= c && c <= 'Z')
          return true;
        return 'a' <= c && c <= 'z';
      }

      public static bool IsValidIdCharacter(char c)
      {
        return TagBuilder.Html401IdUtil.IsLetter(c) || TagBuilder.Html401IdUtil.IsDigit(c) || TagBuilder.Html401IdUtil.IsAllowableSpecialCharacter(c);
      }
    }
  }
}
