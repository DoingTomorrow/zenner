// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.PropertyChangeNotifier
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace MahApps.Metro.Controls
{
  internal sealed class PropertyChangeNotifier : DependencyObject, IDisposable
  {
    private WeakReference _propertySource;
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (object), typeof (PropertyChangeNotifier), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(PropertyChangeNotifier.OnPropertyChanged)));

    public PropertyChangeNotifier(DependencyObject propertySource, string path)
      : this(propertySource, new PropertyPath(path, new object[0]))
    {
    }

    public PropertyChangeNotifier(DependencyObject propertySource, DependencyProperty property)
      : this(propertySource, new PropertyPath((object) property))
    {
    }

    public PropertyChangeNotifier(DependencyObject propertySource, PropertyPath property)
    {
      if (propertySource == null)
        throw new ArgumentNullException(nameof (propertySource));
      if (property == null)
        throw new ArgumentNullException(nameof (property));
      this._propertySource = new WeakReference((object) propertySource);
      BindingOperations.SetBinding((DependencyObject) this, PropertyChangeNotifier.ValueProperty, (BindingBase) new Binding()
      {
        Path = property,
        Mode = BindingMode.OneWay,
        Source = (object) propertySource
      });
    }

    public DependencyObject PropertySource
    {
      get
      {
        try
        {
          return this._propertySource.IsAlive ? this._propertySource.Target as DependencyObject : (DependencyObject) null;
        }
        catch
        {
          return (DependencyObject) null;
        }
      }
    }

    [Description("Returns/sets the value of the property")]
    [Category("Behavior")]
    [Bindable(true)]
    public object Value
    {
      get => this.GetValue(PropertyChangeNotifier.ValueProperty);
      set => this.SetValue(PropertyChangeNotifier.ValueProperty, value);
    }

    private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      PropertyChangeNotifier propertyChangeNotifier = (PropertyChangeNotifier) d;
      if (propertyChangeNotifier.ValueChanged == null)
        return;
      propertyChangeNotifier.ValueChanged((object) propertyChangeNotifier.PropertySource, EventArgs.Empty);
    }

    public event EventHandler ValueChanged;

    public void Dispose()
    {
      BindingOperations.ClearBinding((DependencyObject) this, PropertyChangeNotifier.ValueProperty);
    }
  }
}
