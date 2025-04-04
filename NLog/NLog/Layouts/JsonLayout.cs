// Decompiled with JetBrains decompiler
// Type: NLog.Layouts.JsonLayout
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.MessageTemplates;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NLog.Layouts
{
  [Layout("JsonLayout")]
  [NLog.Config.ThreadAgnostic]
  [NLog.Config.ThreadSafe]
  public class JsonLayout : Layout
  {
    private JsonLayout.LimitRecursionJsonConvert _jsonConverter;
    private IValueFormatter _valueFormatter;

    private JsonLayout.LimitRecursionJsonConvert JsonConverter
    {
      get
      {
        return this._jsonConverter ?? (this._jsonConverter = new JsonLayout.LimitRecursionJsonConvert(this.MaxRecursionLimit, ConfigurationItemFactory.Default.JsonConverter));
      }
      set => this._jsonConverter = value;
    }

    private IValueFormatter ValueFormatter
    {
      get
      {
        return this._valueFormatter ?? (this._valueFormatter = ConfigurationItemFactory.Default.ValueFormatter);
      }
      set => this._valueFormatter = value;
    }

    public JsonLayout()
    {
      this.Attributes = (IList<JsonAttribute>) new List<JsonAttribute>();
      this.RenderEmptyObject = true;
      this.IncludeAllProperties = false;
      this.ExcludeProperties = (ISet<string>) new HashSet<string>();
      this.MaxRecursionLimit = 0;
    }

    [ArrayParameter(typeof (JsonAttribute), "attribute")]
    public IList<JsonAttribute> Attributes { get; private set; }

    public bool SuppressSpaces { get; set; }

    public bool RenderEmptyObject { get; set; }

    public bool IncludeMdc { get; set; }

    public bool IncludeMdlc { get; set; }

    public bool IncludeAllProperties { get; set; }

    public ISet<string> ExcludeProperties { get; set; }

    public int MaxRecursionLimit { get; set; }

    protected override void InitializeLayout()
    {
      base.InitializeLayout();
      if (this.IncludeMdc)
        this.ThreadAgnostic = false;
      if (!this.IncludeMdlc)
        return;
      this.ThreadAgnostic = false;
    }

    protected override void CloseLayout()
    {
      this.JsonConverter = (JsonLayout.LimitRecursionJsonConvert) null;
      this.ValueFormatter = (IValueFormatter) null;
      base.CloseLayout();
    }

    internal override void PrecalculateBuilder(LogEventInfo logEvent, StringBuilder target)
    {
      if (this.ThreadAgnostic)
        return;
      this.RenderAppendBuilder(logEvent, target, true);
    }

    protected override void RenderFormattedMessage(LogEventInfo logEvent, StringBuilder target)
    {
      int length = target.Length;
      this.RenderJsonFormattedMessage(logEvent, target);
      if (target.Length != length || !this.RenderEmptyObject)
        return;
      target.Append(this.SuppressSpaces ? "{}" : "{  }");
    }

    protected override string GetFormattedMessage(LogEventInfo logEvent)
    {
      return this.RenderAllocateBuilder(logEvent);
    }

    private void RenderJsonFormattedMessage(LogEventInfo logEvent, StringBuilder sb)
    {
      int length1 = sb.Length;
      for (int index = 0; index < this.Attributes.Count; ++index)
      {
        JsonAttribute attribute = this.Attributes[index];
        int length2 = sb.Length;
        if (!this.RenderAppendJsonPropertyValue(attribute, logEvent, sb, sb.Length == length1))
          sb.Length = length2;
      }
      if (this.IncludeMdc)
      {
        foreach (string name in (IEnumerable<string>) MappedDiagnosticsContext.GetNames())
        {
          if (!string.IsNullOrEmpty(name))
          {
            object propertyValue = MappedDiagnosticsContext.GetObject(name);
            this.AppendJsonPropertyValue(name, propertyValue, (string) null, (IFormatProvider) null, CaptureType.Unknown, sb, sb.Length == length1);
          }
        }
      }
      if (this.IncludeMdlc)
      {
        foreach (string name in (IEnumerable<string>) MappedDiagnosticsLogicalContext.GetNames())
        {
          if (!string.IsNullOrEmpty(name))
          {
            object propertyValue = MappedDiagnosticsLogicalContext.GetObject(name);
            this.AppendJsonPropertyValue(name, propertyValue, (string) null, (IFormatProvider) null, CaptureType.Unknown, sb, sb.Length == length1);
          }
        }
      }
      if (this.IncludeAllProperties && logEvent.HasProperties)
      {
        foreach (MessageTemplateParameter templateParameter in (IEnumerable<MessageTemplateParameter>) logEvent.CreateOrUpdatePropertiesInternal())
        {
          if (!string.IsNullOrEmpty(templateParameter.Name) && !this.ExcludeProperties.Contains(templateParameter.Name))
            this.AppendJsonPropertyValue(templateParameter.Name, templateParameter.Value, templateParameter.Format, logEvent.FormatProvider, templateParameter.CaptureType, sb, sb.Length == length1);
        }
      }
      if (sb.Length <= length1)
        return;
      this.CompleteJsonMessage(sb);
    }

    private void BeginJsonProperty(StringBuilder sb, string propName, bool beginJsonMessage)
    {
      if (beginJsonMessage)
      {
        sb.Append(this.SuppressSpaces ? "{" : "{ ");
      }
      else
      {
        sb.Append(',');
        if (!this.SuppressSpaces)
          sb.Append(' ');
      }
      sb.Append('"');
      sb.Append(propName);
      sb.Append('"');
      sb.Append(':');
      if (this.SuppressSpaces)
        return;
      sb.Append(' ');
    }

    private void CompleteJsonMessage(StringBuilder sb)
    {
      sb.Append(this.SuppressSpaces ? "}" : " }");
    }

    private void AppendJsonPropertyValue(
      string propName,
      object propertyValue,
      string format,
      IFormatProvider formatProvider,
      CaptureType captureType,
      StringBuilder sb,
      bool beginJsonMessage)
    {
      this.BeginJsonProperty(sb, propName, beginJsonMessage);
      if (this.MaxRecursionLimit <= 1 && captureType == CaptureType.Serialize)
        this.JsonConverter.SerializeObjectNoLimit(propertyValue, sb);
      else if (this.MaxRecursionLimit <= 1 && captureType == CaptureType.Stringify)
      {
        int length = sb.Length;
        this.ValueFormatter.FormatValue(propertyValue, format, captureType, formatProvider, sb);
        JsonLayout.PerformJsonEscapeIfNeeded(sb, length);
      }
      else
        this.JsonConverter.SerializeObject(propertyValue, sb);
    }

    private static void PerformJsonEscapeIfNeeded(StringBuilder sb, int valueStart)
    {
      if (sb.Length - valueStart <= 2)
        return;
      for (int index = valueStart + 1; index < sb.Length - 1; ++index)
      {
        if (DefaultJsonSerializer.RequiresJsonEscape(sb[index], false))
        {
          string str = DefaultJsonSerializer.EscapeString(sb.ToString(valueStart + 1, sb.Length - valueStart - 2), false);
          sb.Length = valueStart;
          sb.Append('"');
          sb.Append(str);
          sb.Append('"');
          break;
        }
      }
    }

    private bool RenderAppendJsonPropertyValue(
      JsonAttribute attrib,
      LogEventInfo logEvent,
      StringBuilder sb,
      bool beginJsonMessage)
    {
      this.BeginJsonProperty(sb, attrib.Name, beginJsonMessage);
      if (attrib.Encode)
        sb.Append('"');
      int length = sb.Length;
      attrib.LayoutWrapper.RenderAppendBuilder(logEvent, sb);
      if (!attrib.IncludeEmptyValue && length == sb.Length)
        return false;
      if (attrib.Encode)
        sb.Append('"');
      return true;
    }

    public override string ToString()
    {
      return this.ToStringWithNestedItems<JsonAttribute>(this.Attributes, (Func<JsonAttribute, string>) (a => a.Name + "-" + a.Layout?.ToString()));
    }

    private class LimitRecursionJsonConvert : IJsonConverter
    {
      private readonly IJsonConverter _converter;
      private readonly DefaultJsonSerializer _serializer;
      private readonly JsonSerializeOptions _serializerOptions;

      public LimitRecursionJsonConvert(int maxRecursionLimit, IJsonConverter converter)
      {
        this._converter = converter;
        this._serializer = converter as DefaultJsonSerializer;
        this._serializerOptions = new JsonSerializeOptions()
        {
          MaxRecursionLimit = Math.Max(0, maxRecursionLimit)
        };
      }

      public bool SerializeObject(object value, StringBuilder builder)
      {
        return this._serializer != null ? this._serializer.SerializeObject(value, builder, this._serializerOptions) : this._converter.SerializeObject(value, builder);
      }

      public bool SerializeObjectNoLimit(object value, StringBuilder builder)
      {
        return this._converter.SerializeObject(value, builder);
      }
    }
  }
}
