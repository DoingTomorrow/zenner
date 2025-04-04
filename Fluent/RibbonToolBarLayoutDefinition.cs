// Decompiled with JetBrains decompiler
// Type: Fluent.RibbonToolBarLayoutDefinition
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;

#nullable disable
namespace Fluent
{
  [ContentProperty("Rows")]
  public class RibbonToolBarLayoutDefinition : DependencyObject
  {
    private ObservableCollection<RibbonToolBarRow> rows = new ObservableCollection<RibbonToolBarRow>();
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(nameof (Size), typeof (RibbonControlSize), typeof (RibbonToolBarLayoutDefinition), (PropertyMetadata) new FrameworkPropertyMetadata((object) RibbonControlSize.Large));
    public static readonly DependencyProperty RowCountProperty = DependencyProperty.Register(nameof (RowCount), typeof (int), typeof (RibbonToolBar), (PropertyMetadata) new UIPropertyMetadata((object) 3));

    public RibbonControlSize Size
    {
      get => (RibbonControlSize) this.GetValue(RibbonToolBarLayoutDefinition.SizeProperty);
      set => this.SetValue(RibbonToolBarLayoutDefinition.SizeProperty, (object) value);
    }

    public int RowCount
    {
      get => (int) this.GetValue(RibbonToolBarLayoutDefinition.RowCountProperty);
      set => this.SetValue(RibbonToolBarLayoutDefinition.RowCountProperty, (object) value);
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ObservableCollection<RibbonToolBarRow> Rows => this.rows;
  }
}
