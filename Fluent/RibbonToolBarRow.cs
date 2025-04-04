// Decompiled with JetBrains decompiler
// Type: Fluent.RibbonToolBarRow
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Markup;

#nullable disable
namespace Fluent
{
  [ContentProperty("Children")]
  [SuppressMessage("Microsoft.Naming", "CA1702", Justification = "We mean here 'bar row' instead of 'barrow'")]
  public class RibbonToolBarRow : DependencyObject
  {
    private readonly ObservableCollection<DependencyObject> children = new ObservableCollection<DependencyObject>();

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ObservableCollection<DependencyObject> Children => this.children;
  }
}
