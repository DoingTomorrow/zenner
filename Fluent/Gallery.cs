// Decompiled with JetBrains decompiler
// Type: Fluent.Gallery
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Fluent
{
  [ContentProperty("Items")]
  public class Gallery : ListBox
  {
    private ObservableCollection<GalleryGroupFilter> filters;
    private DropDownButton groupsMenuButton;
    public static readonly DependencyProperty MinItemsInRowProperty = DependencyProperty.Register(nameof (MinItemsInRow), typeof (int), typeof (Gallery), (PropertyMetadata) new UIPropertyMetadata((object) 1));
    public static readonly DependencyProperty MaxItemsInRowProperty = DependencyProperty.Register(nameof (MaxItemsInRow), typeof (int), typeof (Gallery), (PropertyMetadata) new UIPropertyMetadata((object) int.MaxValue));
    public static readonly DependencyProperty GroupByProperty = DependencyProperty.Register(nameof (GroupBy), typeof (string), typeof (Gallery), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (Gallery), (PropertyMetadata) new UIPropertyMetadata((object) Orientation.Horizontal));
    public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(nameof (ItemWidth), typeof (double), typeof (Gallery), (PropertyMetadata) new UIPropertyMetadata((object) double.NaN));
    public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(nameof (ItemHeight), typeof (double), typeof (Gallery), (PropertyMetadata) new UIPropertyMetadata((object) double.NaN));
    public static readonly DependencyProperty SelectedFilterProperty = DependencyProperty.Register(nameof (SelectedFilter), typeof (GalleryGroupFilter), typeof (Gallery), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(Gallery.OnFilterChanged), new CoerceValueCallback(Gallery.CoerceSelectedFilter)));
    private static readonly DependencyPropertyKey SelectedFilterTitlePropertyKey = DependencyProperty.RegisterReadOnly(nameof (SelectedFilterTitle), typeof (string), typeof (Gallery), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty SelectedFilterTitleProperty = Gallery.SelectedFilterTitlePropertyKey.DependencyProperty;
    private static readonly DependencyPropertyKey SelectedFilterGroupsPropertyKey = DependencyProperty.RegisterReadOnly(nameof (SelectedFilterGroups), typeof (string), typeof (Gallery), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty SelectedFilterGroupsProperty = Gallery.SelectedFilterGroupsPropertyKey.DependencyProperty;
    private static readonly DependencyPropertyKey HasFilterPropertyKey = DependencyProperty.RegisterReadOnly(nameof (HasFilter), typeof (bool), typeof (Gallery), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty HasFilterProperty = Gallery.HasFilterPropertyKey.DependencyProperty;
    public static readonly DependencyProperty SelectableProperty = DependencyProperty.Register(nameof (Selectable), typeof (bool), typeof (Gallery), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(Gallery.OnSelectableChanged)));
    public static readonly DependencyPropertyKey IsLastItemPropertyKey = DependencyProperty.RegisterReadOnly(nameof (IsLastItem), typeof (bool), typeof (Gallery), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty IsLastItemProperty = Gallery.IsLastItemPropertyKey.DependencyProperty;

    public int MinItemsInRow
    {
      get => (int) this.GetValue(Gallery.MinItemsInRowProperty);
      set => this.SetValue(Gallery.MinItemsInRowProperty, (object) value);
    }

    public int MaxItemsInRow
    {
      get => (int) this.GetValue(Gallery.MaxItemsInRowProperty);
      set => this.SetValue(Gallery.MaxItemsInRowProperty, (object) value);
    }

    public string GroupBy
    {
      get => (string) this.GetValue(Gallery.GroupByProperty);
      set => this.SetValue(Gallery.GroupByProperty, (object) value);
    }

    public Orientation Orientation
    {
      get => (Orientation) this.GetValue(Gallery.OrientationProperty);
      set => this.SetValue(Gallery.OrientationProperty, (object) value);
    }

    public double ItemWidth
    {
      get => (double) this.GetValue(Gallery.ItemWidthProperty);
      set => this.SetValue(Gallery.ItemWidthProperty, (object) value);
    }

    public double ItemHeight
    {
      get => (double) this.GetValue(Gallery.ItemHeightProperty);
      set => this.SetValue(Gallery.ItemHeightProperty, (object) value);
    }

    public ObservableCollection<GalleryGroupFilter> Filters
    {
      get
      {
        if (this.filters == null)
        {
          this.filters = new ObservableCollection<GalleryGroupFilter>();
          this.filters.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnFilterCollectionChanged);
        }
        return this.filters;
      }
    }

    private void OnFilterCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      this.HasFilter = this.Filters.Count > 0;
      this.InvalidateProperty(Gallery.SelectedFilterProperty);
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          for (int index = 0; index < e.NewItems.Count; ++index)
          {
            if (this.groupsMenuButton != null)
            {
              GalleryGroupFilter newItem = (GalleryGroupFilter) e.NewItems[index];
              MenuItem insertItem = new MenuItem();
              insertItem.Header = (object) newItem.Title;
              insertItem.Tag = (object) newItem;
              if (newItem == this.SelectedFilter)
                insertItem.IsChecked = true;
              insertItem.Click += new RoutedEventHandler(this.OnFilterMenuItemClick);
              this.groupsMenuButton.Items.Insert(e.NewStartingIndex + index, (object) insertItem);
            }
          }
          break;
        case NotifyCollectionChangedAction.Remove:
          IEnumerator enumerator1 = e.OldItems.GetEnumerator();
          try
          {
            while (enumerator1.MoveNext())
            {
              object current = enumerator1.Current;
              if (this.groupsMenuButton != null)
                this.groupsMenuButton.Items.Remove((object) this.GetFilterMenuItem(current as GalleryGroupFilter));
            }
            break;
          }
          finally
          {
            if (enumerator1 is IDisposable disposable)
              disposable.Dispose();
          }
        case NotifyCollectionChangedAction.Replace:
          foreach (object oldItem in (IEnumerable) e.OldItems)
          {
            if (this.groupsMenuButton != null)
              this.groupsMenuButton.Items.Remove((object) this.GetFilterMenuItem(oldItem as GalleryGroupFilter));
          }
          IEnumerator enumerator2 = e.NewItems.GetEnumerator();
          try
          {
            while (enumerator2.MoveNext())
            {
              object current = enumerator2.Current;
              if (this.groupsMenuButton != null)
              {
                GalleryGroupFilter galleryGroupFilter = (GalleryGroupFilter) current;
                MenuItem newItem = new MenuItem();
                newItem.Header = (object) galleryGroupFilter.Title;
                newItem.Tag = (object) galleryGroupFilter;
                if (galleryGroupFilter == this.SelectedFilter)
                  newItem.IsChecked = true;
                newItem.Click += new RoutedEventHandler(this.OnFilterMenuItemClick);
                this.groupsMenuButton.Items.Add((object) newItem);
              }
            }
            break;
          }
          finally
          {
            if (enumerator2 is IDisposable disposable)
              disposable.Dispose();
          }
      }
    }

    public GalleryGroupFilter SelectedFilter
    {
      get => (GalleryGroupFilter) this.GetValue(Gallery.SelectedFilterProperty);
      set => this.SetValue(Gallery.SelectedFilterProperty, (object) value);
    }

    private static object CoerceSelectedFilter(DependencyObject d, object basevalue)
    {
      Gallery gallery = (Gallery) d;
      return basevalue == null && gallery.Filters.Count > 0 ? (object) gallery.Filters[0] : basevalue;
    }

    private static void OnFilterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      Gallery gallery = (Gallery) d;
      if (e.NewValue is GalleryGroupFilter newValue)
      {
        gallery.SelectedFilterTitle = newValue.Title;
        gallery.SelectedFilterGroups = newValue.Groups;
      }
      else
      {
        gallery.SelectedFilterTitle = "";
        gallery.SelectedFilterGroups = (string) null;
      }
      gallery.UpdateLayout();
    }

    public string SelectedFilterTitle
    {
      get => (string) this.GetValue(Gallery.SelectedFilterTitleProperty);
      private set => this.SetValue(Gallery.SelectedFilterTitlePropertyKey, (object) value);
    }

    public string SelectedFilterGroups
    {
      get => (string) this.GetValue(Gallery.SelectedFilterGroupsProperty);
      private set => this.SetValue(Gallery.SelectedFilterGroupsPropertyKey, (object) value);
    }

    public bool HasFilter
    {
      get => (bool) this.GetValue(Gallery.HasFilterProperty);
      private set => this.SetValue(Gallery.HasFilterPropertyKey, (object) value);
    }

    private void OnFilterMenuItemClick(object sender, RoutedEventArgs e)
    {
      MenuItem menuItem = (MenuItem) sender;
      this.GetFilterMenuItem(this.SelectedFilter).IsChecked = false;
      menuItem.IsChecked = true;
      this.SelectedFilter = menuItem.Tag as GalleryGroupFilter;
      this.groupsMenuButton.IsDropDownOpen = false;
      e.Handled = true;
    }

    private MenuItem GetFilterMenuItem(GalleryGroupFilter filter)
    {
      return filter == null ? (MenuItem) null : this.groupsMenuButton.Items.Cast<MenuItem>().FirstOrDefault<MenuItem>((Func<MenuItem, bool>) (item => item != null && item.Header.ToString() == filter.Title));
    }

    public bool Selectable
    {
      get => (bool) this.GetValue(Gallery.SelectableProperty);
      set => this.SetValue(Gallery.SelectableProperty, (object) value);
    }

    private static void OnSelectableChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      d.CoerceValue(Selector.SelectedItemProperty);
    }

    public bool IsLastItem
    {
      get => (bool) this.GetValue(Gallery.IsLastItemProperty);
      private set => this.SetValue(Gallery.IsLastItemPropertyKey, (object) value);
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static Gallery()
    {
      Type type = typeof (Gallery);
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (Gallery)));
      Selector.SelectedItemProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(Gallery.CoerceSelectedItem)));
      ContextMenuService.Attach(type);
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (Gallery), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(Gallery.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (Gallery));
      return basevalue;
    }

    private static object CoerceSelectedItem(DependencyObject d, object basevalue)
    {
      Gallery gallery = d as Gallery;
      if (gallery.Selectable)
        return basevalue;
      if (basevalue != null)
        (gallery.ItemContainerGenerator.ContainerFromItem(basevalue) as GalleryItem).IsSelected = false;
      return (object) null;
    }

    public Gallery()
    {
      ContextMenuService.Coerce((DependencyObject) this);
      this.Loaded += new RoutedEventHandler(this.OnLoaded);
      this.Focusable = false;
      KeyboardNavigation.SetDirectionalNavigation((DependencyObject) this, KeyboardNavigationMode.Continue);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      if (!(this.Parent is ItemsControl parent))
        return;
      if (parent.Items.IndexOf((object) this) == parent.Items.Count - 1)
        this.IsLastItem = true;
      else
        this.IsLastItem = false;
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
      return (DependencyObject) new GalleryItem();
    }

    protected override bool IsItemItsOwnContainerOverride(object item) => item is GalleryItem;

    public override void OnApplyTemplate()
    {
      if (this.groupsMenuButton != null)
        this.groupsMenuButton.Items.Clear();
      this.groupsMenuButton = this.GetTemplateChild("PART_DropDownButton") as DropDownButton;
      if (this.groupsMenuButton != null)
      {
        for (int index = 0; index < this.Filters.Count; ++index)
        {
          MenuItem newItem = new MenuItem();
          newItem.Header = (object) this.Filters[index].Title;
          newItem.Tag = (object) this.Filters[index];
          if (this.Filters[index] == this.SelectedFilter)
            newItem.IsChecked = true;
          newItem.Click += new RoutedEventHandler(this.OnFilterMenuItemClick);
          this.groupsMenuButton.Items.Add((object) newItem);
        }
      }
      base.OnApplyTemplate();
    }
  }
}
