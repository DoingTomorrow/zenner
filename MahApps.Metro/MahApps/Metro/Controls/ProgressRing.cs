﻿// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.ProgressRing
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace MahApps.Metro.Controls
{
  [TemplateVisualState(Name = "Large", GroupName = "SizeStates")]
  [TemplateVisualState(Name = "Small", GroupName = "SizeStates")]
  [TemplateVisualState(Name = "Inactive", GroupName = "ActiveStates")]
  [TemplateVisualState(Name = "Active", GroupName = "ActiveStates")]
  public class ProgressRing : Control
  {
    public static readonly DependencyProperty BindableWidthProperty = DependencyProperty.Register(nameof (BindableWidth), typeof (double), typeof (ProgressRing), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ProgressRing.BindableWidthCallback)));
    public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(nameof (IsActive), typeof (bool), typeof (ProgressRing), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(ProgressRing.IsActiveChanged)));
    public static readonly DependencyProperty IsLargeProperty = DependencyProperty.Register(nameof (IsLarge), typeof (bool), typeof (ProgressRing), new PropertyMetadata((object) true, new PropertyChangedCallback(ProgressRing.IsLargeChangedCallback)));
    public static readonly DependencyProperty MaxSideLengthProperty = DependencyProperty.Register(nameof (MaxSideLength), typeof (double), typeof (ProgressRing), new PropertyMetadata((object) 0.0));
    public static readonly DependencyProperty EllipseDiameterProperty = DependencyProperty.Register(nameof (EllipseDiameter), typeof (double), typeof (ProgressRing), new PropertyMetadata((object) 0.0));
    public static readonly DependencyProperty EllipseOffsetProperty = DependencyProperty.Register(nameof (EllipseOffset), typeof (Thickness), typeof (ProgressRing), new PropertyMetadata((object) new Thickness()));
    private List<Action> _deferredActions = new List<Action>();

    static ProgressRing()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ProgressRing), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ProgressRing)));
      UIElement.VisibilityProperty.OverrideMetadata(typeof (ProgressRing), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) ((ringObject, e) =>
      {
        if (e.NewValue == e.OldValue)
          return;
        ProgressRing progressRing = (ProgressRing) ringObject;
        if ((Visibility) e.NewValue != Visibility.Visible)
          progressRing.SetCurrentValue(ProgressRing.IsActiveProperty, (object) false);
        else
          progressRing.IsActive = true;
      })));
    }

    public ProgressRing() => this.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);

    public double MaxSideLength
    {
      get => (double) this.GetValue(ProgressRing.MaxSideLengthProperty);
      private set => this.SetValue(ProgressRing.MaxSideLengthProperty, (object) value);
    }

    public double EllipseDiameter
    {
      get => (double) this.GetValue(ProgressRing.EllipseDiameterProperty);
      private set => this.SetValue(ProgressRing.EllipseDiameterProperty, (object) value);
    }

    public Thickness EllipseOffset
    {
      get => (Thickness) this.GetValue(ProgressRing.EllipseOffsetProperty);
      private set => this.SetValue(ProgressRing.EllipseOffsetProperty, (object) value);
    }

    public double BindableWidth
    {
      get => (double) this.GetValue(ProgressRing.BindableWidthProperty);
      private set => this.SetValue(ProgressRing.BindableWidthProperty, (object) value);
    }

    public bool IsActive
    {
      get => (bool) this.GetValue(ProgressRing.IsActiveProperty);
      set => this.SetValue(ProgressRing.IsActiveProperty, (object) value);
    }

    public bool IsLarge
    {
      get => (bool) this.GetValue(ProgressRing.IsLargeProperty);
      set => this.SetValue(ProgressRing.IsLargeProperty, (object) value);
    }

    private static void BindableWidthCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
      ProgressRing ring = dependencyObject as ProgressRing;
      if (ring == null)
        return;
      Action action = (Action) (() =>
      {
        ring.SetEllipseDiameter((double) dependencyPropertyChangedEventArgs.NewValue);
        ring.SetEllipseOffset((double) dependencyPropertyChangedEventArgs.NewValue);
        ring.SetMaxSideLength((double) dependencyPropertyChangedEventArgs.NewValue);
      });
      if (ring._deferredActions != null)
        ring._deferredActions.Add(action);
      else
        action();
    }

    private void SetMaxSideLength(double width)
    {
      this.MaxSideLength = width <= 20.0 ? 20.0 : width;
    }

    private void SetEllipseDiameter(double width) => this.EllipseDiameter = width / 8.0;

    private void SetEllipseOffset(double width)
    {
      this.EllipseOffset = new Thickness(0.0, width / 2.0, 0.0, 0.0);
    }

    private static void IsLargeChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
      if (!(dependencyObject is ProgressRing progressRing))
        return;
      progressRing.UpdateLargeState();
    }

    private void UpdateLargeState()
    {
      Action action = !this.IsLarge ? (Action) (() => VisualStateManager.GoToState((FrameworkElement) this, "Small", true)) : (Action) (() => VisualStateManager.GoToState((FrameworkElement) this, "Large", true));
      if (this._deferredActions != null)
        this._deferredActions.Add(action);
      else
        action();
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
    {
      this.BindableWidth = this.ActualWidth;
    }

    private static void IsActiveChanged(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
      if (!(dependencyObject is ProgressRing progressRing))
        return;
      progressRing.UpdateActiveState();
    }

    private void UpdateActiveState()
    {
      Action action = !this.IsActive ? (Action) (() => VisualStateManager.GoToState((FrameworkElement) this, "Inactive", true)) : (Action) (() => VisualStateManager.GoToState((FrameworkElement) this, "Active", true));
      if (this._deferredActions != null)
        this._deferredActions.Add(action);
      else
        action();
    }

    public override void OnApplyTemplate()
    {
      this.UpdateLargeState();
      this.UpdateActiveState();
      base.OnApplyTemplate();
      if (this._deferredActions != null)
      {
        foreach (Action deferredAction in this._deferredActions)
          deferredAction();
      }
      this._deferredActions = (List<Action>) null;
    }
  }
}
