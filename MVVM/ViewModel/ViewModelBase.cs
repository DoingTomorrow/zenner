// Decompiled with JetBrains decompiler
// Type: MVVM.ViewModel.ViewModelBase
// Assembly: MVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5D33A3C4-E333-437E-9AB2-FFDB9D6F32F8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MVVM.dll

using MSS.Core.Events;
using MSS.Interfaces;
using MSS.Localisation;
using MVVM.Commands;
using NHibernate;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace MVVM.ViewModel
{
  public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable, IViewModel
  {
    private const string KEYBOARD_PATH = "\\Common Files\\Microsoft shared\\ink\\TabTip.exe";
    private string _keyboardControlText = Resources.MSS_Client_ShowKeyboard;
    private Visibility _isKeyboardControlVisible = Visibility.Collapsed;

    protected ViewModelBase(ISession nhSession) => this.NhSession = nhSession;

    protected ViewModelBase()
    {
    }

    ~ViewModelBase()
    {
      Debug.WriteLine(string.Format("{0} ({1}) ({2}) Finalized", (object) this.GetType().Name, (object) this.DisplayName, (object) this.GetHashCode()));
      this.OnDispose();
    }

    public ISession NhSession { get; }

    public virtual string DisplayName { get; protected set; }

    protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

    [Conditional("DEBUG")]
    [DebuggerStepThrough]
    public void VerifyPropertyName(string propertyName)
    {
      if (TypeDescriptor.GetProperties((object) this)[propertyName] != null)
        return;
      string message = "Invalid property name: " + propertyName;
      if (this.ThrowOnInvalidPropertyName)
        throw new Exception(message);
      Debug.Fail(message);
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
      this.VerifyPropertyName(propertyName);
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
      propertyChanged((object) this, e);
    }

    public void Dispose() => this.OnDispose();

    protected virtual void OnDispose()
    {
      this.NhSession?.Dispose();
      this.Disposed();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public event Action Disposed = () => { };

    public event EventHandler<RequestCloseEventArgs> RequestCloseDialog;

    public void OnRequestClose(bool dialogResult)
    {
      EventHandler<RequestCloseEventArgs> requestCloseDialog = this.RequestCloseDialog;
      if (requestCloseDialog != null)
        requestCloseDialog((object) this, new RequestCloseEventArgs(dialogResult));
      this.Dispose();
    }

    public void OnRequestClose(bool dialogResult, bool withoutDispose)
    {
      EventHandler<RequestCloseEventArgs> requestCloseDialog = this.RequestCloseDialog;
      if (requestCloseDialog != null)
        requestCloseDialog((object) this, new RequestCloseEventArgs(dialogResult));
      this.Dispose();
    }

    public event EventHandler RequestCancel;

    protected void OnRequestCancel()
    {
      EventHandler requestCancel = this.RequestCancel;
      if (requestCancel != null)
        requestCancel((object) this, EventArgs.Empty);
      this.Dispose();
    }

    public string KeyboardControlText
    {
      get => this._keyboardControlText;
      set
      {
        this._keyboardControlText = value;
        this.OnPropertyChanged(nameof (KeyboardControlText));
      }
    }

    public Visibility IsKeyboardControlVisible
    {
      get => this._isKeyboardControlVisible;
      set
      {
        this._isKeyboardControlVisible = value;
        this.KeyboardControlText = value == Visibility.Collapsed ? Resources.MSS_Client_ShowKeyboard : Resources.MSS_Client_HideKeyboard;
        this.OnPropertyChanged(nameof (IsKeyboardControlVisible));
      }
    }

    public ICommand KeyboardControlCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          if (this.IsKeyboardControlVisible == Visibility.Collapsed)
          {
            this.IsKeyboardControlVisible = Visibility.Visible;
            this.KeyboardControlText = Resources.MSS_Client_HideKeyboard;
          }
          else
          {
            this.IsKeyboardControlVisible = Visibility.Collapsed;
            this.KeyboardControlText = Resources.MSS_Client_ShowKeyboard;
          }
        }));
      }
    }

    public virtual ICommand CancelWindowCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (Delegate => this.OnRequestClose(false)));
    }

    public delegate void UpdateProgressBarDelegate(int percentage, string additionalText);
  }
}
