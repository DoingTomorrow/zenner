// Decompiled with JetBrains decompiler
// Type: Fluent.PopupService
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Fluent
{
  public static class PopupService
  {
    public static readonly RoutedEvent DismissPopupEvent = EventManager.RegisterRoutedEvent("DismissPopup", RoutingStrategy.Bubble, typeof (DismissPopupEventHandler), typeof (PopupService));

    public static void RaiseDismissPopupEvent(object sender, DismissPopupMode mode)
    {
      (sender as UIElement).RaiseEvent((RoutedEventArgs) new DismissPopupEventArgs(mode));
    }

    public static void Attach(Type classType)
    {
      EventManager.RegisterClassHandler(classType, Mouse.PreviewMouseDownOutsideCapturedElementEvent, (Delegate) new MouseButtonEventHandler(PopupService.OnClickThroughThunk));
      EventManager.RegisterClassHandler(classType, PopupService.DismissPopupEvent, (Delegate) new DismissPopupEventHandler(PopupService.OnDismissPopup));
      EventManager.RegisterClassHandler(classType, FrameworkElement.ContextMenuOpeningEvent, (Delegate) new ContextMenuEventHandler(PopupService.OnContextMenuOpened), true);
      EventManager.RegisterClassHandler(classType, FrameworkElement.ContextMenuClosingEvent, (Delegate) new ContextMenuEventHandler(PopupService.OnContextMenuClosed), true);
      EventManager.RegisterClassHandler(classType, UIElement.LostMouseCaptureEvent, (Delegate) new MouseEventHandler(PopupService.OnLostMouseCapture));
    }

    public static void OnClickThroughThunk(object sender, MouseButtonEventArgs e)
    {
      if (e.ChangedButton != MouseButton.Left && e.ChangedButton != MouseButton.Right || Mouse.Captured != sender)
        return;
      PopupService.RaiseDismissPopupEvent(sender, DismissPopupMode.MouseNotOver);
    }

    public static void OnLostMouseCapture(object sender, MouseEventArgs e)
    {
      if (!(sender is IDropDownControl dropDownControl) || Mouse.Captured == sender || !dropDownControl.IsDropDownOpen || dropDownControl.IsContextMenuOpened)
        return;
      Popup dropDownPopup = dropDownControl.DropDownPopup;
      if (dropDownPopup == null || dropDownPopup.Child == null)
        PopupService.RaiseDismissPopupEvent(sender, DismissPopupMode.MouseNotOver);
      else if (e.OriginalSource == sender)
      {
        if (Mouse.Captured != null && PopupService.IsAncestorOf((DependencyObject) dropDownPopup.Child, Mouse.Captured as DependencyObject))
          return;
        PopupService.RaiseDismissPopupEvent(sender, DismissPopupMode.MouseNotOver);
      }
      else if (PopupService.IsAncestorOf((DependencyObject) dropDownPopup.Child, e.OriginalSource as DependencyObject))
      {
        if (Mouse.Captured != null)
          return;
        Mouse.Capture(sender as IInputElement, CaptureMode.SubTree);
        e.Handled = true;
      }
      else
        PopupService.RaiseDismissPopupEvent(sender, DismissPopupMode.MouseNotOver);
    }

    public static bool IsAncestorOf(DependencyObject parent, DependencyObject element)
    {
      for (; element != null; element = VisualTreeHelper.GetParent(element) ?? LogicalTreeHelper.GetParent(element))
      {
        if (element == parent)
          return true;
      }
      return false;
    }

    public static void OnDismissPopup(object sender, DismissPopupEventArgs e)
    {
      if (!(sender is IDropDownControl dropDownControl))
        return;
      if (e.DismissMode == DismissPopupMode.Always)
      {
        if (Mouse.Captured == dropDownControl)
          Mouse.Capture((IInputElement) null);
        dropDownControl.IsDropDownOpen = false;
      }
      else if (dropDownControl.IsDropDownOpen && !PopupService.IsMousePhysicallyOver(dropDownControl.DropDownPopup.Child))
      {
        if (Mouse.Captured == dropDownControl)
          Mouse.Capture((IInputElement) null);
        dropDownControl.IsDropDownOpen = false;
      }
      else
      {
        if (dropDownControl.IsDropDownOpen && Mouse.Captured != dropDownControl)
          Mouse.Capture(sender as IInputElement, CaptureMode.SubTree);
        if (!dropDownControl.IsDropDownOpen)
          return;
        e.Handled = true;
      }
    }

    public static bool IsMousePhysicallyOver(UIElement element)
    {
      if (element == null)
        return false;
      Point position = Mouse.GetPosition((IInputElement) element);
      return position.X >= 0.0 && position.Y >= 0.0 && position.X <= element.RenderSize.Width && position.Y <= element.RenderSize.Height;
    }

    public static void OnContextMenuOpened(object sender, ContextMenuEventArgs e)
    {
      if (!(sender is IDropDownControl dropDownControl))
        return;
      dropDownControl.IsContextMenuOpened = true;
    }

    public static void OnContextMenuClosed(object sender, ContextMenuEventArgs e)
    {
      if (!(sender is IDropDownControl sender1))
        return;
      sender1.IsContextMenuOpened = false;
      PopupService.RaiseDismissPopupEvent((object) sender1, DismissPopupMode.MouseNotOver);
    }
  }
}
