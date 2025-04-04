// Decompiled with JetBrains decompiler
// Type: MVVM.Commands.RelayCommand
// Assembly: MVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5D33A3C4-E333-437E-9AB2-FFDB9D6F32F8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MVVM.dll

using System;
using System.Diagnostics;
using System.Windows.Input;

#nullable disable
namespace MVVM.Commands
{
  public class RelayCommand : ICommand
  {
    private readonly Action<object> iExecute;
    private readonly Predicate<object> iCanExecute;

    public RelayCommand(Action<object> execute)
      : this(execute, (Predicate<object>) null)
    {
    }

    public RelayCommand(Action<object> execute, Predicate<object> canExecute)
    {
      this.iExecute = execute != null ? execute : throw new ArgumentNullException(nameof (execute));
      this.iCanExecute = canExecute;
    }

    [DebuggerStepThrough]
    public bool CanExecute(object parameter)
    {
      Predicate<object> iCanExecute = this.iCanExecute;
      return iCanExecute == null || iCanExecute(parameter);
    }

    public event EventHandler CanExecuteChanged
    {
      add => CommandManager.RequerySuggested += value;
      remove => CommandManager.RequerySuggested -= value;
    }

    public void Execute(object parameter)
    {
      RelayCommandLogger.LogDebug(string.Format("Executing Command {0} {1}", this.iExecute.Target, (object) this.iExecute.Method.Name));
      this.iExecute(parameter);
    }
  }
}
