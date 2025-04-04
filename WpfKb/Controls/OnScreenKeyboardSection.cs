// Decompiled with JetBrains decompiler
// Type: WpfKb.Controls.OnScreenKeyboardSection
// Assembly: WpfKb, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B294CC70-CB21-4202-BD7A-A4E6693370B9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\WpfKb.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace WpfKb.Controls
{
  public class OnScreenKeyboardSection : Grid
  {
    private ObservableCollection<OnScreenKey> _keys;
    private List<Grid> _buttonRows;

    public ObservableCollection<OnScreenKey> Keys
    {
      get => this._keys;
      set
      {
        if (value == this._keys)
          return;
        this.Reset();
        this._keys = value;
        this.LayoutKeys();
      }
    }

    public OnScreenKeyboardSection()
    {
      this.Margin = new Thickness(5.0);
      this._buttonRows = new List<Grid>();
      this._keys = new ObservableCollection<OnScreenKey>();
      this._keys.CollectionChanged += new NotifyCollectionChangedEventHandler(this.Keys_CollectionChanged);
    }

    ~OnScreenKeyboardSection()
    {
      this._keys.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.Keys_CollectionChanged);
    }

    private void Keys_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
        case NotifyCollectionChangedAction.Remove:
          this.LayoutKeys();
          break;
        case NotifyCollectionChangedAction.Replace:
          throw new NotSupportedException("You cannot currently replace keys.");
        case NotifyCollectionChangedAction.Reset:
          this.Reset();
          break;
      }
    }

    private void LayoutKeys()
    {
      if (this._keys == null || this._keys.Count == 0)
        return;
      this.ResizeGrid((IEnumerable<OnScreenKey>) this._keys);
      for (int buttonRowIndex = 0; buttonRowIndex < this._buttonRows.Count; buttonRowIndex++)
      {
        Grid buttonRow = this._buttonRows[buttonRowIndex];
        buttonRow.Children.Clear();
        foreach (OnScreenKey element in this._keys.Where<OnScreenKey>((Func<OnScreenKey, bool>) (x => x.GridRow == buttonRowIndex)))
          buttonRow.Children.Add((UIElement) element);
      }
    }

    private void ResizeGrid(IEnumerable<OnScreenKey> keys)
    {
      if (keys == null)
        throw new ArgumentNullException(nameof (keys));
      int num1 = keys.Max<OnScreenKey>((Func<OnScreenKey, int>) (x => x.GridRow)) + 1;
      for (int count = this.RowDefinitions.Count; count < num1; ++count)
      {
        this.RowDefinitions.Add(new RowDefinition());
        Grid element = new Grid();
        this._buttonRows.Add(element);
        this.Children.Add((UIElement) element);
        element.SetValue(Grid.RowProperty, (object) count);
      }
      for (int buttonRowIndex = 0; buttonRowIndex < num1; buttonRowIndex++)
      {
        Grid grid = this._buttonRows[buttonRowIndex];
        int num2 = keys.Where<OnScreenKey>((Func<OnScreenKey, bool>) (x => x.GridRow == buttonRowIndex)).Max<OnScreenKey>((Func<OnScreenKey, int>) (x => x.GridColumn)) + 1;
        for (int index = num2 - grid.ColumnDefinitions.Count; index > 0; --index)
          grid.ColumnDefinitions.Add(new ColumnDefinition());
        for (int index = grid.ColumnDefinitions.Count - num2; index > 0; --index)
          grid.ColumnDefinitions.RemoveAt(0);
        keys.Where<OnScreenKey>((Func<OnScreenKey, bool>) (x => x.GridRow == buttonRowIndex && x.GridWidth.Value != 1.0)).ToList<OnScreenKey>().ForEach((Action<OnScreenKey>) (x => grid.ColumnDefinitions[x.GridColumn].Width = x.GridWidth));
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
