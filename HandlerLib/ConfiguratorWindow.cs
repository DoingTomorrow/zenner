// Decompiled with JetBrains decompiler
// Type: HandlerLib.ConfiguratorWindow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

#nullable disable
namespace HandlerLib
{
  public class ConfiguratorWindow : Window, IComponentConnector
  {
    internal ConfiguratorControl ConfiguratorCtrl;
    private bool _contentLoaded;

    public ConfiguratorWindow() => this.InitializeComponent();

    public static void ShowDialog(Window owner, IHandler handler)
    {
      ConfiguratorWindow configuratorWindow = new ConfiguratorWindow();
      configuratorWindow.Owner = owner;
      ConfiguratorWindow owner1 = configuratorWindow;
      try
      {
        owner1.ConfiguratorCtrl.InitializeComponent(handler);
        if (owner1.ShowDialog().Value)
          ;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((Window) owner1, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
      }
    }

    public static void ShowDialog(IHandler handler)
    {
      ConfiguratorWindow owner = new ConfiguratorWindow();
      try
      {
        owner.ConfiguratorCtrl.InitializeComponent(handler);
        if (owner.ShowDialog().Value)
          ;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((Window) owner, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/configuratorwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.ConfiguratorCtrl = (ConfiguratorControl) target;
      else
        this._contentLoaded = true;
    }
  }
}
