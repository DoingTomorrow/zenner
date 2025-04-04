// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.AllEventPropertiesLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("all-event-properties")]
  [ThreadAgnostic]
  [ThreadSafe]
  public class AllEventPropertiesLayoutRenderer : LayoutRenderer
  {
    private string _format;
    private string _beforeKey;
    private string _afterKey;
    private string _afterValue;
    private static List<string> CallerInformationAttributeNames = new List<string>()
    {
      "CallerMemberName",
      "CallerFilePath",
      "CallerLineNumber"
    };

    public AllEventPropertiesLayoutRenderer()
    {
      this.Separator = ", ";
      this.Format = "[key]=[value]";
    }

    public string Separator { get; set; }

    [DefaultValue(false)]
    public bool IncludeCallerInformation { get; set; }

    public string Format
    {
      get => this._format;
      set
      {
        if (!value.Contains("[key]"))
          throw new ArgumentException("Invalid format: [key] placeholder is missing.");
        this._format = value.Contains("[value]") ? value : throw new ArgumentException("Invalid format: [value] placeholder is missing.");
        string[] strArray = this._format.Split(new string[2]
        {
          "[key]",
          "[value]"
        }, StringSplitOptions.None);
        if (strArray.Length == 3)
        {
          this._beforeKey = strArray[0];
          this._afterKey = strArray[1];
          this._afterValue = strArray[2];
        }
        else
        {
          this._beforeKey = (string) null;
          this._afterKey = (string) null;
          this._afterValue = (string) null;
        }
      }
    }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      if (!logEvent.HasProperties)
        return;
      IFormatProvider formatProvider = this.GetFormatProvider(logEvent);
      bool flag = true;
      foreach (KeyValuePair<object, object> property in (IEnumerable<KeyValuePair<object, object>>) this.GetProperties(logEvent))
      {
        if (!flag)
          builder.Append(this.Separator);
        flag = false;
        if (this._beforeKey == null || this._afterKey == null || this._afterValue == null)
        {
          string newValue1 = Convert.ToString(property.Key, formatProvider);
          string newValue2 = Convert.ToString(property.Value, formatProvider);
          string str = this.Format.Replace("[key]", newValue1).Replace("[value]", newValue2);
          builder.Append(str);
        }
        else
        {
          builder.Append(this._beforeKey);
          builder.AppendFormattedValue(property.Key, (string) null, formatProvider);
          builder.Append(this._afterKey);
          builder.AppendFormattedValue(property.Value, (string) null, formatProvider);
          builder.Append(this._afterValue);
        }
      }
    }

    private IDictionary<object, object> GetProperties(LogEventInfo logEvent)
    {
      if (this.IncludeCallerInformation || logEvent.CallSiteInformation == null)
        return logEvent.Properties;
      foreach (string informationAttributeName in AllEventPropertiesLayoutRenderer.CallerInformationAttributeNames)
      {
        if (logEvent.Properties.ContainsKey((object) informationAttributeName))
          return (IDictionary<object, object>) logEvent.Properties.Where<KeyValuePair<object, object>>((Func<KeyValuePair<object, object>, bool>) (p => !((IEnumerable<object>) AllEventPropertiesLayoutRenderer.CallerInformationAttributeNames).Contains<object>(p.Key))).ToDictionary<KeyValuePair<object, object>, object, object>((Func<KeyValuePair<object, object>, object>) (p => p.Key), (Func<KeyValuePair<object, object>, object>) (p => p.Value));
      }
      return logEvent.Properties;
    }
  }
}
