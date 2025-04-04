// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.DataGridRowHelper
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace MahApps.Metro.Controls
{
  public static class DataGridRowHelper
  {
    public static readonly DependencyProperty SelectionUnitProperty = DependencyProperty.RegisterAttached("SelectionUnit", typeof (DataGridSelectionUnit), typeof (DataGridRowHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) DataGridSelectionUnit.FullRow));

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (DataGridRow))]
    public static DataGridSelectionUnit GetSelectionUnit(UIElement element)
    {
      return (DataGridSelectionUnit) element.GetValue(DataGridRowHelper.SelectionUnitProperty);
    }

    public static void SetSelectionUnit(UIElement element, DataGridSelectionUnit value)
    {
      element.SetValue(DataGridRowHelper.SelectionUnitProperty, (object) value);
    }
  }
}
