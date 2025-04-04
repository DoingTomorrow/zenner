// Decompiled with JetBrains decompiler
// Type: Fluent.RibbonMenu
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace Fluent
{
  [ContentProperty("Items")]
  public class RibbonMenu : MenuBase
  {
    static RibbonMenu()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (RibbonMenu), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (RibbonMenu)));
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (RibbonMenu), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(RibbonMenu.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (RibbonMenu));
      return basevalue;
    }

    public RibbonMenu() => FocusManager.SetIsFocusScope((DependencyObject) this, false);

    protected override DependencyObject GetContainerForItemOverride()
    {
      return (DependencyObject) new MenuItem();
    }

    protected override bool IsItemItsOwnContainerOverride(object item) => item is System.Windows.Controls.MenuItem;

    protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
    {
      if (!(this.GetRootDropDownControl() is IInputElement rootDropDownControl))
        return;
      Keyboard.Focus(rootDropDownControl);
    }

    private IDropDownControl GetRootDropDownControl()
    {
      for (DependencyObject dependencyObject = (DependencyObject) this; dependencyObject != null; dependencyObject = VisualTreeHelper.GetParent(dependencyObject) ?? LogicalTreeHelper.GetParent(dependencyObject))
      {
        if (dependencyObject is IDropDownControl rootDropDownControl)
          return rootDropDownControl;
      }
      return (IDropDownControl) null;
    }
  }
}
