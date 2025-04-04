// Decompiled with JetBrains decompiler
// Type: Fluent.ColorGallery
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace Fluent
{
  [ContentProperty("ThemeColors")]
  public class ColorGallery : Control
  {
    public static readonly Color[] HighlightColors = new Color[15]
    {
      Color.FromRgb(byte.MaxValue, byte.MaxValue, (byte) 0),
      Color.FromRgb((byte) 0, byte.MaxValue, (byte) 0),
      Color.FromRgb((byte) 0, byte.MaxValue, byte.MaxValue),
      Color.FromRgb(byte.MaxValue, (byte) 0, byte.MaxValue),
      Color.FromRgb((byte) 0, (byte) 0, byte.MaxValue),
      Color.FromRgb(byte.MaxValue, (byte) 0, (byte) 0),
      Color.FromRgb((byte) 0, (byte) 0, (byte) 128),
      Color.FromRgb((byte) 0, (byte) 128, (byte) 128),
      Color.FromRgb((byte) 0, (byte) 128, (byte) 0),
      Color.FromRgb((byte) 128, (byte) 0, (byte) 128),
      Color.FromRgb((byte) 128, (byte) 0, (byte) 0),
      Color.FromRgb((byte) 128, (byte) 128, (byte) 0),
      Color.FromRgb((byte) 128, (byte) 128, (byte) 128),
      Color.FromRgb((byte) 192, (byte) 192, (byte) 192),
      Color.FromRgb((byte) 0, (byte) 0, (byte) 0)
    };
    public static readonly Color[] StandardColors = new Color[30]
    {
      Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue),
      Color.FromRgb(byte.MaxValue, (byte) 0, (byte) 0),
      Color.FromRgb((byte) 192, (byte) 80, (byte) 77),
      Color.FromRgb((byte) 209, (byte) 99, (byte) 73),
      Color.FromRgb((byte) 221, (byte) 132, (byte) 132),
      Color.FromRgb((byte) 204, (byte) 204, (byte) 204),
      Color.FromRgb(byte.MaxValue, (byte) 192, (byte) 0),
      Color.FromRgb((byte) 247, (byte) 150, (byte) 70),
      Color.FromRgb((byte) 209, (byte) 144, (byte) 73),
      Color.FromRgb((byte) 243, (byte) 164, (byte) 71),
      Color.FromRgb((byte) 165, (byte) 165, (byte) 165),
      Color.FromRgb(byte.MaxValue, byte.MaxValue, (byte) 0),
      Color.FromRgb((byte) 155, (byte) 187, (byte) 89),
      Color.FromRgb((byte) 204, (byte) 180, (byte) 0),
      Color.FromRgb((byte) 223, (byte) 206, (byte) 4),
      Color.FromRgb((byte) 102, (byte) 102, (byte) 102),
      Color.FromRgb((byte) 0, (byte) 176, (byte) 80),
      Color.FromRgb((byte) 75, (byte) 172, (byte) 198),
      Color.FromRgb((byte) 143, (byte) 176, (byte) 140),
      Color.FromRgb((byte) 165, (byte) 181, (byte) 146),
      Color.FromRgb((byte) 51, (byte) 51, (byte) 51),
      Color.FromRgb((byte) 0, (byte) 77, (byte) 187),
      Color.FromRgb((byte) 79, (byte) 129, (byte) 189),
      Color.FromRgb((byte) 100, (byte) 107, (byte) 134),
      Color.FromRgb((byte) 128, (byte) 158, (byte) 194),
      Color.FromRgb((byte) 0, (byte) 0, (byte) 0),
      Color.FromRgb((byte) 155, (byte) 0, (byte) 211),
      Color.FromRgb((byte) 128, (byte) 100, (byte) 162),
      Color.FromRgb((byte) 158, (byte) 124, (byte) 124),
      Color.FromRgb((byte) 156, (byte) 133, (byte) 192)
    };
    public static readonly Color[] StandardThemeColors = new Color[10]
    {
      Color.FromRgb((byte) 192, (byte) 0, (byte) 0),
      Color.FromRgb(byte.MaxValue, (byte) 0, (byte) 0),
      Color.FromRgb(byte.MaxValue, (byte) 192, (byte) 0),
      Color.FromRgb(byte.MaxValue, byte.MaxValue, (byte) 0),
      Color.FromRgb((byte) 146, (byte) 208, (byte) 80),
      Color.FromRgb((byte) 0, (byte) 176, (byte) 80),
      Color.FromRgb((byte) 0, (byte) 176, (byte) 240),
      Color.FromRgb((byte) 0, (byte) 112, (byte) 192),
      Color.FromRgb((byte) 0, (byte) 32, (byte) 96),
      Color.FromRgb((byte) 112, (byte) 48, (byte) 160)
    };
    private static ObservableCollection<Color> recentColors;
    private MenuItem noColorButton;
    private MenuItem automaticButton;
    private MenuItem moreColorsButton;
    private ListBox themeColorsListBox;
    private ListBox themeGradientsListBox;
    private ListBox standardColorsListBox;
    private ListBox standardGradientsListBox;
    private ListBox recentColorsListBox;
    private List<ListBox> listBoxes = new List<ListBox>();
    private bool isSelectionChanged;
    public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof (Mode), typeof (ColorGalleryMode), typeof (ColorGallery), (PropertyMetadata) new UIPropertyMetadata((object) ColorGalleryMode.StandardColors, new PropertyChangedCallback(ColorGallery.OnModeChanged)));
    public static readonly DependencyProperty ChipWidthProperty = DependencyProperty.Register(nameof (ChipWidth), typeof (double), typeof (ColorGallery), (PropertyMetadata) new UIPropertyMetadata((object) 13.0, (PropertyChangedCallback) null, new CoerceValueCallback(ColorGallery.CoerceChipSize)));
    public static readonly DependencyProperty ChipHeightProperty = DependencyProperty.Register(nameof (ChipHeight), typeof (double), typeof (ColorGallery), (PropertyMetadata) new UIPropertyMetadata((object) 13.0, (PropertyChangedCallback) null, new CoerceValueCallback(ColorGallery.CoerceChipSize)));
    public static readonly DependencyProperty IsAutomaticColorButtonVisibleProperty = DependencyProperty.Register(nameof (IsAutomaticColorButtonVisible), typeof (bool), typeof (ColorGallery), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty IsNoColorButtonVisibleProperty = DependencyProperty.Register(nameof (IsNoColorButtonVisible), typeof (bool), typeof (ColorGallery), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty IsMoreColorsButtonVisibleProperty = DependencyProperty.Register(nameof (IsMoreColorsButtonVisible), typeof (bool), typeof (ColorGallery), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty IsRecentColorsVisibleProperty = DependencyProperty.Register(nameof (IsRecentColorsVisible), typeof (bool), typeof (ColorGallery), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(nameof (Columns), typeof (int), typeof (ColorGallery), (PropertyMetadata) new UIPropertyMetadata((object) 10, new PropertyChangedCallback(ColorGallery.OnColumnsChanged), new CoerceValueCallback(ColorGallery.CoerceColumns)));
    public static readonly DependencyProperty StandardColorGridRowsProperty = DependencyProperty.Register(nameof (StandardColorGridRows), typeof (int), typeof (ColorGallery), (PropertyMetadata) new UIPropertyMetadata((object) 0, new PropertyChangedCallback(ColorGallery.OnStandardColorGridRowsChanged), new CoerceValueCallback(ColorGallery.CoeceGridRows)));
    public static readonly DependencyProperty ThemeColorGridRowsProperty = DependencyProperty.Register(nameof (ThemeColorGridRows), typeof (int), typeof (ColorGallery), (PropertyMetadata) new UIPropertyMetadata((object) 0, new PropertyChangedCallback(ColorGallery.OnThemeColorGridRowsChanged), new CoerceValueCallback(ColorGallery.CoeceGridRows)));
    public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(nameof (SelectedColor), typeof (Color?), typeof (ColorGallery), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(ColorGallery.OnSelectedColorChanged)));
    private ObservableCollection<Color> themeColors;
    public static readonly DependencyProperty ThemeColorsSourceProperty = DependencyProperty.Register(nameof (ThemeColorsSource), typeof (IEnumerable<Color>), typeof (ColorGallery), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(ColorGallery.OnThemeColorsSourceChanged)));
    private static readonly DependencyPropertyKey ThemeGradientsPropertyKey = DependencyProperty.RegisterReadOnly(nameof (ThemeGradients), typeof (Color[]), typeof (ColorGallery), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty ThemeGradientsProperty = ColorGallery.ThemeGradientsPropertyKey.DependencyProperty;
    private static readonly DependencyPropertyKey StandardGradientsPropertyKey = DependencyProperty.RegisterReadOnly(nameof (StandardGradients), typeof (Color[]), typeof (ColorGallery), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty StandardGradientsProperty = ColorGallery.StandardGradientsPropertyKey.DependencyProperty;
    public static readonly RoutedEvent SelectedColorChangedEvent = EventManager.RegisterRoutedEvent("SelectedColorChanged", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (ColorGallery));
    private static IntPtr customColors = IntPtr.Zero;
    private int[] colorsArray = new int[16];

    public static ObservableCollection<Color> RecentColors
    {
      get
      {
        if (ColorGallery.recentColors == null)
          ColorGallery.recentColors = new ObservableCollection<Color>();
        return ColorGallery.recentColors;
      }
    }

    public ColorGalleryMode Mode
    {
      get => (ColorGalleryMode) this.GetValue(ColorGallery.ModeProperty);
      set => this.SetValue(ColorGallery.ModeProperty, (object) value);
    }

    private static void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      (d as ColorGallery).UpdateGradients();
    }

    public double ChipWidth
    {
      get => (double) this.GetValue(ColorGallery.ChipWidthProperty);
      set => this.SetValue(ColorGallery.ChipWidthProperty, (object) value);
    }

    private static object CoerceChipSize(DependencyObject d, object basevalue)
    {
      return (double) basevalue < 0.0 ? (object) 0 : basevalue;
    }

    public double ChipHeight
    {
      get => (double) this.GetValue(ColorGallery.ChipHeightProperty);
      set => this.SetValue(ColorGallery.ChipHeightProperty, (object) value);
    }

    public bool IsAutomaticColorButtonVisible
    {
      get => (bool) this.GetValue(ColorGallery.IsAutomaticColorButtonVisibleProperty);
      set => this.SetValue(ColorGallery.IsAutomaticColorButtonVisibleProperty, (object) value);
    }

    public bool IsNoColorButtonVisible
    {
      get => (bool) this.GetValue(ColorGallery.IsNoColorButtonVisibleProperty);
      set => this.SetValue(ColorGallery.IsNoColorButtonVisibleProperty, (object) value);
    }

    public bool IsMoreColorsButtonVisible
    {
      get => (bool) this.GetValue(ColorGallery.IsMoreColorsButtonVisibleProperty);
      set => this.SetValue(ColorGallery.IsMoreColorsButtonVisibleProperty, (object) value);
    }

    public bool IsRecentColorsVisible
    {
      get => (bool) this.GetValue(ColorGallery.IsRecentColorsVisibleProperty);
      set => this.SetValue(ColorGallery.IsRecentColorsVisibleProperty, (object) value);
    }

    public int Columns
    {
      get => (int) this.GetValue(ColorGallery.ColumnsProperty);
      set => this.SetValue(ColorGallery.ColumnsProperty, (object) value);
    }

    private static object CoerceColumns(DependencyObject d, object basevalue)
    {
      return (int) basevalue < 1 ? (object) 1 : basevalue;
    }

    private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      (d as ColorGallery).UpdateGradients();
    }

    public int StandardColorGridRows
    {
      get => (int) this.GetValue(ColorGallery.StandardColorGridRowsProperty);
      set => this.SetValue(ColorGallery.StandardColorGridRowsProperty, (object) value);
    }

    private static object CoeceGridRows(DependencyObject d, object basevalue)
    {
      return (int) basevalue < 0 ? (object) 0 : basevalue;
    }

    private static void OnStandardColorGridRowsChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      (d as ColorGallery).UpdateGradients();
    }

    public int ThemeColorGridRows
    {
      get => (int) this.GetValue(ColorGallery.ThemeColorGridRowsProperty);
      set => this.SetValue(ColorGallery.ThemeColorGridRowsProperty, (object) value);
    }

    private static void OnThemeColorGridRowsChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      (d as ColorGallery).UpdateGradients();
    }

    public Color? SelectedColor
    {
      get => (Color?) this.GetValue(ColorGallery.SelectedColorProperty);
      set => this.SetValue(ColorGallery.SelectedColorProperty, (object) value);
    }

    private static void OnSelectedColorChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ColorGallery colorGallery = d as ColorGallery;
      colorGallery.RaiseEvent(new RoutedEventArgs(ColorGallery.SelectedColorChangedEvent));
      Color? newValue = (Color?) e.NewValue;
      colorGallery.UpdateSelectedColor(newValue);
    }

    private void UpdateSelectedColor(Color? color)
    {
      if (this.isSelectionChanged || !this.IsLoaded)
        return;
      this.isSelectionChanged = true;
      bool flag = false;
      if (!color.HasValue)
      {
        flag = true;
        this.automaticButton.IsChecked = true;
        this.noColorButton.IsChecked = false;
      }
      else if (color.Value == Colors.Transparent)
      {
        flag = true;
        this.automaticButton.IsChecked = false;
        this.noColorButton.IsChecked = true;
      }
      else
      {
        this.automaticButton.IsChecked = false;
        this.noColorButton.IsChecked = false;
      }
      for (int index = 0; index < this.listBoxes.Count; ++index)
      {
        if (!flag && this.listBoxes[index].Visibility == Visibility.Visible)
        {
          if (this.listBoxes[index].Items.Contains((object) color.Value))
          {
            this.listBoxes[index].SelectedItem = (object) color.Value;
            flag = true;
          }
        }
        else
          this.listBoxes[index].SelectedItem = (object) null;
      }
      this.isSelectionChanged = false;
    }

    public ObservableCollection<Color> ThemeColors
    {
      get
      {
        if (this.themeColors == null)
        {
          this.themeColors = new ObservableCollection<Color>();
          this.themeColors.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnThemeColorsChanged);
        }
        return this.themeColors;
      }
    }

    private void OnThemeColorsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      this.UpdateGradients();
    }

    public IEnumerable<Color> ThemeColorsSource
    {
      get => (IEnumerable<Color>) this.GetValue(ColorGallery.ThemeColorsSourceProperty);
      set => this.SetValue(ColorGallery.ThemeColorsSourceProperty, (object) value);
    }

    private static void OnThemeColorsSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ColorGallery colorGallery = d as ColorGallery;
      colorGallery.ThemeColors.Clear();
      if (e.NewValue == null)
        return;
      foreach (Color color in e.NewValue as IEnumerable<Color>)
        colorGallery.ThemeColors.Add(color);
    }

    public Color[] ThemeGradients
    {
      get => (Color[]) this.GetValue(ColorGallery.ThemeGradientsProperty);
      private set => this.SetValue(ColorGallery.ThemeGradientsPropertyKey, (object) value);
    }

    public Color[] StandardGradients
    {
      get => (Color[]) this.GetValue(ColorGallery.StandardGradientsProperty);
      private set => this.SetValue(ColorGallery.StandardGradientsPropertyKey, (object) value);
    }

    public event RoutedEventHandler SelectedColorChanged
    {
      add => this.AddHandler(ColorGallery.SelectedColorChangedEvent, (Delegate) value);
      remove => this.RemoveHandler(ColorGallery.SelectedColorChangedEvent, (Delegate) value);
    }

    public void RaiseSelectedColorChanged()
    {
      this.RaiseEvent(new RoutedEventArgs(ColorGallery.SelectedColorChangedEvent, (object) this));
    }

    public event EventHandler<MoreColorsExecutingEventArgs> MoreColorsExecuting;

    static ColorGallery()
    {
      Type type = typeof (ColorGallery);
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) type));
      ContextMenuService.Attach(type);
      FrameworkElement.StyleProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(ColorGallery.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (ColorGallery));
      return basevalue;
    }

    public override void OnApplyTemplate()
    {
      if (this.moreColorsButton != null)
        this.moreColorsButton.Click += new RoutedEventHandler(this.OnMoreColorsClick);
      this.moreColorsButton = this.GetTemplateChild("PART_MoreColors") as MenuItem;
      if (this.moreColorsButton != null)
        this.moreColorsButton.Click += new RoutedEventHandler(this.OnMoreColorsClick);
      if (this.noColorButton != null)
        this.noColorButton.Click -= new RoutedEventHandler(this.OnNoColorClick);
      this.noColorButton = this.GetTemplateChild("PART_NoColor") as MenuItem;
      if (this.noColorButton != null)
        this.noColorButton.Click += new RoutedEventHandler(this.OnNoColorClick);
      if (this.automaticButton != null)
        this.automaticButton.Click -= new RoutedEventHandler(this.OnAutomaticClick);
      this.automaticButton = this.GetTemplateChild("PART_AutomaticColor") as MenuItem;
      if (this.automaticButton != null)
        this.automaticButton.Click += new RoutedEventHandler(this.OnAutomaticClick);
      this.listBoxes.Clear();
      if (this.themeColorsListBox != null)
        this.themeColorsListBox.SelectionChanged -= new SelectionChangedEventHandler(this.OnListBoxSelectedChanged);
      this.themeColorsListBox = this.GetTemplateChild("PART_ThemeColorsListBox") as ListBox;
      this.listBoxes.Add(this.themeColorsListBox);
      if (this.themeColorsListBox != null)
        this.themeColorsListBox.SelectionChanged += new SelectionChangedEventHandler(this.OnListBoxSelectedChanged);
      if (this.themeGradientsListBox != null)
        this.themeGradientsListBox.SelectionChanged -= new SelectionChangedEventHandler(this.OnListBoxSelectedChanged);
      this.themeGradientsListBox = this.GetTemplateChild("PART_ThemeGradientColorsListBox") as ListBox;
      this.listBoxes.Add(this.themeGradientsListBox);
      if (this.themeGradientsListBox != null)
        this.themeGradientsListBox.SelectionChanged += new SelectionChangedEventHandler(this.OnListBoxSelectedChanged);
      if (this.standardColorsListBox != null)
        this.standardColorsListBox.SelectionChanged -= new SelectionChangedEventHandler(this.OnListBoxSelectedChanged);
      this.standardColorsListBox = this.GetTemplateChild("PART_StandardColorsListBox") as ListBox;
      this.listBoxes.Add(this.standardColorsListBox);
      if (this.standardColorsListBox != null)
        this.standardColorsListBox.SelectionChanged += new SelectionChangedEventHandler(this.OnListBoxSelectedChanged);
      if (this.standardGradientsListBox != null)
        this.standardGradientsListBox.SelectionChanged -= new SelectionChangedEventHandler(this.OnListBoxSelectedChanged);
      this.standardGradientsListBox = this.GetTemplateChild("PART_StandardGradientColorsListBox") as ListBox;
      this.listBoxes.Add(this.standardGradientsListBox);
      if (this.standardGradientsListBox != null)
        this.standardGradientsListBox.SelectionChanged += new SelectionChangedEventHandler(this.OnListBoxSelectedChanged);
      if (this.recentColorsListBox != null)
        this.recentColorsListBox.SelectionChanged -= new SelectionChangedEventHandler(this.OnListBoxSelectedChanged);
      this.recentColorsListBox = this.GetTemplateChild("PART_RecentColorsListBox") as ListBox;
      this.listBoxes.Add(this.recentColorsListBox);
      if (this.recentColorsListBox != null)
        this.recentColorsListBox.SelectionChanged += new SelectionChangedEventHandler(this.OnListBoxSelectedChanged);
      base.OnApplyTemplate();
      this.UpdateSelectedColor(this.SelectedColor);
    }

    private void OnMoreColorsClick(object sender, RoutedEventArgs e)
    {
      if (this.MoreColorsExecuting != null)
      {
        MoreColorsExecutingEventArgs e1 = new MoreColorsExecutingEventArgs();
        this.MoreColorsExecuting((object) this, e1);
        if (e1.Canceled)
          return;
        Color color = e1.Color;
        if (ColorGallery.RecentColors.Contains(color))
          ColorGallery.RecentColors.Remove(color);
        ColorGallery.RecentColors.Insert(0, color);
        this.recentColorsListBox.SelectedIndex = 0;
      }
      else
      {
        NativeMethods.CHOOSECOLOR lpcc = new NativeMethods.CHOOSECOLOR();
        Window window = Window.GetWindow((DependencyObject) this);
        if (window != null)
          lpcc.hwndOwner = new WindowInteropHelper(window).Handle;
        lpcc.Flags = 256;
        if (ColorGallery.customColors == IntPtr.Zero)
        {
          for (int index = 0; index < this.colorsArray.Length; ++index)
            this.colorsArray[index] = 16777215;
          ColorGallery.customColors = GCHandle.Alloc((object) this.colorsArray, GCHandleType.Pinned).AddrOfPinnedObject();
        }
        lpcc.lpCustColors = ColorGallery.customColors;
        if (!NativeMethods.ChooseColor(lpcc))
          return;
        Color color = ColorGallery.ConvertFromWin32Color(lpcc.rgbResult);
        if (ColorGallery.RecentColors.Contains(color))
          ColorGallery.RecentColors.Remove(color);
        ColorGallery.RecentColors.Insert(0, color);
        this.recentColorsListBox.SelectedIndex = 0;
      }
    }

    private static Color ConvertFromWin32Color(int color)
    {
      return Color.FromArgb(byte.MaxValue, (byte) (color & (int) byte.MaxValue), (byte) ((color & 65280) >> 8), (byte) ((color & 16711680) >> 16));
    }

    private void OnAutomaticClick(object sender, RoutedEventArgs e)
    {
      this.isSelectionChanged = true;
      this.noColorButton.IsChecked = false;
      this.automaticButton.IsChecked = true;
      for (int index = 0; index < this.listBoxes.Count; ++index)
        this.listBoxes[index].SelectedItem = (object) null;
      this.SelectedColor = new Color?();
      this.isSelectionChanged = false;
    }

    private void OnNoColorClick(object sender, RoutedEventArgs e)
    {
      this.isSelectionChanged = true;
      this.noColorButton.IsChecked = true;
      this.automaticButton.IsChecked = false;
      for (int index = 0; index < this.listBoxes.Count; ++index)
        this.listBoxes[index].SelectedItem = (object) null;
      this.SelectedColor = new Color?(Colors.Transparent);
      this.isSelectionChanged = false;
    }

    private void OnListBoxSelectedChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.isSelectionChanged)
        return;
      this.isSelectionChanged = true;
      if (e.AddedItems != null && e.AddedItems.Count > 0)
      {
        this.noColorButton.IsChecked = false;
        this.automaticButton.IsChecked = false;
        for (int index = 0; index < this.listBoxes.Count; ++index)
        {
          if (this.listBoxes[index] != sender)
            this.listBoxes[index].SelectedItem = (object) null;
        }
        this.SelectedColor = new Color?((Color) e.AddedItems[0]);
        PopupService.RaiseDismissPopupEvent((object) this, DismissPopupMode.Always);
      }
      this.isSelectionChanged = false;
    }

    private void UpdateGradients()
    {
      if (this.Mode == ColorGalleryMode.ThemeColors && this.Columns > 0)
      {
        this.ThemeGradients = this.ThemeColorGridRows <= 0 ? (Color[]) null : this.GenerateThemeGradients();
        if (this.StandardColorGridRows > 0)
          this.StandardGradients = this.GenerateStandardGradients();
        else
          this.StandardGradients = (Color[]) null;
      }
      else
      {
        this.StandardGradients = (Color[]) null;
        this.ThemeGradients = (Color[]) null;
      }
    }

    private Color[] GenerateStandardGradients()
    {
      int num = Math.Min(this.Columns, ColorGallery.StandardThemeColors.Length);
      Color[] standardGradients = new Color[this.Columns * this.StandardColorGridRows];
      for (int index1 = 0; index1 < num; ++index1)
      {
        Color[] gradient = ColorGallery.GetGradient(ColorGallery.StandardThemeColors[index1], this.StandardColorGridRows);
        for (int index2 = 0; index2 < this.StandardColorGridRows; ++index2)
          standardGradients[index1 + index2 * this.Columns] = gradient[index2];
      }
      return standardGradients;
    }

    private Color[] GenerateThemeGradients()
    {
      int num = Math.Min(this.Columns, this.ThemeColors.Count);
      Color[] themeGradients = new Color[this.Columns * this.ThemeColorGridRows];
      for (int index1 = 0; index1 < num; ++index1)
      {
        Color[] gradient = ColorGallery.GetGradient(this.ThemeColors[index1], this.ThemeColorGridRows);
        for (int index2 = 0; index2 < this.ThemeColorGridRows; ++index2)
          themeGradients[index1 + index2 * this.Columns] = gradient[index2];
      }
      return themeGradients;
    }

    private static double GetBrightness(Color color)
    {
      return ((double) color.R + (double) color.G + (double) color.B) / 765.0;
    }

    private static Color Lighter(Color color, double power)
    {
      double num1 = 765.0 - (double) color.R + (double) color.G + (double) color.B;
      double num2;
      double num3;
      double num4;
      double num5;
      if ((int) color.R + (int) color.G + (int) color.B == 0)
      {
        num2 = 1.0 / 3.0;
        num3 = 1.0 / 3.0;
        num4 = 1.0 / 3.0;
        num5 = power * (double) byte.MaxValue * 3.0;
      }
      else
      {
        num2 = ((double) byte.MaxValue - (double) color.R) / num1;
        num3 = ((double) byte.MaxValue - (double) color.G) / num1;
        num4 = ((double) byte.MaxValue - (double) color.B) / num1;
        num5 = ((double) color.R + (double) color.G + (double) color.B) * (power - 1.0);
      }
      return Color.FromRgb((byte) ((uint) color.R + (uint) (byte) (num2 * num5)), (byte) ((uint) color.G + (uint) (byte) (num3 * num5)), (byte) ((uint) color.B + (uint) (byte) (num4 * num5)));
    }

    private static Color Darker(Color color, double power)
    {
      double num1 = (double) color.R + (double) color.G + (double) color.B;
      double num2 = (double) color.R / num1;
      double num3 = (double) color.G / num1;
      double num4 = (double) color.B / num1;
      double num5 = (double) color.R + (double) color.G + (double) color.B;
      double num6 = num5 - num5 * power;
      return Color.FromRgb((byte) ((uint) color.R - (uint) (byte) (num2 * num6)), (byte) ((uint) color.G - (uint) (byte) (num3 * num6)), (byte) ((uint) color.B - (uint) (byte) (num4 * num6)));
    }

    private static Color Rebright(Color color, double newBrightness)
    {
      double brightness = ColorGallery.GetBrightness(color);
      double power = brightness != 0.0 ? newBrightness / brightness : 1.0 + newBrightness;
      return power > 1.0 ? ColorGallery.Lighter(color, power) : ColorGallery.Darker(color, power);
    }

    private static Color[] GetGradient(Color color, int count)
    {
      Color[] gradient = new Color[count];
      for (int index = 0; index < count; ++index)
      {
        double newBrightness = 0.15 + (double) index * 0.7 / (double) count;
        gradient[count - index - 1] = ColorGallery.Rebright(color, newBrightness);
      }
      return gradient;
    }
  }
}
