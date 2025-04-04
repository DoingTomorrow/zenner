// Decompiled with JetBrains decompiler
// Type: StartupLib.GmmCorporateControl
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace StartupLib
{
  public partial class GmmCorporateControl : UserControl, IComponentConnector
  {
    internal Image image0;
    internal Image image1;
    internal Image image2;
    private bool _contentLoaded;

    public GmmCorporateControl() => this.InitializeComponent();

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      this.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/StartupLib;component/gmmcorporatecontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.UserControl_Loaded);
          break;
        case 2:
          this.image0 = (Image) target;
          break;
        case 3:
          this.image1 = (Image) target;
          break;
        case 4:
          this.image2 = (Image) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
