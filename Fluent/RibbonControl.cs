// Decompiled with JetBrains decompiler
// Type: Fluent.RibbonControl
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Fluent
{
  public abstract class RibbonControl : 
    Control,
    ICommandSource,
    IQuickAccessItemProvider,
    IRibbonControl,
    IKeyTipedControl
  {
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(nameof (Size), typeof (RibbonControlSize), typeof (RibbonControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) RibbonControlSize.Large, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(RibbonControl.OnSizePropertyChanged)));
    public static readonly DependencyProperty SizeDefinitionProperty = DependencyProperty.Register(nameof (SizeDefinition), typeof (string), typeof (RibbonControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) "Large, Middle, Small", FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(RibbonControl.OnSizeDefinitionPropertyChanged)));
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (object), typeof (RibbonControl), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof (Icon), typeof (object), typeof (RibbonControl), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(RibbonControl.OnIconChanged)));
    private bool currentCanExecute = true;
    public static readonly DependencyProperty CommandParameterProperty = ButtonBase.CommandParameterProperty.AddOwner(typeof (RibbonControl), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty CommandProperty = ButtonBase.CommandProperty.AddOwner(typeof (RibbonControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(RibbonControl.OnCommandChanged)));
    public static readonly DependencyProperty CommandTargetProperty = ButtonBase.CommandTargetProperty.AddOwner(typeof (RibbonControl), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
    [SuppressMessage("Microsoft.Performance", "CA1823")]
    private EventHandler canExecuteChangedHandler;
    public static readonly DependencyProperty CanAddToQuickAccessToolBarProperty = DependencyProperty.Register(nameof (CanAddToQuickAccessToolBar), typeof (bool), typeof (RibbonControl), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(RibbonControl.OnCanAddToQuickAccessToolbarChanged)));
    public static readonly DependencyProperty QuickAccessElementStyleProperty = DependencyProperty.Register(nameof (QuickAccessElementStyle), typeof (Style), typeof (RibbonControl), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));

    private static void OnSizePropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ((RibbonControl) d).OnSizePropertyChanged((RibbonControlSize) e.OldValue, (RibbonControlSize) e.NewValue);
    }

    public RibbonControlSize Size
    {
      get => (RibbonControlSize) this.GetValue(RibbonControl.SizeProperty);
      set => this.SetValue(RibbonControl.SizeProperty, (object) value);
    }

    internal static DependencyProperty AttachSizeDefinition(Type type)
    {
      return RibbonControl.SizeDefinitionProperty.AddOwner(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) "Large, Middle, Small", FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(RibbonControl.OnSizeDefinitionPropertyChanged)));
    }

    internal static void OnSizeDefinitionPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      RibbonGroupBox parentRibbonGroupBox = RibbonControl.FindParentRibbonGroupBox(d);
      UIElement element = (UIElement) d;
      if (parentRibbonGroupBox != null)
        RibbonControl.SetAppropriateSize(element, parentRibbonGroupBox.State);
      else
        RibbonControl.SetAppropriateSize(element, RibbonGroupBoxState.Large);
    }

    [SuppressMessage("Microsoft.Performance", "CA1800")]
    internal static RibbonGroupBox FindParentRibbonGroupBox(DependencyObject o)
    {
      while (!(o is RibbonGroupBox))
      {
        o = VisualTreeHelper.GetParent(o) ?? LogicalTreeHelper.GetParent(o);
        if (o == null)
          break;
      }
      return (RibbonGroupBox) o;
    }

    public static void SetAppropriateSize(UIElement element, RibbonGroupBoxState state)
    {
      int index = (int) state;
      if (state == RibbonGroupBoxState.Collapsed)
        index = 0;
      if (!(element is IRibbonControl ribbonControl))
        return;
      ribbonControl.Size = RibbonControl.GetThreeSizeDefinition(element)[index];
    }

    public string SizeDefinition
    {
      get => (string) this.GetValue(RibbonControl.SizeDefinitionProperty);
      set => this.SetValue(RibbonControl.SizeDefinitionProperty, (object) value);
    }

    public static RibbonControlSize[] GetThreeSizeDefinition(UIElement element)
    {
      string[] strArray = (element as IRibbonControl).SizeDefinition.Split(new char[5]
      {
        ' ',
        ',',
        ';',
        '-',
        '>'
      }, StringSplitOptions.RemoveEmptyEntries);
      int num = Math.Min(strArray.Length, 3);
      if (num == 0)
        return new RibbonControlSize[3];
      RibbonControlSize[] threeSizeDefinition = new RibbonControlSize[3];
      for (int index = 0; index < num; ++index)
      {
        switch (strArray[index])
        {
          case "Large":
            threeSizeDefinition[index] = RibbonControlSize.Large;
            break;
          case "Middle":
            threeSizeDefinition[index] = RibbonControlSize.Middle;
            break;
          case "Small":
            threeSizeDefinition[index] = RibbonControlSize.Small;
            break;
          default:
            threeSizeDefinition[index] = RibbonControlSize.Large;
            break;
        }
      }
      for (int index = num; index < 3; ++index)
        threeSizeDefinition[index] = threeSizeDefinition[num - 1];
      return threeSizeDefinition;
    }

    public object Header
    {
      get => (object) (string) this.GetValue(RibbonControl.HeaderProperty);
      set => this.SetValue(RibbonControl.HeaderProperty, value);
    }

    public object Icon
    {
      get => this.GetValue(RibbonControl.IconProperty);
      set => this.SetValue(RibbonControl.IconProperty, value);
    }

    private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      RibbonControl ribbonControl = d as RibbonControl;
      if (e.OldValue is FrameworkElement oldValue)
        ribbonControl.RemoveLogicalChild((object) oldValue);
      if (!(e.NewValue is FrameworkElement newValue))
        return;
      ribbonControl.AddLogicalChild((object) newValue);
    }

    [Bindable(true)]
    [Category("Action")]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public ICommand Command
    {
      get => (ICommand) this.GetValue(RibbonControl.CommandProperty);
      set => this.SetValue(RibbonControl.CommandProperty, (object) value);
    }

    [Category("Action")]
    [Bindable(true)]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public object CommandParameter
    {
      get => this.GetValue(RibbonControl.CommandParameterProperty);
      set => this.SetValue(RibbonControl.CommandParameterProperty, value);
    }

    [Bindable(true)]
    [Category("Action")]
    public IInputElement CommandTarget
    {
      get => (IInputElement) this.GetValue(RibbonControl.CommandTargetProperty);
      set => this.SetValue(RibbonControl.CommandTargetProperty, (object) value);
    }

    private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      RibbonControl ribbonControl = d as RibbonControl;
      EventHandler eventHandler1 = new EventHandler(ribbonControl.OnCommandCanExecuteChanged);
      if (e.OldValue != null)
        (e.OldValue as ICommand).CanExecuteChanged -= eventHandler1;
      if (e.NewValue != null)
      {
        EventHandler eventHandler2 = new EventHandler(ribbonControl.OnCommandCanExecuteChanged);
        ribbonControl.canExecuteChangedHandler = eventHandler2;
        (e.NewValue as ICommand).CanExecuteChanged += eventHandler2;
        if (e.NewValue is RoutedUICommand newValue && ribbonControl.Header == null)
          ribbonControl.Header = (object) newValue.Text;
      }
      ribbonControl.UpdateCanExecute();
    }

    private void OnCommandCanExecuteChanged(object sender, EventArgs e) => this.UpdateCanExecute();

    private void UpdateCanExecute()
    {
      bool flag = this.Command != null && this.CanExecuteCommand();
      if (this.currentCanExecute == flag)
        return;
      this.currentCanExecute = flag;
      this.CoerceValue(UIElement.IsEnabledProperty);
    }

    protected void ExecuteCommand()
    {
      ICommand command = this.Command;
      if (command == null)
        return;
      object commandParameter = this.CommandParameter;
      if (command is RoutedCommand routedCommand)
      {
        if (!routedCommand.CanExecute(commandParameter, this.CommandTarget))
          return;
        routedCommand.Execute(commandParameter, this.CommandTarget);
      }
      else
      {
        if (!command.CanExecute(commandParameter))
          return;
        command.Execute(commandParameter);
      }
    }

    protected bool CanExecuteCommand()
    {
      ICommand command = this.Command;
      if (command == null)
        return false;
      object commandParameter = this.CommandParameter;
      return !(command is RoutedCommand routedCommand) ? command.CanExecute(commandParameter) : routedCommand.CanExecute(commandParameter, this.CommandTarget);
    }

    protected override bool IsEnabledCore
    {
      get
      {
        if (!base.IsEnabledCore)
          return false;
        return this.currentCanExecute || this.Command == null;
      }
    }

    private static object CoerceIsEnabled(DependencyObject d, object basevalue)
    {
      RibbonControl current = (RibbonControl) d;
      bool flag1 = !(LogicalTreeHelper.GetParent((DependencyObject) current) is UIElement parent) || parent.IsEnabled;
      bool flag2 = current.Command == null || current.currentCanExecute;
      return (object) (bool) (!(bool) basevalue || !flag1 ? 0 : (flag2 ? 1 : 0));
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static RibbonControl()
    {
      Type type = typeof (RibbonControl);
      ContextMenuService.Attach(type);
      ToolTipService.Attach(type);
    }

    protected RibbonControl() => ContextMenuService.Coerce((DependencyObject) this);

    public abstract FrameworkElement CreateQuickAccessItem();

    public static void BindQuickAccessItem(FrameworkElement source, FrameworkElement element)
    {
      if (source is ICommandSource)
      {
        if (source is MenuItem)
        {
          RibbonControl.Bind((object) source, element, "CommandParameter", ButtonBase.CommandParameterProperty, BindingMode.OneWay);
          RibbonControl.Bind((object) source, element, "CommandTarget", System.Windows.Controls.MenuItem.CommandTargetProperty, BindingMode.OneWay);
          RibbonControl.Bind((object) source, element, "Command", System.Windows.Controls.MenuItem.CommandProperty, BindingMode.OneWay);
        }
        else
        {
          RibbonControl.Bind((object) source, element, "CommandParameter", ButtonBase.CommandParameterProperty, BindingMode.OneWay);
          RibbonControl.Bind((object) source, element, "CommandTarget", ButtonBase.CommandTargetProperty, BindingMode.OneWay);
          RibbonControl.Bind((object) source, element, "Command", ButtonBase.CommandProperty, BindingMode.OneWay);
        }
      }
      RibbonControl.Bind((object) source, element, "ToolTip", FrameworkElement.ToolTipProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) source, element, "FontFamily", Control.FontFamilyProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) source, element, "FontSize", Control.FontSizeProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) source, element, "FontStretch", Control.FontStretchProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) source, element, "FontStyle", Control.FontStyleProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) source, element, "FontWeight", Control.FontWeightProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) source, element, "Foreground", Control.ForegroundProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) source, element, "IsEnabled", UIElement.IsEnabledProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) source, element, "Opacity", UIElement.OpacityProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) source, element, "SnapsToDevicePixels", UIElement.SnapsToDevicePixelsProperty, BindingMode.OneWay);
      IRibbonControl ribbonControl = source as IRibbonControl;
      if (ribbonControl.Icon != null)
      {
        if (ribbonControl.Icon is Visual icon)
        {
          Rectangle rectangle = new Rectangle();
          rectangle.Width = 16.0;
          rectangle.Height = 16.0;
          rectangle.Fill = (Brush) new VisualBrush(icon);
          (element as IRibbonControl).Icon = (object) rectangle;
        }
        else
          RibbonControl.Bind((object) source, element, "Icon", RibbonControl.IconProperty, BindingMode.OneWay);
      }
      if (ribbonControl.Header != null)
        RibbonControl.Bind((object) source, element, "Header", RibbonControl.HeaderProperty, BindingMode.OneWay);
      if (source is IQuickAccessItemProvider source1 && source1.QuickAccessElementStyle != null)
        RibbonControl.Bind((object) source1, element, "QuickAccessElementStyle", FrameworkElement.StyleProperty, BindingMode.OneWay);
      (element as IRibbonControl).Size = RibbonControlSize.Small;
    }

    public bool CanAddToQuickAccessToolBar
    {
      get => (bool) this.GetValue(RibbonControl.CanAddToQuickAccessToolBarProperty);
      set => this.SetValue(RibbonControl.CanAddToQuickAccessToolBarProperty, (object) value);
    }

    public static void OnCanAddToQuickAccessToolbarChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      d.CoerceValue(FrameworkElement.ContextMenuProperty);
    }

    public Style QuickAccessElementStyle
    {
      get => (Style) this.GetValue(RibbonControl.QuickAccessElementStyleProperty);
      set => this.SetValue(RibbonControl.QuickAccessElementStyleProperty, (object) value);
    }

    internal static void Bind(
      object source,
      FrameworkElement target,
      string path,
      DependencyProperty property,
      BindingMode mode)
    {
      target.SetBinding(property, (BindingBase) new Binding()
      {
        Path = new PropertyPath(path, new object[0]),
        Source = source,
        Mode = mode
      });
    }

    public virtual void OnKeyTipPressed()
    {
    }

    protected virtual void OnSizePropertyChanged(
      RibbonControlSize previous,
      RibbonControlSize current)
    {
    }
  }
}
