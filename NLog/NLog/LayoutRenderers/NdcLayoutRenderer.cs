// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.NdcLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("ndc")]
  [ThreadSafe]
  public class NdcLayoutRenderer : LayoutRenderer
  {
    public NdcLayoutRenderer()
    {
      this.Separator = " ";
      this.BottomFrames = -1;
      this.TopFrames = -1;
    }

    public int TopFrames { get; set; }

    public int BottomFrames { get; set; }

    public string Separator { get; set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      object[] allObjects = NestedDiagnosticsContext.GetAllObjects();
      int num1 = 0;
      int num2 = allObjects.Length;
      if (this.TopFrames != -1)
        num2 = Math.Min(this.TopFrames, allObjects.Length);
      else if (this.BottomFrames != -1)
        num1 = allObjects.Length - Math.Min(this.BottomFrames, allObjects.Length);
      string str1 = string.Empty;
      for (int index = num2 - 1; index >= num1; --index)
      {
        string str2 = FormatHelper.ConvertToString(allObjects[index], logEvent.FormatProvider);
        builder.Append(str1);
        builder.Append(str2);
        str1 = this.Separator;
      }
    }
  }
}
