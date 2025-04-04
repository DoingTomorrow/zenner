// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.ProcessInfoLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("processinfo")]
  [ThreadSafe]
  public class ProcessInfoLayoutRenderer : LayoutRenderer
  {
    private Process _process;
    private PropertyInfo _propertyInfo;
    private ReflectionHelpers.LateBoundMethod _lateBoundPropertyGet;

    public ProcessInfoLayoutRenderer() => this.Property = ProcessInfoProperty.Id;

    [DefaultValue("Id")]
    [DefaultParameter]
    public ProcessInfoProperty Property { get; set; }

    [DefaultValue(null)]
    public string Format { get; set; }

    protected override void InitializeLayoutRenderer()
    {
      base.InitializeLayoutRenderer();
      this._propertyInfo = typeof (Process).GetProperty(this.Property.ToString());
      this._lateBoundPropertyGet = !(this._propertyInfo == (PropertyInfo) null) ? ReflectionHelpers.CreateLateBoundMethod(this._propertyInfo.GetGetMethod()) : throw new ArgumentException(string.Format("Property '{0}' not found in System.Diagnostics.Process", (object) this._propertyInfo));
      this._process = Process.GetCurrentProcess();
    }

    protected override void CloseLayoutRenderer()
    {
      if (this._process != null)
      {
        this._process.Close();
        this._process = (Process) null;
      }
      base.CloseLayoutRenderer();
    }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      if (this._lateBoundPropertyGet == null)
        return;
      IFormatProvider formatProvider = this.GetFormatProvider(logEvent);
      object obj = this._lateBoundPropertyGet((object) this._process, (object[]) null);
      builder.AppendFormattedValue(obj, this.Format, formatProvider);
    }
  }
}
