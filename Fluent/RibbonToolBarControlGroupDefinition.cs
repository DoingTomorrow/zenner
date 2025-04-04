// Decompiled with JetBrains decompiler
// Type: Fluent.RibbonToolBarControlGroupDefinition
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;

#nullable disable
namespace Fluent
{
  [ContentProperty("Children")]
  public class RibbonToolBarControlGroupDefinition : DependencyObject
  {
    private readonly ObservableCollection<RibbonToolBarControlDefinition> children = new ObservableCollection<RibbonToolBarControlDefinition>();

    public event NotifyCollectionChangedEventHandler ChildrenChanged;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ObservableCollection<RibbonToolBarControlDefinition> Children => this.children;

    public RibbonToolBarControlGroupDefinition()
    {
      this.children.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnChildrenCollectionChanged);
    }

    private void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (this.ChildrenChanged == null)
        return;
      this.ChildrenChanged(sender, e);
    }
  }
}
