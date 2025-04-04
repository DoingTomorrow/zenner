// Decompiled with JetBrains decompiler
// Type: Fluent.RibbonToolBarControlDefinition
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System.ComponentModel;
using System.Windows;

#nullable disable
namespace Fluent
{
  public class RibbonToolBarControlDefinition : DependencyObject, INotifyPropertyChanged
  {
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(nameof (Size), typeof (RibbonControlSize), typeof (RibbonToolBarControlDefinition), (PropertyMetadata) new FrameworkPropertyMetadata((object) RibbonControlSize.Small, new PropertyChangedCallback(RibbonToolBarControlDefinition.OnSizePropertyChanged)));
    public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(nameof (Target), typeof (string), typeof (RibbonToolBarControlDefinition), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(RibbonToolBarControlDefinition.OnTargetPropertyChanged)));
    public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(nameof (Width), typeof (double), typeof (RibbonToolBarControlDefinition), (PropertyMetadata) new UIPropertyMetadata((object) double.NaN, new PropertyChangedCallback(RibbonToolBarControlDefinition.OnWidthPropertyChanged)));

    public RibbonControlSize Size
    {
      get => (RibbonControlSize) this.GetValue(RibbonToolBarControlDefinition.SizeProperty);
      set => this.SetValue(RibbonToolBarControlDefinition.SizeProperty, (object) value);
    }

    private static void OnSizePropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ((RibbonToolBarControlDefinition) d).Invalidate("Size");
    }

    public string Target
    {
      get => (string) this.GetValue(RibbonToolBarControlDefinition.TargetProperty);
      set => this.SetValue(RibbonToolBarControlDefinition.TargetProperty, (object) value);
    }

    private static void OnTargetPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ((RibbonToolBarControlDefinition) d).Invalidate("Target");
    }

    public double Width
    {
      get => (double) this.GetValue(RibbonToolBarControlDefinition.WidthProperty);
      set => this.SetValue(RibbonToolBarControlDefinition.WidthProperty, (object) value);
    }

    private static void OnWidthPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ((RibbonToolBarControlDefinition) d).Invalidate("Width");
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void Invalidate(string propertyName)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
