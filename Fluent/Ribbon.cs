// Decompiled with JetBrains decompiler
// Type: Fluent.Ribbon
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace Fluent
{
  [SuppressMessage("Microsoft.Design", "CA1001")]
  [ContentProperty("Tabs")]
  [SuppressMessage("Microsoft.Maintainability", "CA1506")]
  public class Ribbon : Control
  {
    public const double MinimalVisibleWidth = 300.0;
    public const double MinimalVisibleHeight = 250.0;
    private static readonly RibbonLocalization localization = new RibbonLocalization();
    private static Dictionary<int, System.Windows.Controls.ContextMenu> contextMenus = new Dictionary<int, System.Windows.Controls.ContextMenu>();
    private static Ribbon contextMenuOwner;
    private static Dictionary<int, System.Windows.Controls.MenuItem> addToQuickAccessMenuItemDictionary = new Dictionary<int, System.Windows.Controls.MenuItem>();
    private static Dictionary<int, System.Windows.Controls.MenuItem> addGroupToQuickAccessMenuItemDictionary = new Dictionary<int, System.Windows.Controls.MenuItem>();
    private static Dictionary<int, System.Windows.Controls.MenuItem> addMenuToQuickAccessMenuItemDictionary = new Dictionary<int, System.Windows.Controls.MenuItem>();
    private static Dictionary<int, System.Windows.Controls.MenuItem> addGalleryToQuickAccessMenuItemDictionary = new Dictionary<int, System.Windows.Controls.MenuItem>();
    private static Dictionary<int, System.Windows.Controls.MenuItem> removeFromQuickAccessMenuItemDictionary = new Dictionary<int, System.Windows.Controls.MenuItem>();
    private static Dictionary<int, System.Windows.Controls.MenuItem> showQuickAccessToolbarBelowTheRibbonMenuItemDictionary = new Dictionary<int, System.Windows.Controls.MenuItem>();
    private static Dictionary<int, System.Windows.Controls.MenuItem> showQuickAccessToolbarAboveTheRibbonMenuItemDictionary = new Dictionary<int, System.Windows.Controls.MenuItem>();
    private static Dictionary<int, System.Windows.Controls.MenuItem> minimizeTheRibbonMenuItemDictionary = new Dictionary<int, System.Windows.Controls.MenuItem>();
    private static Dictionary<int, System.Windows.Controls.MenuItem> customizeQuickAccessToolbarMenuItemDictionary = new Dictionary<int, System.Windows.Controls.MenuItem>();
    private static Dictionary<int, System.Windows.Controls.MenuItem> customizeTheRibbonMenuItemDictionary = new Dictionary<int, System.Windows.Controls.MenuItem>();
    private static Dictionary<int, Separator> firstSeparatorDictionary = new Dictionary<int, Separator>();
    private static Dictionary<int, Separator> secondSeparatorDictionary = new Dictionary<int, Separator>();
    private ObservableCollection<RibbonContextualTabGroup> groups;
    private ObservableCollection<RibbonTabItem> tabs;
    private ObservableCollection<UIElement> toolBarItems;
    private RibbonTitleBar titleBar;
    private RibbonTabControl tabControl;
    private QuickAccessToolBar quickAccessToolBar;
    private Panel layoutRoot;
    private readonly KeyTipService keyTipService;
    private ObservableCollection<QuickAccessMenuItem> quickAccessItems;
    private readonly Dictionary<UIElement, UIElement> quickAccessElements = new Dictionary<UIElement, UIElement>();
    private MemoryStream quickAccessStream;
    private Window ownerWindow;
    public static readonly DependencyProperty MenuProperty = DependencyProperty.Register(nameof (Menu), typeof (UIElement), typeof (Ribbon), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(Ribbon.OnApplicationMenuChanged)));
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof (Title), typeof (string), typeof (Ribbon), (PropertyMetadata) new UIPropertyMetadata((object) "", new PropertyChangedCallback(Ribbon.OnTitleChanged)));
    public static readonly DependencyProperty SelectedTabItemProperty = DependencyProperty.Register(nameof (SelectedTabItem), typeof (RibbonTabItem), typeof (Ribbon), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(Ribbon.OnSelectedTabItemChanged)));
    public static readonly DependencyProperty SelectedTabIndexProperty = DependencyProperty.Register(nameof (SelectedTabIndex), typeof (int), typeof (Ribbon), (PropertyMetadata) new UIPropertyMetadata((object) -1, new PropertyChangedCallback(Ribbon.OnSelectedTabIndexChanged)));
    public static readonly DependencyProperty ShowQuickAccessToolBarAboveRibbonProperty = DependencyProperty.Register(nameof (ShowQuickAccessToolBarAboveRibbon), typeof (bool), typeof (Ribbon), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(Ribbon.OnShowQuickAccesToolBarAboveRibbonChanged)));
    public static readonly DependencyProperty CanCustomizeQuickAccessToolBarProperty = DependencyProperty.Register(nameof (CanCustomizeQuickAccessToolBar), typeof (bool), typeof (Ribbon), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty CanCustomizeRibbonProperty = DependencyProperty.Register(nameof (CanCustomizeRibbon), typeof (bool), typeof (Ribbon), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty IsMinimizedProperty = DependencyProperty.Register(nameof (IsMinimized), typeof (bool), typeof (Ribbon), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(Ribbon.OnIsMinimizedChanged)));
    private static readonly DependencyPropertyKey IsCollapsedPropertyKey = DependencyProperty.RegisterReadOnly(nameof (IsCollapsed), typeof (bool), typeof (Ribbon), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(Ribbon.OnIsCollapsedChanged)));
    public static readonly DependencyProperty IsCollapsedProperty = Ribbon.IsCollapsedPropertyKey.DependencyProperty;
    public static readonly DependencyProperty IsQuickAccessToolBarVisibleProperty = DependencyProperty.Register(nameof (IsQuickAccessToolBarVisible), typeof (bool), typeof (Ribbon), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty CanQuickAccessLocationChangingProperty = DependencyProperty.Register(nameof (CanQuickAccessLocationChanging), typeof (bool), typeof (Ribbon), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static RoutedCommand AddToQuickAccessCommand = new RoutedCommand(nameof (AddToQuickAccessCommand), typeof (Ribbon));
    public static RoutedCommand RemoveFromQuickAccessCommand = new RoutedCommand(nameof (RemoveFromQuickAccessCommand), typeof (Ribbon));
    public static RoutedCommand ShowQuickAccessAboveCommand = new RoutedCommand(nameof (ShowQuickAccessAboveCommand), typeof (Ribbon));
    public static RoutedCommand ShowQuickAccessBelowCommand = new RoutedCommand(nameof (ShowQuickAccessBelowCommand), typeof (Ribbon));
    public static RoutedCommand ToggleMinimizeTheRibbonCommand = new RoutedCommand(nameof (ToggleMinimizeTheRibbonCommand), typeof (Ribbon));
    public static RoutedCommand CustomizeQuickAccessToolbarCommand = new RoutedCommand(nameof (CustomizeQuickAccessToolbarCommand), typeof (Ribbon));
    public static RoutedCommand CustomizeTheRibbonCommand = new RoutedCommand(nameof (CustomizeTheRibbonCommand), typeof (Ribbon));
    private string isolatedStorageFileName;
    private bool suppressAutomaticStateManagement;
    public static readonly DependencyProperty AutomaticStateManagementProperty = DependencyProperty.Register(nameof (AutomaticStateManagement), typeof (bool), typeof (Ribbon), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(Ribbon.OnAutoStateManagement), new CoerceValueCallback(Ribbon.CoerceAutoStateManagement)));

    public static RibbonLocalization Localization => Ribbon.localization;

    public static System.Windows.Controls.ContextMenu RibbonContextMenu
    {
      get
      {
        if (!Ribbon.contextMenus.ContainsKey(Thread.CurrentThread.ManagedThreadId))
          Ribbon.InitRibbonContextMenu();
        return Ribbon.contextMenus[Thread.CurrentThread.ManagedThreadId];
      }
    }

    private static System.Windows.Controls.MenuItem addToQuickAccessMenuItem
    {
      get => Ribbon.addToQuickAccessMenuItemDictionary[Thread.CurrentThread.ManagedThreadId];
    }

    private static System.Windows.Controls.MenuItem addGroupToQuickAccessMenuItem
    {
      get => Ribbon.addGroupToQuickAccessMenuItemDictionary[Thread.CurrentThread.ManagedThreadId];
    }

    private static System.Windows.Controls.MenuItem addMenuToQuickAccessMenuItem
    {
      get => Ribbon.addMenuToQuickAccessMenuItemDictionary[Thread.CurrentThread.ManagedThreadId];
    }

    private static System.Windows.Controls.MenuItem addGalleryToQuickAccessMenuItem
    {
      get => Ribbon.addGalleryToQuickAccessMenuItemDictionary[Thread.CurrentThread.ManagedThreadId];
    }

    private static System.Windows.Controls.MenuItem removeFromQuickAccessMenuItem
    {
      get => Ribbon.removeFromQuickAccessMenuItemDictionary[Thread.CurrentThread.ManagedThreadId];
    }

    private static System.Windows.Controls.MenuItem showQuickAccessToolbarBelowTheRibbonMenuItem
    {
      get
      {
        return Ribbon.showQuickAccessToolbarBelowTheRibbonMenuItemDictionary[Thread.CurrentThread.ManagedThreadId];
      }
    }

    private static System.Windows.Controls.MenuItem showQuickAccessToolbarAboveTheRibbonMenuItem
    {
      get
      {
        return Ribbon.showQuickAccessToolbarAboveTheRibbonMenuItemDictionary[Thread.CurrentThread.ManagedThreadId];
      }
    }

    private static System.Windows.Controls.MenuItem minimizeTheRibbonMenuItem
    {
      get => Ribbon.minimizeTheRibbonMenuItemDictionary[Thread.CurrentThread.ManagedThreadId];
    }

    private static System.Windows.Controls.MenuItem customizeQuickAccessToolbarMenuItem
    {
      get
      {
        return Ribbon.customizeQuickAccessToolbarMenuItemDictionary[Thread.CurrentThread.ManagedThreadId];
      }
    }

    private static System.Windows.Controls.MenuItem customizeTheRibbonMenuItem
    {
      get => Ribbon.customizeTheRibbonMenuItemDictionary[Thread.CurrentThread.ManagedThreadId];
    }

    private static Separator firstSeparator
    {
      get => Ribbon.firstSeparatorDictionary[Thread.CurrentThread.ManagedThreadId];
    }

    private static Separator secondSeparator
    {
      get => Ribbon.secondSeparatorDictionary[Thread.CurrentThread.ManagedThreadId];
    }

    private static void InitRibbonContextMenu()
    {
      Ribbon.contextMenus.Add(Thread.CurrentThread.ManagedThreadId, new System.Windows.Controls.ContextMenu());
      Ribbon.RibbonContextMenu.Opened += new RoutedEventHandler(Ribbon.OnContextMenuOpened);
      Ribbon.addToQuickAccessMenuItemDictionary.Add(Thread.CurrentThread.ManagedThreadId, new System.Windows.Controls.MenuItem()
      {
        Command = (ICommand) Ribbon.AddToQuickAccessCommand
      });
      Ribbon.RibbonContextMenu.Items.Add((object) Ribbon.addToQuickAccessMenuItem);
      RibbonControl.Bind((object) Ribbon.Localization, (FrameworkElement) Ribbon.addToQuickAccessMenuItem, "RibbonContextMenuAddItem", HeaderedItemsControl.HeaderProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) Ribbon.RibbonContextMenu, (FrameworkElement) Ribbon.addToQuickAccessMenuItem, "PlacementTarget", System.Windows.Controls.MenuItem.CommandParameterProperty, BindingMode.OneWay);
      Ribbon.addGroupToQuickAccessMenuItemDictionary.Add(Thread.CurrentThread.ManagedThreadId, new System.Windows.Controls.MenuItem()
      {
        Command = (ICommand) Ribbon.AddToQuickAccessCommand
      });
      Ribbon.RibbonContextMenu.Items.Add((object) Ribbon.addGroupToQuickAccessMenuItem);
      RibbonControl.Bind((object) Ribbon.Localization, (FrameworkElement) Ribbon.addGroupToQuickAccessMenuItem, "RibbonContextMenuAddGroup", HeaderedItemsControl.HeaderProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) Ribbon.RibbonContextMenu, (FrameworkElement) Ribbon.addGroupToQuickAccessMenuItem, "PlacementTarget", System.Windows.Controls.MenuItem.CommandParameterProperty, BindingMode.OneWay);
      Ribbon.addMenuToQuickAccessMenuItemDictionary.Add(Thread.CurrentThread.ManagedThreadId, new System.Windows.Controls.MenuItem()
      {
        Command = (ICommand) Ribbon.AddToQuickAccessCommand
      });
      Ribbon.RibbonContextMenu.Items.Add((object) Ribbon.addMenuToQuickAccessMenuItem);
      RibbonControl.Bind((object) Ribbon.Localization, (FrameworkElement) Ribbon.addMenuToQuickAccessMenuItem, "RibbonContextMenuAddMenu", HeaderedItemsControl.HeaderProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) Ribbon.RibbonContextMenu, (FrameworkElement) Ribbon.addMenuToQuickAccessMenuItem, "PlacementTarget", System.Windows.Controls.MenuItem.CommandParameterProperty, BindingMode.OneWay);
      Ribbon.addGalleryToQuickAccessMenuItemDictionary.Add(Thread.CurrentThread.ManagedThreadId, new System.Windows.Controls.MenuItem()
      {
        Command = (ICommand) Ribbon.AddToQuickAccessCommand
      });
      Ribbon.RibbonContextMenu.Items.Add((object) Ribbon.addGalleryToQuickAccessMenuItem);
      RibbonControl.Bind((object) Ribbon.Localization, (FrameworkElement) Ribbon.addGalleryToQuickAccessMenuItem, "RibbonContextMenuAddGallery", HeaderedItemsControl.HeaderProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) Ribbon.RibbonContextMenu, (FrameworkElement) Ribbon.addGalleryToQuickAccessMenuItem, "PlacementTarget", System.Windows.Controls.MenuItem.CommandParameterProperty, BindingMode.OneWay);
      Ribbon.removeFromQuickAccessMenuItemDictionary.Add(Thread.CurrentThread.ManagedThreadId, new System.Windows.Controls.MenuItem()
      {
        Command = (ICommand) Ribbon.RemoveFromQuickAccessCommand
      });
      Ribbon.RibbonContextMenu.Items.Add((object) Ribbon.removeFromQuickAccessMenuItem);
      RibbonControl.Bind((object) Ribbon.Localization, (FrameworkElement) Ribbon.removeFromQuickAccessMenuItem, "RibbonContextMenuRemoveItem", HeaderedItemsControl.HeaderProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) Ribbon.RibbonContextMenu, (FrameworkElement) Ribbon.removeFromQuickAccessMenuItem, "PlacementTarget", System.Windows.Controls.MenuItem.CommandParameterProperty, BindingMode.OneWay);
      Ribbon.firstSeparatorDictionary.Add(Thread.CurrentThread.ManagedThreadId, new Separator());
      Ribbon.RibbonContextMenu.Items.Add((object) Ribbon.firstSeparator);
      Ribbon.customizeQuickAccessToolbarMenuItemDictionary.Add(Thread.CurrentThread.ManagedThreadId, new System.Windows.Controls.MenuItem()
      {
        Command = (ICommand) Ribbon.CustomizeQuickAccessToolbarCommand
      });
      Ribbon.RibbonContextMenu.Items.Add((object) Ribbon.customizeQuickAccessToolbarMenuItem);
      RibbonControl.Bind((object) Ribbon.Localization, (FrameworkElement) Ribbon.customizeQuickAccessToolbarMenuItem, "RibbonContextMenuCustomizeQuickAccessToolBar", HeaderedItemsControl.HeaderProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) Ribbon.RibbonContextMenu, (FrameworkElement) Ribbon.customizeQuickAccessToolbarMenuItem, "PlacementTarget", System.Windows.Controls.MenuItem.CommandParameterProperty, BindingMode.OneWay);
      Ribbon.showQuickAccessToolbarBelowTheRibbonMenuItemDictionary.Add(Thread.CurrentThread.ManagedThreadId, new System.Windows.Controls.MenuItem()
      {
        Command = (ICommand) Ribbon.ShowQuickAccessBelowCommand
      });
      Ribbon.RibbonContextMenu.Items.Add((object) Ribbon.showQuickAccessToolbarBelowTheRibbonMenuItem);
      RibbonControl.Bind((object) Ribbon.Localization, (FrameworkElement) Ribbon.showQuickAccessToolbarBelowTheRibbonMenuItem, "RibbonContextMenuShowBelow", HeaderedItemsControl.HeaderProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) Ribbon.RibbonContextMenu, (FrameworkElement) Ribbon.showQuickAccessToolbarBelowTheRibbonMenuItem, "PlacementTarget", System.Windows.Controls.MenuItem.CommandParameterProperty, BindingMode.OneWay);
      Ribbon.showQuickAccessToolbarAboveTheRibbonMenuItemDictionary.Add(Thread.CurrentThread.ManagedThreadId, new System.Windows.Controls.MenuItem()
      {
        Command = (ICommand) Ribbon.ShowQuickAccessAboveCommand
      });
      Ribbon.RibbonContextMenu.Items.Add((object) Ribbon.showQuickAccessToolbarAboveTheRibbonMenuItem);
      RibbonControl.Bind((object) Ribbon.Localization, (FrameworkElement) Ribbon.showQuickAccessToolbarAboveTheRibbonMenuItem, "RibbonContextMenuShowAbove", HeaderedItemsControl.HeaderProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) Ribbon.RibbonContextMenu, (FrameworkElement) Ribbon.showQuickAccessToolbarAboveTheRibbonMenuItem, "PlacementTarget", System.Windows.Controls.MenuItem.CommandParameterProperty, BindingMode.OneWay);
      Ribbon.secondSeparatorDictionary.Add(Thread.CurrentThread.ManagedThreadId, new Separator());
      Ribbon.RibbonContextMenu.Items.Add((object) Ribbon.secondSeparator);
      Ribbon.customizeTheRibbonMenuItemDictionary.Add(Thread.CurrentThread.ManagedThreadId, new System.Windows.Controls.MenuItem()
      {
        Command = (ICommand) Ribbon.CustomizeTheRibbonCommand
      });
      Ribbon.RibbonContextMenu.Items.Add((object) Ribbon.customizeTheRibbonMenuItem);
      RibbonControl.Bind((object) Ribbon.Localization, (FrameworkElement) Ribbon.customizeTheRibbonMenuItem, "RibbonContextMenuCustomizeRibbon", HeaderedItemsControl.HeaderProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) Ribbon.RibbonContextMenu, (FrameworkElement) Ribbon.customizeTheRibbonMenuItem, "PlacementTarget", System.Windows.Controls.MenuItem.CommandParameterProperty, BindingMode.OneWay);
      Ribbon.minimizeTheRibbonMenuItemDictionary.Add(Thread.CurrentThread.ManagedThreadId, new System.Windows.Controls.MenuItem()
      {
        Command = (ICommand) Ribbon.ToggleMinimizeTheRibbonCommand
      });
      Ribbon.RibbonContextMenu.Items.Add((object) Ribbon.minimizeTheRibbonMenuItem);
      RibbonControl.Bind((object) Ribbon.Localization, (FrameworkElement) Ribbon.minimizeTheRibbonMenuItem, "RibbonContextMenuMinimizeRibbon", HeaderedItemsControl.HeaderProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) Ribbon.RibbonContextMenu, (FrameworkElement) Ribbon.minimizeTheRibbonMenuItem, "PlacementTarget", System.Windows.Controls.MenuItem.CommandParameterProperty, BindingMode.OneWay);
    }

    protected override void OnContextMenuOpening(ContextMenuEventArgs e)
    {
      Ribbon.contextMenuOwner = this;
      base.OnContextMenuOpening(e);
    }

    protected override void OnContextMenuClosing(ContextMenuEventArgs e)
    {
      Ribbon.contextMenuOwner = (Ribbon) null;
      base.OnContextMenuClosing(e);
    }

    private static void OnContextMenuOpened(object sender, RoutedEventArgs e)
    {
      Ribbon contextMenuOwner = Ribbon.contextMenuOwner;
      if (Ribbon.RibbonContextMenu == null || contextMenuOwner == null)
        return;
      Ribbon.addToQuickAccessMenuItem.CommandTarget = (IInputElement) contextMenuOwner;
      Ribbon.addGroupToQuickAccessMenuItem.CommandTarget = (IInputElement) contextMenuOwner;
      Ribbon.addMenuToQuickAccessMenuItem.CommandTarget = (IInputElement) contextMenuOwner;
      Ribbon.addGalleryToQuickAccessMenuItem.CommandTarget = (IInputElement) contextMenuOwner;
      Ribbon.removeFromQuickAccessMenuItem.CommandTarget = (IInputElement) contextMenuOwner;
      Ribbon.customizeQuickAccessToolbarMenuItem.CommandTarget = (IInputElement) contextMenuOwner;
      Ribbon.customizeTheRibbonMenuItem.CommandTarget = (IInputElement) contextMenuOwner;
      Ribbon.minimizeTheRibbonMenuItem.CommandTarget = (IInputElement) contextMenuOwner;
      Ribbon.showQuickAccessToolbarBelowTheRibbonMenuItem.CommandTarget = (IInputElement) contextMenuOwner;
      Ribbon.showQuickAccessToolbarAboveTheRibbonMenuItem.CommandTarget = (IInputElement) contextMenuOwner;
      Ribbon.addToQuickAccessMenuItem.Visibility = Visibility.Collapsed;
      Ribbon.addGroupToQuickAccessMenuItem.Visibility = Visibility.Collapsed;
      Ribbon.addMenuToQuickAccessMenuItem.Visibility = Visibility.Collapsed;
      Ribbon.addGalleryToQuickAccessMenuItem.Visibility = Visibility.Collapsed;
      Ribbon.removeFromQuickAccessMenuItem.Visibility = Visibility.Collapsed;
      Ribbon.firstSeparator.Visibility = Visibility.Collapsed;
      Ribbon.customizeQuickAccessToolbarMenuItem.Visibility = Visibility.Collapsed;
      Ribbon.secondSeparator.Visibility = Visibility.Visible;
      Ribbon.minimizeTheRibbonMenuItem.IsChecked = contextMenuOwner.IsMinimized;
      if (contextMenuOwner.CanCustomizeRibbon)
        Ribbon.customizeTheRibbonMenuItem.Visibility = Visibility.Visible;
      else
        Ribbon.customizeTheRibbonMenuItem.Visibility = Visibility.Collapsed;
      Ribbon.showQuickAccessToolbarBelowTheRibbonMenuItem.Visibility = Visibility.Collapsed;
      Ribbon.showQuickAccessToolbarAboveTheRibbonMenuItem.Visibility = Visibility.Collapsed;
      if (!contextMenuOwner.IsQuickAccessToolBarVisible)
        return;
      if (contextMenuOwner.ShowQuickAccessToolBarAboveRibbon)
        Ribbon.showQuickAccessToolbarBelowTheRibbonMenuItem.Visibility = Visibility.Visible;
      else
        Ribbon.showQuickAccessToolbarAboveTheRibbonMenuItem.Visibility = Visibility.Visible;
      if (contextMenuOwner.CanCustomizeQuickAccessToolBar)
        Ribbon.customizeQuickAccessToolbarMenuItem.Visibility = Visibility.Visible;
      Ribbon.secondSeparator.Visibility = Visibility.Visible;
      UIElement placementTarget = Ribbon.RibbonContextMenu.PlacementTarget;
      Ribbon.AddToQuickAccessCommand.CanExecute((object) null, (IInputElement) placementTarget);
      Ribbon.RemoveFromQuickAccessCommand.CanExecute((object) null, (IInputElement) placementTarget);
      if (placementTarget == null)
        return;
      Ribbon.firstSeparator.Visibility = Visibility.Visible;
      if (contextMenuOwner.quickAccessElements.ContainsValue(placementTarget))
      {
        Ribbon.removeFromQuickAccessMenuItem.Visibility = Visibility.Visible;
      }
      else
      {
        switch (placementTarget)
        {
          case System.Windows.Controls.MenuItem _:
            Ribbon.addMenuToQuickAccessMenuItem.Visibility = Visibility.Visible;
            break;
          case Gallery _:
          case InRibbonGallery _:
            Ribbon.addGalleryToQuickAccessMenuItem.Visibility = Visibility.Visible;
            break;
          case RibbonGroupBox _:
            Ribbon.addGroupToQuickAccessMenuItem.Visibility = Visibility.Visible;
            break;
          case IQuickAccessItemProvider _:
            Ribbon.addToQuickAccessMenuItem.Visibility = Visibility.Visible;
            break;
          default:
            Ribbon.firstSeparator.Visibility = Visibility.Collapsed;
            break;
        }
      }
    }

    public event SelectionChangedEventHandler SelectedTabChanged;

    public event EventHandler CustomizeTheRibbon;

    public event EventHandler CustomizeQuickAccessToolbar;

    public event DependencyPropertyChangedEventHandler IsMinimizedChanged;

    public event DependencyPropertyChangedEventHandler IsCollapsedChanged;

    public UIElement Menu
    {
      get => (UIElement) this.GetValue(Ribbon.MenuProperty);
      set => this.SetValue(Ribbon.MenuProperty, (object) value);
    }

    private static void OnApplicationMenuChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      Ribbon ribbon = (Ribbon) d;
      if (e.OldValue != null)
        ribbon.RemoveLogicalChild(e.OldValue);
      if (e.NewValue == null)
        return;
      ribbon.AddLogicalChild(e.NewValue);
    }

    public string Title
    {
      get => (string) this.GetValue(Ribbon.TitleProperty);
      set => this.SetValue(Ribbon.TitleProperty, (object) value);
    }

    private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if ((d as Ribbon).titleBar == null)
        return;
      (d as Ribbon).titleBar.InvalidateMeasure();
    }

    public RibbonTabItem SelectedTabItem
    {
      get => (RibbonTabItem) this.GetValue(Ribbon.SelectedTabItemProperty);
      set => this.SetValue(Ribbon.SelectedTabItemProperty, (object) value);
    }

    private static void OnSelectedTabItemChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      Ribbon ribbon = d as Ribbon;
      if (ribbon.tabControl != null)
        ribbon.tabControl.SelectedItem = e.NewValue;
      if (e.NewValue is RibbonTabItem newValue && ribbon.Tabs.Contains(newValue))
        ribbon.SelectedTabIndex = ribbon.Tabs.IndexOf(newValue);
      else
        ribbon.SelectedTabIndex = -1;
    }

    public int SelectedTabIndex
    {
      get => (int) this.GetValue(Ribbon.SelectedTabIndexProperty);
      set => this.SetValue(Ribbon.SelectedTabIndexProperty, (object) value);
    }

    private static void OnSelectedTabIndexChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      Ribbon ribbon = d as Ribbon;
      int newValue = (int) e.NewValue;
      if (ribbon.tabControl != null)
        ribbon.tabControl.SelectedIndex = newValue;
      if (newValue >= 0 && newValue < ribbon.Tabs.Count)
        ribbon.SelectedTabItem = ribbon.Tabs[newValue];
      else
        ribbon.SelectedTabItem = (RibbonTabItem) null;
    }

    internal RibbonTitleBar TitleBar => this.titleBar;

    public bool ShowQuickAccessToolBarAboveRibbon
    {
      get => (bool) this.GetValue(Ribbon.ShowQuickAccessToolBarAboveRibbonProperty);
      set => this.SetValue(Ribbon.ShowQuickAccessToolBarAboveRibbonProperty, (object) value);
    }

    private static void OnShowQuickAccesToolBarAboveRibbonChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      Ribbon ribbon = (Ribbon) d;
      if (ribbon.titleBar != null)
        ribbon.titleBar.InvalidateMeasure();
      ribbon.SaveState();
    }

    public ObservableCollection<RibbonContextualTabGroup> ContextualGroups
    {
      get
      {
        if (this.groups == null)
        {
          this.groups = new ObservableCollection<RibbonContextualTabGroup>();
          this.groups.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnGroupsCollectionChanged);
        }
        return this.groups;
      }
    }

    private void OnGroupsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          for (int index = 0; index < e.NewItems.Count; ++index)
          {
            if (this.titleBar != null)
              this.titleBar.Items.Insert(e.NewStartingIndex + index, e.NewItems[index]);
          }
          break;
        case NotifyCollectionChangedAction.Remove:
          IEnumerator enumerator1 = e.OldItems.GetEnumerator();
          try
          {
            while (enumerator1.MoveNext())
            {
              object current = enumerator1.Current;
              if (this.titleBar != null)
                this.titleBar.Items.Remove(current);
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
            if (this.titleBar != null)
              this.titleBar.Items.Remove(oldItem);
          }
          IEnumerator enumerator2 = e.NewItems.GetEnumerator();
          try
          {
            while (enumerator2.MoveNext())
            {
              object current = enumerator2.Current;
              if (this.titleBar != null)
                this.titleBar.Items.Add(current);
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

    public ObservableCollection<RibbonTabItem> Tabs
    {
      get
      {
        if (this.tabs == null)
        {
          this.tabs = new ObservableCollection<RibbonTabItem>();
          this.tabs.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnTabsCollectionChanged);
        }
        return this.tabs;
      }
    }

    private void OnTabsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          for (int index = 0; index < e.NewItems.Count; ++index)
          {
            if (this.tabControl != null)
              this.tabControl.Items.Insert(e.NewStartingIndex + index, e.NewItems[index]);
          }
          break;
        case NotifyCollectionChangedAction.Remove:
          IEnumerator enumerator1 = e.OldItems.GetEnumerator();
          try
          {
            while (enumerator1.MoveNext())
            {
              object current = enumerator1.Current;
              if (this.tabControl != null)
                this.tabControl.Items.Remove(current);
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
            if (this.tabControl != null)
              this.tabControl.Items.Remove(oldItem);
          }
          IEnumerator enumerator2 = e.NewItems.GetEnumerator();
          try
          {
            while (enumerator2.MoveNext())
            {
              object current = enumerator2.Current;
              if (this.tabControl != null)
                this.tabControl.Items.Add(current);
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

    public ObservableCollection<UIElement> ToolBarItems
    {
      get
      {
        if (this.toolBarItems == null)
        {
          this.toolBarItems = new ObservableCollection<UIElement>();
          this.toolBarItems.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnToolbarItemsCollectionChanged);
        }
        return this.toolBarItems;
      }
    }

    private void OnToolbarItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          for (int index = 0; index < e.NewItems.Count; ++index)
          {
            if (this.tabControl != null)
              this.tabControl.ToolBarItems.Insert(e.NewStartingIndex + index, (UIElement) e.NewItems[index]);
          }
          break;
        case NotifyCollectionChangedAction.Remove:
          IEnumerator enumerator1 = e.OldItems.GetEnumerator();
          try
          {
            while (enumerator1.MoveNext())
            {
              object current = enumerator1.Current;
              if (this.tabControl != null)
                this.tabControl.ToolBarItems.Remove(current as UIElement);
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
            if (this.tabControl != null)
              this.tabControl.ToolBarItems.Remove(oldItem as UIElement);
          }
          IEnumerator enumerator2 = e.NewItems.GetEnumerator();
          try
          {
            while (enumerator2.MoveNext())
            {
              object current = enumerator2.Current;
              if (this.tabControl != null)
                this.tabControl.ToolBarItems.Add(current as UIElement);
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

    internal QuickAccessToolBar QuickAccessToolBar => this.quickAccessToolBar;

    protected override IEnumerator LogicalChildren
    {
      get
      {
        ArrayList arrayList = new ArrayList();
        if (this.layoutRoot != null)
          arrayList.Add((object) this.layoutRoot);
        if (this.Menu != null)
          arrayList.Add((object) this.Menu);
        if (this.quickAccessToolBar != null)
          arrayList.Add((object) this.quickAccessToolBar);
        if (this.tabControl != null && this.tabControl.ToolbarPanel != null)
          arrayList.Add((object) this.tabControl.ToolbarPanel);
        return arrayList.GetEnumerator();
      }
    }

    public ObservableCollection<QuickAccessMenuItem> QuickAccessItems
    {
      get
      {
        if (this.quickAccessItems == null)
        {
          this.quickAccessItems = new ObservableCollection<QuickAccessMenuItem>();
          this.quickAccessItems.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnQuickAccessItemsCollectionChanged);
        }
        return this.quickAccessItems;
      }
    }

    private void OnQuickAccessItemsCollectionChanged(
      object sender,
      NotifyCollectionChangedEventArgs e)
    {
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          for (int index = 0; index < e.NewItems.Count; ++index)
          {
            QuickAccessMenuItem newItem = (QuickAccessMenuItem) e.NewItems[index];
            if (this.quickAccessToolBar != null)
              this.quickAccessToolBar.QuickAccessItems.Insert(e.NewStartingIndex + index, newItem);
            newItem.Ribbon = this;
          }
          break;
        case NotifyCollectionChangedAction.Remove:
          IEnumerator enumerator1 = e.OldItems.GetEnumerator();
          try
          {
            while (enumerator1.MoveNext())
            {
              QuickAccessMenuItem current = (QuickAccessMenuItem) enumerator1.Current;
              if (this.quickAccessToolBar != null)
                this.quickAccessToolBar.QuickAccessItems.Remove(current);
              current.Ribbon = (Ribbon) null;
            }
            break;
          }
          finally
          {
            if (enumerator1 is IDisposable disposable)
              disposable.Dispose();
          }
        case NotifyCollectionChangedAction.Replace:
          foreach (QuickAccessMenuItem oldItem in (IEnumerable) e.OldItems)
          {
            if (this.quickAccessToolBar != null)
              this.quickAccessToolBar.QuickAccessItems.Remove(oldItem);
            oldItem.Ribbon = (Ribbon) null;
          }
          IEnumerator enumerator2 = e.NewItems.GetEnumerator();
          try
          {
            while (enumerator2.MoveNext())
            {
              QuickAccessMenuItem current = (QuickAccessMenuItem) enumerator2.Current;
              if (this.quickAccessToolBar != null)
                this.quickAccessToolBar.QuickAccessItems.Add(current);
              current.Ribbon = this;
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

    public bool CanCustomizeQuickAccessToolBar
    {
      get => (bool) this.GetValue(Ribbon.CanCustomizeQuickAccessToolBarProperty);
      set => this.SetValue(Ribbon.CanCustomizeQuickAccessToolBarProperty, (object) value);
    }

    public bool CanCustomizeRibbon
    {
      get => (bool) this.GetValue(Ribbon.CanCustomizeRibbonProperty);
      set => this.SetValue(Ribbon.CanCustomizeRibbonProperty, (object) value);
    }

    public bool IsMinimized
    {
      get => (bool) this.GetValue(Ribbon.IsMinimizedProperty);
      set => this.SetValue(Ribbon.IsMinimizedProperty, (object) value);
    }

    private static void OnIsMinimizedChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      Ribbon sender = (Ribbon) d;
      sender.SaveState();
      if (!(bool) e.NewValue && sender.tabControl.SelectedIndex == -1)
        sender.LayoutUpdated += new EventHandler(sender.OnIsOpenLayoutUpdated);
      if (sender.IsMinimizedChanged == null)
        return;
      sender.IsMinimizedChanged((object) sender, e);
    }

    private void OnIsOpenLayoutUpdated(object sender, EventArgs e)
    {
      this.LayoutUpdated -= new EventHandler(this.OnIsOpenLayoutUpdated);
      this.tabControl.SelectedIndex = 0;
    }

    public bool IsCollapsed
    {
      get => (bool) this.GetValue(Ribbon.IsCollapsedProperty);
      private set => this.SetValue(Ribbon.IsCollapsedPropertyKey, (object) value);
    }

    private static void OnIsCollapsedChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      Ribbon sender = (Ribbon) d;
      if (sender.IsCollapsedChanged == null)
        return;
      sender.IsCollapsedChanged((object) sender, e);
    }

    public bool IsQuickAccessToolBarVisible
    {
      get => (bool) this.GetValue(Ribbon.IsQuickAccessToolBarVisibleProperty);
      set => this.SetValue(Ribbon.IsQuickAccessToolBarVisibleProperty, (object) value);
    }

    public bool CanQuickAccessLocationChanging
    {
      get => (bool) this.GetValue(Ribbon.CanQuickAccessLocationChangingProperty);
      set => this.SetValue(Ribbon.CanQuickAccessLocationChangingProperty, (object) value);
    }

    private static void OnToggleMinimizeTheRibbonCommandExecuted(
      object sender,
      ExecutedRoutedEventArgs e)
    {
      Ribbon ribbon = sender as Ribbon;
      if (ribbon.tabControl == null)
        return;
      ribbon.tabControl.IsMinimized = !ribbon.tabControl.IsMinimized;
    }

    private static void OnShowQuickAccessBelowCommandExecuted(
      object sender,
      ExecutedRoutedEventArgs e)
    {
      (sender as Ribbon).ShowQuickAccessToolBarAboveRibbon = false;
    }

    private static void OnShowQuickAccessAboveCommandExecuted(
      object sender,
      ExecutedRoutedEventArgs e)
    {
      (sender as Ribbon).ShowQuickAccessToolBarAboveRibbon = true;
    }

    private static void OnRemoveFromQuickAccessCommandExecuted(
      object sender,
      ExecutedRoutedEventArgs e)
    {
      Ribbon ribbon = sender as Ribbon;
      if (ribbon.quickAccessToolBar == null)
        return;
      UIElement key = ribbon.quickAccessElements.First<KeyValuePair<UIElement, UIElement>>((Func<KeyValuePair<UIElement, UIElement>, bool>) (x => x.Value == e.Parameter)).Key;
      ribbon.RemoveFromQuickAccessToolBar(key);
    }

    private static void OnAddToQuickAccessCommandExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      Ribbon ribbon = sender as Ribbon;
      if (ribbon.quickAccessToolBar == null)
        return;
      ribbon.AddToQuickAccessToolBar(e.Parameter as UIElement);
    }

    private static void OnCustomizeQuickAccessToolbarCommandExecuted(
      object sender,
      ExecutedRoutedEventArgs e)
    {
      Ribbon ribbon = sender as Ribbon;
      if (ribbon.CustomizeQuickAccessToolbar == null)
        return;
      ribbon.CustomizeQuickAccessToolbar(sender, EventArgs.Empty);
    }

    private static void OnCustomizeTheRibbonCommandExecuted(
      object sender,
      ExecutedRoutedEventArgs e)
    {
      Ribbon ribbon = sender as Ribbon;
      if (ribbon.CustomizeTheRibbon == null)
        return;
      ribbon.CustomizeTheRibbon(sender, EventArgs.Empty);
    }

    private static void OnCustomizeQuickAccessToolbarCommandCanExecute(
      object sender,
      CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = (sender as Ribbon).CanCustomizeQuickAccessToolBar;
    }

    private static void OnCustomizeTheRibbonCommandCanExecute(
      object sender,
      CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = (sender as Ribbon).CanCustomizeRibbon;
    }

    private static void OnRemoveFromQuickAccessCommandCanExecute(
      object sender,
      CanExecuteRoutedEventArgs e)
    {
      Ribbon ribbon = sender as Ribbon;
      if (ribbon.IsQuickAccessToolBarVisible)
        e.CanExecute = ribbon.quickAccessElements.ContainsValue(e.Parameter as UIElement);
      else
        e.CanExecute = false;
    }

    private static void OnAddToQuickAccessCommandCanExecute(
      object sender,
      CanExecuteRoutedEventArgs e)
    {
      Ribbon ribbon = sender as Ribbon;
      if (ribbon.IsQuickAccessToolBarVisible)
      {
        if (e.Parameter is Gallery)
          e.CanExecute = !ribbon.IsInQuickAccessToolBar(Ribbon.FindParentRibbonControl(e.Parameter as DependencyObject) as UIElement);
        else
          e.CanExecute = !ribbon.IsInQuickAccessToolBar(e.Parameter as UIElement);
      }
      else
        e.CanExecute = false;
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static Ribbon()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (Ribbon), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (Ribbon)));
      CommandManager.RegisterClassCommandBinding(typeof (Ribbon), new CommandBinding((ICommand) Ribbon.AddToQuickAccessCommand, new ExecutedRoutedEventHandler(Ribbon.OnAddToQuickAccessCommandExecuted), new CanExecuteRoutedEventHandler(Ribbon.OnAddToQuickAccessCommandCanExecute)));
      CommandManager.RegisterClassCommandBinding(typeof (Ribbon), new CommandBinding((ICommand) Ribbon.RemoveFromQuickAccessCommand, new ExecutedRoutedEventHandler(Ribbon.OnRemoveFromQuickAccessCommandExecuted), new CanExecuteRoutedEventHandler(Ribbon.OnRemoveFromQuickAccessCommandCanExecute)));
      CommandManager.RegisterClassCommandBinding(typeof (Ribbon), new CommandBinding((ICommand) Ribbon.ShowQuickAccessAboveCommand, new ExecutedRoutedEventHandler(Ribbon.OnShowQuickAccessAboveCommandExecuted)));
      CommandManager.RegisterClassCommandBinding(typeof (Ribbon), new CommandBinding((ICommand) Ribbon.ShowQuickAccessBelowCommand, new ExecutedRoutedEventHandler(Ribbon.OnShowQuickAccessBelowCommandExecuted)));
      CommandManager.RegisterClassCommandBinding(typeof (Ribbon), new CommandBinding((ICommand) Ribbon.ToggleMinimizeTheRibbonCommand, new ExecutedRoutedEventHandler(Ribbon.OnToggleMinimizeTheRibbonCommandExecuted)));
      CommandManager.RegisterClassCommandBinding(typeof (Ribbon), new CommandBinding((ICommand) Ribbon.CustomizeTheRibbonCommand, new ExecutedRoutedEventHandler(Ribbon.OnCustomizeTheRibbonCommandExecuted), new CanExecuteRoutedEventHandler(Ribbon.OnCustomizeTheRibbonCommandCanExecute)));
      CommandManager.RegisterClassCommandBinding(typeof (Ribbon), new CommandBinding((ICommand) Ribbon.CustomizeQuickAccessToolbarCommand, new ExecutedRoutedEventHandler(Ribbon.OnCustomizeQuickAccessToolbarCommandExecuted), new CanExecuteRoutedEventHandler(Ribbon.OnCustomizeQuickAccessToolbarCommandCanExecute)));
      Ribbon.InitRibbonContextMenu();
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (Ribbon), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(Ribbon.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (Ribbon));
      return basevalue;
    }

    public Ribbon()
    {
      this.VerticalAlignment = VerticalAlignment.Top;
      KeyboardNavigation.SetDirectionalNavigation((DependencyObject) this, KeyboardNavigationMode.Contained);
      this.Loaded += new RoutedEventHandler(this.OnLoaded);
      this.Unloaded += new RoutedEventHandler(this.OnUnloaded);
      this.keyTipService = new KeyTipService(this);
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (e.NewSize.Width < 300.0 || e.NewSize.Height < 250.0)
        this.IsCollapsed = true;
      else
        this.IsCollapsed = false;
    }

    protected override void OnGotFocus(RoutedEventArgs e)
    {
      if (this.tabControl == null)
        return;
      ((UIElement) this.tabControl.SelectedItem)?.Focus();
    }

    [SuppressMessage("Microsoft.Maintainability", "CA1502")]
    public override void OnApplyTemplate()
    {
      if (this.layoutRoot != null)
        this.RemoveLogicalChild((object) this.layoutRoot);
      this.layoutRoot = this.GetTemplateChild("PART_LayoutRoot") as Panel;
      if (this.layoutRoot != null)
        this.AddLogicalChild((object) this.layoutRoot);
      if (this.titleBar != null && this.groups != null)
      {
        for (int index = 0; index < this.groups.Count; ++index)
          this.titleBar.Items.Remove((object) this.groups[index]);
      }
      this.titleBar = this.GetTemplateChild("PART_RibbonTitleBar") as RibbonTitleBar;
      if (this.titleBar != null && this.groups != null)
      {
        for (int index = 0; index < this.groups.Count; ++index)
          this.titleBar.Items.Add((object) this.groups[index]);
      }
      RibbonTabItem ribbonTabItem = this.SelectedTabItem;
      if (this.tabControl != null)
      {
        this.tabControl.SelectionChanged -= new SelectionChangedEventHandler(this.OnTabControlSelectionChanged);
        ribbonTabItem = this.tabControl.SelectedItem as RibbonTabItem;
      }
      if (this.tabControl != null && this.tabs != null)
      {
        for (int index = 0; index < this.tabs.Count; ++index)
          this.tabControl.Items.Remove((object) this.tabs[index]);
      }
      if (this.tabControl != null && this.toolBarItems != null)
      {
        for (int index = 0; index < this.toolBarItems.Count; ++index)
          this.tabControl.ToolBarItems.Remove(this.toolBarItems[index]);
      }
      this.tabControl = this.GetTemplateChild("PART_RibbonTabControl") as RibbonTabControl;
      if (this.tabControl != null)
      {
        this.tabControl.SelectionChanged += new SelectionChangedEventHandler(this.OnTabControlSelectionChanged);
        this.tabControl.IsMinimized = this.IsMinimized;
        this.tabControl.SetBinding(RibbonTabControl.IsMinimizedProperty, (BindingBase) new Binding("IsMinimized")
        {
          Source = (object) this,
          Mode = BindingMode.TwoWay
        });
      }
      if (this.tabControl != null && this.tabs != null)
      {
        for (int index = 0; index < this.tabs.Count; ++index)
          this.tabControl.Items.Add((object) this.tabs[index]);
        this.tabControl.SelectedItem = (object) ribbonTabItem;
        object selectedItem = this.tabControl.SelectedItem;
      }
      if (this.tabControl != null && this.toolBarItems != null)
      {
        for (int index = 0; index < this.toolBarItems.Count; ++index)
          this.tabControl.ToolBarItems.Add(this.toolBarItems[index]);
      }
      if (this.quickAccessToolBar != null)
      {
        this.quickAccessStream = new MemoryStream();
        if (!this.AutomaticStateManagement || this.IsStateLoaded)
          this.SaveState((Stream) this.quickAccessStream);
        this.ClearQuickAccessToolBar();
      }
      if (this.quickAccessToolBar != null)
      {
        this.quickAccessToolBar.ItemsChanged -= new NotifyCollectionChangedEventHandler(this.OnQuickAccessItemsChanged);
        if (this.quickAccessItems != null)
        {
          for (int index = 0; index < this.quickAccessItems.Count; ++index)
            this.quickAccessToolBar.QuickAccessItems.Remove(this.quickAccessItems[index]);
        }
      }
      this.quickAccessToolBar = this.GetTemplateChild("PART_QuickAccessToolBar") as QuickAccessToolBar;
      if (this.quickAccessToolBar != null)
      {
        if (this.quickAccessItems != null)
        {
          for (int index = 0; index < this.quickAccessItems.Count; ++index)
            this.quickAccessToolBar.QuickAccessItems.Add(this.quickAccessItems[index]);
        }
        this.quickAccessToolBar.ItemsChanged += new NotifyCollectionChangedEventHandler(this.OnQuickAccessItemsChanged);
        this.quickAccessToolBar.SetBinding(QuickAccessToolBar.CanQuickAccessLocationChangingProperty, (BindingBase) new Binding("CanQuickAccessLocationChanging")
        {
          Source = (object) this,
          Mode = BindingMode.OneWay
        });
      }
      if (this.quickAccessToolBar != null)
      {
        if (this.quickAccessToolBar.Parent == null)
          this.AddLogicalChild((object) this.quickAccessToolBar);
        this.quickAccessToolBar.Loaded += new RoutedEventHandler(this.OnFirstToolbarLoaded);
      }
      if (this.ownerWindow != null)
        return;
      this.ownerWindow = Window.GetWindow((DependencyObject) this);
      this.SetBinding(Ribbon.TitleProperty, (BindingBase) new Binding("Title")
      {
        Mode = BindingMode.OneWay,
        Source = (object) this.ownerWindow
      });
    }

    private void OnFirstToolbarLoaded(object sender, RoutedEventArgs e)
    {
      this.quickAccessToolBar.Loaded -= new RoutedEventHandler(this.OnFirstToolbarLoaded);
      if (this.quickAccessStream == null)
        return;
      this.quickAccessStream.Position = 0L;
      this.LoadState((Stream) this.quickAccessStream);
      this.quickAccessStream.Close();
      this.quickAccessStream = (MemoryStream) null;
    }

    public bool IsInQuickAccessToolBar(UIElement element)
    {
      return element != null && this.quickAccessElements.ContainsKey(element);
    }

    public void AddToQuickAccessToolBar(UIElement element)
    {
      if (element is Gallery)
        element = Ribbon.FindParentRibbonControl((DependencyObject) element) as UIElement;
      if (!QuickAccessItemsProvider.IsSupported(element) || this.IsInQuickAccessToolBar(element))
        return;
      UIElement quickAccessItem = (UIElement) QuickAccessItemsProvider.GetQuickAccessItem(element);
      this.quickAccessElements.Add(element, quickAccessItem);
      this.quickAccessToolBar.Items.Add(quickAccessItem);
      this.quickAccessToolBar.InvalidateMeasure();
    }

    private static IRibbonControl FindParentRibbonControl(DependencyObject element)
    {
      for (DependencyObject parent = LogicalTreeHelper.GetParent(element); parent != null; parent = LogicalTreeHelper.GetParent(parent))
      {
        if (parent is IRibbonControl parentRibbonControl)
          return parentRibbonControl;
      }
      return (IRibbonControl) null;
    }

    public void RemoveFromQuickAccessToolBar(UIElement element)
    {
      if (!this.IsInQuickAccessToolBar(element))
        return;
      UIElement quickAccessElement = this.quickAccessElements[element];
      this.quickAccessElements.Remove(element);
      this.quickAccessToolBar.Items.Remove(quickAccessElement);
      this.quickAccessToolBar.InvalidateMeasure();
    }

    public void ClearQuickAccessToolBar()
    {
      this.quickAccessElements.Clear();
      if (this.quickAccessToolBar == null)
        return;
      this.quickAccessToolBar.Items.Clear();
    }

    private void OnTabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.tabControl != null)
      {
        this.SelectedTabItem = this.tabControl.SelectedItem as RibbonTabItem;
        this.SelectedTabIndex = this.tabControl.SelectedIndex;
      }
      if (this.SelectedTabChanged != null)
        this.SelectedTabChanged((object) this, e);
      int count = e.AddedItems.Count;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      this.keyTipService.Attach();
      Window window = Window.GetWindow((DependencyObject) this);
      if (window != null)
      {
        window.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
        window.KeyDown += new KeyEventHandler(this.OnKeyDown);
      }
      this.InitialLoadState();
      if (this.tabControl == null || this.tabControl.SelectedIndex != -1 || this.IsMinimized)
        return;
      this.tabControl.SelectedIndex = 0;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.F1 || (Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control)
        return;
      this.IsMinimized = !this.IsMinimized;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
      this.keyTipService.Detach();
      Window window = Window.GetWindow((DependencyObject) this);
      if (window == null)
        return;
      window.SizeChanged -= new SizeChangedEventHandler(this.OnSizeChanged);
      window.KeyDown -= new KeyEventHandler(this.OnKeyDown);
    }

    private void OnBackstageEscapeKeyDown(object sender, KeyEventArgs e)
    {
    }

    private static AdornerLayer GetAdornerLayer(UIElement element)
    {
      UIElement reference = element;
      do
      {
        reference = (UIElement) VisualTreeHelper.GetParent((DependencyObject) reference);
      }
      while (!(reference is AdornerDecorator));
      return AdornerLayer.GetAdornerLayer((Visual) VisualTreeHelper.GetChild((DependencyObject) reference, 0));
    }

    private void SaveWindowSize(Window wnd)
    {
      NativeMethods.WINDOWINFO pwi = new NativeMethods.WINDOWINFO();
      pwi.cbSize = (uint) Marshal.SizeOf((object) pwi);
      NativeMethods.GetWindowInfo(new WindowInteropHelper(wnd).Handle, ref pwi);
    }

    private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
    {
      this.SaveWindowSize(Window.GetWindow((DependencyObject) this));
    }

    private string IsolatedStorageFileName
    {
      get
      {
        if (this.isolatedStorageFileName == null)
        {
          string str = "";
          Window window = Window.GetWindow((DependencyObject) this);
          if (window != null)
          {
            str = str + "." + window.GetType().FullName;
            if (!string.IsNullOrEmpty(window.Name) && window.Name.Trim().Length > 0)
              str = str + "." + window.Name;
          }
          if (!string.IsNullOrEmpty(this.Name) && this.Name.Trim().Length > 0)
            str = str + "." + this.Name;
          this.isolatedStorageFileName = "Fluent.Ribbon.State.2.0." + str.GetHashCode().ToString("X");
        }
        return this.isolatedStorageFileName;
      }
    }

    private void InitialLoadState()
    {
      this.LayoutUpdated += new EventHandler(this.OnJustLayoutUpdated);
    }

    private void OnJustLayoutUpdated(object sender, EventArgs e)
    {
      this.LayoutUpdated -= new EventHandler(this.OnJustLayoutUpdated);
      if (!this.QuickAccessToolBar.IsLoaded)
      {
        this.InitialLoadState();
      }
      else
      {
        if (this.IsStateLoaded)
          return;
        this.LoadState();
      }
    }

    private void OnQuickAccessItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      this.SaveState();
    }

    private void SaveState()
    {
      if (!this.AutomaticStateManagement || !this.IsStateLoaded)
        return;
      using (IsolatedStorageFileStream storageFileStream = new IsolatedStorageFileStream(this.IsolatedStorageFileName, FileMode.Create, FileAccess.Write, Ribbon.GetIsolatedStorageFile()))
        this.SaveState((Stream) storageFileStream);
    }

    private void LoadState()
    {
      if (!this.AutomaticStateManagement)
        return;
      IsolatedStorageFile isolatedStorageFile = Ribbon.GetIsolatedStorageFile();
      if (Ribbon.FileExists(isolatedStorageFile, this.IsolatedStorageFileName))
      {
        using (IsolatedStorageFileStream storageFileStream = new IsolatedStorageFileStream(this.IsolatedStorageFileName, FileMode.Open, FileAccess.Read, isolatedStorageFile))
          this.LoadState((Stream) storageFileStream);
      }
      this.IsStateLoaded = true;
    }

    private static IsolatedStorageFile GetIsolatedStorageFile()
    {
      try
      {
        return IsolatedStorageFile.GetUserStoreForDomain();
      }
      catch
      {
        return IsolatedStorageFile.GetUserStoreForAssembly();
      }
    }

    public static void ResetState()
    {
      IsolatedStorageFile isolatedStorageFile = Ribbon.GetIsolatedStorageFile();
      foreach (string fileName in isolatedStorageFile.GetFileNames("*Fluent.Ribbon.State*"))
        isolatedStorageFile.DeleteFile(fileName);
    }

    private static bool FileExists(IsolatedStorageFile storage, string fileName)
    {
      return storage.GetFileNames(fileName).Length != 0;
    }

    public void SaveState(Stream stream)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.IsMinimized.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      stringBuilder.Append(',');
      stringBuilder.Append(this.ShowQuickAccessToolBarAboveRibbon.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      stringBuilder.Append('|');
      Dictionary<FrameworkElement, string> paths = new Dictionary<FrameworkElement, string>();
      this.TraverseLogicalTree((DependencyObject) this, "", (IDictionary<FrameworkElement, string>) paths);
      foreach (KeyValuePair<UIElement, UIElement> quickAccessElement in this.quickAccessElements)
      {
        if (paths.ContainsKey((FrameworkElement) quickAccessElement.Key))
        {
          stringBuilder.Append(paths[(FrameworkElement) quickAccessElement.Key]);
          stringBuilder.Append(';');
        }
        else if (quickAccessElement.Key is FrameworkElement key && !string.IsNullOrEmpty(key.Name))
          string.Format((IFormatProvider) CultureInfo.InvariantCulture, " (name of the control is {0})", new object[1]
          {
            (object) key.Name
          });
      }
      StreamWriter streamWriter = new StreamWriter(stream);
      streamWriter.Write(stringBuilder.ToString());
      streamWriter.Flush();
    }

    private void TraverseLogicalTree(
      DependencyObject item,
      string path,
      IDictionary<FrameworkElement, string> paths)
    {
      if (item is FrameworkElement key && this.quickAccessElements.ContainsKey((UIElement) key) && !paths.ContainsKey(key))
        paths.Add(key, path);
      object[] array = LogicalTreeHelper.GetChildren(item).Cast<object>().ToArray<object>();
      for (int index = 0; index < array.Length; ++index)
      {
        if (array[index] is DependencyObject dependencyObject)
          this.TraverseLogicalTree(dependencyObject, path + (object) index + ",", paths);
      }
    }

    public void LoadState(Stream stream)
    {
      this.suppressAutomaticStateManagement = true;
      string[] strArray1 = new StreamReader(stream).ReadToEnd().Split('|');
      if (strArray1.Length != 2)
        return;
      string[] strArray2 = strArray1[0].Split(',');
      this.IsMinimized = bool.Parse(strArray2[0]);
      this.ShowQuickAccessToolBarAboveRibbon = bool.Parse(strArray2[1]);
      string[] strArray3 = strArray1[1].Split(new char[1]
      {
        ';'
      }, StringSplitOptions.RemoveEmptyEntries);
      if (this.quickAccessToolBar != null)
        this.quickAccessToolBar.Items.Clear();
      this.quickAccessElements.Clear();
      for (int index = 0; index < strArray3.Length; ++index)
        this.ParseAndAddToQuickAccessToolBar(strArray3[index]);
      foreach (QuickAccessMenuItem quickAccessItem in (Collection<QuickAccessMenuItem>) this.QuickAccessItems)
        quickAccessItem.IsChecked = this.IsInQuickAccessToolBar((UIElement) quickAccessItem.Target);
      this.suppressAutomaticStateManagement = false;
    }

    private void ParseAndAddToQuickAccessToolBar(string data)
    {
      int[] array1 = ((IEnumerable<string>) data.Split(new char[1]
      {
        ','
      }, StringSplitOptions.RemoveEmptyEntries)).Select<string, int>((Func<string, int>) (x => int.Parse(x, (IFormatProvider) CultureInfo.InvariantCulture))).ToArray<int>();
      DependencyObject current = (DependencyObject) this;
      for (int index = 0; index < array1.Length; ++index)
      {
        object[] array2 = LogicalTreeHelper.GetChildren(current).OfType<object>().ToArray<object>();
        DependencyObject dependencyObject = array2.Length <= array1[index] ? (DependencyObject) null : array2[array1[index]] as DependencyObject;
        if (dependencyObject == null)
          return;
        current = dependencyObject;
      }
      if (!(current is UIElement element) || !QuickAccessItemsProvider.IsSupported(element))
        return;
      this.AddToQuickAccessToolBar(element);
    }

    private bool IsStateLoaded { get; set; }

    public bool AutomaticStateManagement
    {
      get => (bool) this.GetValue(Ribbon.AutomaticStateManagementProperty);
      set => this.SetValue(Ribbon.AutomaticStateManagementProperty, (object) value);
    }

    private static object CoerceAutoStateManagement(DependencyObject d, object basevalue)
    {
      return ((Ribbon) d).suppressAutomaticStateManagement ? (object) false : basevalue;
    }

    private static void OnAutoStateManagement(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      Ribbon ribbon = (Ribbon) d;
      if (!(bool) e.NewValue)
        return;
      ribbon.InitialLoadState();
    }
  }
}
