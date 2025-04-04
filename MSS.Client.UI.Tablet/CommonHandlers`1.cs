// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.CommonHandlers`1
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using MSS.Client.UI.Tablet.Common;
using MSS.Client.UI.Tablet.Common.Interfaces;
using MSS.Utils.Utils;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Telerik.Windows.Controls;
using WpfKb.Controls;

#nullable disable
namespace MSS.Client.UI.Tablet
{
  public static class CommonHandlers<TParam> where TParam : IEventParams
  {
    private static Action<Func<TParam>> openKeyboard = (Action<Func<TParam>>) (inv =>
    {
      TParam o = inv();
      int num = (int) Enumerable.Cast<InputScopeName>(o.FrameworkElement.InputScope.Names).FirstOrDefault<InputScopeName>().NameValue.If<InputScopeNameValue>((Func<InputScopeNameValue, bool>) (_ => _ == InputScopeNameValue.Number), (Action<InputScopeNameValue>) (_ =>
      {
        o.UserControlElement.SafeCast<TouchScreenKeyboardUserControl>().KeyBoardInputType = "Numeric";
        o.UserControlElement.SafeCast<TouchScreenKeyboardUserControl>().Visibility = Visibility.Visible;
      }), (Action<InputScopeNameValue>) (_ =>
      {
        o.UserControlElement.SafeCast<TouchScreenKeyboardUserControl>().KeyBoardInputType = "AlphaNumeric";
        o.UserControlElement.SafeCast<TouchScreenKeyboardUserControl>().Visibility = Visibility.Visible;
      }));
    });
    private static Action<TParam> closeKeyboard = (Action<TParam>) (o => o.UserControlElement.SafeCast<TouchScreenKeyboardUserControl>().Visibility = Visibility.Collapsed);
    private static double RememberedPoisitionOfScrollBar;
    private static Control LastFocusedItem;
    public static Action<Control, TouchScreenKeyboardUserControl> RegisterKeyboardEvents = (Action<Control, TouchScreenKeyboardUserControl>) ((item, kb) => item.GotFocus += (RoutedEventHandler) ((s, e) =>
    {
      Application.Current.Dispatcher.InvokeAsync((Action) (() =>
      {
        ScrollViewer ancestor = s.SafeCast<Control>().ParentOfType<ScrollViewer>();
        Point point = item.TransformToAncestor((Visual) ancestor).Transform(new Point(0.0, 0.0));
        ancestor.CanContentScroll = false;
        CommonHandlers<TParam>.RememberedPoisitionOfScrollBar = point.Y - 5.0;
        double offset = ancestor.VerticalOffset + point.Y - 5.0;
        ancestor.ScrollToVerticalOffset(offset);
        ancestor.UpdateLayout();
      }), DispatcherPriority.Render);
      CommonHandlers<OpenKeyboardEventParams>.OpenKeyboard((Func<OpenKeyboardEventParams>) (() => new OpenKeyboardEventParams()
      {
        FrameworkElement = s.SafeCast<FrameworkElement>(),
        RoutedEventArgs = e,
        UserControlElement = (object) kb
      }));
      CommonHandlers<TParam>.LastFocusedItem = item;
    }));

    public static void SetFirstFocusableItem(Control control)
    {
      CommonHandlers<TParam>.LastFocusedItem = control;
      control?.Focus();
    }

    public static void FocusLastItem()
    {
      if (CommonHandlers<TParam>.LastFocusedItem == null)
        return;
      CommonHandlers<TParam>.LastFocusedItem.Focus();
    }

    public static Action<Func<TParam>> OpenKeyboard
    {
      get => CommonHandlers<TParam>.openKeyboard;
      set => CommonHandlers<TParam>.openKeyboard = value;
    }

    public static Action<TParam> CloseKeyboard
    {
      get => CommonHandlers<TParam>.closeKeyboard;
      set => CommonHandlers<TParam>.closeKeyboard = value;
    }
  }
}
