// Decompiled with JetBrains decompiler
// Type: HandlerLib.Retry
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace HandlerLib
{
  public static class Retry
  {
    public static void Do(Action action, TimeSpan retryInterval, int retryCount = 3)
    {
      Retry.Do<object>((Func<object>) (() =>
      {
        action();
        return (object) null;
      }), retryInterval, retryCount);
    }

    public static T Do<T>(
      Func<T> action,
      TimeSpan retryInterval,
      int retryCount = 3,
      Action nextAttemptAction = null)
    {
      List<Exception> innerExceptions = new List<Exception>();
      for (int index = 0; index < retryCount; ++index)
      {
        try
        {
          if (index > 0)
          {
            Thread.Sleep(retryInterval);
            if (nextAttemptAction != null)
              nextAttemptAction();
          }
          return action();
        }
        catch (Exception ex)
        {
          innerExceptions.Add(ex);
        }
      }
      throw new AggregateException((IEnumerable<Exception>) innerExceptions);
    }
  }
}
