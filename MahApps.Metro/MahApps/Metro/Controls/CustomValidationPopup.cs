// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.CustomValidationPopup
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using MahApps.Metro.Native;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class CustomValidationPopup : Popup
  {
    public static readonly DependencyProperty CloseOnMouseLeftButtonDownProperty = DependencyProperty.Register(nameof (CloseOnMouseLeftButtonDown), typeof (bool), typeof (CustomValidationPopup), new PropertyMetadata((object) true));
    private Window hostWindow;
    private bool? appliedTopMost;
    private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
    private static readonly IntPtr HWND_TOP = new IntPtr(0);
    private static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

    public CustomValidationPopup()
    {
      this.Loaded += new RoutedEventHandler(this.CustomValidationPopup_Loaded);
      this.Opened += new EventHandler(this.CustomValidationPopup_Opened);
    }

    public bool CloseOnMouseLeftButtonDown
    {
      get => (bool) this.GetValue(CustomValidationPopup.CloseOnMouseLeftButtonDownProperty);
      set
      {
        this.SetValue(CustomValidationPopup.CloseOnMouseLeftButtonDownProperty, (object) value);
      }
    }

    protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      if (!this.CloseOnMouseLeftButtonDown)
        return;
      this.IsOpen = false;
    }

    private void CustomValidationPopup_Loaded(object sender, RoutedEventArgs e)
    {
      if (!(this.PlacementTarget is FrameworkElement placementTarget))
        return;
      this.hostWindow = Window.GetWindow((DependencyObject) placementTarget);
      if (this.hostWindow == null)
        return;
      this.hostWindow.LocationChanged -= new EventHandler(this.hostWindow_SizeOrLocationChanged);
      this.hostWindow.LocationChanged += new EventHandler(this.hostWindow_SizeOrLocationChanged);
      this.hostWindow.SizeChanged -= new SizeChangedEventHandler(this.hostWindow_SizeOrLocationChanged);
      this.hostWindow.SizeChanged += new SizeChangedEventHandler(this.hostWindow_SizeOrLocationChanged);
      placementTarget.SizeChanged -= new SizeChangedEventHandler(this.hostWindow_SizeOrLocationChanged);
      placementTarget.SizeChanged += new SizeChangedEventHandler(this.hostWindow_SizeOrLocationChanged);
      this.hostWindow.StateChanged -= new EventHandler(this.hostWindow_StateChanged);
      this.hostWindow.StateChanged += new EventHandler(this.hostWindow_StateChanged);
      this.hostWindow.Activated -= new EventHandler(this.hostWindow_Activated);
      this.hostWindow.Activated += new EventHandler(this.hostWindow_Activated);
      this.hostWindow.Deactivated -= new EventHandler(this.hostWindow_Deactivated);
      this.hostWindow.Deactivated += new EventHandler(this.hostWindow_Deactivated);
      this.Unloaded -= new RoutedEventHandler(this.CustomValidationPopup_Unloaded);
      this.Unloaded += new RoutedEventHandler(this.CustomValidationPopup_Unloaded);
    }

    private void CustomValidationPopup_Opened(object sender, EventArgs e)
    {
      this.SetTopmostState(true);
    }

    private void hostWindow_Activated(object sender, EventArgs e) => this.SetTopmostState(true);

    private void hostWindow_Deactivated(object sender, EventArgs e) => this.SetTopmostState(false);

    private void CustomValidationPopup_Unloaded(object sender, RoutedEventArgs e)
    {
      if (this.PlacementTarget is FrameworkElement placementTarget)
        placementTarget.SizeChanged -= new SizeChangedEventHandler(this.hostWindow_SizeOrLocationChanged);
      if (this.hostWindow != null)
      {
        this.hostWindow.LocationChanged -= new EventHandler(this.hostWindow_SizeOrLocationChanged);
        this.hostWindow.SizeChanged -= new SizeChangedEventHandler(this.hostWindow_SizeOrLocationChanged);
        this.hostWindow.StateChanged -= new EventHandler(this.hostWindow_StateChanged);
        this.hostWindow.Activated -= new EventHandler(this.hostWindow_Activated);
        this.hostWindow.Deactivated -= new EventHandler(this.hostWindow_Deactivated);
      }
      this.Unloaded -= new RoutedEventHandler(this.CustomValidationPopup_Unloaded);
      this.Opened -= new EventHandler(this.CustomValidationPopup_Opened);
      this.hostWindow = (Window) null;
    }

    private void hostWindow_StateChanged(object sender, EventArgs e)
    {
      if (this.hostWindow == null || this.hostWindow.WindowState == WindowState.Minimized)
        return;
      AdornedElementPlaceholder dataContext = this.PlacementTarget is FrameworkElement placementTarget ? placementTarget.DataContext as AdornedElementPlaceholder : (AdornedElementPlaceholder) null;
      if (dataContext == null || dataContext.AdornedElement == null)
        return;
      this.PopupAnimation = PopupAnimation.None;
      this.IsOpen = false;
      object obj = dataContext.AdornedElement.GetValue(Validation.ErrorTemplateProperty);
      dataContext.AdornedElement.SetValue(Validation.ErrorTemplateProperty, (object) null);
      dataContext.AdornedElement.SetValue(Validation.ErrorTemplateProperty, obj);
    }

    private void hostWindow_SizeOrLocationChanged(object sender, EventArgs e)
    {
      this.HorizontalOffset = this.HorizontalOffset++;
    }

    private void SetTopmostState(bool isTop)
    {
      if (this.appliedTopMost.HasValue)
      {
        bool? appliedTopMost = this.appliedTopMost;
        bool flag = isTop;
        if ((appliedTopMost.GetValueOrDefault() == flag ? (appliedTopMost.HasValue ? 1 : 0) : 0) != 0)
          return;
      }
      if (this.Child == null || !(PresentationSource.FromVisual((Visual) this.Child) is HwndSource hwndSource))
        return;
      IntPtr handle = hwndSource.Handle;
      RECT lpRect;
      if (!UnsafeNativeMethods.GetWindowRect(handle, out lpRect))
        return;
      int left = lpRect.left;
      int top = lpRect.top;
      int width = lpRect.Width;
      int height = lpRect.Height;
      if (isTop)
      {
        UnsafeNativeMethods.SetWindowPos(handle, CustomValidationPopup.HWND_TOPMOST, left, top, width, height, 1563U);
      }
      else
      {
        UnsafeNativeMethods.SetWindowPos(handle, CustomValidationPopup.HWND_BOTTOM, left, top, width, height, 1563U);
        UnsafeNativeMethods.SetWindowPos(handle, CustomValidationPopup.HWND_TOP, left, top, width, height, 1563U);
        UnsafeNativeMethods.SetWindowPos(handle, CustomValidationPopup.HWND_NOTOPMOST, left, top, width, height, 1563U);
      }
      this.appliedTopMost = new bool?(isTop);
    }
  }
}
