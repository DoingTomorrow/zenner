// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Orders.MinomatIdPopupView
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using MSS.Client.UI.Common;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Orders
{
  public partial class MinomatIdPopupView : ResizableMetroWindow, IComponentConnector
  {
    internal Button NewIdButton;
    private bool _contentLoaded;

    public MinomatIdPopupView()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    ~MinomatIdPopupView()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    private void NewIdButton_OnTouchDown(object sender, TouchEventArgs e)
    {
      this.DialogResult = new bool?(true);
    }

    private void NewIdButton_OnMouseDown(object sender, RoutedEventArgs routedEventArgs)
    {
      this.DialogResult = new bool?(true);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/orders/minomatidpopupview.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
      {
        this.NewIdButton = (Button) target;
        this.NewIdButton.Click += new RoutedEventHandler(this.NewIdButton_OnMouseDown);
        this.NewIdButton.TouchDown += new EventHandler<TouchEventArgs>(this.NewIdButton_OnTouchDown);
      }
      else
        this._contentLoaded = true;
    }
  }
}
