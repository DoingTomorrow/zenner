// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.Meters.MeterPhotosDialog
// Assembly: MSS.Client.UI.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B34A4718-63B5-4C6C-93C2-0A28BCAE0F44
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Desktop.dll

using Microsoft.CSharp.RuntimeBinder;
using MSS.Client.UI.Common;
using MSS.Utils.Utils;
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
using System.Windows.Media;

#nullable disable
namespace MSS.Client.UI.Desktop.View.Meters
{
  public partial class MeterPhotosDialog : ResizableMetroWindow, IComponentConnector
  {
    internal ListBox PhotosListBox;
    private bool _contentLoaded;

    public MeterPhotosDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged += new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated += new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
    }

    ~MeterPhotosDialog()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged -= new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated -= new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
      this.PhotosListBox.SelectionChanged -= new SelectionChangedEventHandler(this.ListBox_SelectionChanged);
    }

    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      if (MeterPhotosDialog.\u003C\u003Eo__2.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterPhotosDialog.\u003C\u003Eo__2.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, ImageSource, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "SelectedPhoto", typeof (MeterPhotosDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = MeterPhotosDialog.\u003C\u003Eo__2.\u003C\u003Ep__0.Target((CallSite) MeterPhotosDialog.\u003C\u003Eo__2.\u003C\u003Ep__0, this.DataContext, this.PhotosListBox.SelectedItem as ImageSource);
      // ISSUE: reference to a compiler-generated field
      if (MeterPhotosDialog.\u003C\u003Eo__2.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterPhotosDialog.\u003C\u003Eo__2.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "SelectedPhotoIndex", typeof (MeterPhotosDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = MeterPhotosDialog.\u003C\u003Eo__2.\u003C\u003Ep__1.Target((CallSite) MeterPhotosDialog.\u003C\u003Eo__2.\u003C\u003Ep__1, this.DataContext, this.PhotosListBox.SelectedIndex);
      // ISSUE: reference to a compiler-generated field
      if (MeterPhotosDialog.\u003C\u003Eo__2.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MeterPhotosDialog.\u003C\u003Eo__2.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "IsDeleteButtonActive", typeof (MeterPhotosDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = MeterPhotosDialog.\u003C\u003Eo__2.\u003C\u003Ep__2.Target((CallSite) MeterPhotosDialog.\u003C\u003Eo__2.\u003C\u003Ep__2, this.DataContext, true);
    }

    private void ViewPhoto(object sender, RoutedEventArgs e)
    {
      PhotoDialog photoDialog1 = new PhotoDialog();
      PhotoDialog photoDialog2 = photoDialog1;
      object selectedItem = this.PhotosListBox.SelectedItem;
      ImageSource imageSource = selectedItem != null ? selectedItem.SafeCast<ImageSource>() : (ImageSource) null;
      photoDialog2.SelectedPhoto = imageSource;
      photoDialog1.Show();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/meters/meterphotosdialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.PhotosListBox = (ListBox) target;
          this.PhotosListBox.SelectionChanged += new SelectionChangedEventHandler(this.ListBox_SelectionChanged);
          break;
        case 2:
          ((MenuItem) target).Click += new RoutedEventHandler(this.ViewPhoto);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
