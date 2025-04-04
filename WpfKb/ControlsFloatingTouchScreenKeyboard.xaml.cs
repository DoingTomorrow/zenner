// Decompiled with JetBrains decompiler
// Type: WpfKb.Controls.FloatingTouchScreenKeyboard
// Assembly: WpfKb, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B294CC70-CB21-4202-BD7A-A4E6693370B9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\WpfKb.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

#nullable disable
namespace WpfKb.Controls
{
  public partial class FloatingTouchScreenKeyboard : Popup, IComponentConnector
  {
    public static readonly DependencyProperty AreAnimationsEnabledProperty = DependencyProperty.Register(nameof (AreAnimationsEnabled), typeof (bool), typeof (FloatingTouchScreenKeyboard), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty IsAllowedToFadeProperty = DependencyProperty.Register(nameof (IsAllowedToFade), typeof (bool), typeof (FloatingTouchScreenKeyboard), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty IsDraggingProperty = DependencyProperty.Register(nameof (IsDragging), typeof (bool), typeof (FloatingTouchScreenKeyboard), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty IsDragHelperAllowedToHideProperty = DependencyProperty.Register(nameof (IsDragHelperAllowedToHide), typeof (bool), typeof (FloatingTouchScreenKeyboard), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty IsKeyboardShownProperty = DependencyProperty.Register(nameof (IsKeyboardShown), typeof (bool), typeof (FloatingTouchScreenKeyboard), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty MaximumKeyboardOpacityProperty = DependencyProperty.Register(nameof (MaximumKeyboardOpacity), typeof (double), typeof (FloatingTouchScreenKeyboard), (PropertyMetadata) new UIPropertyMetadata((object) 0.9));
    public static readonly DependencyProperty MinimumKeyboardOpacityProperty = DependencyProperty.Register(nameof (MinimumKeyboardOpacity), typeof (double), typeof (FloatingTouchScreenKeyboard), (PropertyMetadata) new UIPropertyMetadata((object) 0.2));
    public static readonly DependencyProperty KeyboardHideDelayProperty = DependencyProperty.Register(nameof (KeyboardHideDelay), typeof (double), typeof (FloatingTouchScreenKeyboard), (PropertyMetadata) new UIPropertyMetadata((object) 5.0));
    public static readonly DependencyProperty KeyboardHideAnimationDurationProperty = DependencyProperty.Register(nameof (KeyboardHideAnimationDuration), typeof (double), typeof (FloatingTouchScreenKeyboard), (PropertyMetadata) new UIPropertyMetadata((object) 0.5));
    public static readonly DependencyProperty KeyboardShowAnimationDurationProperty = DependencyProperty.Register(nameof (KeyboardShowAnimationDuration), typeof (double), typeof (FloatingTouchScreenKeyboard), (PropertyMetadata) new UIPropertyMetadata((object) 0.5));
    public static readonly DependencyProperty DeadZoneProperty = DependencyProperty.Register(nameof (DeadZone), typeof (double), typeof (FloatingTouchScreenKeyboard), (PropertyMetadata) new UIPropertyMetadata((object) 5.0));
    public static readonly DependencyProperty KeyBoardInputTypeProperty = DependencyProperty.Register(nameof (KeyBoardInputType), typeof (string), typeof (FloatingTouchScreenKeyboard), (PropertyMetadata) new UIPropertyMetadata((object) "AlphaNumeric", new PropertyChangedCallback(FloatingTouchScreenKeyboard.OnInputPropertyChanged)));
    [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
    internal FloatingTouchScreenKeyboard keyboard;
    [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
    internal Grid LayoutGrid;
    private bool _contentLoaded;

    private static void OnInputPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!((string) e.NewValue == "AlphaNumeric"))
        ;
      if (!((string) e.NewValue == "Numeric"))
        ;
    }

    public string KeyBoardInputType
    {
      get => (string) this.GetValue(FloatingTouchScreenKeyboard.KeyBoardInputTypeProperty);
      set => this.SetValue(FloatingTouchScreenKeyboard.KeyBoardInputTypeProperty, (object) value);
    }

    public FloatingTouchScreenKeyboard() => this.InitializeComponent();

    public bool AreAnimationsEnabled
    {
      get => (bool) this.GetValue(FloatingTouchScreenKeyboard.AreAnimationsEnabledProperty);
      set
      {
        this.SetValue(FloatingTouchScreenKeyboard.AreAnimationsEnabledProperty, (object) value);
      }
    }

    public bool IsAllowedToFade
    {
      get => (bool) this.GetValue(FloatingTouchScreenKeyboard.IsAllowedToFadeProperty);
      set => this.SetValue(FloatingTouchScreenKeyboard.IsAllowedToFadeProperty, (object) value);
    }

    public bool IsDragging
    {
      get => (bool) this.GetValue(FloatingTouchScreenKeyboard.IsDraggingProperty);
      private set => this.SetValue(FloatingTouchScreenKeyboard.IsDraggingProperty, (object) value);
    }

    public bool IsDragHelperAllowedToHide
    {
      get => (bool) this.GetValue(FloatingTouchScreenKeyboard.IsDragHelperAllowedToHideProperty);
      set
      {
        this.SetValue(FloatingTouchScreenKeyboard.IsDragHelperAllowedToHideProperty, (object) value);
      }
    }

    public bool IsKeyboardShown
    {
      get => (bool) this.GetValue(FloatingTouchScreenKeyboard.IsKeyboardShownProperty);
      private set
      {
        this.SetValue(FloatingTouchScreenKeyboard.IsKeyboardShownProperty, (object) value);
      }
    }

    public double MaximumKeyboardOpacity
    {
      get => (double) this.GetValue(FloatingTouchScreenKeyboard.MaximumKeyboardOpacityProperty);
      set
      {
        this.SetValue(FloatingTouchScreenKeyboard.MaximumKeyboardOpacityProperty, (object) value);
      }
    }

    public double MinimumKeyboardOpacity
    {
      get => (double) this.GetValue(FloatingTouchScreenKeyboard.MinimumKeyboardOpacityProperty);
      set
      {
        this.SetValue(FloatingTouchScreenKeyboard.MinimumKeyboardOpacityProperty, (object) value);
      }
    }

    public double KeyboardHideDelay
    {
      get => (double) this.GetValue(FloatingTouchScreenKeyboard.KeyboardHideDelayProperty);
      set => this.SetValue(FloatingTouchScreenKeyboard.KeyboardHideDelayProperty, (object) value);
    }

    public double KeyboardHideAnimationDuration
    {
      get
      {
        return (double) this.GetValue(FloatingTouchScreenKeyboard.KeyboardHideAnimationDurationProperty);
      }
      set
      {
        this.SetValue(FloatingTouchScreenKeyboard.KeyboardHideAnimationDurationProperty, (object) value);
      }
    }

    public double KeyboardShowAnimationDuration
    {
      get
      {
        return (double) this.GetValue(FloatingTouchScreenKeyboard.KeyboardShowAnimationDurationProperty);
      }
      set
      {
        this.SetValue(FloatingTouchScreenKeyboard.KeyboardShowAnimationDurationProperty, (object) value);
      }
    }

    public double DeadZone
    {
      get => (double) this.GetValue(FloatingTouchScreenKeyboard.DeadZoneProperty);
      set => this.SetValue(FloatingTouchScreenKeyboard.DeadZoneProperty, (object) value);
    }

    protected override void OnOpened(EventArgs e)
    {
      this.IsKeyboardShown = true;
      base.OnOpened(e);
    }

    protected override void OnClosed(EventArgs e)
    {
      this.IsKeyboardShown = false;
      base.OnClosed(e);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/WpfKb;component/controls/floatingtouchscreenkeyboard.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    internal Delegate _CreateDelegate(Type delegateType, string handler)
    {
      return Delegate.CreateDelegate(delegateType, (object) this, handler);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
    [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.keyboard = (FloatingTouchScreenKeyboard) target;
          break;
        case 2:
          this.LayoutGrid = (Grid) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
