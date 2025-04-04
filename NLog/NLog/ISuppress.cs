// Decompiled with JetBrains decompiler
// Type: NLog.ISuppress
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.Threading.Tasks;

#nullable disable
namespace NLog
{
  public interface ISuppress
  {
    void Swallow(Action action);

    T Swallow<T>(Func<T> func);

    T Swallow<T>(Func<T> func, T fallback);

    void Swallow(Task task);

    Task SwallowAsync(Task task);

    Task SwallowAsync(Func<Task> asyncAction);

    Task<TResult> SwallowAsync<TResult>(Func<Task<TResult>> asyncFunc);

    Task<TResult> SwallowAsync<TResult>(Func<Task<TResult>> asyncFunc, TResult fallback);
  }
}
