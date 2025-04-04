// Decompiled with JetBrains decompiler
// Type: MVVM.Commands.AsyncCommandBase
// Assembly: MVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5D33A3C4-E333-437E-9AB2-FFDB9D6F32F8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MVVM.dll

using MVVM.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

#nullable disable
namespace MVVM.Commands
{
  public abstract class AsyncCommandBase : IAsyncCommand, ICommand
  {
    public abstract bool CanExecute(object parameter);

    public abstract Task ExecuteAsync(object parameter);

    public async void Execute(object parameter) => await this.ExecuteAsync(parameter);

    public event EventHandler CanExecuteChanged
    {
      add => CommandManager.RequerySuggested += value;
      remove => CommandManager.RequerySuggested -= value;
    }

    protected void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
  }
}
