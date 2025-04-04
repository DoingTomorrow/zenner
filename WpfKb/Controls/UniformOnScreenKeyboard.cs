// Decompiled with JetBrains decompiler
// Type: WpfKb.Controls.UniformOnScreenKeyboard
// Assembly: WpfKb, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B294CC70-CB21-4202-BD7A-A4E6693370B9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\WpfKb.dll

using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace WpfKb.Controls
{
  public class UniformOnScreenKeyboard : Grid
  {
    public static readonly DependencyProperty AreAnimationsEnabledProperty = DependencyProperty.Register(nameof (AreAnimationsEnabled), typeof (bool), typeof (UniformOnScreenKeyboard), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(UniformOnScreenKeyboard.OnAreAnimationsEnabledPropertyChanged)));
    private ObservableCollection<OnScreenKey> _keys;

    public bool AreAnimationsEnabled
    {
      get => (bool) this.GetValue(UniformOnScreenKeyboard.AreAnimationsEnabledProperty);
      set => this.SetValue(UniformOnScreenKeyboard.AreAnimationsEnabledProperty, (object) value);
    }

    public ObservableCollection<OnScreenKey> Keys
    {
      get => this._keys;
      set
      {
        if (value == this._keys)
          return;
        this.Reset();
        this._keys = value;
        if (this._keys != null)
        {
          foreach (UIElement key in (Collection<OnScreenKey>) this._keys)
            this.Children.Add(key);
        }
        this.ResizeGrid();
      }
    }

    public UniformOnScreenKeyboard()
    {
      this._keys = new ObservableCollection<OnScreenKey>();
      this._keys.CollectionChanged += new NotifyCollectionChangedEventHandler(this.Keys_CollectionChanged);
    }

    ~UniformOnScreenKeyboard()
    {
      this._keys.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.Keys_CollectionChanged);
    }

    private static void OnAreAnimationsEnabledPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ((UniformOnScreenKeyboard) d).Keys.ToList<OnScreenKey>().ForEach((Action<OnScreenKey>) (x => x.AreAnimationsEnabled = (bool) e.NewValue));
    }

    private void Keys_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          foreach (OnScreenKey element in e.NewItems.OfType<OnScreenKey>())
          {
            element.AreAnimationsEnabled = this.AreAnimationsEnabled;
            this.Children.Add((UIElement) element);
          }
          this.ResizeGrid();
          break;
        case NotifyCollectionChangedAction.Remove:
          foreach (UIElement element in e.OldItems.OfType<OnScreenKey>())
            this.Children.Remove(element);
          this.ResizeGrid();
          break;
        case NotifyCollectionChangedAction.Replace:
          this.Children.Clear();
          foreach (UIElement key in (Collection<OnScreenKey>) this._keys)
            this.Children.Add(key);
          this.ResizeGrid();
          break;
        case NotifyCollectionChangedAction.Reset:
          this.Reset();
          break;
      }
    }

    private void ResizeGrid()
    {
      if (this._keys == null)
      {
        this.Reset();
      }
      else
      {
        int num1 = this._keys.Max<OnScreenKey>((Func<OnScreenKey, int>) (x => x.GridRow)) + 1;
        for (int index = num1 - this.RowDefinitions.Count; index > 0; --index)
          this.RowDefinitions.Add(new RowDefinition());
        for (int index = this.RowDefinitions.Count - num1; index > 0; --index)
          this.RowDefinitions.RemoveAt(0);
        int num2 = this._keys.Max<OnScreenKey>((Func<OnScreenKey, int>) (x => x.GridColumn)) + 1;
        for (int index = num2 - this.ColumnDefinitions.Count; index > 0; --index)
          this.ColumnDefinitions.Add(new ColumnDefinition());
        for (int index = this.ColumnDefinitions.Count - num2; index > 0; --index)
          this.ColumnDefinitions.RemoveAt(0);
      }
    }

    private void Reset()
    {
      this._keys = (ObservableCollection<OnScreenKey>) null;
      this.Children.Clear();
      this.RowDefinitions.Clear();
      this.ColumnDefinitions.Clear();
    }
  }
}
