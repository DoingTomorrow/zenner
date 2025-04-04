// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.NewsAndUpdates.UpdatesAndNews
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
using Telerik.Windows.Controls;
using Telerik.Windows.Documents.FormatProviders.Html;

#nullable disable
namespace MSS.Client.UI.Tablet.View.NewsAndUpdates
{
  public partial class UpdatesAndNews : ResizableMetroWindow, IComponentConnector
  {
    internal RadRichTextBox richTextBox;
    internal HtmlDataProvider HtmlProvider;
    internal Button LeftButton;
    internal Button RightButton;
    private bool _contentLoaded;

    public UpdatesAndNews()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    ~UpdatesAndNews()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/newsandupdates/updatesandnews.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.richTextBox = (RadRichTextBox) target;
          break;
        case 2:
          this.HtmlProvider = (HtmlDataProvider) target;
          break;
        case 3:
          this.LeftButton = (Button) target;
          break;
        case 4:
          this.RightButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
