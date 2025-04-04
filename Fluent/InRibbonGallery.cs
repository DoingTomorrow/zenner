// Decompiled with JetBrains decompiler
// Type: Fluent.InRibbonGallery
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
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

#nullable disable
namespace Fluent
{
  [SuppressMessage("Microsoft.Maintainability", "CA1506")]
  [ContentProperty("Items")]
  public class InRibbonGallery : 
    Selector,
    IScalableRibbonControl,
    IDropDownControl,
    IRibbonControl,
    IKeyTipedControl,
    IQuickAccessItemProvider
  {
    private ObservableCollection<GalleryGroupFilter> filters;
    private ToggleButton expandButton;
    private ToggleButton dropDownButton;
    private Panel menuPanel;
    private int currentItemsInRow;
    private Panel layoutRoot;
    private Image snappedImage;
    private bool isSnapped;
    private Popup popup;
    private Thumb resizeBothThumb;
    private Thumb resizeVerticalThumb;
    private DropDownButton groupsMenuButton;
    private GalleryPanel galleryPanel;
    private ContentControl controlPresenter;
    private ContentControl popupControlPresenter;
    private ScrollViewer scrollViewer;
    private bool canOpenDropDown = true;
    private IInputElement focusedElement;
    private bool isButtonClicked;
    public static readonly DependencyProperty SizeProperty = RibbonControl.SizeProperty.AddOwner(typeof (InRibbonGallery), new PropertyMetadata(new PropertyChangedCallback(InRibbonGallery.OnSizeChanged)));
    public static readonly DependencyProperty SizeDefinitionProperty = RibbonControl.AttachSizeDefinition(typeof (InRibbonGallery));
    public static readonly DependencyProperty HeaderProperty = RibbonControl.HeaderProperty.AddOwner(typeof (InRibbonGallery));
    public static readonly DependencyProperty IconProperty = RibbonControl.IconProperty.AddOwner(typeof (InRibbonGallery));
    public static readonly DependencyProperty MinItemsInDropDownRowProperty = DependencyProperty.Register(nameof (MinItemsInDropDownRow), typeof (int), typeof (InRibbonGallery), (PropertyMetadata) new UIPropertyMetadata((object) 1));
    public static readonly DependencyProperty MaxItemsInDropDownRowProperty = DependencyProperty.Register(nameof (MaxItemsInDropDownRow), typeof (int), typeof (InRibbonGallery), (PropertyMetadata) new UIPropertyMetadata((object) int.MaxValue));
    public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(nameof (ItemWidth), typeof (double), typeof (InRibbonGallery), (PropertyMetadata) new UIPropertyMetadata((object) double.NaN));
    public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(nameof (ItemHeight), typeof (double), typeof (InRibbonGallery), (PropertyMetadata) new UIPropertyMetadata((object) double.NaN));
    public static readonly DependencyProperty GroupByProperty = DependencyProperty.Register(nameof (GroupBy), typeof (string), typeof (InRibbonGallery), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (InRibbonGallery), (PropertyMetadata) new UIPropertyMetadata((object) Orientation.Horizontal));
    public static readonly DependencyProperty SelectedFilterProperty = DependencyProperty.Register(nameof (SelectedFilter), typeof (GalleryGroupFilter), typeof (InRibbonGallery), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(InRibbonGallery.OnFilterChanged), new CoerceValueCallback(InRibbonGallery.CoerceSelectedFilter)));
    private static readonly DependencyPropertyKey SelectedFilterTitlePropertyKey = DependencyProperty.RegisterReadOnly(nameof (SelectedFilterTitle), typeof (string), typeof (InRibbonGallery), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty SelectedFilterTitleProperty = InRibbonGallery.SelectedFilterTitlePropertyKey.DependencyProperty;
    private static readonly DependencyPropertyKey SelectedFilterGroupsPropertyKey = DependencyProperty.RegisterReadOnly(nameof (SelectedFilterGroups), typeof (string), typeof (InRibbonGallery), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty SelectedFilterGroupsProperty = InRibbonGallery.SelectedFilterGroupsPropertyKey.DependencyProperty;
    private static readonly DependencyPropertyKey HasFilterPropertyKey = DependencyProperty.RegisterReadOnly(nameof (HasFilter), typeof (bool), typeof (InRibbonGallery), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty HasFilterProperty = InRibbonGallery.HasFilterPropertyKey.DependencyProperty;
    public static readonly DependencyProperty SelectableProperty = DependencyProperty.Register(nameof (Selectable), typeof (bool), typeof (InRibbonGallery), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(InRibbonGallery.OnSelectableChanged)));
    public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(nameof (IsDropDownOpen), typeof (bool), typeof (InRibbonGallery), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty ResizeModeProperty = DependencyProperty.Register(nameof (ResizeMode), typeof (ContextMenuResizeMode), typeof (InRibbonGallery), (PropertyMetadata) new UIPropertyMetadata((object) ContextMenuResizeMode.None));
    public static readonly DependencyProperty CanCollapseToButtonProperty = DependencyProperty.Register(nameof (CanCollapseToButton), typeof (bool), typeof (InRibbonGallery), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty IsCollapsedProperty = DependencyProperty.Register(nameof (IsCollapsed), typeof (bool), typeof (InRibbonGallery), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty LargeIconProperty = DependencyProperty.Register(nameof (LargeIcon), typeof (ImageSource), typeof (InRibbonGallery), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty MenuProperty = DependencyProperty.Register(nameof (Menu), typeof (RibbonMenu), typeof (InRibbonGallery), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty MaxItemsInRowProperty = DependencyProperty.Register(nameof (MaxItemsInRow), typeof (int), typeof (InRibbonGallery), (PropertyMetadata) new UIPropertyMetadata((object) 8, new PropertyChangedCallback(InRibbonGallery.OnMaxItemsInRowChanged)));
    public static readonly DependencyProperty MinItemsInRowProperty = DependencyProperty.Register(nameof (MinItemsInRow), typeof (int), typeof (InRibbonGallery), (PropertyMetadata) new UIPropertyMetadata((object) 1));
    public static readonly DependencyProperty MaxDropDownHeightProperty = DependencyProperty.Register(nameof (MaxDropDownHeight), typeof (double), typeof (InRibbonGallery), (PropertyMetadata) new UIPropertyMetadata((object) 1000.0));
    private double minimalGallerylWidth;
    private object selectedItem;
    private InRibbonGallery quickAccessGallery;
    public static readonly DependencyProperty CanAddToQuickAccessToolBarProperty = RibbonControl.CanAddToQuickAccessToolBarProperty.AddOwner(typeof (InRibbonGallery), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(RibbonControl.OnCanAddToQuickAccessToolbarChanged)));
    public static readonly DependencyProperty QuickAccessElementStyleProperty = RibbonControl.QuickAccessElementStyleProperty.AddOwner(typeof (InRibbonGallery));

    private static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      (d as InRibbonGallery).OnSizePropertyChanged((RibbonControlSize) e.OldValue, (RibbonControlSize) e.NewValue);
    }

    public RibbonControlSize Size
    {
      get => (RibbonControlSize) this.GetValue(InRibbonGallery.SizeProperty);
      set => this.SetValue(InRibbonGallery.SizeProperty, (object) value);
    }

    public string SizeDefinition
    {
      get => (string) this.GetValue(InRibbonGallery.SizeDefinitionProperty);
      set => this.SetValue(InRibbonGallery.SizeDefinitionProperty, (object) value);
    }

    public object Header
    {
      get => this.GetValue(InRibbonGallery.HeaderProperty);
      set => this.SetValue(InRibbonGallery.HeaderProperty, value);
    }

    public object Icon
    {
      get => this.GetValue(InRibbonGallery.IconProperty);
      set => this.SetValue(InRibbonGallery.IconProperty, value);
    }

    public int MinItemsInDropDownRow
    {
      get => (int) this.GetValue(InRibbonGallery.MinItemsInDropDownRowProperty);
      set => this.SetValue(InRibbonGallery.MinItemsInDropDownRowProperty, (object) value);
    }

    public int MaxItemsInDropDownRow
    {
      get => (int) this.GetValue(InRibbonGallery.MaxItemsInDropDownRowProperty);
      set => this.SetValue(InRibbonGallery.MaxItemsInDropDownRowProperty, (object) value);
    }

    public double ItemWidth
    {
      get => (double) this.GetValue(InRibbonGallery.ItemWidthProperty);
      set => this.SetValue(InRibbonGallery.ItemWidthProperty, (object) value);
    }

    public double ItemHeight
    {
      get => (double) this.GetValue(InRibbonGallery.ItemHeightProperty);
      set => this.SetValue(InRibbonGallery.ItemHeightProperty, (object) value);
    }

    public string GroupBy
    {
      get => (string) this.GetValue(InRibbonGallery.GroupByProperty);
      set => this.SetValue(InRibbonGallery.GroupByProperty, (object) value);
    }

    public Orientation Orientation
    {
      get => (Orientation) this.GetValue(InRibbonGallery.OrientationProperty);
      set => this.SetValue(InRibbonGallery.OrientationProperty, (object) value);
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
      this.InvalidateProperty(InRibbonGallery.SelectedFilterProperty);
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          IEnumerator enumerator1 = e.NewItems.GetEnumerator();
          try
          {
            while (enumerator1.MoveNext())
            {
              object current = enumerator1.Current;
              if (this.groupsMenuButton != null)
              {
                GalleryGroupFilter galleryGroupFilter = (GalleryGroupFilter) current;
                MenuItem newItem = new MenuItem();
                newItem.Header = (object) galleryGroupFilter.Title;
                newItem.Tag = (object) galleryGroupFilter;
                newItem.IsDefinitive = false;
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
            if (enumerator1 is IDisposable disposable)
              disposable.Dispose();
          }
        case NotifyCollectionChangedAction.Remove:
          IEnumerator enumerator2 = e.OldItems.GetEnumerator();
          try
          {
            while (enumerator2.MoveNext())
            {
              object current = enumerator2.Current;
              if (this.groupsMenuButton != null)
                this.groupsMenuButton.Items.Remove((object) this.GetFilterMenuItem(current as GalleryGroupFilter));
            }
            break;
          }
          finally
          {
            if (enumerator2 is IDisposable disposable)
              disposable.Dispose();
          }
        case NotifyCollectionChangedAction.Replace:
          foreach (object oldItem in (IEnumerable) e.OldItems)
          {
            if (this.groupsMenuButton != null)
              this.groupsMenuButton.Items.Remove((object) this.GetFilterMenuItem(oldItem as GalleryGroupFilter));
          }
          IEnumerator enumerator3 = e.NewItems.GetEnumerator();
          try
          {
            while (enumerator3.MoveNext())
            {
              object current = enumerator3.Current;
              if (this.groupsMenuButton != null)
              {
                GalleryGroupFilter galleryGroupFilter = (GalleryGroupFilter) current;
                MenuItem newItem = new MenuItem();
                newItem.Header = (object) galleryGroupFilter.Title;
                newItem.Tag = (object) galleryGroupFilter;
                newItem.IsDefinitive = false;
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
            if (enumerator3 is IDisposable disposable)
              disposable.Dispose();
          }
        case NotifyCollectionChangedAction.Reset:
          if (this.groupsMenuButton == null)
            break;
          this.groupsMenuButton.Items.Clear();
          break;
      }
    }

    public GalleryGroupFilter SelectedFilter
    {
      get => (GalleryGroupFilter) this.GetValue(InRibbonGallery.SelectedFilterProperty);
      set => this.SetValue(InRibbonGallery.SelectedFilterProperty, (object) value);
    }

    private static object CoerceSelectedFilter(DependencyObject d, object basevalue)
    {
      InRibbonGallery inRibbonGallery = (InRibbonGallery) d;
      return basevalue == null && inRibbonGallery.Filters.Count > 0 ? (object) inRibbonGallery.Filters[0] : basevalue;
    }

    private static void OnFilterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      InRibbonGallery inRibbonGallery = (InRibbonGallery) d;
      if (e.OldValue is GalleryGroupFilter oldValue)
      {
        System.Windows.Controls.MenuItem filterMenuItem = (System.Windows.Controls.MenuItem) inRibbonGallery.GetFilterMenuItem(oldValue);
        if (filterMenuItem != null)
          filterMenuItem.IsChecked = false;
      }
      if (e.NewValue is GalleryGroupFilter newValue)
      {
        inRibbonGallery.SelectedFilterTitle = newValue.Title;
        inRibbonGallery.SelectedFilterGroups = newValue.Groups;
        System.Windows.Controls.MenuItem filterMenuItem = (System.Windows.Controls.MenuItem) inRibbonGallery.GetFilterMenuItem(newValue);
        if (filterMenuItem != null)
          filterMenuItem.IsChecked = true;
      }
      else
      {
        inRibbonGallery.SelectedFilterTitle = "";
        inRibbonGallery.SelectedFilterGroups = (string) null;
      }
      inRibbonGallery.UpdateLayout();
    }

    public string SelectedFilterTitle
    {
      get => (string) this.GetValue(InRibbonGallery.SelectedFilterTitleProperty);
      private set => this.SetValue(InRibbonGallery.SelectedFilterTitlePropertyKey, (object) value);
    }

    public string SelectedFilterGroups
    {
      get => (string) this.GetValue(InRibbonGallery.SelectedFilterGroupsProperty);
      private set => this.SetValue(InRibbonGallery.SelectedFilterGroupsPropertyKey, (object) value);
    }

    public bool HasFilter
    {
      get => (bool) this.GetValue(InRibbonGallery.HasFilterProperty);
      private set => this.SetValue(InRibbonGallery.HasFilterPropertyKey, (object) value);
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
      if (filter == null)
        return (MenuItem) null;
      return this.groupsMenuButton == null ? (MenuItem) null : this.groupsMenuButton.Items.Cast<MenuItem>().FirstOrDefault<MenuItem>((Func<MenuItem, bool>) (item => item != null && item.Header.ToString() == filter.Title));
    }

    public bool Selectable
    {
      get => (bool) this.GetValue(InRibbonGallery.SelectableProperty);
      set => this.SetValue(InRibbonGallery.SelectableProperty, (object) value);
    }

    private static void OnSelectableChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      d.CoerceValue(Selector.SelectedItemProperty);
    }

    public Popup DropDownPopup => this.popup;

    public bool IsContextMenuOpened { get; set; }

    public bool IsDropDownOpen
    {
      get => (bool) this.GetValue(InRibbonGallery.IsDropDownOpenProperty);
      set => this.SetValue(InRibbonGallery.IsDropDownOpenProperty, (object) value);
    }

    public ContextMenuResizeMode ResizeMode
    {
      get => (ContextMenuResizeMode) this.GetValue(InRibbonGallery.ResizeModeProperty);
      set => this.SetValue(InRibbonGallery.ResizeModeProperty, (object) value);
    }

    public bool CanCollapseToButton
    {
      get => (bool) this.GetValue(InRibbonGallery.CanCollapseToButtonProperty);
      set => this.SetValue(InRibbonGallery.CanCollapseToButtonProperty, (object) value);
    }

    public bool IsCollapsed
    {
      get => (bool) this.GetValue(InRibbonGallery.IsCollapsedProperty);
      set => this.SetValue(InRibbonGallery.IsCollapsedProperty, (object) value);
    }

    public ImageSource LargeIcon
    {
      get => (ImageSource) this.GetValue(InRibbonGallery.LargeIconProperty);
      set => this.SetValue(InRibbonGallery.LargeIconProperty, (object) value);
    }

    public bool IsSnapped
    {
      get => this.isSnapped;
      set
      {
        if (value == this.isSnapped || this.IsCollapsed || !this.IsVisible)
          return;
        if (value && (int) this.ActualWidth > 0 && (int) this.ActualHeight > 0)
        {
          RenderOptions.SetBitmapScalingMode((DependencyObject) this.snappedImage, BitmapScalingMode.NearestNeighbor);
          RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int) this.galleryPanel.ActualWidth, (int) this.galleryPanel.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);
          renderTargetBitmap.Render((Visual) this.galleryPanel);
          this.snappedImage.Source = (ImageSource) renderTargetBitmap;
          this.snappedImage.FlowDirection = this.FlowDirection;
          this.snappedImage.Width = this.galleryPanel.ActualWidth;
          this.snappedImage.Height = this.galleryPanel.ActualHeight;
          this.snappedImage.Visibility = Visibility.Visible;
          this.isSnapped = value;
        }
        else
        {
          this.snappedImage.Visibility = Visibility.Collapsed;
          this.isSnapped = value;
          this.InvalidateVisual();
        }
        this.InvalidateVisual();
      }
    }

    public RibbonMenu Menu
    {
      get => (RibbonMenu) this.GetValue(InRibbonGallery.MenuProperty);
      set => this.SetValue(InRibbonGallery.MenuProperty, (object) value);
    }

    public int MaxItemsInRow
    {
      get => (int) this.GetValue(InRibbonGallery.MaxItemsInRowProperty);
      set => this.SetValue(InRibbonGallery.MaxItemsInRowProperty, (object) value);
    }

    public int MinItemsInRow
    {
      get => (int) this.GetValue(InRibbonGallery.MinItemsInRowProperty);
      set => this.SetValue(InRibbonGallery.MinItemsInRowProperty, (object) value);
    }

    private static void OnMaxItemsInRowChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      InRibbonGallery inRibbonGallery = d as InRibbonGallery;
      int newValue = (int) e.NewValue;
      if (inRibbonGallery.IsDropDownOpen || inRibbonGallery.galleryPanel == null || inRibbonGallery.galleryPanel.MinItemsInRow >= newValue)
        return;
      inRibbonGallery.galleryPanel.MinItemsInRow = newValue;
      inRibbonGallery.galleryPanel.MaxItemsInRow = newValue;
    }

    public double MaxDropDownHeight
    {
      get => (double) this.GetValue(InRibbonGallery.MaxDropDownHeightProperty);
      set => this.SetValue(InRibbonGallery.MaxDropDownHeightProperty, (object) value);
    }

    public event EventHandler Scaled;

    public event EventHandler DropDownOpened;

    public event EventHandler DropDownClosed;

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static InRibbonGallery()
    {
      Type type = typeof (InRibbonGallery);
      ToolTipService.Attach(type);
      PopupService.Attach(type);
      ContextMenuService.Attach(type);
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) type));
      FrameworkElement.StyleProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(InRibbonGallery.OnCoerceStyle)));
      Selector.SelectedItemProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(InRibbonGallery.CoerceSelectedItem)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (InRibbonGallery));
      return basevalue;
    }

    private static object CoerceSelectedItem(DependencyObject d, object basevalue)
    {
      InRibbonGallery inRibbonGallery = d as InRibbonGallery;
      if (inRibbonGallery.Selectable)
        return basevalue;
      if (basevalue != null)
        (inRibbonGallery.ItemContainerGenerator.ContainerFromItem(basevalue) as GalleryItem).IsSelected = false;
      return (object) null;
    }

    public InRibbonGallery() => ContextMenuService.Coerce((DependencyObject) this);

    public void OnKeyTipPressed() => this.IsDropDownOpen = true;

    protected override void OnSelectionChanged(SelectionChangedEventArgs e)
    {
      foreach (object removedItem in (IEnumerable) e.RemovedItems)
      {
        if (this.ItemContainerGenerator.ContainerFromItem(removedItem) is GalleryItem galleryItem)
          galleryItem.IsSelected = false;
      }
      foreach (object addedItem in (IEnumerable) e.AddedItems)
      {
        if (this.ItemContainerGenerator.ContainerFromItem(addedItem) is GalleryItem galleryItem)
          galleryItem.IsSelected = true;
      }
      base.OnSelectionChanged(e);
    }

    public override void OnApplyTemplate()
    {
      if (this.expandButton != null)
        this.expandButton.Click -= new RoutedEventHandler(this.OnExpandClick);
      this.expandButton = this.GetTemplateChild("PART_ExpandButton") as ToggleButton;
      if (this.expandButton != null)
        this.expandButton.Click += new RoutedEventHandler(this.OnExpandClick);
      if (this.dropDownButton != null)
        this.dropDownButton.Click -= new RoutedEventHandler(this.OnDropDownClick);
      this.dropDownButton = this.GetTemplateChild("PART_DropDownButton") as ToggleButton;
      if (this.dropDownButton != null)
        this.dropDownButton.Click += new RoutedEventHandler(this.OnDropDownClick);
      this.layoutRoot = this.GetTemplateChild("PART_LayoutRoot") as Panel;
      if (this.popup != null)
      {
        this.popup.Opened -= new EventHandler(this.OnDropDownOpened);
        this.popup.Closed -= new EventHandler(this.OnDropDownClosed);
        this.popup.PreviewMouseLeftButtonUp -= new MouseButtonEventHandler(this.OnPopupPreviewMouseUp);
        this.popup.PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(this.OnPopupPreviewMouseDown);
      }
      this.popup = this.GetTemplateChild("PART_Popup") as Popup;
      if (this.popup != null)
      {
        this.popup.Opened += new EventHandler(this.OnDropDownOpened);
        this.popup.Closed += new EventHandler(this.OnDropDownClosed);
        this.popup.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this.OnPopupPreviewMouseUp);
        this.popup.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.OnPopupPreviewMouseDown);
        KeyboardNavigation.SetControlTabNavigation((DependencyObject) this.popup, KeyboardNavigationMode.Cycle);
        KeyboardNavigation.SetDirectionalNavigation((DependencyObject) this.popup, KeyboardNavigationMode.Cycle);
        KeyboardNavigation.SetTabNavigation((DependencyObject) this.popup, KeyboardNavigationMode.Cycle);
      }
      if (this.resizeVerticalThumb != null)
        this.resizeVerticalThumb.DragDelta -= new DragDeltaEventHandler(this.OnResizeVerticalDelta);
      this.resizeVerticalThumb = this.GetTemplateChild("PART_ResizeVerticalThumb") as Thumb;
      if (this.resizeVerticalThumb != null)
        this.resizeVerticalThumb.DragDelta += new DragDeltaEventHandler(this.OnResizeVerticalDelta);
      if (this.resizeBothThumb != null)
        this.resizeBothThumb.DragDelta -= new DragDeltaEventHandler(this.OnResizeBothDelta);
      this.resizeBothThumb = this.GetTemplateChild("PART_ResizeBothThumb") as Thumb;
      if (this.resizeBothThumb != null)
        this.resizeBothThumb.DragDelta += new DragDeltaEventHandler(this.OnResizeBothDelta);
      this.menuPanel = this.GetTemplateChild("PART_MenuPanel") as Panel;
      if (this.groupsMenuButton != null)
        this.groupsMenuButton.Items.Clear();
      this.groupsMenuButton = this.GetTemplateChild("PART_FilterDropDownButton") as DropDownButton;
      if (this.groupsMenuButton != null)
      {
        for (int index = 0; index < this.Filters.Count; ++index)
        {
          MenuItem newItem = new MenuItem();
          newItem.Header = (object) this.Filters[index].Title;
          newItem.Tag = (object) this.Filters[index];
          newItem.IsDefinitive = false;
          if (this.Filters[index] == this.SelectedFilter)
            newItem.IsChecked = true;
          newItem.Click += new RoutedEventHandler(this.OnFilterMenuItemClick);
          this.groupsMenuButton.Items.Add((object) newItem);
        }
      }
      this.galleryPanel = this.GetTemplateChild("PART_GalleryPanel") as GalleryPanel;
      if (this.galleryPanel != null)
      {
        this.galleryPanel.MinItemsInRow = this.MaxItemsInRow;
        this.galleryPanel.MaxItemsInRow = this.MaxItemsInRow;
      }
      this.snappedImage = this.GetTemplateChild("PART_FakeImage") as Image;
      this.controlPresenter = this.GetTemplateChild("PART_ContentPresenter") as ContentControl;
      this.popupControlPresenter = this.GetTemplateChild("PART_PopupContentPresenter") as ContentControl;
      this.scrollViewer = this.GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
    }

    private void OnPopupPreviewMouseUp(object sender, MouseButtonEventArgs e)
    {
      if (!this.isButtonClicked)
        return;
      this.isButtonClicked = false;
      e.Handled = true;
    }

    private void OnPopupPreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
      this.isButtonClicked = false;
    }

    private void OnExpandClick(object sender, RoutedEventArgs e) => this.isButtonClicked = true;

    private void OnDropDownClick(object sender, RoutedEventArgs e)
    {
      if (!this.canOpenDropDown)
        return;
      this.IsDropDownOpen = true;
    }

    private void OnDropDownClosed(object sender, EventArgs e)
    {
      this.galleryPanel.Width = double.NaN;
      this.galleryPanel.IsGrouped = false;
      this.galleryPanel.MinItemsInRow = this.currentItemsInRow;
      this.galleryPanel.MaxItemsInRow = this.currentItemsInRow;
      this.popupControlPresenter.Content = (object) null;
      this.controlPresenter.Content = (object) this.galleryPanel;
      this.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, (Delegate) (() =>
      {
        if (this.quickAccessGallery != null && (this.quickAccessGallery == null || this.quickAccessGallery.IsDropDownOpen))
          return;
        this.IsSnapped = false;
      }));
      if (this.DropDownClosed != null)
        this.DropDownClosed((object) this, e);
      if (Mouse.Captured == this)
        Mouse.Capture((IInputElement) null);
      this.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, (Delegate) (() =>
      {
        if (!(this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem) is GalleryItem galleryItem2))
          return;
        galleryItem2.BringIntoView();
      }));
      this.dropDownButton.IsChecked = new bool?(false);
      this.canOpenDropDown = true;
    }

    private void OnDropDownOpened(object sender, EventArgs e)
    {
      this.IsSnapped = true;
      this.minimalGallerylWidth = Math.Max(this.galleryPanel.ActualWidth, this.galleryPanel.GetActualMinWidth(this.MinItemsInDropDownRow));
      this.currentItemsInRow = this.galleryPanel.MinItemsInRow;
      this.controlPresenter.Content = (object) null;
      this.popupControlPresenter.Content = (object) this.galleryPanel;
      this.galleryPanel.Width = double.NaN;
      this.scrollViewer.Height = double.NaN;
      if (this.DropDownOpened != null)
        this.DropDownOpened((object) this, e);
      this.galleryPanel.MinItemsInRow = Math.Max(this.currentItemsInRow, this.MinItemsInDropDownRow);
      this.galleryPanel.MaxItemsInRow = this.MaxItemsInDropDownRow;
      this.galleryPanel.IsGrouped = true;
      this.dropDownButton.IsChecked = new bool?(true);
      this.canOpenDropDown = false;
      Mouse.Capture((IInputElement) this, CaptureMode.SubTree);
      this.focusedElement = Keyboard.FocusedElement;
      if (this.focusedElement == null)
        return;
      this.focusedElement.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.OnFocusedElementLostKeyboardFocus);
      this.focusedElement.PreviewKeyDown += new KeyEventHandler(this.OnFocusedElementPreviewKeyDown);
    }

    protected void OnSizePropertyChanged(RibbonControlSize previous, RibbonControlSize current)
    {
      if (this.CanCollapseToButton)
      {
        if (current == RibbonControlSize.Large && this.galleryPanel.MinItemsInRow > this.MinItemsInRow)
          this.IsCollapsed = false;
        else
          this.IsCollapsed = true;
      }
      else
        this.IsCollapsed = false;
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
      return (DependencyObject) new GalleryItem();
    }

    protected override bool IsItemItsOwnContainerOverride(object item) => item is GalleryItem;

    protected override void OnKeyDown(KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
        this.IsDropDownOpen = false;
      base.OnKeyDown(e);
    }

    private void OnFocusedElementPreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Escape)
        return;
      this.IsDropDownOpen = false;
    }

    private void OnFocusedElementLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
      this.focusedElement.LostKeyboardFocus -= new KeyboardFocusChangedEventHandler(this.OnFocusedElementLostKeyboardFocus);
      this.focusedElement.PreviewKeyDown -= new KeyEventHandler(this.OnFocusedElementPreviewKeyDown);
    }

    private void OnResizeBothDelta(object sender, DragDeltaEventArgs e)
    {
      if (double.IsNaN(this.scrollViewer.Height))
        this.scrollViewer.Height = this.scrollViewer.ActualHeight;
      this.scrollViewer.Height = Math.Min(Math.Max(this.galleryPanel.GetItemSize().Height, this.scrollViewer.Height + e.VerticalChange), this.MaxDropDownHeight);
      this.menuPanel.Width = double.NaN;
      if (double.IsNaN(this.galleryPanel.Width))
        this.galleryPanel.Width = 500.0;
      this.galleryPanel.Width = Math.Max(this.galleryPanel.Width + e.HorizontalChange, this.minimalGallerylWidth);
    }

    private void OnResizeVerticalDelta(object sender, DragDeltaEventArgs e)
    {
      if (double.IsNaN(this.scrollViewer.Height))
        this.scrollViewer.Height = this.scrollViewer.ActualHeight;
      this.scrollViewer.Height = Math.Min(Math.Max(this.galleryPanel.GetItemSize().Height, this.scrollViewer.Height + e.VerticalChange), this.MaxDropDownHeight);
    }

    public virtual FrameworkElement CreateQuickAccessItem()
    {
      InRibbonGallery quickAccessItem = new InRibbonGallery();
      RibbonControl.BindQuickAccessItem((FrameworkElement) this, (FrameworkElement) quickAccessItem);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "GroupBy", InRibbonGallery.GroupByProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "ItemHeight", InRibbonGallery.ItemHeightProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "ItemWidth", InRibbonGallery.ItemWidthProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "ResizeMode", InRibbonGallery.ResizeModeProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "MinItemsInDropDownRow", InRibbonGallery.MinItemsInDropDownRowProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "MaxItemsInDropDownRow", InRibbonGallery.MaxItemsInDropDownRowProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "DisplayMemberPath", ItemsControl.DisplayMemberPathProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "GroupStyleSelector", ItemsControl.GroupStyleSelectorProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "ItemContainerStyle", ItemsControl.ItemContainerStyleProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "ItemsPanel", ItemsControl.ItemsPanelProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "ItemStringFormat", ItemsControl.ItemStringFormatProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "ItemTemplate", ItemsControl.ItemTemplateProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "SelectedValuePath", Selector.SelectedValuePathProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "MaxDropDownHeight", InRibbonGallery.MaxDropDownHeightProperty, BindingMode.OneWay);
      quickAccessItem.DropDownOpened += new EventHandler(this.OnQuickAccessOpened);
      quickAccessItem.Size = RibbonControlSize.Small;
      this.quickAccessGallery = quickAccessItem;
      return (FrameworkElement) quickAccessItem;
    }

    private void OnQuickAccessOpened(object sender, EventArgs e)
    {
      for (int index = 0; index < this.Filters.Count; ++index)
        this.quickAccessGallery.Filters.Add(this.Filters[index]);
      this.quickAccessGallery.SelectedFilter = this.SelectedFilter;
      this.quickAccessGallery.DropDownClosed += new EventHandler(this.OnQuickAccessMenuClosed);
      this.UpdateLayout();
      this.Dispatcher.BeginInvoke(DispatcherPriority.Render, (Delegate) (() => this.Freeze()));
    }

    private void OnQuickAccessMenuClosed(object sender, EventArgs e)
    {
      this.quickAccessGallery.DropDownClosed -= new EventHandler(this.OnQuickAccessMenuClosed);
      this.SelectedFilter = this.quickAccessGallery.SelectedFilter;
      this.quickAccessGallery.Filters.Clear();
      this.Unfreeze();
    }

    private void Freeze()
    {
      this.IsSnapped = true;
      this.selectedItem = this.SelectedItem;
      this.SelectedItem = (object) null;
      if (this.ItemsSource != null)
      {
        this.quickAccessGallery.ItemsSource = this.ItemsSource;
        this.ItemsSource = (IEnumerable) null;
      }
      else
      {
        for (int index = 0; index < this.Items.Count; index = index - 1 + 1)
        {
          object obj = this.Items[0];
          this.Items.Remove(obj);
          this.quickAccessGallery.Items.Add(obj);
        }
      }
      this.quickAccessGallery.SelectedItem = this.selectedItem;
      this.quickAccessGallery.Menu = this.Menu;
      this.Menu = (RibbonMenu) null;
    }

    private void Unfreeze()
    {
      this.selectedItem = this.quickAccessGallery.SelectedItem;
      this.quickAccessGallery.SelectedItem = (object) null;
      if (this.quickAccessGallery.ItemsSource != null)
      {
        this.ItemsSource = this.quickAccessGallery.ItemsSource;
        this.quickAccessGallery.ItemsSource = (IEnumerable) null;
      }
      else
      {
        for (int index = 0; index < this.quickAccessGallery.Items.Count; index = index - 1 + 1)
        {
          object obj = this.quickAccessGallery.Items[0];
          this.quickAccessGallery.Items.Remove(obj);
          this.Items.Add(obj);
        }
      }
      this.SelectedItem = this.selectedItem;
      this.Menu = this.quickAccessGallery.Menu;
      this.quickAccessGallery.Menu = (RibbonMenu) null;
      if (!this.IsDropDownOpen)
      {
        this.controlPresenter.Content = (object) null;
        this.popupControlPresenter.Content = (object) this.galleryPanel;
        this.galleryPanel.IsGrouped = true;
        this.galleryPanel.IsGrouped = false;
        this.popupControlPresenter.Content = (object) null;
        this.controlPresenter.Content = (object) this.galleryPanel;
      }
      this.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, (Delegate) (() =>
      {
        if (!this.IsDropDownOpen)
          this.IsSnapped = false;
        if (!(this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem) is GalleryItem galleryItem2))
          return;
        galleryItem2.BringIntoView();
      }));
    }

    private void OnItemsContainerGeneratorStatusChanged(object sender, EventArgs e)
    {
      if (this.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
        return;
      this.SelectedItem = this.selectedItem;
      this.ItemContainerGenerator.StatusChanged -= new EventHandler(this.OnItemsContainerGeneratorStatusChanged);
    }

    public bool CanAddToQuickAccessToolBar
    {
      get => (bool) this.GetValue(InRibbonGallery.CanAddToQuickAccessToolBarProperty);
      set => this.SetValue(InRibbonGallery.CanAddToQuickAccessToolBarProperty, (object) value);
    }

    public Style QuickAccessElementStyle
    {
      get => (Style) this.GetValue(InRibbonGallery.QuickAccessElementStyleProperty);
      set => this.SetValue(InRibbonGallery.QuickAccessElementStyleProperty, (object) value);
    }

    public void Enlarge()
    {
      if (this.IsCollapsed && this.Size == RibbonControlSize.Large)
      {
        this.IsCollapsed = false;
      }
      else
      {
        if (this.galleryPanel.MinItemsInRow >= this.MaxItemsInRow)
          return;
        ++this.galleryPanel.MinItemsInRow;
        this.galleryPanel.MaxItemsInRow = this.galleryPanel.MinItemsInRow;
      }
      this.InvalidateMeasure();
      if (this.Scaled == null)
        return;
      this.Scaled((object) this, EventArgs.Empty);
    }

    public void Reduce()
    {
      if (this.galleryPanel.MinItemsInRow > this.MinItemsInRow)
      {
        --this.galleryPanel.MinItemsInRow;
        this.galleryPanel.MaxItemsInRow = this.galleryPanel.MinItemsInRow;
      }
      else
      {
        if (!this.CanCollapseToButton || this.IsCollapsed)
          return;
        this.IsCollapsed = true;
      }
      this.InvalidateMeasure();
      if (this.Scaled == null)
        return;
      this.Scaled((object) this, EventArgs.Empty);
    }
  }
}
