// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Meters.TakePhotoDialog
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using Microsoft.CSharp.RuntimeBinder;
using MSS.Client.UI.Common;
using MSS.Client.UI.Tablet.CustomControls;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Meters
{
  public partial class TakePhotoDialog : KeyboardMetroWindow, IComponentConnector
  {
    internal Image frameHolder;
    internal Image RezultImage;
    internal ContentPresenter ContentPrt;
    private bool _contentLoaded;

    public TakePhotoDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    ~TakePhotoDialog()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    private void PhotoViewWindowLoaded(object sender, RoutedEventArgs e)
    {
    }

    private void KeyboardMetroWindow_Closed(object sender, EventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      if (TakePhotoDialog.\u003C\u003Eo__3.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TakePhotoDialog.\u003C\u003Eo__3.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "DettachWebCam", (IEnumerable<Type>) null, typeof (TakePhotoDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      TakePhotoDialog.\u003C\u003Eo__3.\u003C\u003Ep__0.Target((CallSite) TakePhotoDialog.\u003C\u003Eo__3.\u003C\u003Ep__0, this.DataContext);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/meters/takephotodialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler)
    {
      return Delegate.CreateDelegate(delegateType, (object) this, handler);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.frameHolder = (Image) target;
          break;
        case 2:
          this.RezultImage = (Image) target;
          break;
        case 3:
          this.ContentPrt = (ContentPresenter) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
