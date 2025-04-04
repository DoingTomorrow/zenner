// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.GarbageCollectorInfoLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("gc")]
  [ThreadSafe]
  public class GarbageCollectorInfoLayoutRenderer : LayoutRenderer
  {
    public GarbageCollectorInfoLayoutRenderer()
    {
      this.Property = GarbageCollectorProperty.TotalMemory;
    }

    [DefaultValue("TotalMemory")]
    public GarbageCollectorProperty Property { get; set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      IFormatProvider formatProvider = this.GetFormatProvider(logEvent);
      if (formatProvider == CultureInfo.InvariantCulture)
        formatProvider = (IFormatProvider) null;
      long num = 0;
      switch (this.Property)
      {
        case GarbageCollectorProperty.TotalMemory:
          num = GC.GetTotalMemory(false);
          break;
        case GarbageCollectorProperty.TotalMemoryForceCollection:
          num = GC.GetTotalMemory(true);
          break;
        case GarbageCollectorProperty.CollectionCount0:
          num = (long) GC.CollectionCount(0);
          break;
        case GarbageCollectorProperty.CollectionCount1:
          num = (long) GC.CollectionCount(1);
          break;
        case GarbageCollectorProperty.CollectionCount2:
          num = (long) GC.CollectionCount(2);
          break;
        case GarbageCollectorProperty.MaxGeneration:
          num = (long) GC.MaxGeneration;
          break;
      }
      if (formatProvider == null && num >= 0L && num < (long) uint.MaxValue)
        builder.AppendInvariant((uint) num);
      else
        builder.Append(Convert.ToString(num, formatProvider ?? (IFormatProvider) CultureInfo.InvariantCulture));
    }
  }
}
