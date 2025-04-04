// Decompiled with JetBrains decompiler
// Type: WpfKb.Controls.OnScreenKey
// Assembly: WpfKb, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B294CC70-CB21-4202-BD7A-A4E6693370B9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\WpfKb.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using WpfKb.LogicalKeys;

#nullable disable
namespace WpfKb.Controls
{
  public class OnScreenKey : Border
  {
    public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(nameof (Key), typeof (ILogicalKey), typeof (OnScreenKey), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(OnScreenKey.OnKeyChanged)));
    public static readonly DependencyProperty AreAnimationsEnabledProperty = DependencyProperty.Register(nameof (AreAnimationsEnabled), typeof (bool), typeof (OnScreenKey), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty IsMouseOverAnimationEnabledProperty = DependencyProperty.Register(nameof (IsMouseOverAnimationEnabled), typeof (bool), typeof (OnScreenKey), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty IsOnScreenKeyDownProperty = DependencyProperty.Register(nameof (IsOnScreenKeyDown), typeof (bool), typeof (OnScreenKey), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty GridWidthProperty = DependencyProperty.Register(nameof (GridWidth), typeof (GridLength), typeof (OnScreenKey), (PropertyMetadata) new UIPropertyMetadata((object) new GridLength(1.0, GridUnitType.Star)));
    public static readonly RoutedEvent PreviewOnScreenKeyDownEvent = EventManager.RegisterRoutedEvent("PreviewOnScreenKeyDown", RoutingStrategy.Direct, typeof (OnScreenKeyEventHandler), typeof (OnScreenKey));
    public static readonly RoutedEvent PreviewOnScreenKeyUpEvent = EventManager.RegisterRoutedEvent("PreviewOnScreenKeyUp", RoutingStrategy.Direct, typeof (OnScreenKeyEventHandler), typeof (OnScreenKey));
    public static readonly RoutedEvent OnScreenKeyDownEvent = EventManager.RegisterRoutedEvent("OnScreenKeyDown", RoutingStrategy.Direct, typeof (OnScreenKeyEventHandler), typeof (OnScreenKey));
    public static readonly RoutedEvent OnScreenKeyUpEvent = EventManager.RegisterRoutedEvent("OnScreenKeyUp", RoutingStrategy.Direct, typeof (OnScreenKeyEventHandler), typeof (OnScreenKey));
    public static readonly RoutedEvent OnScreenKeyPressEvent = EventManager.RegisterRoutedEvent("OnScreenKeyPress", RoutingStrategy.Direct, typeof (OnScreenKeyEventHandler), typeof (OnScreenKey));
    private Border _keySurface;
    private Border _mouseDownSurface;
    private TextBlock _keyText;
    private readonly GradientBrush _keySurfaceBrush = (GradientBrush) new LinearGradientBrush(new GradientStopCollection()
    {
      new GradientStop(Color.FromRgb((byte) 35, (byte) 96, (byte) 144), 0.0),
      new GradientStop(Color.FromRgb((byte) 35, (byte) 96, (byte) 144), 0.6),
      new GradientStop(Color.FromRgb((byte) 35, (byte) 96, (byte) 165), 1.0)
    }, 90.0);
    private readonly GradientBrush _keySurfaceBorderBrush = (GradientBrush) new LinearGradientBrush(new GradientStopCollection()
    {
      new GradientStop(Color.FromRgb((byte) 200, (byte) 200, (byte) 200), 0.0),
      new GradientStop(Color.FromRgb((byte) 56, (byte) 56, (byte) 56), 1.0)
    }, 90.0);
    private readonly GradientBrush _keySurfaceMouseOverBrush = (GradientBrush) new LinearGradientBrush(new GradientStopCollection()
    {
      new GradientStop(Color.FromRgb((byte) 120, (byte) 120, (byte) 120), 0.0),
      new GradientStop(Color.FromRgb((byte) 120, (byte) 120, (byte) 120), 0.6),
      new GradientStop(Color.FromRgb((byte) 80, (byte) 80, (byte) 80), 1.0)
    }, 90.0);
    private readonly GradientBrush _keySurfaceMouseOverBorderBrush = (GradientBrush) new LinearGradientBrush(new GradientStopCollection()
    {
      new GradientStop(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue), 0.0),
      new GradientStop(Color.FromRgb((byte) 100, (byte) 100, (byte) 100), 1.0)
    }, 90.0);
    private readonly SolidColorBrush _keyOutsideBorderBrush = new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 26, (byte) 26, (byte) 26));

    public ILogicalKey Key
    {
      get => (ILogicalKey) this.GetValue(OnScreenKey.KeyProperty);
      set => this.SetValue(OnScreenKey.KeyProperty, (object) value);
    }

    public bool AreAnimationsEnabled
    {
      get => (bool) this.GetValue(OnScreenKey.AreAnimationsEnabledProperty);
      set => this.SetValue(OnScreenKey.AreAnimationsEnabledProperty, (object) value);
    }

    public bool IsMouseOverAnimationEnabled
    {
      get => (bool) this.GetValue(OnScreenKey.IsMouseOverAnimationEnabledProperty);
      set => this.SetValue(OnScreenKey.IsMouseOverAnimationEnabledProperty, (object) value);
    }

    public bool IsOnScreenKeyDown
    {
      get => (bool) this.GetValue(OnScreenKey.IsOnScreenKeyDownProperty);
      private set => this.SetValue(OnScreenKey.IsOnScreenKeyDownProperty, (object) value);
    }

    public int GridRow
    {
      get => (int) this.GetValue(Grid.RowProperty);
      set => this.SetValue(Grid.RowProperty, (object) value);
    }

    public int GridColumn
    {
      get => (int) this.GetValue(Grid.ColumnProperty);
      set => this.SetValue(Grid.ColumnProperty, (object) value);
    }

    public GridLength GridWidth
    {
      get => (GridLength) this.GetValue(OnScreenKey.GridWidthProperty);
      set => this.SetValue(OnScreenKey.GridWidthProperty, (object) value);
    }

    protected static void OnKeyChanged(
      DependencyObject sender,
      DependencyPropertyChangedEventArgs e)
    {
      ((OnScreenKey) sender).SetupControl((ILogicalKey) e.NewValue);
    }

    public event OnScreenKeyEventHandler PreviewOnScreenKeyDown
    {
      add => this.AddHandler(OnScreenKey.PreviewOnScreenKeyDownEvent, (Delegate) value);
      remove => this.RemoveHandler(OnScreenKey.PreviewOnScreenKeyDownEvent, (Delegate) value);
    }

    protected OnScreenKeyEventArgs RaisePreviewOnScreenKeyDownEvent()
    {
      OnScreenKeyEventArgs e = new OnScreenKeyEventArgs(OnScreenKey.PreviewOnScreenKeyDownEvent, this);
      this.RaiseEvent((RoutedEventArgs) e);
      return e;
    }

    public event OnScreenKeyEventHandler PreviewOnScreenKeyUp
    {
      add => this.AddHandler(OnScreenKey.PreviewOnScreenKeyUpEvent, (Delegate) value);
      remove => this.RemoveHandler(OnScreenKey.PreviewOnScreenKeyUpEvent, (Delegate) value);
    }

    protected OnScreenKeyEventArgs RaisePreviewOnScreenKeyUpEvent()
    {
      OnScreenKeyEventArgs e = new OnScreenKeyEventArgs(OnScreenKey.PreviewOnScreenKeyUpEvent, this);
      this.RaiseEvent((RoutedEventArgs) e);
      return e;
    }

    public event OnScreenKeyEventHandler OnScreenKeyDown
    {
      add => this.AddHandler(OnScreenKey.OnScreenKeyDownEvent, (Delegate) value);
      remove => this.RemoveHandler(OnScreenKey.OnScreenKeyDownEvent, (Delegate) value);
    }

    protected OnScreenKeyEventArgs RaiseOnScreenKeyDownEvent()
    {
      OnScreenKeyEventArgs e = new OnScreenKeyEventArgs(OnScreenKey.OnScreenKeyDownEvent, this);
      this.RaiseEvent((RoutedEventArgs) e);
      return e;
    }

    public event OnScreenKeyEventHandler OnScreenKeyUp
    {
      add => this.AddHandler(OnScreenKey.OnScreenKeyUpEvent, (Delegate) value);
      remove => this.RemoveHandler(OnScreenKey.OnScreenKeyUpEvent, (Delegate) value);
    }

    protected OnScreenKeyEventArgs RaiseOnScreenKeyUpEvent()
    {
      OnScreenKeyEventArgs e = new OnScreenKeyEventArgs(OnScreenKey.OnScreenKeyUpEvent, this);
      this.RaiseEvent((RoutedEventArgs) e);
      return e;
    }

    public event OnScreenKeyEventHandler OnScreenKeyPress
    {
      add => this.AddHandler(OnScreenKey.OnScreenKeyPressEvent, (Delegate) value);
      remove => this.RemoveHandler(OnScreenKey.OnScreenKeyPressEvent, (Delegate) value);
    }

    protected OnScreenKeyEventArgs RaiseOnScreenKeyPressEvent()
    {
      OnScreenKeyEventArgs e = new OnScreenKeyEventArgs(OnScreenKey.OnScreenKeyPressEvent, this);
      this.RaiseEvent((RoutedEventArgs) e);
      return e;
    }

    private void SetupControl(ILogicalKey key)
    {
      this.CornerRadius = new CornerRadius(3.0);
      this.BorderBrush = (Brush) this._keyOutsideBorderBrush;
      this.BorderThickness = new Thickness(1.0);
      this.SnapsToDevicePixels = true;
      Grid grid = new Grid();
      this.Child = (UIElement) grid;
      Border border1 = new Border();
      border1.CornerRadius = new CornerRadius(3.0);
      border1.BorderBrush = (Brush) this._keySurfaceBorderBrush;
      border1.BorderThickness = new Thickness(1.0);
      border1.Background = (Brush) this._keySurfaceBrush;
      border1.SnapsToDevicePixels = true;
      this._keySurface = border1;
      grid.Children.Add((UIElement) this._keySurface);
      Border border2 = new Border();
      border2.CornerRadius = new CornerRadius(3.0);
      border2.Background = (Brush) Brushes.White;
      border2.Opacity = 0.0;
      border2.SnapsToDevicePixels = true;
      this._mouseDownSurface = border2;
      grid.Children.Add((UIElement) this._mouseDownSurface);
      TextBlock textBlock = new TextBlock();
      textBlock.Margin = new Thickness(3.0, 0.0, 0.0, 0.0);
      textBlock.FontSize = 20.0;
      textBlock.FontWeight = FontWeights.Bold;
      textBlock.HorizontalAlignment = HorizontalAlignment.Center;
      textBlock.VerticalAlignment = VerticalAlignment.Center;
      textBlock.Foreground = (Brush) Brushes.White;
      textBlock.SnapsToDevicePixels = true;
      this._keyText = textBlock;
      this._keyText.SetBinding(TextBlock.TextProperty, (BindingBase) new Binding("DisplayName")
      {
        Source = (object) key
      });
      grid.Children.Add((UIElement) this._keyText);
      key.PropertyChanged += new PropertyChangedEventHandler(this.Key_PropertyChanged);
      key.LogicalKeyPressed += new LogicalKeyPressedEventHandler(this.Key_VirtualKeyPressed);
    }

    private void Key_VirtualKeyPressed(object sender, LogicalKeyEventArgs e)
    {
      this.RaiseOnScreenKeyPressEvent();
    }

    private void Key_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!(this.Key is ModifierKeyBase) || !(e.PropertyName == "IsInEffect"))
        return;
      if (((ModifierKeyBase) this.Key).IsInEffect)
        this.AnimateMouseDown();
      else
        this.AnimateMouseUp();
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
      this.HandleKeyDown();
      base.OnMouseDown(e);
    }

    protected void HandleKeyDown()
    {
      if (!this.RaisePreviewOnScreenKeyDownEvent().Handled)
      {
        this.IsOnScreenKeyDown = true;
        this.AnimateMouseDown();
        this.Key.Press();
      }
      this.RaiseOnScreenKeyDownEvent();
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
      this.HandleKeyUp();
      base.OnMouseUp(e);
    }

    private void HandleKeyUp()
    {
      if (!this.RaisePreviewOnScreenKeyUpEvent().Handled)
      {
        this.IsOnScreenKeyDown = false;
        this.AnimateMouseUp();
      }
      this.RaiseOnScreenKeyUpEvent();
    }

    private void AnimateMouseDown()
    {
      this._mouseDownSurface.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) new DoubleAnimation(1.0, new Duration(TimeSpan.Zero)));
      this._keyText.Foreground = (Brush) this._keyOutsideBorderBrush;
    }

    private void AnimateMouseUp()
    {
      if ((this.Key is TogglingModifierKey || this.Key is InstantaneousModifierKey) && ((ModifierKeyBase) this.Key).IsInEffect)
        return;
      this._keySurface.BorderBrush = (Brush) this._keySurfaceBorderBrush;
      this._keyText.Foreground = (Brush) Brushes.White;
      if (!this.AreAnimationsEnabled || this.Key is TogglingModifierKey || this.Key is InstantaneousModifierKey)
        this._mouseDownSurface.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) new DoubleAnimation(0.0, new Duration(TimeSpan.Zero)));
      else
        this._mouseDownSurface.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) new DoubleAnimation(0.0, Duration.Automatic));
    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {
      if (this.IsMouseOverAnimationEnabled)
      {
        this._keySurface.Background = (Brush) this._keySurfaceMouseOverBrush;
        this._keySurface.BorderBrush = (Brush) this._keySurfaceMouseOverBorderBrush;
      }
      base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
      if (this.IsMouseOverAnimationEnabled)
      {
        if (this.Key is TogglingModifierKey && ((ModifierKeyBase) this.Key).IsInEffect)
          return;
        this._keySurface.Background = (Brush) this._keySurfaceBrush;
        this._keySurface.BorderBrush = (Brush) this._keySurfaceBorderBrush;
      }
      if (this.IsOnScreenKeyDown)
        this.HandleKeyUp();
      base.OnMouseLeave(e);
    }
  }
}
