// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Behaviours.TiltBehavior
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using MahApps.Metro.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

#nullable disable
namespace MahApps.Metro.Behaviours
{
  public class TiltBehavior : Behavior<FrameworkElement>
  {
    public static readonly DependencyProperty KeepDraggingProperty = DependencyProperty.Register(nameof (KeepDragging), typeof (bool), typeof (TiltBehavior), new PropertyMetadata((object) true));
    public static readonly DependencyProperty TiltFactorProperty = DependencyProperty.Register(nameof (TiltFactor), typeof (int), typeof (TiltBehavior), new PropertyMetadata((object) 20));
    private bool isPressed;
    private Thickness originalMargin;
    private Panel originalPanel;
    private Size originalSize;
    private FrameworkElement attachedElement;
    private Point current = new Point(-99.0, -99.0);
    private int times = -1;

    public bool KeepDragging
    {
      get => (bool) this.GetValue(TiltBehavior.KeepDraggingProperty);
      set => this.SetValue(TiltBehavior.KeepDraggingProperty, (object) value);
    }

    public int TiltFactor
    {
      get => (int) this.GetValue(TiltBehavior.TiltFactorProperty);
      set => this.SetValue(TiltBehavior.TiltFactorProperty, (object) value);
    }

    public Planerator RotatorParent { get; set; }

    protected override void OnAttached()
    {
      this.attachedElement = this.AssociatedObject;
      if (this.attachedElement is ListBox)
        return;
      Panel attachedElementPanel = this.attachedElement as Panel;
      if (attachedElementPanel != null)
      {
        attachedElementPanel.Loaded += (RoutedEventHandler) ((sl, el) => attachedElementPanel.Children.Cast<UIElement>().ToList<UIElement>().ForEach((Action<UIElement>) (element => Interaction.GetBehaviors((DependencyObject) element).Add((Behavior) new TiltBehavior()
        {
          KeepDragging = this.KeepDragging,
          TiltFactor = this.TiltFactor
        }))));
      }
      else
      {
        if (!(this.attachedElement.Parent is Panel panel))
          panel = TiltBehavior.GetParentPanel((DependencyObject) this.attachedElement);
        this.originalPanel = panel;
        this.originalMargin = this.attachedElement.Margin;
        this.originalSize = new Size(this.attachedElement.Width, this.attachedElement.Height);
        double left = Canvas.GetLeft((UIElement) this.attachedElement);
        double right = Canvas.GetRight((UIElement) this.attachedElement);
        double top = Canvas.GetTop((UIElement) this.attachedElement);
        double bottom = Canvas.GetBottom((UIElement) this.attachedElement);
        int zindex = Panel.GetZIndex((UIElement) this.attachedElement);
        VerticalAlignment verticalAlignment = this.attachedElement.VerticalAlignment;
        HorizontalAlignment horizontalAlignment = this.attachedElement.HorizontalAlignment;
        Planerator planerator = new Planerator();
        planerator.Margin = this.originalMargin;
        planerator.Width = this.originalSize.Width;
        planerator.Height = this.originalSize.Height;
        planerator.VerticalAlignment = verticalAlignment;
        planerator.HorizontalAlignment = horizontalAlignment;
        this.RotatorParent = planerator;
        this.RotatorParent.SetValue(Canvas.LeftProperty, (object) left);
        this.RotatorParent.SetValue(Canvas.RightProperty, (object) right);
        this.RotatorParent.SetValue(Canvas.TopProperty, (object) top);
        this.RotatorParent.SetValue(Canvas.BottomProperty, (object) bottom);
        this.RotatorParent.SetValue(Panel.ZIndexProperty, (object) zindex);
        this.originalPanel.Children.Remove((UIElement) this.attachedElement);
        this.attachedElement.Margin = new Thickness();
        this.attachedElement.Width = double.NaN;
        this.attachedElement.Height = double.NaN;
        this.originalPanel.Children.Add((UIElement) this.RotatorParent);
        this.RotatorParent.Child = this.attachedElement;
        CompositionTarget.Rendering += new EventHandler(this.CompositionTargetRendering);
      }
    }

    protected override void OnDetaching()
    {
      base.OnDetaching();
      CompositionTarget.Rendering -= new EventHandler(this.CompositionTargetRendering);
    }

    private void CompositionTargetRendering(object sender, EventArgs e)
    {
      if (this.KeepDragging)
      {
        this.current = Mouse.GetPosition((IInputElement) this.RotatorParent.Child);
        if (Mouse.LeftButton == MouseButtonState.Pressed)
        {
          if (this.current.X <= 0.0 || this.current.X >= this.attachedElement.ActualWidth || this.current.Y <= 0.0 || this.current.Y >= this.attachedElement.ActualHeight)
            return;
          this.RotatorParent.RotationY = (double) (-1 * this.TiltFactor) + this.current.X * 2.0 * (double) this.TiltFactor / this.attachedElement.ActualWidth;
          this.RotatorParent.RotationX = (double) (-1 * this.TiltFactor) + this.current.Y * 2.0 * (double) this.TiltFactor / this.attachedElement.ActualHeight;
        }
        else
        {
          this.RotatorParent.RotationY = this.RotatorParent.RotationY - 5.0 < 0.0 ? 0.0 : this.RotatorParent.RotationY - 5.0;
          this.RotatorParent.RotationX = this.RotatorParent.RotationX - 5.0 < 0.0 ? 0.0 : this.RotatorParent.RotationX - 5.0;
        }
      }
      else if (Mouse.LeftButton == MouseButtonState.Pressed)
      {
        if (!this.isPressed)
        {
          this.current = Mouse.GetPosition((IInputElement) this.RotatorParent.Child);
          if (this.current.X > 0.0 && this.current.X < this.attachedElement.ActualWidth && this.current.Y > 0.0 && this.current.Y < this.attachedElement.ActualHeight)
          {
            this.RotatorParent.RotationY = (double) (-1 * this.TiltFactor) + this.current.X * 2.0 * (double) this.TiltFactor / this.attachedElement.ActualWidth;
            this.RotatorParent.RotationX = (double) (-1 * this.TiltFactor) + this.current.Y * 2.0 * (double) this.TiltFactor / this.attachedElement.ActualHeight;
          }
          this.isPressed = true;
        }
        if (this.isPressed && this.times == 7)
        {
          this.RotatorParent.RotationY = this.RotatorParent.RotationY - 5.0 < 0.0 ? 0.0 : this.RotatorParent.RotationY - 5.0;
          this.RotatorParent.RotationX = this.RotatorParent.RotationX - 5.0 < 0.0 ? 0.0 : this.RotatorParent.RotationX - 5.0;
        }
        else
        {
          if (!this.isPressed || this.times >= 7)
            return;
          ++this.times;
        }
      }
      else
      {
        this.isPressed = false;
        this.times = -1;
        this.RotatorParent.RotationY = this.RotatorParent.RotationY - 5.0 < 0.0 ? 0.0 : this.RotatorParent.RotationY - 5.0;
        this.RotatorParent.RotationX = this.RotatorParent.RotationX - 5.0 < 0.0 ? 0.0 : this.RotatorParent.RotationX - 5.0;
      }
    }

    private static Panel GetParentPanel(DependencyObject element)
    {
      DependencyObject parent = VisualTreeHelper.GetParent(element);
      if (parent is Panel parentPanel)
        return parentPanel;
      return parent != null ? TiltBehavior.GetParentPanel(parent) : (Panel) null;
    }
  }
}
