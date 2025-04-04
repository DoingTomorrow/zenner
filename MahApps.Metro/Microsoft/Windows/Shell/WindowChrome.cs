// Decompiled with JetBrains decompiler
// Type: Microsoft.Windows.Shell.WindowChrome
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace Microsoft.Windows.Shell
{
  public class WindowChrome : Freezable
  {
    public static readonly DependencyProperty WindowChromeProperty = DependencyProperty.RegisterAttached(nameof (WindowChrome), typeof (WindowChrome), typeof (WindowChrome), new PropertyMetadata((object) null, new PropertyChangedCallback(WindowChrome._OnChromeChanged)));
    public static readonly DependencyProperty IsHitTestVisibleInChromeProperty = DependencyProperty.RegisterAttached("IsHitTestVisibleInChrome", typeof (bool), typeof (WindowChrome), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.Inherits));
    public static readonly DependencyProperty ResizeGripDirectionProperty = DependencyProperty.RegisterAttached("ResizeGripDirection", typeof (ResizeGripDirection), typeof (WindowChrome), (PropertyMetadata) new FrameworkPropertyMetadata((object) ResizeGripDirection.None, FrameworkPropertyMetadataOptions.Inherits));
    public static readonly DependencyProperty CaptionHeightProperty = DependencyProperty.Register(nameof (CaptionHeight), typeof (double), typeof (WindowChrome), new PropertyMetadata((object) 0.0, (PropertyChangedCallback) ((d, e) => ((WindowChrome) d)._OnPropertyChangedThatRequiresRepaint())), (ValidateValueCallback) (value => (double) value >= 0.0));
    public static readonly DependencyProperty ResizeBorderThicknessProperty = DependencyProperty.Register(nameof (ResizeBorderThickness), typeof (Thickness), typeof (WindowChrome), new PropertyMetadata((object) new Thickness()), (ValidateValueCallback) (value => ((Thickness) value).IsNonNegative()));
    public static readonly DependencyProperty GlassFrameThicknessProperty = DependencyProperty.Register(nameof (GlassFrameThickness), typeof (Thickness), typeof (WindowChrome), new PropertyMetadata((object) new Thickness(), (PropertyChangedCallback) ((d, e) => ((WindowChrome) d)._OnPropertyChangedThatRequiresRepaint()), (CoerceValueCallback) ((d, o) => WindowChrome._CoerceGlassFrameThickness((Thickness) o))));
    public static readonly DependencyProperty UseAeroCaptionButtonsProperty = DependencyProperty.Register(nameof (UseAeroCaptionButtons), typeof (bool), typeof (WindowChrome), (PropertyMetadata) new FrameworkPropertyMetadata((object) true));
    public static readonly DependencyProperty IgnoreTaskbarOnMaximizeProperty = DependencyProperty.Register(nameof (IgnoreTaskbarOnMaximize), typeof (bool), typeof (WindowChrome), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, (PropertyChangedCallback) ((d, e) => ((WindowChrome) d)._OnPropertyChangedThatRequiresRepaint())));
    public static readonly DependencyProperty UseNoneWindowStyleProperty = DependencyProperty.Register(nameof (UseNoneWindowStyle), typeof (bool), typeof (WindowChrome), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, (PropertyChangedCallback) ((d, e) => ((WindowChrome) d)._OnPropertyChangedThatRequiresRepaint())));
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof (CornerRadius), typeof (CornerRadius), typeof (WindowChrome), new PropertyMetadata((object) new CornerRadius(), (PropertyChangedCallback) ((d, e) => ((WindowChrome) d)._OnPropertyChangedThatRequiresRepaint())), (ValidateValueCallback) (value => ((CornerRadius) value).IsValid()));
    public static readonly DependencyProperty SacrificialEdgeProperty = DependencyProperty.Register(nameof (SacrificialEdge), typeof (SacrificialEdge), typeof (WindowChrome), new PropertyMetadata((object) SacrificialEdge.None, (PropertyChangedCallback) ((d, e) => ((WindowChrome) d)._OnPropertyChangedThatRequiresRepaint())), new ValidateValueCallback(WindowChrome._IsValidSacrificialEdge));
    private static readonly SacrificialEdge SacrificialEdge_All = SacrificialEdge.Office | SacrificialEdge.Top;
    private static readonly List<WindowChrome._SystemParameterBoundProperty> _BoundProperties = new List<WindowChrome._SystemParameterBoundProperty>()
    {
      new WindowChrome._SystemParameterBoundProperty()
      {
        DependencyProperty = WindowChrome.CornerRadiusProperty,
        SystemParameterPropertyName = "WindowCornerRadius"
      },
      new WindowChrome._SystemParameterBoundProperty()
      {
        DependencyProperty = WindowChrome.CaptionHeightProperty,
        SystemParameterPropertyName = "WindowCaptionHeight"
      },
      new WindowChrome._SystemParameterBoundProperty()
      {
        DependencyProperty = WindowChrome.ResizeBorderThicknessProperty,
        SystemParameterPropertyName = "WindowResizeBorderThickness"
      },
      new WindowChrome._SystemParameterBoundProperty()
      {
        DependencyProperty = WindowChrome.GlassFrameThicknessProperty,
        SystemParameterPropertyName = "WindowNonClientFrameThickness"
      }
    };

    public static Thickness GlassFrameCompleteThickness => new Thickness(-1.0);

    private static void _OnChromeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (DesignerProperties.GetIsInDesignMode(d))
        return;
      Window window = (Window) d;
      WindowChrome newValue = (WindowChrome) e.NewValue;
      WindowChromeWorker chrome = WindowChromeWorker.GetWindowChromeWorker(window);
      if (chrome == null)
      {
        chrome = new WindowChromeWorker();
        WindowChromeWorker.SetWindowChromeWorker(window, chrome);
      }
      chrome.SetWindowChrome(newValue);
    }

    [Category("MahApps.Metro")]
    public static WindowChrome GetWindowChrome(Window window)
    {
      Verify.IsNotNull<Window>(window, nameof (window));
      return (WindowChrome) window.GetValue(WindowChrome.WindowChromeProperty);
    }

    public static void SetWindowChrome(Window window, WindowChrome chrome)
    {
      Verify.IsNotNull<Window>(window, nameof (window));
      window.SetValue(WindowChrome.WindowChromeProperty, (object) chrome);
    }

    [Category("MahApps.Metro")]
    public static bool GetIsHitTestVisibleInChrome(IInputElement inputElement)
    {
      Verify.IsNotNull<IInputElement>(inputElement, nameof (inputElement));
      return inputElement is DependencyObject dependencyObject ? (bool) dependencyObject.GetValue(WindowChrome.IsHitTestVisibleInChromeProperty) : throw new ArgumentException("The element must be a DependencyObject", nameof (inputElement));
    }

    public static void SetIsHitTestVisibleInChrome(IInputElement inputElement, bool hitTestVisible)
    {
      Verify.IsNotNull<IInputElement>(inputElement, nameof (inputElement));
      if (!(inputElement is DependencyObject dependencyObject))
        throw new ArgumentException("The element must be a DependencyObject", nameof (inputElement));
      dependencyObject.SetValue(WindowChrome.IsHitTestVisibleInChromeProperty, (object) hitTestVisible);
    }

    [Category("MahApps.Metro")]
    public static ResizeGripDirection GetResizeGripDirection(IInputElement inputElement)
    {
      Verify.IsNotNull<IInputElement>(inputElement, nameof (inputElement));
      return inputElement is DependencyObject dependencyObject ? (ResizeGripDirection) dependencyObject.GetValue(WindowChrome.ResizeGripDirectionProperty) : throw new ArgumentException("The element must be a DependencyObject", nameof (inputElement));
    }

    public static void SetResizeGripDirection(
      IInputElement inputElement,
      ResizeGripDirection direction)
    {
      Verify.IsNotNull<IInputElement>(inputElement, nameof (inputElement));
      if (!(inputElement is DependencyObject dependencyObject))
        throw new ArgumentException("The element must be a DependencyObject", nameof (inputElement));
      dependencyObject.SetValue(WindowChrome.ResizeGripDirectionProperty, (object) direction);
    }

    public double CaptionHeight
    {
      get => (double) this.GetValue(WindowChrome.CaptionHeightProperty);
      set => this.SetValue(WindowChrome.CaptionHeightProperty, (object) value);
    }

    public Thickness ResizeBorderThickness
    {
      get => (Thickness) this.GetValue(WindowChrome.ResizeBorderThicknessProperty);
      set => this.SetValue(WindowChrome.ResizeBorderThicknessProperty, (object) value);
    }

    private static object _CoerceGlassFrameThickness(Thickness thickness)
    {
      return !thickness.IsNonNegative() ? (object) WindowChrome.GlassFrameCompleteThickness : (object) thickness;
    }

    public Thickness GlassFrameThickness
    {
      get => (Thickness) this.GetValue(WindowChrome.GlassFrameThicknessProperty);
      set => this.SetValue(WindowChrome.GlassFrameThicknessProperty, (object) value);
    }

    public bool UseAeroCaptionButtons
    {
      get => (bool) this.GetValue(WindowChrome.UseAeroCaptionButtonsProperty);
      set => this.SetValue(WindowChrome.UseAeroCaptionButtonsProperty, (object) value);
    }

    public bool IgnoreTaskbarOnMaximize
    {
      get => (bool) this.GetValue(WindowChrome.IgnoreTaskbarOnMaximizeProperty);
      set => this.SetValue(WindowChrome.IgnoreTaskbarOnMaximizeProperty, (object) value);
    }

    public bool UseNoneWindowStyle
    {
      get => (bool) this.GetValue(WindowChrome.UseNoneWindowStyleProperty);
      set => this.SetValue(WindowChrome.UseNoneWindowStyleProperty, (object) value);
    }

    public CornerRadius CornerRadius
    {
      get => (CornerRadius) this.GetValue(WindowChrome.CornerRadiusProperty);
      set => this.SetValue(WindowChrome.CornerRadiusProperty, (object) value);
    }

    private static bool _IsValidSacrificialEdge(object value)
    {
      SacrificialEdge sacrificialEdge;
      try
      {
        sacrificialEdge = (SacrificialEdge) value;
      }
      catch (InvalidCastException ex)
      {
        return false;
      }
      return sacrificialEdge == SacrificialEdge.None || (sacrificialEdge | WindowChrome.SacrificialEdge_All) == WindowChrome.SacrificialEdge_All && sacrificialEdge != WindowChrome.SacrificialEdge_All;
    }

    public SacrificialEdge SacrificialEdge
    {
      get => (SacrificialEdge) this.GetValue(WindowChrome.SacrificialEdgeProperty);
      set => this.SetValue(WindowChrome.SacrificialEdgeProperty, (object) value);
    }

    protected override Freezable CreateInstanceCore() => (Freezable) new WindowChrome();

    public WindowChrome()
    {
      foreach (WindowChrome._SystemParameterBoundProperty boundProperty in WindowChrome._BoundProperties)
        BindingOperations.SetBinding((DependencyObject) this, boundProperty.DependencyProperty, (BindingBase) new Binding()
        {
          Path = new PropertyPath("(SystemParameters." + boundProperty.SystemParameterPropertyName + ")", new object[0]),
          Mode = BindingMode.OneWay,
          UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        });
    }

    private void _OnPropertyChangedThatRequiresRepaint()
    {
      EventHandler thatRequiresRepaint = this.PropertyChangedThatRequiresRepaint;
      if (thatRequiresRepaint == null)
        return;
      thatRequiresRepaint((object) this, EventArgs.Empty);
    }

    internal event EventHandler PropertyChangedThatRequiresRepaint;

    private struct _SystemParameterBoundProperty
    {
      public string SystemParameterPropertyName { get; set; }

      public DependencyProperty DependencyProperty { get; set; }
    }
  }
}
