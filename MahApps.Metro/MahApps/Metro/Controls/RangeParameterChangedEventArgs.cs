// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.RangeParameterChangedEventArgs
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Windows;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class RangeParameterChangedEventArgs : RoutedEventArgs
  {
    public RangeParameterChangeType ParameterType { get; private set; }

    public double OldValue { get; private set; }

    public double NewValue { get; private set; }

    internal RangeParameterChangedEventArgs(
      RangeParameterChangeType type,
      double _old,
      double _new)
    {
      this.ParameterType = type;
      this.OldValue = _old;
      this.NewValue = _new;
    }
  }
}
