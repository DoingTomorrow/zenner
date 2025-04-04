// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.QueryPerformanceCounterLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Internal;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("qpc")]
  public class QueryPerformanceCounterLayoutRenderer : LayoutRenderer
  {
    private bool raw;
    private ulong firstQpcValue;
    private ulong lastQpcValue;
    private double frequency = 1.0;

    public QueryPerformanceCounterLayoutRenderer()
    {
      this.Normalize = true;
      this.Difference = false;
      this.Precision = 4;
      this.AlignDecimalPoint = true;
    }

    [DefaultValue(true)]
    public bool Normalize { get; set; }

    [DefaultValue(false)]
    public bool Difference { get; set; }

    [DefaultValue(true)]
    public bool Seconds
    {
      get => !this.raw;
      set => this.raw = !value;
    }

    [DefaultValue(4)]
    public int Precision { get; set; }

    [DefaultValue(true)]
    public bool AlignDecimalPoint { get; set; }

    protected override void InitializeLayoutRenderer()
    {
      base.InitializeLayoutRenderer();
      ulong lpPerformanceFrequency;
      if (!NativeMethods.QueryPerformanceFrequency(out lpPerformanceFrequency))
        throw new InvalidOperationException("Cannot determine high-performance counter frequency.");
      ulong lpPerformanceCount;
      if (!NativeMethods.QueryPerformanceCounter(out lpPerformanceCount))
        throw new InvalidOperationException("Cannot determine high-performance counter value.");
      this.frequency = (double) lpPerformanceFrequency;
      this.firstQpcValue = lpPerformanceCount;
      this.lastQpcValue = lpPerformanceCount;
    }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      ulong lpPerformanceCount;
      if (!NativeMethods.QueryPerformanceCounter(out lpPerformanceCount))
        return;
      ulong num1 = lpPerformanceCount;
      if (this.Difference)
        lpPerformanceCount -= this.lastQpcValue;
      else if (this.Normalize)
        lpPerformanceCount -= this.firstQpcValue;
      this.lastQpcValue = num1;
      string str;
      if (this.Seconds)
      {
        str = Convert.ToString(Math.Round((double) lpPerformanceCount / this.frequency, this.Precision), (IFormatProvider) CultureInfo.InvariantCulture);
        if (this.AlignDecimalPoint)
        {
          int num2 = str.IndexOf('.');
          str = num2 != -1 ? str + new string('0', this.Precision - (str.Length - 1 - num2)) : str + "." + new string('0', this.Precision);
        }
      }
      else
        str = Convert.ToString(lpPerformanceCount, (IFormatProvider) CultureInfo.InvariantCulture);
      builder.Append(str);
    }
  }
}
