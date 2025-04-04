// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.GuidLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using System;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("guid")]
  [ThreadSafe]
  public class GuidLayoutRenderer : LayoutRenderer
  {
    public GuidLayoutRenderer() => this.Format = "N";

    [DefaultValue("N")]
    public string Format { get; set; }

    [DefaultValue(false)]
    public bool GeneratedFromLogEvent { get; set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      if (this.GeneratedFromLogEvent)
      {
        int hashCode = logEvent.GetHashCode();
        short b = (short) (hashCode >> 16 & (int) ushort.MaxValue);
        short c = (short) (hashCode & (int) ushort.MaxValue);
        long ticks = LogEventInfo.ZeroDate.Ticks;
        byte d = (byte) ((ulong) (ticks >> 56) & (ulong) byte.MaxValue);
        byte e = (byte) ((ulong) (ticks >> 48) & (ulong) byte.MaxValue);
        byte f = (byte) ((ulong) (ticks >> 40) & (ulong) byte.MaxValue);
        byte g = (byte) ((ulong) (ticks >> 32) & (ulong) byte.MaxValue);
        byte h = (byte) ((ulong) (ticks >> 24) & (ulong) byte.MaxValue);
        byte i = (byte) ((ulong) (ticks >> 16) & (ulong) byte.MaxValue);
        byte j = (byte) ((ulong) (ticks >> 8) & (ulong) byte.MaxValue);
        byte k = (byte) ((ulong) ticks & (ulong) byte.MaxValue);
        builder.Append(new Guid(logEvent.SequenceID, b, c, d, e, f, g, h, i, j, k).ToString(this.Format));
      }
      else
        builder.Append(Guid.NewGuid().ToString(this.Format));
    }
  }
}
