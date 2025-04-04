// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Ajax.AjaxOptions
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Globalization;
using System.Text;

#nullable disable
namespace System.Web.Mvc.Ajax
{
  public class AjaxOptions
  {
    private string _confirm;
    private string _httpMethod;
    private InsertionMode _insertionMode;
    private string _loadingElementId;
    private string _onBegin;
    private string _onComplete;
    private string _onFailure;
    private string _onSuccess;
    private string _updateTargetId;
    private string _url;

    public string Confirm
    {
      get => this._confirm ?? string.Empty;
      set => this._confirm = value;
    }

    public string HttpMethod
    {
      get => this._httpMethod ?? string.Empty;
      set => this._httpMethod = value;
    }

    public InsertionMode InsertionMode
    {
      get => this._insertionMode;
      set
      {
        switch (value)
        {
          case InsertionMode.Replace:
          case InsertionMode.InsertBefore:
          case InsertionMode.InsertAfter:
            this._insertionMode = value;
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof (value));
        }
      }
    }

    internal string InsertionModeString
    {
      get
      {
        switch (this.InsertionMode)
        {
          case InsertionMode.Replace:
            return "Sys.Mvc.InsertionMode.replace";
          case InsertionMode.InsertBefore:
            return "Sys.Mvc.InsertionMode.insertBefore";
          case InsertionMode.InsertAfter:
            return "Sys.Mvc.InsertionMode.insertAfter";
          default:
            return ((int) this.InsertionMode).ToString((IFormatProvider) CultureInfo.InvariantCulture);
        }
      }
    }

    internal string InsertionModeUnobtrusive
    {
      get
      {
        switch (this.InsertionMode)
        {
          case InsertionMode.Replace:
            return "replace";
          case InsertionMode.InsertBefore:
            return "before";
          case InsertionMode.InsertAfter:
            return "after";
          default:
            return ((int) this.InsertionMode).ToString((IFormatProvider) CultureInfo.InvariantCulture);
        }
      }
    }

    public int LoadingElementDuration { get; set; }

    public string LoadingElementId
    {
      get => this._loadingElementId ?? string.Empty;
      set => this._loadingElementId = value;
    }

    public string OnBegin
    {
      get => this._onBegin ?? string.Empty;
      set => this._onBegin = value;
    }

    public string OnComplete
    {
      get => this._onComplete ?? string.Empty;
      set => this._onComplete = value;
    }

    public string OnFailure
    {
      get => this._onFailure ?? string.Empty;
      set => this._onFailure = value;
    }

    public string OnSuccess
    {
      get => this._onSuccess ?? string.Empty;
      set => this._onSuccess = value;
    }

    public string UpdateTargetId
    {
      get => this._updateTargetId ?? string.Empty;
      set => this._updateTargetId = value;
    }

    public string Url
    {
      get => this._url ?? string.Empty;
      set => this._url = value;
    }

    internal string ToJavascriptString()
    {
      StringBuilder stringBuilder = new StringBuilder("{");
      stringBuilder.Append(string.Format((IFormatProvider) CultureInfo.InvariantCulture, " insertionMode: {0},", new object[1]
      {
        (object) this.InsertionModeString
      }));
      stringBuilder.Append(AjaxOptions.PropertyStringIfSpecified("confirm", this.Confirm));
      stringBuilder.Append(AjaxOptions.PropertyStringIfSpecified("httpMethod", this.HttpMethod));
      stringBuilder.Append(AjaxOptions.PropertyStringIfSpecified("loadingElementId", this.LoadingElementId));
      stringBuilder.Append(AjaxOptions.PropertyStringIfSpecified("updateTargetId", this.UpdateTargetId));
      stringBuilder.Append(AjaxOptions.PropertyStringIfSpecified("url", this.Url));
      stringBuilder.Append(AjaxOptions.EventStringIfSpecified("onBegin", this.OnBegin));
      stringBuilder.Append(AjaxOptions.EventStringIfSpecified("onComplete", this.OnComplete));
      stringBuilder.Append(AjaxOptions.EventStringIfSpecified("onFailure", this.OnFailure));
      stringBuilder.Append(AjaxOptions.EventStringIfSpecified("onSuccess", this.OnSuccess));
      --stringBuilder.Length;
      stringBuilder.Append(" }");
      return stringBuilder.ToString();
    }

    public IDictionary<string, object> ToUnobtrusiveHtmlAttributes()
    {
      Dictionary<string, object> unobtrusiveHtmlAttributes = new Dictionary<string, object>()
      {
        {
          "data-ajax",
          (object) "true"
        }
      };
      AjaxOptions.AddToDictionaryIfSpecified((IDictionary<string, object>) unobtrusiveHtmlAttributes, "data-ajax-url", this.Url);
      AjaxOptions.AddToDictionaryIfSpecified((IDictionary<string, object>) unobtrusiveHtmlAttributes, "data-ajax-method", this.HttpMethod);
      AjaxOptions.AddToDictionaryIfSpecified((IDictionary<string, object>) unobtrusiveHtmlAttributes, "data-ajax-confirm", this.Confirm);
      AjaxOptions.AddToDictionaryIfSpecified((IDictionary<string, object>) unobtrusiveHtmlAttributes, "data-ajax-begin", this.OnBegin);
      AjaxOptions.AddToDictionaryIfSpecified((IDictionary<string, object>) unobtrusiveHtmlAttributes, "data-ajax-complete", this.OnComplete);
      AjaxOptions.AddToDictionaryIfSpecified((IDictionary<string, object>) unobtrusiveHtmlAttributes, "data-ajax-failure", this.OnFailure);
      AjaxOptions.AddToDictionaryIfSpecified((IDictionary<string, object>) unobtrusiveHtmlAttributes, "data-ajax-success", this.OnSuccess);
      if (!string.IsNullOrWhiteSpace(this.LoadingElementId))
      {
        unobtrusiveHtmlAttributes.Add("data-ajax-loading", (object) ("#" + this.LoadingElementId));
        if (this.LoadingElementDuration > 0)
          unobtrusiveHtmlAttributes.Add("data-ajax-loading-duration", (object) this.LoadingElementDuration);
      }
      if (!string.IsNullOrWhiteSpace(this.UpdateTargetId))
      {
        unobtrusiveHtmlAttributes.Add("data-ajax-update", (object) ("#" + this.UpdateTargetId));
        unobtrusiveHtmlAttributes.Add("data-ajax-mode", (object) this.InsertionModeUnobtrusive);
      }
      return (IDictionary<string, object>) unobtrusiveHtmlAttributes;
    }

    private static void AddToDictionaryIfSpecified(
      IDictionary<string, object> dictionary,
      string name,
      string value)
    {
      if (string.IsNullOrWhiteSpace(value))
        return;
      dictionary.Add(name, (object) value);
    }

    private static string EventStringIfSpecified(string propertyName, string handler)
    {
      if (string.IsNullOrEmpty(handler))
        return string.Empty;
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, " {0}: Function.createDelegate(this, {1}),", new object[2]
      {
        (object) propertyName,
        (object) handler.ToString()
      });
    }

    private static string PropertyStringIfSpecified(string propertyName, string propertyValue)
    {
      if (string.IsNullOrEmpty(propertyValue))
        return string.Empty;
      string str = propertyValue.Replace("'", "\\'");
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, " {0}: '{1}',", new object[2]
      {
        (object) propertyName,
        (object) str
      });
    }
  }
}
