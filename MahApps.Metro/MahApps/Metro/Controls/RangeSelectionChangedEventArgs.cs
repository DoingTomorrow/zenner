// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.RangeSelectionChangedEventArgs
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Windows;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class RangeSelectionChangedEventArgs : RoutedEventArgs
  {
    public double NewLowerValue { get; set; }

    public double NewUpperValue { get; set; }

    public double OldLowerValue { get; set; }

    public double OldUpperValue { get; set; }

    internal RangeSelectionChangedEventArgs(
      double newLowerValue,
      double newUpperValue,
      double oldLowerValue,
      double oldUpperValue)
    {
      this.NewLowerValue = newLowerValue;
      this.NewUpperValue = newUpperValue;
      this.OldLowerValue = oldLowerValue;
      this.OldUpperValue = oldUpperValue;
    }
  }
}
