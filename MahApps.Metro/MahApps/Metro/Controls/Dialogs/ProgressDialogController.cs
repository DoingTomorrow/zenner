// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.Dialogs.ProgressDialogController
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Threading.Tasks;
using System.Windows;

#nullable disable
namespace MahApps.Metro.Controls.Dialogs
{
  public class ProgressDialogController
  {
    private ProgressDialog WrappedDialog { get; set; }

    private Func<Task> CloseCallback { get; set; }

    public event EventHandler Closed;

    public event EventHandler Canceled;

    public bool IsCanceled { get; private set; }

    public bool IsOpen { get; private set; }

    internal ProgressDialogController(ProgressDialog dialog, Func<Task> closeCallBack)
    {
      this.WrappedDialog = dialog;
      this.CloseCallback = closeCallBack;
      this.IsOpen = dialog.IsVisible;
      this.InvokeAction((Action) (() => this.WrappedDialog.PART_NegativeButton.Click += new RoutedEventHandler(this.PART_NegativeButton_Click)));
      dialog.CancellationToken.Register((Action) (() => this.PART_NegativeButton_Click((object) null, new RoutedEventArgs())));
    }

    private void PART_NegativeButton_Click(object sender, RoutedEventArgs e)
    {
      this.InvokeAction((Action) (() =>
      {
        this.IsCanceled = true;
        EventHandler canceled = this.Canceled;
        if (canceled != null)
          canceled((object) this, EventArgs.Empty);
        this.WrappedDialog.PART_NegativeButton.IsEnabled = false;
      }));
    }

    public void SetIndeterminate()
    {
      this.InvokeAction((Action) (() => this.WrappedDialog.SetIndeterminate()));
    }

    public void SetCancelable(bool value)
    {
      this.InvokeAction((Action) (() => this.WrappedDialog.IsCancelable = value));
    }

    public void SetProgress(double value)
    {
      this.InvokeAction((Action) (() =>
      {
        if (value < this.WrappedDialog.Minimum || value > this.WrappedDialog.Maximum)
          throw new ArgumentOutOfRangeException(nameof (value));
        this.WrappedDialog.ProgressValue = value;
      }));
    }

    public double Minimum
    {
      get => this.InvokeFunc((Func<double>) (() => this.WrappedDialog.Minimum));
      set => this.InvokeAction((Action) (() => this.WrappedDialog.Minimum = value));
    }

    public double Maximum
    {
      get => this.InvokeFunc((Func<double>) (() => this.WrappedDialog.Maximum));
      set => this.InvokeAction((Action) (() => this.WrappedDialog.Maximum = value));
    }

    public void SetMessage(string message)
    {
      this.InvokeAction((Action) (() => this.WrappedDialog.Message = message));
    }

    public void SetTitle(string title)
    {
      this.InvokeAction((Action) (() => this.WrappedDialog.Title = title));
    }

    public Task CloseAsync()
    {
      this.InvokeAction((Action) (() =>
      {
        if (!this.WrappedDialog.IsVisible)
          throw new InvalidOperationException("Dialog isn't visible to close");
        this.WrappedDialog.Dispatcher.VerifyAccess();
        this.WrappedDialog.PART_NegativeButton.Click -= new RoutedEventHandler(this.PART_NegativeButton_Click);
      }));
      return this.CloseCallback().ContinueWith((Action<Task>) (_ => this.InvokeAction((Action) (() =>
      {
        this.IsOpen = false;
        EventHandler closed = this.Closed;
        if (closed == null)
          return;
        closed((object) this, EventArgs.Empty);
      }))));
    }

    private double InvokeFunc(Func<double> getValueFunc)
    {
      return this.WrappedDialog.Dispatcher.CheckAccess() ? getValueFunc() : this.WrappedDialog.Dispatcher.Invoke<double>(new Func<double>(getValueFunc.Invoke));
    }

    private void InvokeAction(Action setValueAction)
    {
      if (this.WrappedDialog.Dispatcher.CheckAccess())
        setValueAction();
      else
        this.WrappedDialog.Dispatcher.Invoke(setValueAction);
    }
  }
}
