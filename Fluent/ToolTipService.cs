// Decompiled with JetBrains decompiler
// Type: Fluent.ToolTipService
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Windows;

#nullable disable
namespace Fluent
{
  public static class ToolTipService
  {
    public static void Attach(Type type)
    {
      System.Windows.Controls.ToolTipService.ShowOnDisabledProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) true));
      System.Windows.Controls.ToolTipService.InitialShowDelayProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) 900));
      System.Windows.Controls.ToolTipService.BetweenShowDelayProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) 0));
      System.Windows.Controls.ToolTipService.ShowDurationProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) 20000));
    }
  }
}
