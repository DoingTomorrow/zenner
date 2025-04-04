// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Common.Utils.DelegateCommand
// Assembly: MSS.Client.UI.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15ED3F62-7ABB-4067-AE48-CE636F8F9754
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Common.dll

using System;
using System.Windows.Input;

#nullable disable
namespace MSS.Client.UI.Common.Utils
{
  public class DelegateCommand : ICommand
  {
    private readonly Action execute;
    private readonly Func<bool> canExecute;

    public DelegateCommand(Action executeAction, Func<bool> canExecuteFunc)
    {
      this.execute = executeAction;
      this.canExecute = canExecuteFunc;
    }

    event EventHandler ICommand.CanExecuteChanged
    {
      add => CommandManager.RequerySuggested += value;
      remove => CommandManager.RequerySuggested -= value;
    }

    bool ICommand.CanExecute(object parameter) => this.canExecute == null || this.canExecute();

    void ICommand.Execute(object parameter)
    {
      if (this.execute == null)
        return;
      this.execute();
    }
  }
}
