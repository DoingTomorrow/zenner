// Decompiled with JetBrains decompiler
// Type: Fluent.GalleryItem
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

#nullable disable
namespace Fluent
{
  public class GalleryItem : ListBoxItem, IKeyTipedControl
  {
    private static readonly DependencyPropertyKey IsPressedPropertyKey = DependencyProperty.RegisterReadOnly(nameof (IsPressed), typeof (bool), typeof (GalleryItem), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty IsPressedProperty = GalleryItem.IsPressedPropertyKey.DependencyProperty;
    public static readonly DependencyProperty GroupProperty = DependencyProperty.Register(nameof (Group), typeof (string), typeof (GalleryItem), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    private bool currentCanExecute = true;
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof (CommandParameter), typeof (object), typeof (GalleryItem), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof (Command), typeof (ICommand), typeof (GalleryItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(GalleryItem.OnCommandChanged)));
    public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(nameof (CommandTarget), typeof (IInputElement), typeof (GalleryItem), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
    [SuppressMessage("Microsoft.Performance", "CA1823")]
    private EventHandler canExecuteChangedHandler;
    public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (GalleryItem));

    public bool IsPressed
    {
      get => (bool) this.GetValue(GalleryItem.IsPressedProperty);
      private set => this.SetValue(GalleryItem.IsPressedPropertyKey, (object) value);
    }

    public string Group
    {
      get => (string) this.GetValue(GalleryItem.GroupProperty);
      set => this.SetValue(GalleryItem.GroupProperty, (object) value);
    }

    [Category("Action")]
    [Localizability(LocalizationCategory.NeverLocalize)]
    [Bindable(true)]
    public ICommand Command
    {
      get => (ICommand) this.GetValue(GalleryItem.CommandProperty);
      set => this.SetValue(GalleryItem.CommandProperty, (object) value);
    }

    [Category("Action")]
    [Bindable(true)]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public object CommandParameter
    {
      get => this.GetValue(GalleryItem.CommandParameterProperty);
      set => this.SetValue(GalleryItem.CommandParameterProperty, value);
    }

    [Bindable(true)]
    [Category("Action")]
    public IInputElement CommandTarget
    {
      get => (IInputElement) this.GetValue(GalleryItem.CommandTargetProperty);
      set => this.SetValue(GalleryItem.CommandTargetProperty, (object) value);
    }

    private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      GalleryItem galleryItem = d as GalleryItem;
      EventHandler eventHandler1 = new EventHandler(galleryItem.OnCommandCanExecuteChanged);
      if (e.OldValue != null)
        (e.OldValue as ICommand).CanExecuteChanged -= eventHandler1;
      if (e.NewValue != null)
      {
        EventHandler eventHandler2 = new EventHandler(galleryItem.OnCommandCanExecuteChanged);
        galleryItem.canExecuteChangedHandler = eventHandler2;
        (e.NewValue as ICommand).CanExecuteChanged += eventHandler2;
      }
      galleryItem.UpdateCanExecute();
    }

    private void OnCommandCanExecuteChanged(object sender, EventArgs e) => this.UpdateCanExecute();

    private void UpdateCanExecute()
    {
      bool flag = this.Command != null && this.CanExecuteCommand();
      if (this.currentCanExecute == flag)
        return;
      this.currentCanExecute = flag;
      this.CoerceValue(UIElement.IsEnabledProperty);
    }

    protected void ExecuteCommand()
    {
      ICommand command = this.Command;
      if (command == null)
        return;
      object commandParameter = this.CommandParameter;
      if (command is RoutedCommand routedCommand)
      {
        if (!routedCommand.CanExecute(commandParameter, this.CommandTarget))
          return;
        routedCommand.Execute(commandParameter, this.CommandTarget);
      }
      else
      {
        if (!command.CanExecute(commandParameter))
          return;
        command.Execute(commandParameter);
      }
    }

    protected bool CanExecuteCommand()
    {
      ICommand command = this.Command;
      if (command == null)
        return false;
      object commandParameter = this.CommandParameter;
      return !(command is RoutedCommand routedCommand) ? command.CanExecute(commandParameter) : routedCommand.CanExecute(commandParameter, this.CommandTarget);
    }

    protected override bool IsEnabledCore
    {
      get
      {
        if (!base.IsEnabledCore)
          return false;
        return this.currentCanExecute || this.Command == null;
      }
    }

    [Category("Behavior")]
    public event RoutedEventHandler Click
    {
      add => this.AddHandler(GalleryItem.ClickEvent, (Delegate) value);
      remove => this.RemoveHandler(GalleryItem.ClickEvent, (Delegate) value);
    }

    [SuppressMessage("Microsoft.Design", "CA1030")]
    public void RaiseClick()
    {
      this.RaiseEvent(new RoutedEventArgs(GalleryItem.ClickEvent, (object) this));
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static GalleryItem()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (GalleryItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (GalleryItem)));
      ListBoxItem.IsSelectedProperty.AddOwner(typeof (GalleryItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(GalleryItem.OnIsSelectedPropertyChanged)));
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (GalleryItem), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(GalleryItem.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (GalleryItem));
      return basevalue;
    }

    private static void OnIsSelectedPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(bool) e.NewValue)
        return;
      ((FrameworkElement) d).BringIntoView();
      Selector selector = ItemsControl.ItemsControlFromItemContainer(d) as Selector;
      selector.SelectedItem = selector.ItemContainerGenerator.ItemFromContainer(d);
    }

    public GalleryItem() => this.Click += new RoutedEventHandler(this.OnClick);

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      this.IsPressed = true;
      Mouse.Capture((IInputElement) this);
      e.Handled = true;
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
      this.IsPressed = false;
      if (Mouse.Captured == this)
        Mouse.Capture((IInputElement) null);
      Point position = Mouse.PrimaryDevice.GetPosition((IInputElement) this);
      if (position.X >= 0.0 && position.X <= this.ActualWidth && position.Y >= 0.0 && position.Y <= this.ActualHeight && e.ClickCount == 1)
      {
        this.RaiseClick();
        e.Handled = true;
      }
      e.Handled = true;
    }

    protected virtual void OnClick(object sender, RoutedEventArgs e)
    {
      this.ExecuteCommand();
      this.IsSelected = true;
      PopupService.RaiseDismissPopupEvent(sender, DismissPopupMode.Always);
      e.Handled = true;
    }

    public void OnKeyTipPressed() => this.RaiseClick();
  }
}
