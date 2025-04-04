// Decompiled with JetBrains decompiler
// Type: MVVM.Commands.AsyncCommand`1
// Assembly: MVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5D33A3C4-E333-437E-9AB2-FFDB9D6F32F8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MVVM.dll

using System;
using System.Threading.Tasks;

#nullable disable
namespace MVVM.Commands
{
  public class AsyncCommand<TResult> : AsyncCommandBase
  {
    private readonly Func<object, Task<TResult>> _command;

    public AsyncCommand(Func<object, Task<TResult>> command) => this._command = command;

    public override bool CanExecute(object parameter) => true;

    public override Task ExecuteAsync(object parameter) => (Task) this._command(parameter);
  }
}
