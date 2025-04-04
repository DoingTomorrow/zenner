// Decompiled with JetBrains decompiler
// Type: NLog.Targets.TargetWithContext
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Internal;
using NLog.Layouts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.Targets
{
  public abstract class TargetWithContext : TargetWithLayout, IIncludeContext
  {
    private TargetWithContext.TargetWithContextLayout _contextLayout;

    public override sealed Layout Layout
    {
      get => (Layout) this._contextLayout;
      set
      {
        if (this._contextLayout != null)
          this._contextLayout.TargetLayout = value;
        else
          this._contextLayout = new TargetWithContext.TargetWithContextLayout(this, value);
      }
    }

    bool IIncludeContext.IncludeAllProperties
    {
      get => this.IncludeEventProperties;
      set => this.IncludeEventProperties = value;
    }

    public bool IncludeEventProperties
    {
      get => this._contextLayout.IncludeAllProperties;
      set => this._contextLayout.IncludeAllProperties = value;
    }

    public bool IncludeMdc
    {
      get => this._contextLayout.IncludeMdc;
      set => this._contextLayout.IncludeMdc = value;
    }

    public bool IncludeNdc
    {
      get => this._contextLayout.IncludeNdc;
      set => this._contextLayout.IncludeNdc = value;
    }

    public bool IncludeMdlc
    {
      get => this._contextLayout.IncludeMdlc;
      set => this._contextLayout.IncludeMdlc = value;
    }

    public bool IncludeNdlc
    {
      get => this._contextLayout.IncludeNdlc;
      set => this._contextLayout.IncludeNdlc = value;
    }

    public bool IncludeGdc { get; set; }

    public bool IncludeCallSite
    {
      get => this._contextLayout.IncludeCallSite;
      set => this._contextLayout.IncludeCallSite = value;
    }

    public bool IncludeCallSiteStackTrace
    {
      get => this._contextLayout.IncludeCallSiteStackTrace;
      set => this._contextLayout.IncludeCallSiteStackTrace = value;
    }

    [ArrayParameter(typeof (TargetPropertyWithContext), "contextproperty")]
    public virtual IList<TargetPropertyWithContext> ContextProperties { get; }

    protected TargetWithContext()
    {
      this._contextLayout = this._contextLayout ?? new TargetWithContext.TargetWithContextLayout(this, base.Layout);
      this.OptimizeBufferReuse = true;
    }

    protected bool ShouldIncludeProperties(LogEventInfo logEvent)
    {
      if (this.IncludeGdc || this.IncludeMdc || this.IncludeMdlc)
        return true;
      return this.IncludeEventProperties && logEvent != null && logEvent.HasProperties;
    }

    protected IDictionary<string, object> GetContextProperties(LogEventInfo logEvent)
    {
      IDictionary<string, object> combinedPropties = (IDictionary<string, object>) null;
      if (this.IncludeGdc)
        combinedPropties = this.CaptureContextGdc(logEvent, (IDictionary<string, object>) null);
      if (this.IncludeMdc && !TargetWithContext.CombineProperties(logEvent, (Layout) this._contextLayout.MdcLayout, ref combinedPropties))
        combinedPropties = this.CaptureContextMdc(logEvent, combinedPropties);
      if (this.IncludeMdlc && !TargetWithContext.CombineProperties(logEvent, (Layout) this._contextLayout.MdlcLayout, ref combinedPropties))
        combinedPropties = this.CaptureContextMdlc(logEvent, combinedPropties);
      if (this.ContextProperties != null && this.ContextProperties.Count > 0)
      {
        combinedPropties = combinedPropties ?? (IDictionary<string, object>) new Dictionary<string, object>();
        for (int index = 0; index < this.ContextProperties.Count; ++index)
        {
          TargetPropertyWithContext contextProperty = this.ContextProperties[index];
          if (!string.IsNullOrEmpty(contextProperty?.Name) && contextProperty.Layout != null)
          {
            string str = this.RenderLogEvent(contextProperty.Layout, logEvent);
            if (contextProperty.IncludeEmptyValue || !string.IsNullOrEmpty(str))
              combinedPropties[contextProperty.Name] = (object) str;
          }
        }
      }
      return combinedPropties;
    }

    protected IDictionary<string, object> GetAllProperties(LogEventInfo logEvent)
    {
      IDictionary<string, object> dictionary = this.GetContextProperties(logEvent);
      if (this.IncludeEventProperties && logEvent.HasProperties)
      {
        dictionary = dictionary ?? (IDictionary<string, object>) new Dictionary<string, object>();
        foreach (KeyValuePair<object, object> property in (IEnumerable<KeyValuePair<object, object>>) logEvent.Properties)
        {
          string key = property.Key.ToString();
          if (!string.IsNullOrEmpty(key))
            dictionary[key] = property.Value;
        }
      }
      return dictionary ?? (IDictionary<string, object>) new Dictionary<string, object>();
    }

    private static bool CombineProperties(
      LogEventInfo logEvent,
      Layout contextLayout,
      ref IDictionary<string, object> combinedPropties)
    {
      object obj;
      if (!logEvent.TryGetCachedLayoutValue(contextLayout, out obj))
        return false;
      if (obj is IDictionary<string, object> dictionary)
      {
        if (combinedPropties != null)
        {
          foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) dictionary)
            combinedPropties[keyValuePair.Key] = keyValuePair.Value;
        }
        else
          combinedPropties = dictionary;
      }
      return true;
    }

    protected IDictionary<string, object> GetContextMdc(LogEventInfo logEvent)
    {
      object obj;
      return logEvent.TryGetCachedLayoutValue((Layout) this._contextLayout.MdcLayout, out obj) ? obj as IDictionary<string, object> : this.CaptureContextMdc(logEvent, (IDictionary<string, object>) null);
    }

    protected IDictionary<string, object> GetContextMdlc(LogEventInfo logEvent)
    {
      object obj;
      return logEvent.TryGetCachedLayoutValue((Layout) this._contextLayout.MdlcLayout, out obj) ? obj as IDictionary<string, object> : this.CaptureContextMdlc(logEvent, (IDictionary<string, object>) null);
    }

    protected IList<object> GetContextNdc(LogEventInfo logEvent)
    {
      object obj;
      return logEvent.TryGetCachedLayoutValue((Layout) this._contextLayout.NdcLayout, out obj) ? obj as IList<object> : this.CaptureContextNdc(logEvent);
    }

    protected IList<object> GetContextNdlc(LogEventInfo logEvent)
    {
      object obj;
      return logEvent.TryGetCachedLayoutValue((Layout) this._contextLayout.NdlcLayout, out obj) ? obj as IList<object> : this.CaptureContextNdlc(logEvent);
    }

    protected virtual IDictionary<string, object> CaptureContextGdc(
      LogEventInfo logEvent,
      IDictionary<string, object> contextProperties)
    {
      ICollection<string> names = GlobalDiagnosticsContext.GetNames();
      if (names.Count == 0)
        return contextProperties;
      contextProperties = contextProperties ?? (IDictionary<string, object>) new Dictionary<string, object>();
      foreach (string str in (IEnumerable<string>) names)
      {
        object serializedValue = GlobalDiagnosticsContext.GetObject(str);
        if (this.SerializeItemValue(logEvent, str, serializedValue, out serializedValue))
          contextProperties[str] = serializedValue;
      }
      return contextProperties;
    }

    protected virtual IDictionary<string, object> CaptureContextMdc(
      LogEventInfo logEvent,
      IDictionary<string, object> contextProperties)
    {
      ICollection<string> names = MappedDiagnosticsContext.GetNames();
      if (names.Count == 0)
        return contextProperties;
      contextProperties = contextProperties ?? (IDictionary<string, object>) new Dictionary<string, object>();
      foreach (string str in (IEnumerable<string>) names)
      {
        object obj = MappedDiagnosticsContext.GetObject(str);
        object serializedValue;
        if (this.SerializeMdcItem(logEvent, str, obj, out serializedValue))
          contextProperties[str] = serializedValue;
      }
      return contextProperties;
    }

    protected virtual bool SerializeMdcItem(
      LogEventInfo logEvent,
      string name,
      object value,
      out object serializedValue)
    {
      if (!string.IsNullOrEmpty(name))
        return this.SerializeItemValue(logEvent, name, value, out serializedValue);
      serializedValue = (object) null;
      return false;
    }

    protected virtual IDictionary<string, object> CaptureContextMdlc(
      LogEventInfo logEvent,
      IDictionary<string, object> contextProperties)
    {
      ICollection<string> names = MappedDiagnosticsLogicalContext.GetNames();
      if (names.Count == 0)
        return contextProperties;
      contextProperties = contextProperties ?? (IDictionary<string, object>) new Dictionary<string, object>();
      foreach (string str in (IEnumerable<string>) names)
      {
        object obj = MappedDiagnosticsLogicalContext.GetObject(str);
        object serializedValue;
        if (this.SerializeMdlcItem(logEvent, str, obj, out serializedValue))
          contextProperties[str] = serializedValue;
      }
      return contextProperties;
    }

    protected virtual bool SerializeMdlcItem(
      LogEventInfo logEvent,
      string name,
      object value,
      out object serializedValue)
    {
      if (!string.IsNullOrEmpty(name))
        return this.SerializeItemValue(logEvent, name, value, out serializedValue);
      serializedValue = (object) null;
      return false;
    }

    protected virtual IList<object> CaptureContextNdc(LogEventInfo logEvent)
    {
      object[] allObjects = NestedDiagnosticsContext.GetAllObjects();
      if (allObjects.Length == 0)
        return (IList<object>) allObjects;
      IList<object> objectList = (IList<object>) null;
      for (int index1 = 0; index1 < allObjects.Length; ++index1)
      {
        object obj = allObjects[index1];
        object serializedValue;
        if (this.SerializeNdcItem(logEvent, obj, out serializedValue))
        {
          if (objectList != null)
            objectList.Add(serializedValue);
          else
            allObjects[index1] = serializedValue;
        }
        else if (objectList == null)
        {
          objectList = (IList<object>) new List<object>(allObjects.Length);
          for (int index2 = 0; index2 < index1; ++index2)
            objectList.Add(allObjects[index2]);
        }
      }
      return objectList ?? (IList<object>) allObjects;
    }

    protected virtual bool SerializeNdcItem(
      LogEventInfo logEvent,
      object value,
      out object serializedValue)
    {
      return this.SerializeItemValue(logEvent, (string) null, value, out serializedValue);
    }

    protected virtual IList<object> CaptureContextNdlc(LogEventInfo logEvent)
    {
      object[] allObjects = NestedDiagnosticsLogicalContext.GetAllObjects();
      if (allObjects.Length == 0)
        return (IList<object>) allObjects;
      IList<object> objectList = (IList<object>) null;
      for (int index1 = 0; index1 < allObjects.Length; ++index1)
      {
        object obj = allObjects[index1];
        object serializedValue;
        if (this.SerializeNdlcItem(logEvent, obj, out serializedValue))
        {
          if (objectList != null)
            objectList.Add(serializedValue);
          else
            allObjects[index1] = serializedValue;
        }
        else if (objectList == null)
        {
          objectList = (IList<object>) new List<object>(allObjects.Length);
          for (int index2 = 0; index2 < index1; ++index2)
            objectList.Add(allObjects[index2]);
        }
      }
      return objectList ?? (IList<object>) allObjects;
    }

    protected virtual bool SerializeNdlcItem(
      LogEventInfo logEvent,
      object value,
      out object serializedValue)
    {
      return this.SerializeItemValue(logEvent, (string) null, value, out serializedValue);
    }

    protected virtual bool SerializeItemValue(
      LogEventInfo logEvent,
      string name,
      object value,
      out object serializedValue)
    {
      if (value == null)
      {
        serializedValue = (object) null;
        return true;
      }
      if (!(value is string) && Convert.GetTypeCode(value) == TypeCode.Object)
      {
        switch (value)
        {
          case Guid _:
          case TimeSpan _:
          case DateTimeOffset _:
            break;
          default:
            serializedValue = (object) Convert.ToString(value, logEvent.FormatProvider ?? (IFormatProvider) CultureInfo.CurrentCulture);
            return true;
        }
      }
      serializedValue = value;
      return true;
    }

    private class TargetWithContextLayout : Layout, IIncludeContext, IUsesStackTrace
    {
      private Layout _targetLayout;

      public Layout TargetLayout
      {
        get => this._targetLayout;
        set => this._targetLayout = this == value ? this._targetLayout : value;
      }

      internal TargetWithContext.TargetWithContextLayout.LayoutContextMdc MdcLayout { get; }

      internal TargetWithContext.TargetWithContextLayout.LayoutContextNdc NdcLayout { get; }

      internal TargetWithContext.TargetWithContextLayout.LayoutContextMdlc MdlcLayout { get; }

      internal TargetWithContext.TargetWithContextLayout.LayoutContextNdlc NdlcLayout { get; }

      public bool IncludeAllProperties { get; set; }

      public bool IncludeCallSite { get; set; }

      public bool IncludeCallSiteStackTrace { get; set; }

      public bool IncludeMdc
      {
        get => this.MdcLayout.IsActive;
        set => this.MdcLayout.IsActive = value;
      }

      public bool IncludeNdc
      {
        get => this.NdcLayout.IsActive;
        set => this.NdcLayout.IsActive = value;
      }

      public bool IncludeMdlc
      {
        get => this.MdlcLayout.IsActive;
        set => this.MdlcLayout.IsActive = value;
      }

      public bool IncludeNdlc
      {
        get => this.NdlcLayout.IsActive;
        set => this.NdlcLayout.IsActive = value;
      }

      StackTraceUsage IUsesStackTrace.StackTraceUsage
      {
        get
        {
          if (this.IncludeCallSiteStackTrace)
            return StackTraceUsage.WithSource;
          return this.IncludeCallSite ? StackTraceUsage.WithoutSource : StackTraceUsage.None;
        }
      }

      public TargetWithContextLayout(TargetWithContext owner, Layout targetLayout)
      {
        this.TargetLayout = targetLayout;
        this.MdcLayout = new TargetWithContext.TargetWithContextLayout.LayoutContextMdc(owner);
        this.NdcLayout = new TargetWithContext.TargetWithContextLayout.LayoutContextNdc(owner);
        this.MdlcLayout = new TargetWithContext.TargetWithContextLayout.LayoutContextMdlc(owner);
        this.NdlcLayout = new TargetWithContext.TargetWithContextLayout.LayoutContextNdlc(owner);
      }

      protected override void InitializeLayout()
      {
        base.InitializeLayout();
        this.ThreadAgnostic = this.IncludeMdc || this.IncludeNdc || this.IncludeMdlc || this.IncludeNdlc;
      }

      public override string ToString() => this.TargetLayout?.ToString() ?? base.ToString();

      public override void Precalculate(LogEventInfo logEvent)
      {
        Layout targetLayout = this.TargetLayout;
        if ((targetLayout != null ? (targetLayout.ThreadAgnostic ? 1 : 0) : 1) == 0)
        {
          this.TargetLayout.Precalculate(logEvent);
          object obj;
          if (logEvent.TryGetCachedLayoutValue(this.TargetLayout, out obj))
            logEvent.AddCachedLayoutValue((Layout) this, obj);
        }
        this.PrecalculateContext(logEvent);
      }

      internal override void PrecalculateBuilder(LogEventInfo logEvent, StringBuilder target)
      {
        Layout targetLayout = this.TargetLayout;
        if ((targetLayout != null ? (targetLayout.ThreadAgnostic ? 1 : 0) : 1) == 0)
        {
          this.TargetLayout.PrecalculateBuilder(logEvent, target);
          object obj;
          if (logEvent.TryGetCachedLayoutValue(this.TargetLayout, out obj))
            logEvent.AddCachedLayoutValue((Layout) this, obj);
        }
        this.PrecalculateContext(logEvent);
      }

      private void PrecalculateContext(LogEventInfo logEvent)
      {
        if (this.IncludeMdc)
          this.MdcLayout.Precalculate(logEvent);
        if (this.IncludeNdc)
          this.NdcLayout.Precalculate(logEvent);
        if (this.IncludeMdlc)
          this.MdlcLayout.Precalculate(logEvent);
        if (!this.IncludeNdlc)
          return;
        this.NdlcLayout.Precalculate(logEvent);
      }

      protected override string GetFormattedMessage(LogEventInfo logEvent)
      {
        return this.TargetLayout?.Render(logEvent) ?? string.Empty;
      }

      protected override void RenderFormattedMessage(LogEventInfo logEvent, StringBuilder target)
      {
        this.TargetLayout?.RenderAppendBuilder(logEvent, target);
      }

      public class LayoutContextMdc : Layout
      {
        private readonly TargetWithContext _owner;

        public bool IsActive { get; set; }

        public LayoutContextMdc(TargetWithContext owner) => this._owner = owner;

        protected override string GetFormattedMessage(LogEventInfo logEvent)
        {
          this.CaptureContext(logEvent);
          return string.Empty;
        }

        public override void Precalculate(LogEventInfo logEvent) => this.CaptureContext(logEvent);

        private void CaptureContext(LogEventInfo logEvent)
        {
          if (!this.IsActive)
            return;
          IDictionary<string, object> dictionary = this._owner.CaptureContextMdc(logEvent, (IDictionary<string, object>) null);
          logEvent.AddCachedLayoutValue((Layout) this, (object) dictionary);
        }
      }

      public class LayoutContextMdlc : Layout
      {
        private readonly TargetWithContext _owner;

        public bool IsActive { get; set; }

        public LayoutContextMdlc(TargetWithContext owner) => this._owner = owner;

        protected override string GetFormattedMessage(LogEventInfo logEvent)
        {
          this.CaptureContext(logEvent);
          return string.Empty;
        }

        public override void Precalculate(LogEventInfo logEvent) => this.CaptureContext(logEvent);

        private void CaptureContext(LogEventInfo logEvent)
        {
          if (!this.IsActive)
            return;
          IDictionary<string, object> dictionary = this._owner.CaptureContextMdlc(logEvent, (IDictionary<string, object>) null);
          logEvent.AddCachedLayoutValue((Layout) this, (object) dictionary);
        }
      }

      public class LayoutContextNdc : Layout
      {
        private readonly TargetWithContext _owner;

        public bool IsActive { get; set; }

        public LayoutContextNdc(TargetWithContext owner) => this._owner = owner;

        protected override string GetFormattedMessage(LogEventInfo logEvent)
        {
          this.CaptureContext(logEvent);
          return string.Empty;
        }

        public override void Precalculate(LogEventInfo logEvent) => this.CaptureContext(logEvent);

        private void CaptureContext(LogEventInfo logEvent)
        {
          if (!this.IsActive)
            return;
          IList<object> objectList = this._owner.CaptureContextNdc(logEvent);
          logEvent.AddCachedLayoutValue((Layout) this, (object) objectList);
        }
      }

      public class LayoutContextNdlc : Layout
      {
        private readonly TargetWithContext _owner;

        public bool IsActive { get; set; }

        public LayoutContextNdlc(TargetWithContext owner) => this._owner = owner;

        protected override string GetFormattedMessage(LogEventInfo logEvent)
        {
          this.CaptureContext(logEvent);
          return string.Empty;
        }

        public override void Precalculate(LogEventInfo logEvent) => this.CaptureContext(logEvent);

        private void CaptureContext(LogEventInfo logEvent)
        {
          if (!this.IsActive)
            return;
          IList<object> objectList = this._owner.CaptureContextNdlc(logEvent);
          logEvent.AddCachedLayoutValue((Layout) this, (object) objectList);
        }
      }
    }
  }
}
