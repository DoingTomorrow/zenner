// Decompiled with JetBrains decompiler
// Type: Fluent.ApplicationMenu
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Fluent
{
  public class ApplicationMenu : DropDownButton
  {
    public static readonly DependencyProperty RightPaneWidthProperty = DependencyProperty.Register(nameof (RightPaneWidth), typeof (double), typeof (ApplicationMenu), (PropertyMetadata) new UIPropertyMetadata((object) 300.0));
    public static readonly DependencyProperty RightPaneContentProperty = DependencyProperty.Register(nameof (RightPaneContent), typeof (object), typeof (ApplicationMenu), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty FooterPaneContentProperty = DependencyProperty.Register(nameof (FooterPaneContent), typeof (object), typeof (ApplicationMenu), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));

    public double RightPaneWidth
    {
      get => (double) this.GetValue(ApplicationMenu.RightPaneWidthProperty);
      set => this.SetValue(ApplicationMenu.RightPaneWidthProperty, (object) value);
    }

    public object RightPaneContent
    {
      get => this.GetValue(ApplicationMenu.RightPaneContentProperty);
      set => this.SetValue(ApplicationMenu.RightPaneContentProperty, value);
    }

    public object FooterPaneContent
    {
      get => this.GetValue(ApplicationMenu.FooterPaneContentProperty);
      set => this.SetValue(ApplicationMenu.FooterPaneContentProperty, value);
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static ApplicationMenu()
    {
      Type type = typeof (ApplicationMenu);
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) type));
      DropDownButton.CanAddToQuickAccessToolBarProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) false));
      KeyTip.KeysProperty.AddOwner(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) null, (PropertyChangedCallback) null, new CoerceValueCallback(ApplicationMenu.CoerceKeyTipKeys)));
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (ApplicationMenu), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(ApplicationMenu.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (ApplicationMenu));
      return basevalue;
    }

    private static object CoerceKeyTipKeys(DependencyObject d, object basevalue)
    {
      return basevalue ?? (object) Ribbon.Localization.BackstageButtonKeyTip;
    }

    public ApplicationMenu() => this.CoerceValue(KeyTip.KeysProperty);

    private void OnPopupOpened(object sender, EventArgs e)
    {
      Mouse.Capture((IInputElement) this, CaptureMode.SubTree);
    }

    public override FrameworkElement CreateQuickAccessItem() => throw new NotImplementedException();
  }
}
