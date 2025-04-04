// Decompiled with JetBrains decompiler
// Type: Styles.Resources.AppResources
// Assembly: Styles, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ABC9E615-D09A-48E5-A13F-BC53DD762FA1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Styles.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

#nullable disable
namespace Styles.Resources
{
  public partial class AppResources : ResourceDictionary, IComponentConnector
  {
    private bool _contentLoaded;

    public AppResources() => this.InitializeComponent();

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Styles;component/resources/appresources.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
