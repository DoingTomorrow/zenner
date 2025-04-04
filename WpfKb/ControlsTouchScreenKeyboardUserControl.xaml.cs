// Decompiled with JetBrains decompiler
// Type: WpfKb.Controls.TouchScreenKeyboardUserControl
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
using System.Windows.Markup;

#nullable disable
namespace WpfKb.Controls
{
  public partial class TouchScreenKeyboardUserControl : UserControl, IComponentConnector
  {
    public static readonly DependencyProperty AreAnimationsEnabledProperty = DependencyProperty.Register(nameof (AreAnimationsEnabled), typeof (bool), typeof (TouchScreenKeyboardUserControl), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty IsAllowedToFadeProperty = DependencyProperty.Register(nameof (IsAllowedToFade), typeof (bool), typeof (TouchScreenKeyboardUserControl), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty IsDraggingProperty = DependencyProperty.Register(nameof (IsDragging), typeof (bool), typeof (TouchScreenKeyboardUserControl), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty IsDragHelperAllowedToHideProperty = DependencyProperty.Register(nameof (IsDragHelperAllowedToHide), typeof (bool), typeof (TouchScreenKeyboardUserControl), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty IsKeyboardShownProperty = DependencyProperty.Register(nameof (IsKeyboardShown), typeof (bool), typeof (TouchScreenKeyboardUserControl), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty MaximumKeyboardOpacityProperty = DependencyProperty.Register(nameof (MaximumKeyboardOpacity), typeof (double), typeof (TouchScreenKeyboardUserControl), (PropertyMetadata) new UIPropertyMetadata((object) 0.9));
    public static readonly DependencyProperty MinimumKeyboardOpacityProperty = DependencyProperty.Register(nameof (MinimumKeyboardOpacity), typeof (double), typeof (TouchScreenKeyboardUserControl), (PropertyMetadata) new UIPropertyMetadata((object) 0.2));
    public static readonly DependencyProperty KeyboardHideDelayProperty = DependencyProperty.Register(nameof (KeyboardHideDelay), typeof (double), typeof (TouchScreenKeyboardUserControl), (PropertyMetadata) new UIPropertyMetadata((object) 5.0));
    public static readonly DependencyProperty KeyboardHideAnimationDurationProperty = DependencyProperty.Register(nameof (KeyboardHideAnimationDuration), typeof (double), typeof (TouchScreenKeyboardUserControl), (PropertyMetadata) new UIPropertyMetadata((object) 0.5));
    public static readonly DependencyProperty KeyboardShowAnimationDurationProperty = DependencyProperty.Register(nameof (KeyboardShowAnimationDuration), typeof (double), typeof (TouchScreenKeyboardUserControl), (PropertyMetadata) new UIPropertyMetadata((object) 0.5));
    public static readonly DependencyProperty DeadZoneProperty = DependencyProperty.Register(nameof (DeadZone), typeof (double), typeof (TouchScreenKeyboardUserControl), (PropertyMetadata) new UIPropertyMetadata((object) 5.0));
    public static readonly DependencyProperty KeyBoardInputTypeProperty = DependencyProperty.Register(nameof (KeyBoardInputType), typeof (string), typeof (TouchScreenKeyboardUserControl), (PropertyMetadata) new UIPropertyMetadata((object) "AlphaNumeric", new PropertyChangedCallback(TouchScreenKeyboardUserControl.OnInputPropertyChanged)));
    [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
    internal TouchScreenKeyboardUserControl keyboard;
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
      get => (string) this.GetValue(TouchScreenKeyboardUserControl.KeyBoardInputTypeProperty);
      set
      {
        this.SetValue(TouchScreenKeyboardUserControl.KeyBoardInputTypeProperty, (object) value);
      }
    }

    public TouchScreenKeyboardUserControl() => this.InitializeComponent();

    public bool AreAnimationsEnabled
    {
      get => (bool) this.GetValue(TouchScreenKeyboardUserControl.AreAnimationsEnabledProperty);
      set
      {
        this.SetValue(TouchScreenKeyboardUserControl.AreAnimationsEnabledProperty, (object) value);
      }
    }

    public bool IsAllowedToFade
    {
      get => (bool) this.GetValue(TouchScreenKeyboardUserControl.IsAllowedToFadeProperty);
      set => this.SetValue(TouchScreenKeyboardUserControl.IsAllowedToFadeProperty, (object) value);
    }

    public bool IsDragging
    {
      get => (bool) this.GetValue(TouchScreenKeyboardUserControl.IsDraggingProperty);
      private set
      {
        this.SetValue(TouchScreenKeyboardUserControl.IsDraggingProperty, (object) value);
      }
    }

    public bool IsDragHelperAllowedToHide
    {
      get => (bool) this.GetValue(TouchScreenKeyboardUserControl.IsDragHelperAllowedToHideProperty);
      set
      {
        this.SetValue(TouchScreenKeyboardUserControl.IsDragHelperAllowedToHideProperty, (object) value);
      }
    }

    public bool IsKeyboardShown
    {
      get => (bool) this.GetValue(TouchScreenKeyboardUserControl.IsKeyboardShownProperty);
      private set
      {
        this.SetValue(TouchScreenKeyboardUserControl.IsKeyboardShownProperty, (object) value);
      }
    }

    public double MaximumKeyboardOpacity
    {
      get => (double) this.GetValue(TouchScreenKeyboardUserControl.MaximumKeyboardOpacityProperty);
      set
      {
        this.SetValue(TouchScreenKeyboardUserControl.MaximumKeyboardOpacityProperty, (object) value);
      }
    }

    public double MinimumKeyboardOpacity
    {
      get => (double) this.GetValue(TouchScreenKeyboardUserControl.MinimumKeyboardOpacityProperty);
      set
      {
        this.SetValue(TouchScreenKeyboardUserControl.MinimumKeyboardOpacityProperty, (object) value);
      }
    }

    public double KeyboardHideDelay
    {
      get => (double) this.GetValue(TouchScreenKeyboardUserControl.KeyboardHideDelayProperty);
      set
      {
        this.SetValue(TouchScreenKeyboardUserControl.KeyboardHideDelayProperty, (object) value);
      }
    }

    public double KeyboardHideAnimationDuration
    {
      get
      {
        return (double) this.GetValue(TouchScreenKeyboardUserControl.KeyboardHideAnimationDurationProperty);
      }
      set
      {
        this.SetValue(TouchScreenKeyboardUserControl.KeyboardHideAnimationDurationProperty, (object) value);
      }
    }

    public double KeyboardShowAnimationDuration
    {
      get
      {
        return (double) this.GetValue(TouchScreenKeyboardUserControl.KeyboardShowAnimationDurationProperty);
      }
      set
      {
        this.SetValue(TouchScreenKeyboardUserControl.KeyboardShowAnimationDurationProperty, (object) value);
      }
    }

    public double DeadZone
    {
      get => (double) this.GetValue(TouchScreenKeyboardUserControl.DeadZoneProperty);
      set => this.SetValue(TouchScreenKeyboardUserControl.DeadZoneProperty, (object) value);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/WpfKb;component/controls/touchscreenkeyboardusercontrol.xaml", UriKind.Relative));
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
          this.keyboard = (TouchScreenKeyboardUserControl) target;
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
