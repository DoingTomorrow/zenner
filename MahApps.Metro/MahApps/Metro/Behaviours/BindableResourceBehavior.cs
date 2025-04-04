// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Behaviours.BindableResourceBehavior
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Shapes;

#nullable disable
namespace MahApps.Metro.Behaviours
{
  public class BindableResourceBehavior : Behavior<Shape>
  {
    public static readonly DependencyProperty ResourceNameProperty = DependencyProperty.Register(nameof (ResourceName), typeof (string), typeof (BindableResourceBehavior), new PropertyMetadata((object) null));
    public static readonly DependencyProperty PropertyProperty = DependencyProperty.Register(nameof (Property), typeof (DependencyProperty), typeof (BindableResourceBehavior), new PropertyMetadata((object) null));

    protected override void OnAttached()
    {
      this.AssociatedObject.SetResourceReference(this.Property, (object) this.ResourceName);
      base.OnAttached();
    }

    public string ResourceName
    {
      get => (string) this.GetValue(BindableResourceBehavior.ResourceNameProperty);
      set => this.SetValue(BindableResourceBehavior.ResourceNameProperty, (object) value);
    }

    public DependencyProperty Property
    {
      get => (DependencyProperty) this.GetValue(BindableResourceBehavior.PropertyProperty);
      set => this.SetValue(BindableResourceBehavior.PropertyProperty, (object) value);
    }
  }
}
