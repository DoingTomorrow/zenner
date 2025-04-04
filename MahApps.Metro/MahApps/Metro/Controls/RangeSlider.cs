// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.RangeSlider
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

#nullable disable
namespace MahApps.Metro.Controls
{
  [DefaultEvent("RangeSelectionChanged")]
  [TemplatePart(Name = "PART_Container", Type = typeof (StackPanel))]
  [TemplatePart(Name = "PART_RangeSliderContainer", Type = typeof (StackPanel))]
  [TemplatePart(Name = "PART_LeftEdge", Type = typeof (RepeatButton))]
  [TemplatePart(Name = "PART_RightEdge", Type = typeof (RepeatButton))]
  [TemplatePart(Name = "PART_LeftThumb", Type = typeof (Thumb))]
  [TemplatePart(Name = "PART_MiddleThumb", Type = typeof (Thumb))]
  [TemplatePart(Name = "PART_PART_TopTick", Type = typeof (TickBar))]
  [TemplatePart(Name = "PART_PART_BottomTick", Type = typeof (TickBar))]
  [TemplatePart(Name = "PART_RightThumb", Type = typeof (Thumb))]
  public class RangeSlider : RangeBase
  {
    public static RoutedUICommand MoveBack = new RoutedUICommand(nameof (MoveBack), nameof (MoveBack), typeof (RangeSlider), new InputGestureCollection((IList) new InputGesture[1]
    {
      (InputGesture) new KeyGesture(Key.B, ModifierKeys.Control)
    }));
    public static RoutedUICommand MoveForward = new RoutedUICommand(nameof (MoveForward), nameof (MoveForward), typeof (RangeSlider), new InputGestureCollection((IList) new InputGesture[1]
    {
      (InputGesture) new KeyGesture(Key.F, ModifierKeys.Control)
    }));
    public static RoutedUICommand MoveAllForward = new RoutedUICommand(nameof (MoveAllForward), nameof (MoveAllForward), typeof (RangeSlider), new InputGestureCollection((IList) new InputGesture[1]
    {
      (InputGesture) new KeyGesture(Key.F, ModifierKeys.Alt)
    }));
    public static RoutedUICommand MoveAllBack = new RoutedUICommand(nameof (MoveAllBack), nameof (MoveAllBack), typeof (RangeSlider), new InputGestureCollection((IList) new InputGesture[1]
    {
      (InputGesture) new KeyGesture(Key.B, ModifierKeys.Alt)
    }));
    public static readonly RoutedEvent RangeSelectionChangedEvent = EventManager.RegisterRoutedEvent("RangeSelectionChanged", RoutingStrategy.Bubble, typeof (RangeSelectionChangedEventHandler), typeof (RangeSlider));
    public static readonly RoutedEvent LowerValueChangedEvent = EventManager.RegisterRoutedEvent("LowerValueChanged", RoutingStrategy.Bubble, typeof (RangeParameterChangedEventHandler), typeof (RangeSlider));
    public static readonly RoutedEvent UpperValueChangedEvent = EventManager.RegisterRoutedEvent("UpperValueChanged", RoutingStrategy.Bubble, typeof (RangeParameterChangedEventHandler), typeof (RangeSlider));
    public static readonly RoutedEvent LowerThumbDragStartedEvent = EventManager.RegisterRoutedEvent("LowerThumbDragStarted", RoutingStrategy.Bubble, typeof (DragStartedEventHandler), typeof (RangeSlider));
    public static readonly RoutedEvent LowerThumbDragCompletedEvent = EventManager.RegisterRoutedEvent("LowerThumbDragCompleted", RoutingStrategy.Bubble, typeof (DragCompletedEventHandler), typeof (RangeSlider));
    public static readonly RoutedEvent UpperThumbDragStartedEvent = EventManager.RegisterRoutedEvent("UpperThumbDragStarted", RoutingStrategy.Bubble, typeof (DragStartedEventHandler), typeof (RangeSlider));
    public static readonly RoutedEvent UpperThumbDragCompletedEvent = EventManager.RegisterRoutedEvent("UpperThumbDragCompleted", RoutingStrategy.Bubble, typeof (DragCompletedEventHandler), typeof (RangeSlider));
    public static readonly RoutedEvent CentralThumbDragStartedEvent = EventManager.RegisterRoutedEvent("CentralThumbDragStarted", RoutingStrategy.Bubble, typeof (DragStartedEventHandler), typeof (RangeSlider));
    public static readonly RoutedEvent CentralThumbDragCompletedEvent = EventManager.RegisterRoutedEvent("CentralThumbDragCompleted", RoutingStrategy.Bubble, typeof (DragCompletedEventHandler), typeof (RangeSlider));
    public static readonly RoutedEvent LowerThumbDragDeltaEvent = EventManager.RegisterRoutedEvent("LowerThumbDragDelta", RoutingStrategy.Bubble, typeof (DragDeltaEventHandler), typeof (RangeSlider));
    public static readonly RoutedEvent UpperThumbDragDeltaEvent = EventManager.RegisterRoutedEvent("UpperThumbDragDelta", RoutingStrategy.Bubble, typeof (DragDeltaEventHandler), typeof (RangeSlider));
    public static readonly RoutedEvent CentralThumbDragDeltaEvent = EventManager.RegisterRoutedEvent("CentralThumbDragDelta", RoutingStrategy.Bubble, typeof (DragDeltaEventHandler), typeof (RangeSlider));
    public static readonly DependencyProperty UpperValueProperty = DependencyProperty.Register(nameof (UpperValue), typeof (double), typeof (RangeSlider), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(RangeSlider.RangesChanged), new CoerceValueCallback(RangeSlider.CoerceUpperValue)));
    public static readonly DependencyProperty LowerValueProperty = DependencyProperty.Register(nameof (LowerValue), typeof (double), typeof (RangeSlider), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(RangeSlider.RangesChanged), new CoerceValueCallback(RangeSlider.CoerceLowerValue)));
    public static readonly DependencyProperty MinRangeProperty = DependencyProperty.Register(nameof (MinRange), typeof (double), typeof (RangeSlider), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0, new PropertyChangedCallback(RangeSlider.MinRangeChanged), new CoerceValueCallback(RangeSlider.CoerceMinRange)), new ValidateValueCallback(RangeSlider.IsValidMinRange));
    public static readonly DependencyProperty MinRangeWidthProperty = DependencyProperty.Register(nameof (MinRangeWidth), typeof (double), typeof (RangeSlider), (PropertyMetadata) new FrameworkPropertyMetadata((object) 30.0, new PropertyChangedCallback(RangeSlider.MinRangeWidthChanged), new CoerceValueCallback(RangeSlider.CoerceMinRangeWidth)), new ValidateValueCallback(RangeSlider.IsValidMinRange));
    public static readonly DependencyProperty MoveWholeRangeProperty = DependencyProperty.Register(nameof (MoveWholeRange), typeof (bool), typeof (RangeSlider), new PropertyMetadata((object) false));
    public static readonly DependencyProperty ExtendedModeProperty = DependencyProperty.Register(nameof (ExtendedMode), typeof (bool), typeof (RangeSlider), new PropertyMetadata((object) false));
    public static readonly DependencyProperty IsSnapToTickEnabledProperty = DependencyProperty.Register(nameof (IsSnapToTickEnabled), typeof (bool), typeof (RangeSlider), new PropertyMetadata((object) false));
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (RangeSlider), (PropertyMetadata) new FrameworkPropertyMetadata((object) Orientation.Horizontal));
    public static readonly DependencyProperty TickFrequencyProperty = DependencyProperty.Register(nameof (TickFrequency), typeof (double), typeof (RangeSlider), (PropertyMetadata) new FrameworkPropertyMetadata((object) 1.0), new ValidateValueCallback(RangeSlider.IsValidTickFrequency));
    public static readonly DependencyProperty IsMoveToPointEnabledProperty = DependencyProperty.Register(nameof (IsMoveToPointEnabled), typeof (bool), typeof (RangeSlider), new PropertyMetadata((object) false));
    public static readonly DependencyProperty TickPlacementProperty = DependencyProperty.Register(nameof (TickPlacement), typeof (TickPlacement), typeof (RangeSlider), (PropertyMetadata) new FrameworkPropertyMetadata((object) TickPlacement.None));
    public static readonly DependencyProperty AutoToolTipPlacementProperty = DependencyProperty.Register(nameof (AutoToolTipPlacement), typeof (AutoToolTipPlacement), typeof (RangeSlider), (PropertyMetadata) new FrameworkPropertyMetadata((object) AutoToolTipPlacement.None));
    public static readonly DependencyProperty AutoToolTipPrecisionProperty = DependencyProperty.Register(nameof (AutoToolTipPrecision), typeof (int), typeof (RangeSlider), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0), new ValidateValueCallback(RangeSlider.IsValidPrecision));
    public static readonly DependencyProperty AutoToolTipTextConverterProperty = DependencyProperty.Register(nameof (AutoToolTipTextConverter), typeof (IValueConverter), typeof (RangeSlider), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(nameof (Interval), typeof (int), typeof (RangeSlider), (PropertyMetadata) new FrameworkPropertyMetadata((object) 100, new PropertyChangedCallback(RangeSlider.IntervalChangedCallback)), new ValidateValueCallback(RangeSlider.IsValidPrecision));
    private const double Epsilon = 1.53E-06;
    private bool _internalUpdate;
    private Thumb _centerThumb;
    private Thumb _leftThumb;
    private Thumb _rightThumb;
    private RepeatButton _leftButton;
    private RepeatButton _rightButton;
    private StackPanel _visualElementsContainer;
    private StackPanel _container;
    private double _movableWidth;
    private readonly DispatcherTimer _timer;
    private uint _tickCount;
    private double _currentpoint;
    private bool _isInsideRange;
    private bool _centerThumbBlocked;
    private RangeSlider.Direction _direction;
    private RangeSlider.ButtonType _bType;
    private Point _position;
    private Point _basePoint;
    private double _currenValue;
    private double _density;
    private System.Windows.Controls.ToolTip _autoToolTip;
    private double _oldLower;
    private double _oldUpper;
    private bool _isMoved;
    private bool _roundToPrecision;
    private int _precision;
    private readonly PropertyChangeNotifier actualWidthPropertyChangeNotifier;
    private readonly PropertyChangeNotifier actualHeightPropertyChangeNotifier;

    public event RangeSelectionChangedEventHandler RangeSelectionChanged
    {
      add => this.AddHandler(RangeSlider.RangeSelectionChangedEvent, (Delegate) value);
      remove => this.RemoveHandler(RangeSlider.RangeSelectionChangedEvent, (Delegate) value);
    }

    public event RangeParameterChangedEventHandler LowerValueChanged
    {
      add => this.AddHandler(RangeSlider.LowerValueChangedEvent, (Delegate) value);
      remove => this.RemoveHandler(RangeSlider.LowerValueChangedEvent, (Delegate) value);
    }

    public event RangeParameterChangedEventHandler UpperValueChanged
    {
      add => this.AddHandler(RangeSlider.UpperValueChangedEvent, (Delegate) value);
      remove => this.RemoveHandler(RangeSlider.UpperValueChangedEvent, (Delegate) value);
    }

    public event DragStartedEventHandler LowerThumbDragStarted
    {
      add => this.AddHandler(RangeSlider.LowerThumbDragStartedEvent, (Delegate) value);
      remove => this.RemoveHandler(RangeSlider.LowerThumbDragStartedEvent, (Delegate) value);
    }

    public event DragCompletedEventHandler LowerThumbDragCompleted
    {
      add => this.AddHandler(RangeSlider.LowerThumbDragCompletedEvent, (Delegate) value);
      remove => this.RemoveHandler(RangeSlider.LowerThumbDragCompletedEvent, (Delegate) value);
    }

    public event DragStartedEventHandler UpperThumbDragStarted
    {
      add => this.AddHandler(RangeSlider.UpperThumbDragStartedEvent, (Delegate) value);
      remove => this.RemoveHandler(RangeSlider.UpperThumbDragStartedEvent, (Delegate) value);
    }

    public event DragCompletedEventHandler UpperThumbDragCompleted
    {
      add => this.AddHandler(RangeSlider.UpperThumbDragCompletedEvent, (Delegate) value);
      remove => this.RemoveHandler(RangeSlider.UpperThumbDragCompletedEvent, (Delegate) value);
    }

    public event DragStartedEventHandler CentralThumbDragStarted
    {
      add => this.AddHandler(RangeSlider.CentralThumbDragStartedEvent, (Delegate) value);
      remove => this.RemoveHandler(RangeSlider.CentralThumbDragStartedEvent, (Delegate) value);
    }

    public event DragCompletedEventHandler CentralThumbDragCompleted
    {
      add => this.AddHandler(RangeSlider.CentralThumbDragCompletedEvent, (Delegate) value);
      remove => this.RemoveHandler(RangeSlider.CentralThumbDragCompletedEvent, (Delegate) value);
    }

    public event DragDeltaEventHandler LowerThumbDragDelta
    {
      add => this.AddHandler(RangeSlider.LowerThumbDragDeltaEvent, (Delegate) value);
      remove => this.RemoveHandler(RangeSlider.LowerThumbDragDeltaEvent, (Delegate) value);
    }

    public event DragDeltaEventHandler UpperThumbDragDelta
    {
      add => this.AddHandler(RangeSlider.UpperThumbDragDeltaEvent, (Delegate) value);
      remove => this.RemoveHandler(RangeSlider.UpperThumbDragDeltaEvent, (Delegate) value);
    }

    public event DragDeltaEventHandler CentralThumbDragDelta
    {
      add => this.AddHandler(RangeSlider.CentralThumbDragDeltaEvent, (Delegate) value);
      remove => this.RemoveHandler(RangeSlider.CentralThumbDragDeltaEvent, (Delegate) value);
    }

    [Bindable(true)]
    [Category("Behavior")]
    public int Interval
    {
      get => (int) this.GetValue(RangeSlider.IntervalProperty);
      set => this.SetValue(RangeSlider.IntervalProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Appearance")]
    public int AutoToolTipPrecision
    {
      get => (int) this.GetValue(RangeSlider.AutoToolTipPrecisionProperty);
      set => this.SetValue(RangeSlider.AutoToolTipPrecisionProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Behavior")]
    public IValueConverter AutoToolTipTextConverter
    {
      get => (IValueConverter) this.GetValue(RangeSlider.AutoToolTipTextConverterProperty);
      set => this.SetValue(RangeSlider.AutoToolTipTextConverterProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Behavior")]
    public AutoToolTipPlacement AutoToolTipPlacement
    {
      get => (AutoToolTipPlacement) this.GetValue(RangeSlider.AutoToolTipPlacementProperty);
      set => this.SetValue(RangeSlider.AutoToolTipPlacementProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Common")]
    public TickPlacement TickPlacement
    {
      get => (TickPlacement) this.GetValue(RangeSlider.TickPlacementProperty);
      set => this.SetValue(RangeSlider.TickPlacementProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Common")]
    public bool IsMoveToPointEnabled
    {
      get => (bool) this.GetValue(RangeSlider.IsMoveToPointEnabledProperty);
      set => this.SetValue(RangeSlider.IsMoveToPointEnabledProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Common")]
    public double TickFrequency
    {
      get => (double) this.GetValue(RangeSlider.TickFrequencyProperty);
      set => this.SetValue(RangeSlider.TickFrequencyProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Common")]
    public Orientation Orientation
    {
      get => (Orientation) this.GetValue(RangeSlider.OrientationProperty);
      set => this.SetValue(RangeSlider.OrientationProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Appearance")]
    public bool IsSnapToTickEnabled
    {
      get => (bool) this.GetValue(RangeSlider.IsSnapToTickEnabledProperty);
      set => this.SetValue(RangeSlider.IsSnapToTickEnabledProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Behavior")]
    public bool ExtendedMode
    {
      get => (bool) this.GetValue(RangeSlider.ExtendedModeProperty);
      set => this.SetValue(RangeSlider.ExtendedModeProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Behavior")]
    public bool MoveWholeRange
    {
      get => (bool) this.GetValue(RangeSlider.MoveWholeRangeProperty);
      set => this.SetValue(RangeSlider.MoveWholeRangeProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Common")]
    public double MinRangeWidth
    {
      get => (double) this.GetValue(RangeSlider.MinRangeWidthProperty);
      set => this.SetValue(RangeSlider.MinRangeWidthProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Common")]
    public double LowerValue
    {
      get => (double) this.GetValue(RangeSlider.LowerValueProperty);
      set => this.SetValue(RangeSlider.LowerValueProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Common")]
    public double UpperValue
    {
      get => (double) this.GetValue(RangeSlider.UpperValueProperty);
      set => this.SetValue(RangeSlider.UpperValueProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Common")]
    public double MinRange
    {
      get => (double) this.GetValue(RangeSlider.MinRangeProperty);
      set => this.SetValue(RangeSlider.MinRangeProperty, (object) value);
    }

    public double MovableRange => this.Maximum - this.Minimum - this.MinRange;

    public RangeSlider()
    {
      this.CommandBindings.Add(new CommandBinding((ICommand) RangeSlider.MoveBack, new ExecutedRoutedEventHandler(this.MoveBackHandler)));
      this.CommandBindings.Add(new CommandBinding((ICommand) RangeSlider.MoveForward, new ExecutedRoutedEventHandler(this.MoveForwardHandler)));
      this.CommandBindings.Add(new CommandBinding((ICommand) RangeSlider.MoveAllForward, new ExecutedRoutedEventHandler(this.MoveAllForwardHandler)));
      this.CommandBindings.Add(new CommandBinding((ICommand) RangeSlider.MoveAllBack, new ExecutedRoutedEventHandler(this.MoveAllBackHandler)));
      this.actualWidthPropertyChangeNotifier = new PropertyChangeNotifier((DependencyObject) this, FrameworkElement.ActualWidthProperty);
      this.actualWidthPropertyChangeNotifier.ValueChanged += (EventHandler) ((s, e) => this.ReCalculateSize());
      this.actualHeightPropertyChangeNotifier = new PropertyChangeNotifier((DependencyObject) this, FrameworkElement.ActualHeightProperty);
      this.actualHeightPropertyChangeNotifier.ValueChanged += (EventHandler) ((s, e) => this.ReCalculateSize());
      this._timer = new DispatcherTimer();
      this._timer.Tick += new EventHandler(this.MoveToNextValue);
      this._timer.Interval = TimeSpan.FromMilliseconds((double) this.Interval);
    }

    static RangeSlider()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (RangeSlider), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (RangeSlider)));
      RangeBase.MinimumProperty.OverrideMetadata(typeof (RangeSlider), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(RangeSlider.MinPropertyChangedCallback), new CoerceValueCallback(RangeSlider.CoerceMinimum)));
      RangeBase.MaximumProperty.OverrideMetadata(typeof (RangeSlider), (PropertyMetadata) new FrameworkPropertyMetadata((object) 100.0, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(RangeSlider.MaxPropertyChangedCallback), new CoerceValueCallback(RangeSlider.CoerceMaximum)));
    }

    protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
    {
      this.ReCalculateSize();
    }

    protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
    {
      this.ReCalculateSize();
    }

    private void MoveAllBackHandler(object sender, ExecutedRoutedEventArgs e)
    {
      this.ResetSelection(true);
    }

    private void MoveAllForwardHandler(object sender, ExecutedRoutedEventArgs e)
    {
      this.ResetSelection(false);
    }

    private void MoveBackHandler(object sender, ExecutedRoutedEventArgs e)
    {
      this.MoveSelection(true);
    }

    private void MoveForwardHandler(object sender, ExecutedRoutedEventArgs e)
    {
      this.MoveSelection(false);
    }

    private static void MoveThumb(
      FrameworkElement x,
      FrameworkElement y,
      double change,
      Orientation orientation)
    {
      RangeSlider.Direction direction = RangeSlider.Direction.Increase;
      RangeSlider.MoveThumb(x, y, change, orientation, out direction);
    }

    private static void MoveThumb(
      FrameworkElement x,
      FrameworkElement y,
      double change,
      Orientation orientation,
      out RangeSlider.Direction direction)
    {
      direction = RangeSlider.Direction.Increase;
      switch (orientation)
      {
        case Orientation.Horizontal:
          direction = change < 0.0 ? RangeSlider.Direction.Decrease : RangeSlider.Direction.Increase;
          RangeSlider.MoveThumbHorizontal(x, y, change);
          break;
        case Orientation.Vertical:
          direction = change < 0.0 ? RangeSlider.Direction.Increase : RangeSlider.Direction.Decrease;
          RangeSlider.MoveThumbVertical(x, y, change);
          break;
      }
    }

    private static void MoveThumbHorizontal(
      FrameworkElement x,
      FrameworkElement y,
      double horizonalChange)
    {
      if (double.IsNaN(x.Width) || double.IsNaN(y.Width))
        return;
      if (horizonalChange < 0.0)
      {
        double changeKeepPositive = RangeSlider.GetChangeKeepPositive(x.Width, horizonalChange);
        if (x.Name == "PART_MiddleThumb")
        {
          if (x.Width <= x.MinWidth)
            return;
          if (x.Width + changeKeepPositive < x.MinWidth)
          {
            double num = x.Width - x.MinWidth;
            x.Width = x.MinWidth;
            y.Width += num;
          }
          else
          {
            x.Width += changeKeepPositive;
            y.Width -= changeKeepPositive;
          }
        }
        else
        {
          x.Width += changeKeepPositive;
          y.Width -= changeKeepPositive;
        }
      }
      else
      {
        if (horizonalChange <= 0.0)
          return;
        double num1 = -RangeSlider.GetChangeKeepPositive(y.Width, -horizonalChange);
        if (y.Name == "PART_MiddleThumb")
        {
          if (y.Width <= y.MinWidth)
            return;
          if (y.Width - num1 < y.MinWidth)
          {
            double num2 = y.Width - y.MinWidth;
            y.Width = y.MinWidth;
            x.Width += num2;
          }
          else
          {
            x.Width += num1;
            y.Width -= num1;
          }
        }
        else
        {
          x.Width += num1;
          y.Width -= num1;
        }
      }
    }

    private static void MoveThumbVertical(
      FrameworkElement x,
      FrameworkElement y,
      double verticalChange)
    {
      if (double.IsNaN(x.Height) || double.IsNaN(y.Height))
        return;
      if (verticalChange < 0.0)
      {
        double num1 = -RangeSlider.GetChangeKeepPositive(y.Height, verticalChange);
        if (y.Name == "PART_MiddleThumb")
        {
          if (y.Height <= y.MinHeight)
            return;
          if (y.Height - num1 < y.MinHeight)
          {
            double num2 = y.Height - y.MinHeight;
            y.Height = y.MinHeight;
            x.Height += num2;
          }
          else
          {
            x.Height += num1;
            y.Height -= num1;
          }
        }
        else
        {
          x.Height += num1;
          y.Height -= num1;
        }
      }
      else
      {
        if (verticalChange <= 0.0)
          return;
        double changeKeepPositive = RangeSlider.GetChangeKeepPositive(x.Height, -verticalChange);
        if (x.Name == "PART_MiddleThumb")
        {
          if (x.Height <= y.MinHeight)
            return;
          if (x.Height + changeKeepPositive < x.MinHeight)
          {
            double num = x.Height - x.MinHeight;
            x.Height = x.MinHeight;
            y.Height += num;
          }
          else
          {
            x.Height += changeKeepPositive;
            y.Height -= changeKeepPositive;
          }
        }
        else
        {
          x.Height += changeKeepPositive;
          y.Height -= changeKeepPositive;
        }
      }
    }

    private void ReCalculateSize()
    {
      if (this._leftButton == null || this._rightButton == null || this._centerThumb == null)
        return;
      if (this.Orientation == Orientation.Horizontal)
      {
        this._movableWidth = Math.Max(this.ActualWidth - this._rightThumb.ActualWidth - this._leftThumb.ActualWidth - this.MinRangeWidth, 1.0);
        if (this.MovableRange <= 0.0)
        {
          this._leftButton.Width = double.NaN;
          this._rightButton.Width = double.NaN;
        }
        else
        {
          this._leftButton.Width = Math.Max(this._movableWidth * (this.LowerValue - this.Minimum) / this.MovableRange, 0.0);
          this._rightButton.Width = Math.Max(this._movableWidth * (this.Maximum - this.UpperValue) / this.MovableRange, 0.0);
        }
        if (this.IsValidDouble(this._rightButton.Width) && this.IsValidDouble(this._leftButton.Width))
          this._centerThumb.Width = Math.Max(this.ActualWidth - (this._leftButton.Width + this._rightButton.Width + this._rightThumb.ActualWidth + this._leftThumb.ActualWidth), 0.0);
        else
          this._centerThumb.Width = Math.Max(this.ActualWidth - (this._rightThumb.ActualWidth + this._leftThumb.ActualWidth), 0.0);
      }
      else if (this.Orientation == Orientation.Vertical)
      {
        this._movableWidth = Math.Max(this.ActualHeight - this._rightThumb.ActualHeight - this._leftThumb.ActualHeight - this.MinRangeWidth, 1.0);
        if (this.MovableRange <= 0.0)
        {
          this._leftButton.Height = double.NaN;
          this._rightButton.Height = double.NaN;
        }
        else
        {
          this._leftButton.Height = Math.Max(this._movableWidth * (this.LowerValue - this.Minimum) / this.MovableRange, 0.0);
          this._rightButton.Height = Math.Max(this._movableWidth * (this.Maximum - this.UpperValue) / this.MovableRange, 0.0);
        }
        if (this.IsValidDouble(this._rightButton.Height) && this.IsValidDouble(this._leftButton.Height))
          this._centerThumb.Height = Math.Max(this.ActualHeight - (this._leftButton.Height + this._rightButton.Height + this._rightThumb.ActualHeight + this._leftThumb.ActualHeight), 0.0);
        else
          this._centerThumb.Height = Math.Max(this.ActualHeight - (this._rightThumb.ActualHeight + this._leftThumb.ActualHeight), 0.0);
      }
      this._density = this._movableWidth / this.MovableRange;
    }

    private void ReCalculateRangeSelected(
      bool reCalculateLowerValue,
      bool reCalculateUpperValue,
      RangeSlider.Direction direction)
    {
      this._internalUpdate = true;
      if (direction == RangeSlider.Direction.Increase)
      {
        if (reCalculateUpperValue)
        {
          this._oldUpper = this.UpperValue;
          double num1 = this.Orientation == Orientation.Horizontal ? this._rightButton.Width : this._rightButton.Height;
          if (this.IsValidDouble(num1))
          {
            double num2 = object.Equals((object) num1, (object) 0.0) ? this.Maximum : Math.Min(this.Maximum, this.Maximum - this.MovableRange * num1 / this._movableWidth);
            this.UpperValue = this._isMoved ? num2 : (this._roundToPrecision ? Math.Round(num2, this._precision) : num2);
          }
        }
        if (reCalculateLowerValue)
        {
          this._oldLower = this.LowerValue;
          double num3 = this.Orientation == Orientation.Horizontal ? this._leftButton.Width : this._leftButton.Height;
          if (this.IsValidDouble(num3))
          {
            double num4 = object.Equals((object) num3, (object) 0.0) ? this.Minimum : Math.Max(this.Minimum, this.Minimum + this.MovableRange * num3 / this._movableWidth);
            this.LowerValue = this._isMoved ? num4 : (this._roundToPrecision ? Math.Round(num4, this._precision) : num4);
          }
        }
      }
      else
      {
        if (reCalculateLowerValue)
        {
          this._oldLower = this.LowerValue;
          double num5 = this.Orientation == Orientation.Horizontal ? this._leftButton.Width : this._leftButton.Height;
          if (this.IsValidDouble(num5))
          {
            double num6 = object.Equals((object) num5, (object) 0.0) ? this.Minimum : Math.Max(this.Minimum, this.Minimum + this.MovableRange * num5 / this._movableWidth);
            this.LowerValue = this._isMoved ? num6 : (this._roundToPrecision ? Math.Round(num6, this._precision) : num6);
          }
        }
        if (reCalculateUpperValue)
        {
          this._oldUpper = this.UpperValue;
          double num7 = this.Orientation == Orientation.Horizontal ? this._rightButton.Width : this._rightButton.Height;
          if (this.IsValidDouble(num7))
          {
            double num8 = object.Equals((object) num7, (object) 0.0) ? this.Maximum : Math.Min(this.Maximum, this.Maximum - this.MovableRange * num7 / this._movableWidth);
            this.UpperValue = this._isMoved ? num8 : (this._roundToPrecision ? Math.Round(num8, this._precision) : num8);
          }
        }
      }
      this._roundToPrecision = false;
      this._internalUpdate = false;
      RangeSlider.RaiseValueChangedEvents((DependencyObject) this, reCalculateLowerValue, reCalculateUpperValue);
    }

    private void ReCalculateRangeSelected(
      bool reCalculateLowerValue,
      bool reCalculateUpperValue,
      double value,
      RangeSlider.Direction direction)
    {
      this._internalUpdate = true;
      string str = this.TickFrequency.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      if (reCalculateLowerValue)
      {
        this._oldLower = this.LowerValue;
        double num = 0.0;
        if (this.IsSnapToTickEnabled)
          num = direction == RangeSlider.Direction.Increase ? Math.Min(this.UpperValue - this.MinRange, value) : Math.Max(this.Minimum, value);
        if (!str.ToLower().Contains("e+") && str.Contains("."))
        {
          string[] strArray = str.Split('.');
          this.LowerValue = Math.Round(num, strArray[1].Length, MidpointRounding.AwayFromZero);
        }
        else
          this.LowerValue = num;
      }
      if (reCalculateUpperValue)
      {
        this._oldUpper = this.UpperValue;
        double num = 0.0;
        if (this.IsSnapToTickEnabled)
          num = direction == RangeSlider.Direction.Increase ? Math.Min(value, this.Maximum) : Math.Max(this.LowerValue + this.MinRange, value);
        if (!str.ToLower().Contains("e+") && str.Contains("."))
        {
          string[] strArray = str.Split('.');
          this.UpperValue = Math.Round(num, strArray[1].Length, MidpointRounding.AwayFromZero);
        }
        else
          this.UpperValue = num;
      }
      this._internalUpdate = false;
      RangeSlider.RaiseValueChangedEvents((DependencyObject) this, reCalculateLowerValue, reCalculateUpperValue);
    }

    private void ReCalculateRangeSelected(
      double newLower,
      double newUpper,
      RangeSlider.Direction direction)
    {
      this._internalUpdate = true;
      this._oldLower = this.LowerValue;
      this._oldUpper = this.UpperValue;
      if (this.IsSnapToTickEnabled)
      {
        double num1;
        double num2;
        if (direction == RangeSlider.Direction.Increase)
        {
          num1 = Math.Min(newLower, this.Maximum - (this.UpperValue - this.LowerValue));
          num2 = Math.Min(newUpper, this.Maximum);
        }
        else
        {
          num1 = Math.Max(newLower, this.Minimum);
          num2 = Math.Max(this.Minimum + (this.UpperValue - this.LowerValue), newUpper);
        }
        string str = this.TickFrequency.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        if (!str.ToLower().Contains("e+") && str.Contains("."))
        {
          string[] strArray = str.Split('.');
          if (direction == RangeSlider.Direction.Decrease)
          {
            this.LowerValue = Math.Round(num1, strArray[1].Length, MidpointRounding.AwayFromZero);
            this.UpperValue = Math.Round(num2, strArray[1].Length, MidpointRounding.AwayFromZero);
          }
          else
          {
            this.UpperValue = Math.Round(num2, strArray[1].Length, MidpointRounding.AwayFromZero);
            this.LowerValue = Math.Round(num1, strArray[1].Length, MidpointRounding.AwayFromZero);
          }
        }
        else if (direction == RangeSlider.Direction.Decrease)
        {
          this.LowerValue = num1;
          this.UpperValue = num2;
        }
        else
        {
          this.UpperValue = num2;
          this.LowerValue = num1;
        }
      }
      this._internalUpdate = false;
      RangeSlider.RaiseValueChangedEvents((DependencyObject) this);
    }

    private void OnRangeParameterChanged(RangeParameterChangedEventArgs e, RoutedEvent Event)
    {
      e.RoutedEvent = Event;
      this.RaiseEvent((RoutedEventArgs) e);
    }

    public void MoveSelection(bool isLeft)
    {
      double num = this.SmallChange * (this.UpperValue - this.LowerValue) * this._movableWidth / this.MovableRange;
      RangeSlider.MoveThumb((FrameworkElement) this._leftButton, (FrameworkElement) this._rightButton, isLeft ? -num : num, this.Orientation, out this._direction);
      this.ReCalculateRangeSelected(true, true, this._direction);
    }

    public void ResetSelection(bool isStart)
    {
      double num = this.Maximum - this.Minimum;
      RangeSlider.MoveThumb((FrameworkElement) this._leftButton, (FrameworkElement) this._rightButton, isStart ? -num : num, this.Orientation, out this._direction);
      this.ReCalculateRangeSelected(true, true, this._direction);
    }

    private void OnRangeSelectionChanged(RangeSelectionChangedEventArgs e)
    {
      e.RoutedEvent = RangeSlider.RangeSelectionChangedEvent;
      this.RaiseEvent((RoutedEventArgs) e);
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this._container = this.EnforceInstance<StackPanel>("PART_Container");
      this._visualElementsContainer = this.EnforceInstance<StackPanel>("PART_RangeSliderContainer");
      this._centerThumb = this.EnforceInstance<Thumb>("PART_MiddleThumb");
      this._leftButton = this.EnforceInstance<RepeatButton>("PART_LeftEdge");
      this._rightButton = this.EnforceInstance<RepeatButton>("PART_RightEdge");
      this._leftThumb = this.EnforceInstance<Thumb>("PART_LeftThumb");
      this._rightThumb = this.EnforceInstance<Thumb>("PART_RightThumb");
      this.InitializeVisualElementsContainer();
      this.ReCalculateSize();
    }

    private T EnforceInstance<T>(string partName) where T : FrameworkElement, new()
    {
      if (!(this.GetTemplateChild(partName) is T obj))
        obj = new T();
      return obj;
    }

    private void InitializeVisualElementsContainer()
    {
      this._leftThumb.DragCompleted += new DragCompletedEventHandler(this.LeftThumbDragComplete);
      this._rightThumb.DragCompleted += new DragCompletedEventHandler(this.RightThumbDragComplete);
      this._leftThumb.DragStarted += new DragStartedEventHandler(this.LeftThumbDragStart);
      this._rightThumb.DragStarted += new DragStartedEventHandler(this.RightThumbDragStart);
      this._centerThumb.DragStarted += new DragStartedEventHandler(this.CenterThumbDragStarted);
      this._centerThumb.DragCompleted += new DragCompletedEventHandler(this.CenterThumbDragCompleted);
      this._centerThumb.DragDelta += new DragDeltaEventHandler(this.CenterThumbDragDelta);
      this._leftThumb.DragDelta += new DragDeltaEventHandler(this.LeftThumbDragDelta);
      this._rightThumb.DragDelta += new DragDeltaEventHandler(this.RightThumbDragDelta);
      this._visualElementsContainer.PreviewMouseDown += new MouseButtonEventHandler(this.VisualElementsContainerPreviewMouseDown);
      this._visualElementsContainer.PreviewMouseUp += new MouseButtonEventHandler(this.VisualElementsContainerPreviewMouseUp);
      this._visualElementsContainer.MouseLeave += new MouseEventHandler(this.VisualElementsContainerMouseLeave);
      this._visualElementsContainer.MouseDown += new MouseButtonEventHandler(this.VisualElementsContainerMouseDown);
    }

    private void VisualElementsContainerPreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
      Point position = Mouse.GetPosition((IInputElement) this._visualElementsContainer);
      if (this.Orientation == Orientation.Horizontal)
      {
        if (position.X < this._leftButton.ActualWidth)
          this.LeftButtonMouseDown();
        else if (position.X > this.ActualWidth - this._rightButton.ActualWidth)
        {
          this.RightButtonMouseDown();
        }
        else
        {
          if (position.X <= this._leftButton.ActualWidth + this._leftThumb.ActualWidth || position.X >= this.ActualWidth - (this._rightButton.ActualWidth + this._rightThumb.ActualWidth))
            return;
          this.CentralThumbMouseDown();
        }
      }
      else if (position.Y > this.ActualHeight - this._leftButton.ActualHeight)
        this.LeftButtonMouseDown();
      else if (position.Y < this._rightButton.ActualHeight)
      {
        this.RightButtonMouseDown();
      }
      else
      {
        if (position.Y <= this._rightButton.ActualHeight + this._rightButton.ActualHeight || position.Y >= this.ActualHeight - (this._leftButton.ActualHeight + this._leftThumb.ActualHeight))
          return;
        this.CentralThumbMouseDown();
      }
    }

    private void VisualElementsContainerMouseDown(object sender, MouseButtonEventArgs e)
    {
      if (e.MiddleButton != MouseButtonState.Pressed)
        return;
      this.MoveWholeRange = !this.MoveWholeRange;
    }

    private void VisualElementsContainerMouseLeave(object sender, MouseEventArgs e)
    {
      this._tickCount = 0U;
      this._timer.Stop();
    }

    private void VisualElementsContainerPreviewMouseUp(object sender, MouseButtonEventArgs e)
    {
      this._tickCount = 0U;
      this._timer.Stop();
      this._centerThumbBlocked = false;
    }

    private void LeftButtonMouseDown()
    {
      if (Mouse.LeftButton != MouseButtonState.Pressed)
        return;
      Point position = Mouse.GetPosition((IInputElement) this._visualElementsContainer);
      double num = this.Orientation == Orientation.Horizontal ? this._leftButton.ActualWidth - position.X + this._leftThumb.ActualWidth / 2.0 : -(this._leftButton.ActualHeight - (this.ActualHeight - (position.Y + this._leftThumb.ActualHeight / 2.0)));
      if (!this.IsSnapToTickEnabled)
      {
        if (this.IsMoveToPointEnabled && !this.MoveWholeRange)
        {
          RangeSlider.MoveThumb((FrameworkElement) this._leftButton, (FrameworkElement) this._centerThumb, -num, this.Orientation, out this._direction);
          this.ReCalculateRangeSelected(true, false, this._direction);
        }
        else if (this.IsMoveToPointEnabled && this.MoveWholeRange)
        {
          RangeSlider.MoveThumb((FrameworkElement) this._leftButton, (FrameworkElement) this._rightButton, -num, this.Orientation, out this._direction);
          this.ReCalculateRangeSelected(true, true, this._direction);
        }
      }
      else if (this.IsMoveToPointEnabled && !this.MoveWholeRange)
        this.JumpToNextTick(RangeSlider.Direction.Decrease, RangeSlider.ButtonType.BottomLeft, -num, this.LowerValue, true);
      else if (this.IsMoveToPointEnabled && this.MoveWholeRange)
        this.JumpToNextTick(RangeSlider.Direction.Decrease, RangeSlider.ButtonType.Both, -num, this.LowerValue, true);
      if (this.IsMoveToPointEnabled)
        return;
      this._position = Mouse.GetPosition((IInputElement) this._visualElementsContainer);
      this._bType = this.MoveWholeRange ? RangeSlider.ButtonType.Both : RangeSlider.ButtonType.BottomLeft;
      this._currentpoint = this.Orientation == Orientation.Horizontal ? this._position.X : this._position.Y;
      this._currenValue = this.LowerValue;
      this._isInsideRange = false;
      this._direction = RangeSlider.Direction.Decrease;
      this._timer.Start();
    }

    private void RightButtonMouseDown()
    {
      if (Mouse.LeftButton != MouseButtonState.Pressed)
        return;
      Point position = Mouse.GetPosition((IInputElement) this._visualElementsContainer);
      double num = this.Orientation == Orientation.Horizontal ? this._rightButton.ActualWidth - (this.ActualWidth - (position.X + this._rightThumb.ActualWidth / 2.0)) : -(this._rightButton.ActualHeight - (position.Y - this._rightThumb.ActualHeight / 2.0));
      if (!this.IsSnapToTickEnabled)
      {
        if (this.IsMoveToPointEnabled && !this.MoveWholeRange)
        {
          RangeSlider.MoveThumb((FrameworkElement) this._centerThumb, (FrameworkElement) this._rightButton, num, this.Orientation, out this._direction);
          this.ReCalculateRangeSelected(false, true, this._direction);
        }
        else if (this.IsMoveToPointEnabled && this.MoveWholeRange)
        {
          RangeSlider.MoveThumb((FrameworkElement) this._leftButton, (FrameworkElement) this._rightButton, num, this.Orientation, out this._direction);
          this.ReCalculateRangeSelected(true, true, this._direction);
        }
      }
      else if (this.IsMoveToPointEnabled && !this.MoveWholeRange)
        this.JumpToNextTick(RangeSlider.Direction.Increase, RangeSlider.ButtonType.TopRight, num, this.UpperValue, true);
      else if (this.IsMoveToPointEnabled && this.MoveWholeRange)
        this.JumpToNextTick(RangeSlider.Direction.Increase, RangeSlider.ButtonType.Both, num, this.UpperValue, true);
      if (this.IsMoveToPointEnabled)
        return;
      this._position = Mouse.GetPosition((IInputElement) this._visualElementsContainer);
      this._bType = this.MoveWholeRange ? RangeSlider.ButtonType.Both : RangeSlider.ButtonType.TopRight;
      this._currentpoint = this.Orientation == Orientation.Horizontal ? this._position.X : this._position.Y;
      this._currenValue = this.UpperValue;
      this._direction = RangeSlider.Direction.Increase;
      this._isInsideRange = false;
      this._timer.Start();
    }

    private void CentralThumbMouseDown()
    {
      if (!this.ExtendedMode)
        return;
      if (Mouse.LeftButton == MouseButtonState.Pressed && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
      {
        this._centerThumbBlocked = true;
        Point position = Mouse.GetPosition((IInputElement) this._visualElementsContainer);
        double num = this.Orientation == Orientation.Horizontal ? position.X + this._leftThumb.ActualWidth / 2.0 - (this._leftButton.ActualWidth + this._leftThumb.ActualWidth) : -(this.ActualHeight - (position.Y + this._leftThumb.ActualHeight / 2.0 + this._leftButton.ActualHeight));
        if (!this.IsSnapToTickEnabled)
        {
          if (this.IsMoveToPointEnabled && !this.MoveWholeRange)
          {
            RangeSlider.MoveThumb((FrameworkElement) this._leftButton, (FrameworkElement) this._centerThumb, num, this.Orientation, out this._direction);
            this.ReCalculateRangeSelected(true, false, this._direction);
          }
          else if (this.IsMoveToPointEnabled && this.MoveWholeRange)
          {
            RangeSlider.MoveThumb((FrameworkElement) this._leftButton, (FrameworkElement) this._rightButton, num, this.Orientation, out this._direction);
            this.ReCalculateRangeSelected(true, true, this._direction);
          }
        }
        else if (this.IsMoveToPointEnabled && !this.MoveWholeRange)
          this.JumpToNextTick(RangeSlider.Direction.Increase, RangeSlider.ButtonType.BottomLeft, num, this.LowerValue, true);
        else if (this.IsMoveToPointEnabled && this.MoveWholeRange)
          this.JumpToNextTick(RangeSlider.Direction.Increase, RangeSlider.ButtonType.Both, num, this.LowerValue, true);
        if (this.IsMoveToPointEnabled)
          return;
        this._position = Mouse.GetPosition((IInputElement) this._visualElementsContainer);
        this._bType = this.MoveWholeRange ? RangeSlider.ButtonType.Both : RangeSlider.ButtonType.BottomLeft;
        this._currentpoint = this.Orientation == Orientation.Horizontal ? this._position.X : this._position.Y;
        this._currenValue = this.LowerValue;
        this._direction = RangeSlider.Direction.Increase;
        this._isInsideRange = true;
        this._timer.Start();
      }
      else
      {
        if (Mouse.RightButton != MouseButtonState.Pressed || !Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl))
          return;
        this._centerThumbBlocked = true;
        Point position = Mouse.GetPosition((IInputElement) this._visualElementsContainer);
        double num = this.Orientation == Orientation.Horizontal ? this.ActualWidth - (position.X + this._rightThumb.ActualWidth / 2.0 + this._rightButton.ActualWidth) : -(position.Y + this._rightThumb.ActualHeight / 2.0 - (this._rightButton.ActualHeight + this._rightThumb.ActualHeight));
        if (!this.IsSnapToTickEnabled)
        {
          if (this.IsMoveToPointEnabled && !this.MoveWholeRange)
          {
            RangeSlider.MoveThumb((FrameworkElement) this._centerThumb, (FrameworkElement) this._rightButton, -num, this.Orientation, out this._direction);
            this.ReCalculateRangeSelected(false, true, this._direction);
          }
          else if (this.IsMoveToPointEnabled && this.MoveWholeRange)
          {
            RangeSlider.MoveThumb((FrameworkElement) this._leftButton, (FrameworkElement) this._rightButton, -num, this.Orientation, out this._direction);
            this.ReCalculateRangeSelected(true, true, this._direction);
          }
        }
        else if (this.IsMoveToPointEnabled && !this.MoveWholeRange)
          this.JumpToNextTick(RangeSlider.Direction.Decrease, RangeSlider.ButtonType.TopRight, -num, this.UpperValue, true);
        else if (this.IsMoveToPointEnabled && this.MoveWholeRange)
          this.JumpToNextTick(RangeSlider.Direction.Decrease, RangeSlider.ButtonType.Both, -num, this.UpperValue, true);
        if (this.IsMoveToPointEnabled)
          return;
        this._position = Mouse.GetPosition((IInputElement) this._visualElementsContainer);
        this._bType = this.MoveWholeRange ? RangeSlider.ButtonType.Both : RangeSlider.ButtonType.TopRight;
        this._currentpoint = this.Orientation == Orientation.Horizontal ? this._position.X : this._position.Y;
        this._currenValue = this.UpperValue;
        this._direction = RangeSlider.Direction.Decrease;
        this._isInsideRange = true;
        this._timer.Start();
      }
    }

    private void LeftThumbDragStart(object sender, DragStartedEventArgs e)
    {
      this._isMoved = true;
      if (this.AutoToolTipPlacement != AutoToolTipPlacement.None)
      {
        if (this._autoToolTip == null)
        {
          this._autoToolTip = new System.Windows.Controls.ToolTip();
          this._autoToolTip.Placement = PlacementMode.Custom;
          this._autoToolTip.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(this.PopupPlacementCallback);
        }
        this._autoToolTip.Content = (object) this.GetLowerToolTipNumber();
        this._autoToolTip.PlacementTarget = (UIElement) this._leftThumb;
        this._autoToolTip.IsOpen = true;
      }
      this._basePoint = Mouse.GetPosition((IInputElement) this._container);
      e.RoutedEvent = RangeSlider.LowerThumbDragStartedEvent;
      this.RaiseEvent((RoutedEventArgs) e);
    }

    private void LeftThumbDragDelta(object sender, DragDeltaEventArgs e)
    {
      double num = this.Orientation == Orientation.Horizontal ? e.HorizontalChange : e.VerticalChange;
      if (!this.IsSnapToTickEnabled)
      {
        RangeSlider.MoveThumb((FrameworkElement) this._leftButton, (FrameworkElement) this._centerThumb, num, this.Orientation, out this._direction);
        this.ReCalculateRangeSelected(true, false, this._direction);
      }
      else
      {
        Point position = Mouse.GetPosition((IInputElement) this._container);
        if (this.Orientation == Orientation.Horizontal)
        {
          if (position.X >= 0.0 && position.X < this._container.ActualWidth - (this._rightButton.ActualWidth + this._rightThumb.ActualWidth + this._centerThumb.MinWidth))
            this.JumpToNextTick(position.X > this._basePoint.X ? RangeSlider.Direction.Increase : RangeSlider.Direction.Decrease, RangeSlider.ButtonType.BottomLeft, num, this.LowerValue, false);
        }
        else if (position.Y <= this._container.ActualHeight && position.Y > this._rightButton.ActualHeight + this._rightThumb.ActualHeight + this._centerThumb.MinHeight)
          this.JumpToNextTick(position.Y < this._basePoint.Y ? RangeSlider.Direction.Increase : RangeSlider.Direction.Decrease, RangeSlider.ButtonType.BottomLeft, -num, this.LowerValue, false);
      }
      this._basePoint = Mouse.GetPosition((IInputElement) this._container);
      if (this.AutoToolTipPlacement != AutoToolTipPlacement.None)
      {
        this._autoToolTip.Content = (object) this.GetLowerToolTipNumber();
        this.RelocateAutoToolTip();
      }
      e.RoutedEvent = RangeSlider.LowerThumbDragDeltaEvent;
      this.RaiseEvent((RoutedEventArgs) e);
    }

    private void LeftThumbDragComplete(object sender, DragCompletedEventArgs e)
    {
      if (this._autoToolTip != null)
      {
        this._autoToolTip.IsOpen = false;
        this._autoToolTip = (System.Windows.Controls.ToolTip) null;
      }
      e.RoutedEvent = RangeSlider.LowerThumbDragCompletedEvent;
      this.RaiseEvent((RoutedEventArgs) e);
    }

    private void RightThumbDragStart(object sender, DragStartedEventArgs e)
    {
      this._isMoved = true;
      if (this.AutoToolTipPlacement != AutoToolTipPlacement.None)
      {
        if (this._autoToolTip == null)
        {
          this._autoToolTip = new System.Windows.Controls.ToolTip();
          this._autoToolTip.Placement = PlacementMode.Custom;
          this._autoToolTip.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(this.PopupPlacementCallback);
        }
        this._autoToolTip.Content = (object) this.GetUpperToolTipNumber();
        this._autoToolTip.PlacementTarget = (UIElement) this._rightThumb;
        this._autoToolTip.IsOpen = true;
      }
      this._basePoint = Mouse.GetPosition((IInputElement) this._container);
      e.RoutedEvent = RangeSlider.UpperThumbDragStartedEvent;
      this.RaiseEvent((RoutedEventArgs) e);
    }

    private void RightThumbDragDelta(object sender, DragDeltaEventArgs e)
    {
      double num = this.Orientation == Orientation.Horizontal ? e.HorizontalChange : e.VerticalChange;
      if (!this.IsSnapToTickEnabled)
      {
        RangeSlider.MoveThumb((FrameworkElement) this._centerThumb, (FrameworkElement) this._rightButton, num, this.Orientation, out this._direction);
        this.ReCalculateRangeSelected(false, true, this._direction);
      }
      else
      {
        Point position = Mouse.GetPosition((IInputElement) this._container);
        if (this.Orientation == Orientation.Horizontal)
        {
          if (position.X < this._container.ActualWidth && position.X > this._leftButton.ActualWidth + this._leftThumb.ActualWidth + this._centerThumb.MinWidth)
            this.JumpToNextTick(position.X > this._basePoint.X ? RangeSlider.Direction.Increase : RangeSlider.Direction.Decrease, RangeSlider.ButtonType.TopRight, num, this.UpperValue, false);
        }
        else if (position.Y >= 0.0 && position.Y < this._container.ActualHeight - (this._leftButton.ActualHeight + this._leftThumb.ActualHeight + this._centerThumb.MinHeight))
          this.JumpToNextTick(position.Y < this._basePoint.Y ? RangeSlider.Direction.Increase : RangeSlider.Direction.Decrease, RangeSlider.ButtonType.TopRight, -num, this.UpperValue, false);
        this._basePoint = Mouse.GetPosition((IInputElement) this._container);
      }
      if (this.AutoToolTipPlacement != AutoToolTipPlacement.None)
      {
        this._autoToolTip.Content = (object) this.GetUpperToolTipNumber();
        this.RelocateAutoToolTip();
      }
      e.RoutedEvent = RangeSlider.UpperThumbDragDeltaEvent;
      this.RaiseEvent((RoutedEventArgs) e);
    }

    private void RightThumbDragComplete(object sender, DragCompletedEventArgs e)
    {
      if (this._autoToolTip != null)
      {
        this._autoToolTip.IsOpen = false;
        this._autoToolTip = (System.Windows.Controls.ToolTip) null;
      }
      e.RoutedEvent = RangeSlider.UpperThumbDragCompletedEvent;
      this.RaiseEvent((RoutedEventArgs) e);
    }

    private void CenterThumbDragStarted(object sender, DragStartedEventArgs e)
    {
      this._isMoved = true;
      if (this.AutoToolTipPlacement != AutoToolTipPlacement.None)
      {
        if (this._autoToolTip == null)
          this._autoToolTip = new System.Windows.Controls.ToolTip()
          {
            Placement = PlacementMode.Custom,
            CustomPopupPlacementCallback = new CustomPopupPlacementCallback(this.PopupPlacementCallback)
          };
        this._autoToolTip.Content = (object) (this.GetLowerToolTipNumber() + " ; " + this.GetUpperToolTipNumber());
        this._autoToolTip.PlacementTarget = (UIElement) this._centerThumb;
        this._autoToolTip.IsOpen = true;
      }
      this._basePoint = Mouse.GetPosition((IInputElement) this._container);
      e.RoutedEvent = RangeSlider.CentralThumbDragStartedEvent;
      this.RaiseEvent((RoutedEventArgs) e);
    }

    private void CenterThumbDragDelta(object sender, DragDeltaEventArgs e)
    {
      if (!this._centerThumbBlocked)
      {
        double num = this.Orientation == Orientation.Horizontal ? e.HorizontalChange : e.VerticalChange;
        if (!this.IsSnapToTickEnabled)
        {
          RangeSlider.MoveThumb((FrameworkElement) this._leftButton, (FrameworkElement) this._rightButton, num, this.Orientation, out this._direction);
          this.ReCalculateRangeSelected(true, true, this._direction);
        }
        else
        {
          Point position = Mouse.GetPosition((IInputElement) this._container);
          if (this.Orientation == Orientation.Horizontal)
          {
            if (position.X >= 0.0 && position.X < this._container.ActualWidth)
            {
              RangeSlider.Direction direction = position.X > this._basePoint.X ? RangeSlider.Direction.Increase : RangeSlider.Direction.Decrease;
              this.JumpToNextTick(direction, RangeSlider.ButtonType.Both, num, direction == RangeSlider.Direction.Increase ? this.UpperValue : this.LowerValue, false);
            }
          }
          else if (position.Y >= 0.0 && position.Y < this._container.ActualHeight)
          {
            RangeSlider.Direction direction = position.Y < this._basePoint.Y ? RangeSlider.Direction.Increase : RangeSlider.Direction.Decrease;
            this.JumpToNextTick(direction, RangeSlider.ButtonType.Both, -num, direction == RangeSlider.Direction.Increase ? this.UpperValue : this.LowerValue, false);
          }
        }
        this._basePoint = Mouse.GetPosition((IInputElement) this._container);
        if (this.AutoToolTipPlacement != AutoToolTipPlacement.None)
        {
          this._autoToolTip.Content = (object) (this.GetLowerToolTipNumber() + " ; " + this.GetUpperToolTipNumber());
          this.RelocateAutoToolTip();
        }
      }
      e.RoutedEvent = RangeSlider.CentralThumbDragDeltaEvent;
      this.RaiseEvent((RoutedEventArgs) e);
    }

    private void CenterThumbDragCompleted(object sender, DragCompletedEventArgs e)
    {
      if (this._autoToolTip != null)
      {
        this._autoToolTip.IsOpen = false;
        this._autoToolTip = (System.Windows.Controls.ToolTip) null;
      }
      e.RoutedEvent = RangeSlider.CentralThumbDragCompletedEvent;
      this.RaiseEvent((RoutedEventArgs) e);
    }

    private static double GetChangeKeepPositive(double width, double increment)
    {
      return Math.Max(width + increment, 0.0) - width;
    }

    private double UpdateEndPoint(RangeSlider.ButtonType type, RangeSlider.Direction dir)
    {
      double num = 0.0;
      switch (dir)
      {
        case RangeSlider.Direction.Increase:
          if (type == RangeSlider.ButtonType.BottomLeft || type == RangeSlider.ButtonType.Both && this._isInsideRange)
          {
            num = this.Orientation == Orientation.Horizontal ? this._leftButton.ActualWidth + this._leftThumb.ActualWidth : this.ActualHeight - (this._leftButton.ActualHeight + this._leftThumb.ActualHeight);
            break;
          }
          if (type == RangeSlider.ButtonType.TopRight || type == RangeSlider.ButtonType.Both && !this._isInsideRange)
          {
            num = this.Orientation == Orientation.Horizontal ? this.ActualWidth - this._rightButton.ActualWidth : this._rightButton.ActualHeight;
            break;
          }
          break;
        case RangeSlider.Direction.Decrease:
          if (type == RangeSlider.ButtonType.BottomLeft || type == RangeSlider.ButtonType.Both && !this._isInsideRange)
          {
            num = this.Orientation == Orientation.Horizontal ? this._leftButton.ActualWidth : this.ActualHeight - this._leftButton.ActualHeight;
            break;
          }
          if (type == RangeSlider.ButtonType.TopRight || type == RangeSlider.ButtonType.Both && this._isInsideRange)
          {
            num = this.Orientation == Orientation.Horizontal ? this.ActualWidth - this._rightButton.ActualWidth - this._rightThumb.ActualWidth : this._rightButton.ActualHeight + this._rightThumb.ActualHeight;
            break;
          }
          break;
      }
      return num;
    }

    private bool GetResult(double currentPoint, double endPoint, RangeSlider.Direction direction)
    {
      if (direction == RangeSlider.Direction.Increase)
      {
        if (this.Orientation == Orientation.Horizontal && currentPoint > endPoint)
          return true;
        return this.Orientation == Orientation.Vertical && currentPoint < endPoint;
      }
      if (this.Orientation == Orientation.Horizontal && currentPoint < endPoint)
        return true;
      return this.Orientation == Orientation.Vertical && currentPoint > endPoint;
    }

    private void MoveToNextValue(object sender, EventArgs e)
    {
      this._position = Mouse.GetPosition((IInputElement) this._visualElementsContainer);
      this._currentpoint = this.Orientation == Orientation.Horizontal ? this._position.X : this._position.Y;
      bool result = this.GetResult(this._currentpoint, this.UpdateEndPoint(this._bType, this._direction), this._direction);
      if (!this.IsSnapToTickEnabled)
      {
        double num1 = this.SmallChange;
        if (this._tickCount > 5U)
          num1 = this.LargeChange;
        this._roundToPrecision = true;
        if (!num1.ToString((IFormatProvider) CultureInfo.InvariantCulture).ToLower().Contains(nameof (e)) && num1.ToString((IFormatProvider) CultureInfo.InvariantCulture).Contains("."))
          this._precision = num1.ToString((IFormatProvider) CultureInfo.InvariantCulture).Split('.')[1].Length;
        else
          this._precision = 0;
        double num2 = this.Orientation == Orientation.Horizontal ? num1 : -num1;
        double num3 = this._direction == RangeSlider.Direction.Increase ? num2 : -num2;
        if (result)
        {
          switch (this._bType)
          {
            case RangeSlider.ButtonType.BottomLeft:
              RangeSlider.MoveThumb((FrameworkElement) this._leftButton, (FrameworkElement) this._centerThumb, num3 * this._density, this.Orientation, out this._direction);
              this.ReCalculateRangeSelected(true, false, this._direction);
              break;
            case RangeSlider.ButtonType.TopRight:
              RangeSlider.MoveThumb((FrameworkElement) this._centerThumb, (FrameworkElement) this._rightButton, num3 * this._density, this.Orientation, out this._direction);
              this.ReCalculateRangeSelected(false, true, this._direction);
              break;
            case RangeSlider.ButtonType.Both:
              RangeSlider.MoveThumb((FrameworkElement) this._leftButton, (FrameworkElement) this._rightButton, num3 * this._density, this.Orientation, out this._direction);
              this.ReCalculateRangeSelected(true, true, this._direction);
              break;
          }
        }
      }
      else
      {
        double nextTick = this.CalculateNextTick(this._direction, this._currenValue, 0.0, true);
        double num4 = nextTick;
        double num5 = this.Orientation == Orientation.Horizontal ? nextTick : -nextTick;
        if (this._direction == RangeSlider.Direction.Increase)
        {
          if (result)
          {
            switch (this._bType)
            {
              case RangeSlider.ButtonType.BottomLeft:
                RangeSlider.MoveThumb((FrameworkElement) this._leftButton, (FrameworkElement) this._centerThumb, num5 * this._density, this.Orientation);
                this.ReCalculateRangeSelected(true, false, this.LowerValue + num4, this._direction);
                break;
              case RangeSlider.ButtonType.TopRight:
                RangeSlider.MoveThumb((FrameworkElement) this._centerThumb, (FrameworkElement) this._rightButton, num5 * this._density, this.Orientation);
                this.ReCalculateRangeSelected(false, true, this.UpperValue + num4, this._direction);
                break;
              case RangeSlider.ButtonType.Both:
                RangeSlider.MoveThumb((FrameworkElement) this._leftButton, (FrameworkElement) this._rightButton, num5 * this._density, this.Orientation);
                this.ReCalculateRangeSelected(this.LowerValue + num4, this.UpperValue + num4, this._direction);
                break;
            }
          }
        }
        else if (this._direction == RangeSlider.Direction.Decrease && result)
        {
          switch (this._bType)
          {
            case RangeSlider.ButtonType.BottomLeft:
              RangeSlider.MoveThumb((FrameworkElement) this._leftButton, (FrameworkElement) this._centerThumb, -num5 * this._density, this.Orientation);
              this.ReCalculateRangeSelected(true, false, this.LowerValue - num4, this._direction);
              break;
            case RangeSlider.ButtonType.TopRight:
              RangeSlider.MoveThumb((FrameworkElement) this._centerThumb, (FrameworkElement) this._rightButton, -num5 * this._density, this.Orientation);
              this.ReCalculateRangeSelected(false, true, this.UpperValue - num4, this._direction);
              break;
            case RangeSlider.ButtonType.Both:
              RangeSlider.MoveThumb((FrameworkElement) this._leftButton, (FrameworkElement) this._rightButton, -num5 * this._density, this.Orientation);
              this.ReCalculateRangeSelected(this.LowerValue - num4, this.UpperValue - num4, this._direction);
              break;
          }
        }
      }
      ++this._tickCount;
    }

    private void SnapToTickHandle(
      RangeSlider.ButtonType type,
      RangeSlider.Direction direction,
      double difference)
    {
      double num = difference;
      difference = this.Orientation == Orientation.Horizontal ? difference : -difference;
      if (direction == RangeSlider.Direction.Increase)
      {
        switch (type)
        {
          case RangeSlider.ButtonType.BottomLeft:
            if (this.LowerValue >= this.UpperValue - this.MinRange)
              break;
            RangeSlider.MoveThumb((FrameworkElement) this._leftButton, (FrameworkElement) this._centerThumb, difference * this._density, this.Orientation);
            this.ReCalculateRangeSelected(true, false, this.LowerValue + num, direction);
            break;
          case RangeSlider.ButtonType.TopRight:
            if (this.UpperValue >= this.Maximum)
              break;
            RangeSlider.MoveThumb((FrameworkElement) this._centerThumb, (FrameworkElement) this._rightButton, difference * this._density, this.Orientation);
            this.ReCalculateRangeSelected(false, true, this.UpperValue + num, direction);
            break;
          case RangeSlider.ButtonType.Both:
            if (this.UpperValue >= this.Maximum)
              break;
            RangeSlider.MoveThumb((FrameworkElement) this._leftButton, (FrameworkElement) this._rightButton, difference * this._density, this.Orientation);
            this.ReCalculateRangeSelected(this.LowerValue + num, this.UpperValue + num, direction);
            break;
        }
      }
      else
      {
        switch (type)
        {
          case RangeSlider.ButtonType.BottomLeft:
            if (this.LowerValue <= this.Minimum)
              break;
            RangeSlider.MoveThumb((FrameworkElement) this._leftButton, (FrameworkElement) this._centerThumb, -difference * this._density, this.Orientation);
            this.ReCalculateRangeSelected(true, false, this.LowerValue - num, direction);
            break;
          case RangeSlider.ButtonType.TopRight:
            if (this.UpperValue <= this.LowerValue + this.MinRange)
              break;
            RangeSlider.MoveThumb((FrameworkElement) this._centerThumb, (FrameworkElement) this._rightButton, -difference * this._density, this.Orientation);
            this.ReCalculateRangeSelected(false, true, this.UpperValue - num, direction);
            break;
          case RangeSlider.ButtonType.Both:
            if (this.LowerValue <= this.Minimum)
              break;
            RangeSlider.MoveThumb((FrameworkElement) this._leftButton, (FrameworkElement) this._rightButton, -difference * this._density, this.Orientation);
            this.ReCalculateRangeSelected(this.LowerValue - num, this.UpperValue - num, direction);
            break;
        }
      }
    }

    private double CalculateNextTick(
      RangeSlider.Direction direction,
      double checkingValue,
      double distance,
      bool moveDirectlyToNextTick)
    {
      double num1 = checkingValue - this.Minimum;
      if (!this.IsMoveToPointEnabled)
      {
        double val = (num1 + distance) / this.TickFrequency;
        if (!this.IsDoubleCloseToInt(val))
        {
          distance = this.TickFrequency * (double) (int) val;
          if (direction == RangeSlider.Direction.Increase)
            distance += this.TickFrequency;
          distance -= Math.Abs(num1);
          this._currenValue = 0.0;
          return Math.Abs(distance);
        }
      }
      if (moveDirectlyToNextTick)
      {
        distance = this.TickFrequency;
      }
      else
      {
        double num2 = (num1 + distance / this._density) / this.TickFrequency;
        if (direction == RangeSlider.Direction.Increase)
        {
          distance = (num2.ToString((IFormatProvider) CultureInfo.InvariantCulture).ToLower().Contains("e+") ? num2 * this.TickFrequency + this.TickFrequency : (double) (int) num2 * this.TickFrequency + this.TickFrequency) - Math.Abs(num1);
        }
        else
        {
          double num3 = num2.ToString((IFormatProvider) CultureInfo.InvariantCulture).ToLower().Contains("e+") ? num2 * this.TickFrequency : (double) (int) num2 * this.TickFrequency;
          distance = Math.Abs(num1) - num3;
        }
      }
      return Math.Abs(distance);
    }

    private void JumpToNextTick(
      RangeSlider.Direction direction,
      RangeSlider.ButtonType type,
      double distance,
      double checkingValue,
      bool jumpDirectlyToTick)
    {
      double nextTick = this.CalculateNextTick(direction, checkingValue, distance, false);
      Point position = Mouse.GetPosition((IInputElement) this._visualElementsContainer);
      double num1 = this.Orientation == Orientation.Horizontal ? position.X : position.Y;
      double num2 = this.Orientation == Orientation.Horizontal ? this.ActualWidth : this.ActualHeight;
      double num3 = direction == RangeSlider.Direction.Increase ? this.TickFrequency * this._density : -this.TickFrequency * this._density;
      if (jumpDirectlyToTick)
        this.SnapToTickHandle(type, direction, nextTick);
      else if (direction == RangeSlider.Direction.Increase)
      {
        if (!this.IsDoubleCloseToInt(checkingValue / this.TickFrequency))
        {
          if (distance <= nextTick * this._density / 2.0 && distance < num2 - num1 && distance < num1)
            return;
          this.SnapToTickHandle(type, direction, nextTick);
        }
        else
        {
          if (distance <= num3 / 2.0 && distance < num2 - num1 && distance < num1)
            return;
          this.SnapToTickHandle(type, direction, nextTick);
        }
      }
      else if (!this.IsDoubleCloseToInt(checkingValue / this.TickFrequency))
      {
        if (distance > -(nextTick * this._density) / 2.0 && this.UpperValue - this.LowerValue >= nextTick)
          return;
        this.SnapToTickHandle(type, direction, nextTick);
      }
      else
      {
        if (distance >= num3 / 2.0 && this.UpperValue - this.LowerValue >= nextTick)
          return;
        this.SnapToTickHandle(type, direction, nextTick);
      }
    }

    private void RelocateAutoToolTip()
    {
      double horizontalOffset = this._autoToolTip.HorizontalOffset;
      this._autoToolTip.HorizontalOffset = horizontalOffset + 0.001;
      this._autoToolTip.HorizontalOffset = horizontalOffset;
    }

    private bool IsValidDouble(double d) => !double.IsNaN(d) && !double.IsInfinity(d);

    private bool ApproximatelyEquals(double value1, double value2)
    {
      return Math.Abs(value1 - value2) <= 1.53E-06;
    }

    private bool IsDoubleCloseToInt(double val)
    {
      return this.ApproximatelyEquals(Math.Abs(val - Math.Round(val)), 0.0);
    }

    private string GetLowerToolTipNumber() => this.GetToolTipNumber(this.LowerValue);

    private string GetUpperToolTipNumber() => this.GetToolTipNumber(this.UpperValue);

    private string GetToolTipNumber(double value)
    {
      IValueConverter tipTextConverter = this.AutoToolTipTextConverter;
      if (tipTextConverter != null)
      {
        object obj = tipTextConverter.Convert((object) value, typeof (string), (object) null, CultureInfo.InvariantCulture);
        if (obj != null)
          return obj.ToString();
      }
      NumberFormatInfo provider = (NumberFormatInfo) NumberFormatInfo.CurrentInfo.Clone();
      provider.NumberDecimalDigits = this.AutoToolTipPrecision;
      return value.ToString("N", (IFormatProvider) provider);
    }

    private CustomPopupPlacement[] PopupPlacementCallback(
      Size popupSize,
      Size targetSize,
      Point offset)
    {
      switch (this.AutoToolTipPlacement)
      {
        case AutoToolTipPlacement.TopLeft:
          return this.Orientation == Orientation.Horizontal ? new CustomPopupPlacement[1]
          {
            new CustomPopupPlacement(new Point((targetSize.Width - popupSize.Width) * 0.5, -popupSize.Height), PopupPrimaryAxis.Horizontal)
          } : new CustomPopupPlacement[1]
          {
            new CustomPopupPlacement(new Point(-popupSize.Width, (targetSize.Height - popupSize.Height) * 0.5), PopupPrimaryAxis.Vertical)
          };
        case AutoToolTipPlacement.BottomRight:
          return this.Orientation == Orientation.Horizontal ? new CustomPopupPlacement[1]
          {
            new CustomPopupPlacement(new Point((targetSize.Width - popupSize.Width) * 0.5, targetSize.Height), PopupPrimaryAxis.Horizontal)
          } : new CustomPopupPlacement[1]
          {
            new CustomPopupPlacement(new Point(targetSize.Width, (targetSize.Height - popupSize.Height) * 0.5), PopupPrimaryAxis.Vertical)
          };
        default:
          return new CustomPopupPlacement[0];
      }
    }

    private static bool IsValidPrecision(object value) => (int) value >= 0;

    private static bool IsValidMinRange(object value)
    {
      double d = (double) value;
      return d >= 0.0 && !double.IsInfinity(d) && !double.IsNaN(d);
    }

    private static bool IsValidTickFrequency(object value)
    {
      double d = (double) value;
      return d > 0.0 && !double.IsInfinity(d) && !double.IsNaN(d);
    }

    private static object CoerceMinimum(DependencyObject d, object basevalue)
    {
      RangeSlider rangeSlider = (RangeSlider) d;
      return (double) basevalue > rangeSlider.Maximum ? (object) rangeSlider.Maximum : basevalue;
    }

    private static object CoerceMaximum(DependencyObject d, object basevalue)
    {
      RangeSlider rangeSlider = (RangeSlider) d;
      return (double) basevalue < rangeSlider.Minimum ? (object) rangeSlider.Minimum : basevalue;
    }

    private static object CoerceLowerValue(DependencyObject d, object basevalue)
    {
      RangeSlider rangeSlider = (RangeSlider) d;
      double num = (double) basevalue;
      if (num < rangeSlider.Minimum || rangeSlider.UpperValue - rangeSlider.MinRange < rangeSlider.Minimum)
        return (object) rangeSlider.Minimum;
      return num > rangeSlider.UpperValue - rangeSlider.MinRange ? (object) (rangeSlider.UpperValue - rangeSlider.MinRange) : basevalue;
    }

    private static object CoerceUpperValue(DependencyObject d, object basevalue)
    {
      RangeSlider rangeSlider = (RangeSlider) d;
      double num = (double) basevalue;
      if (num > rangeSlider.Maximum || rangeSlider.LowerValue + rangeSlider.MinRange > rangeSlider.Maximum)
        return (object) rangeSlider.Maximum;
      return num < rangeSlider.LowerValue + rangeSlider.MinRange ? (object) (rangeSlider.LowerValue + rangeSlider.MinRange) : basevalue;
    }

    private static object CoerceMinRange(DependencyObject d, object basevalue)
    {
      RangeSlider rangeSlider = (RangeSlider) d;
      double num = (double) basevalue;
      return rangeSlider.LowerValue + num > rangeSlider.Maximum ? (object) (rangeSlider.Maximum - rangeSlider.LowerValue) : basevalue;
    }

    private static object CoerceMinRangeWidth(DependencyObject d, object basevalue)
    {
      RangeSlider rangeSlider = (RangeSlider) d;
      if (rangeSlider._leftThumb == null || rangeSlider._rightThumb == null)
        return basevalue;
      double num = rangeSlider.Orientation != Orientation.Horizontal ? rangeSlider.ActualHeight - rangeSlider._leftThumb.ActualHeight - rangeSlider._rightThumb.ActualHeight : rangeSlider.ActualWidth - rangeSlider._leftThumb.ActualWidth - rangeSlider._rightThumb.ActualWidth;
      return (object) ((double) basevalue > num / 2.0 ? num / 2.0 : (double) basevalue);
    }

    private static void MaxPropertyChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
      dependencyObject.CoerceValue(RangeBase.MaximumProperty);
      dependencyObject.CoerceValue(RangeBase.MinimumProperty);
      dependencyObject.CoerceValue(RangeSlider.UpperValueProperty);
    }

    private static void MinPropertyChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
      dependencyObject.CoerceValue(RangeBase.MinimumProperty);
      dependencyObject.CoerceValue(RangeBase.MaximumProperty);
      dependencyObject.CoerceValue(RangeSlider.LowerValueProperty);
    }

    private static void RangesChanged(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      RangeSlider rangeSlider = (RangeSlider) dependencyObject;
      if (rangeSlider._internalUpdate)
        return;
      dependencyObject.CoerceValue(RangeSlider.UpperValueProperty);
      dependencyObject.CoerceValue(RangeSlider.LowerValueProperty);
      RangeSlider.RaiseValueChangedEvents(dependencyObject);
      rangeSlider._oldLower = rangeSlider.LowerValue;
      rangeSlider._oldUpper = rangeSlider.UpperValue;
      rangeSlider.ReCalculateSize();
    }

    private static void MinRangeChanged(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      double num = (double) e.NewValue;
      if (num < 0.0)
        num = 0.0;
      RangeSlider rangeSlider = (RangeSlider) dependencyObject;
      dependencyObject.CoerceValue(RangeSlider.MinRangeProperty);
      rangeSlider._internalUpdate = true;
      rangeSlider.UpperValue = Math.Max(rangeSlider.UpperValue, rangeSlider.LowerValue + num);
      rangeSlider.UpperValue = Math.Min(rangeSlider.UpperValue, rangeSlider.Maximum);
      rangeSlider._internalUpdate = false;
      rangeSlider.CoerceValue(RangeSlider.UpperValueProperty);
      RangeSlider.RaiseValueChangedEvents(dependencyObject);
      rangeSlider._oldLower = rangeSlider.LowerValue;
      rangeSlider._oldUpper = rangeSlider.UpperValue;
      rangeSlider.ReCalculateSize();
    }

    private static void MinRangeWidthChanged(
      DependencyObject sender,
      DependencyPropertyChangedEventArgs e)
    {
      ((RangeSlider) sender).ReCalculateSize();
    }

    private static void IntervalChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      ((RangeSlider) dependencyObject)._timer.Interval = TimeSpan.FromMilliseconds((double) (int) e.NewValue);
    }

    private static void RaiseValueChangedEvents(
      DependencyObject dependencyObject,
      bool lowerValueReCalculated = true,
      bool upperValueReCalculated = true)
    {
      RangeSlider rangeSlider = (RangeSlider) dependencyObject;
      bool flag1 = object.Equals((object) rangeSlider._oldLower, (object) rangeSlider.LowerValue);
      bool flag2 = object.Equals((object) rangeSlider._oldUpper, (object) rangeSlider.UpperValue);
      if (lowerValueReCalculated | upperValueReCalculated && (!flag1 || !flag2))
        rangeSlider.OnRangeSelectionChanged(new RangeSelectionChangedEventArgs(rangeSlider.LowerValue, rangeSlider.UpperValue, rangeSlider._oldLower, rangeSlider._oldUpper));
      if (lowerValueReCalculated && !flag1)
        rangeSlider.OnRangeParameterChanged(new RangeParameterChangedEventArgs(RangeParameterChangeType.Lower, rangeSlider._oldLower, rangeSlider.LowerValue), RangeSlider.LowerValueChangedEvent);
      if (!upperValueReCalculated || flag2)
        return;
      rangeSlider.OnRangeParameterChanged(new RangeParameterChangedEventArgs(RangeParameterChangeType.Upper, rangeSlider._oldUpper, rangeSlider.UpperValue), RangeSlider.UpperValueChangedEvent);
    }

    private enum ButtonType
    {
      BottomLeft,
      TopRight,
      Both,
    }

    private enum Direction
    {
      Increase,
      Decrease,
    }
  }
}
