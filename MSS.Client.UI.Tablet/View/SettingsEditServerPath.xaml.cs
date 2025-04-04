// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Settings.EditServerPath
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
using System.Windows.Markup;
using WpfKb.Controls;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Settings
{
  public partial class EditServerPath : KeyboardMetroWindow, IComponentConnector
  {
    internal TabletButton KeyboardButton;
    internal TextBox NewServerUrlTextBox;
    internal Button TestButton;
    internal Button OkButton;
    internal Button CancelButton;
    internal TouchScreenKeyboardUserControl Keyboard;
    private bool _contentLoaded;

    public EditServerPath()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.Loaded += new RoutedEventHandler(this.MetroWindow_Loaded);
    }

    ~EditServerPath()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.Loaded -= new RoutedEventHandler(this.MetroWindow_Loaded);
    }

    private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
    {
      this.RegisterKeyboardEvents(this.Keyboard);
      // ISSUE: reference to a compiler-generated field
      if (EditServerPath.\u003C\u003Eo__2.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EditServerPath.\u003C\u003Eo__2.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, Visibility, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "IsKeyboardControlVisible", typeof (EditServerPath), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = EditServerPath.\u003C\u003Eo__2.\u003C\u003Ep__0.Target((CallSite) EditServerPath.\u003C\u003Eo__2.\u003C\u003Ep__0, this.DataContext, this.Keyboard.Visibility);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/settings/editserverpath.xaml", UriKind.Relative));
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
          this.KeyboardButton = (TabletButton) target;
          break;
        case 2:
          this.NewServerUrlTextBox = (TextBox) target;
          break;
        case 3:
          this.TestButton = (Button) target;
          break;
        case 4:
          this.OkButton = (Button) target;
          break;
        case 5:
          this.CancelButton = (Button) target;
          break;
        case 6:
          this.Keyboard = (TouchScreenKeyboardUserControl) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
