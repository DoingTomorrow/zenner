// Decompiled with JetBrains decompiler
// Type: MSS_Client.Utils.DesktopWindowFactory
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Core.Events;
using MSS.Interfaces;
using MSS.Utils.Utils;
using System;
using System.ComponentModel;
using System.Windows;

#nullable disable
namespace MSS_Client.Utils
{
  public class DesktopWindowFactory : WindowDictionaryFactory, IWindowFactory
  {
    public bool? CreateNewModalDialog(IViewModel viewModel, object[] args = null)
    {
      WindowTypes windows = this.windowsDictionary[viewModel.GetType()];
      Window window = args == null ? Activator.CreateInstance(windows.Desktop).SafeCast<Window>() : Activator.CreateInstance(windows.Desktop, args) as Window;
      window.Owner = Application.Current.Windows[0];
      window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
      EventHandler<RequestCloseEventArgs> handlerCloseDialog = (EventHandler<RequestCloseEventArgs>) null;
      handlerCloseDialog = (EventHandler<RequestCloseEventArgs>) ((sender, eventArgs) =>
      {
        viewModel.RequestCloseDialog -= handlerCloseDialog;
        window.DialogResult = new bool?(eventArgs.DialogResult);
      });
      viewModel.RequestCloseDialog += handlerCloseDialog;
      window.DataContext = (object) viewModel;
      return window.ShowDialog();
    }

    public void CreateNewNonModalDialog(IViewModel viewModel)
    {
      Window window = Activator.CreateInstance(this.windowsDictionary[viewModel.GetType()].Desktop) as Window;
      EventHandler<RequestCloseEventArgs> handlerCloseDialog = (EventHandler<RequestCloseEventArgs>) null;
      handlerCloseDialog = (EventHandler<RequestCloseEventArgs>) ((sender, eventArgs) =>
      {
        viewModel.RequestCloseDialog -= handlerCloseDialog;
        window?.Close();
      });
      viewModel.RequestCloseDialog += handlerCloseDialog;
      window.DataContext = (object) viewModel;
      window.Show();
    }

    public bool? CreateNewProgressDialog(IViewModel viewModel, BackgroundWorker backgroundWorker)
    {
      Window window = Activator.CreateInstance(this.windowsDictionary[viewModel.GetType()].Desktop) as Window;
      window.Owner = Application.Current.Windows[0];
      window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
      EventHandler<RequestCloseEventArgs> handlerCloseDialog = (EventHandler<RequestCloseEventArgs>) null;
      handlerCloseDialog = (EventHandler<RequestCloseEventArgs>) ((sender, eventArgs) =>
      {
        viewModel.RequestCloseDialog -= handlerCloseDialog;
        window.DialogResult = new bool?(eventArgs.DialogResult);
        window.Close();
      });
      viewModel.RequestCloseDialog += handlerCloseDialog;
      EventHandler handlerCancel = (EventHandler) null;
      handlerCancel = (EventHandler) delegate
      {
        viewModel.RequestCancel -= handlerCancel;
        backgroundWorker.CancelAsync();
      };
      viewModel.RequestCancel += handlerCancel;
      window.DataContext = (object) viewModel;
      return window.ShowDialog();
    }
  }
}
