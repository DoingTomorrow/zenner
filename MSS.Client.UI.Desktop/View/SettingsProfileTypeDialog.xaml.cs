// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.Settings.ProfileTypeDialog
// Assembly: MSS.Client.UI.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B34A4718-63B5-4C6C-93C2-0A28BCAE0F44
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Desktop.dll

using Microsoft.CSharp.RuntimeBinder;
using MSS.Business.Events;
using MSS.Business.Modules.Configuration;
using MSS.Client.UI.Common;
using MSS.Client.UI.Common.Utils;
using Styles.Controls;
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
namespace MSS.Client.UI.Desktop.View.Settings
{
  public partial class ProfileTypeDialog : ResizableMetroWindow, IComponentConnector
  {
    internal StackPanel ProfileTypeConfigStackPanelDynamic;
    private bool _contentLoaded;

    public ProfileTypeDialog()
    {
      this.InitializeComponent();
      this.Loaded += new RoutedEventHandler(this.OnLoaded);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged += new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated += new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
      EventPublisher.Register<ProfileTypeChangedEvent>(new Action<ProfileTypeChangedEvent>(this.RefreshView));
    }

    ~ProfileTypeDialog()
    {
      this.Loaded -= new RoutedEventHandler(this.OnLoaded);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged -= new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated -= new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
    }

    private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
      // ISSUE: reference to a compiler-generated field
      if (ProfileTypeDialog.\u003C\u003Eo__2.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ProfileTypeDialog.\u003C\u003Eo__2.\u003C\u003Ep__1 = CallSite<Action<CallSite, ProfileTypeDialog, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "BuildGrid", (IEnumerable<Type>) null, typeof (ProfileTypeDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, ProfileTypeDialog, object> target = ProfileTypeDialog.\u003C\u003Eo__2.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, ProfileTypeDialog, object>> p1 = ProfileTypeDialog.\u003C\u003Eo__2.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (ProfileTypeDialog.\u003C\u003Eo__2.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ProfileTypeDialog.\u003C\u003Eo__2.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ProfileTypesDynamicGridTag", typeof (ProfileTypeDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = ProfileTypeDialog.\u003C\u003Eo__2.\u003C\u003Ep__0.Target((CallSite) ProfileTypeDialog.\u003C\u003Eo__2.\u003C\u003Ep__0, this.DataContext);
      target((CallSite) p1, this, obj);
    }

    private void RefreshView(ProfileTypeChangedEvent obj) => this.BuildGrid(obj.ProfileTypeValues);

    public void BuildGrid(List<Config> profileTypeConfigParams)
    {
      this.ProfileTypeConfigStackPanelDynamic.Tag = (object) null;
      this.ProfileTypeConfigStackPanelDynamic.Children.Clear();
      if (profileTypeConfigParams == null || profileTypeConfigParams.Count <= 0)
        return;
      this.ProfileTypeConfigStackPanelDynamic.Children.Clear();
      GridControl gridControl = new GridControl();
      gridControl.Name = "ProfileTypeConfigs";
      GridControl dynamicGrid1 = gridControl;
      int dynamicGrid2 = (int) DynamicGridControl.CreateDynamicGrid((IList<Config>) profileTypeConfigParams, out dynamicGrid1, gridWidth: 680.0, firstColumnPercentage: 30.0);
      this.ProfileTypeConfigStackPanelDynamic.Children.Add((UIElement) dynamicGrid1);
      this.ProfileTypeConfigStackPanelDynamic.Tag = (object) profileTypeConfigParams;
      // ISSUE: reference to a compiler-generated field
      if (ProfileTypeDialog.\u003C\u003Eo__4.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ProfileTypeDialog.\u003C\u003Eo__4.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, List<Config>, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ProfileTypesDynamicGridTag", typeof (ProfileTypeDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = ProfileTypeDialog.\u003C\u003Eo__4.\u003C\u003Ep__0.Target((CallSite) ProfileTypeDialog.\u003C\u003Eo__4.\u003C\u003Ep__0, this.DataContext, profileTypeConfigParams);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/settings/profiletypedialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.ProfileTypeConfigStackPanelDynamic = (StackPanel) target;
      else
        this._contentLoaded = true;
    }
  }
}
