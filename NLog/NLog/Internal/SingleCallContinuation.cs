// Decompiled with JetBrains decompiler
// Type: NLog.Internal.SingleCallContinuation
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using System;
using System.Threading;

#nullable disable
namespace NLog.Internal
{
  internal class SingleCallContinuation
  {
    private AsyncContinuation _asyncContinuation;

    public SingleCallContinuation(AsyncContinuation asyncContinuation)
    {
      this._asyncContinuation = asyncContinuation;
    }

    public void Function(Exception exception)
    {
      try
      {
        AsyncContinuation asyncContinuation = Interlocked.Exchange<AsyncContinuation>(ref this._asyncContinuation, (AsyncContinuation) null);
        if (asyncContinuation == null)
          return;
        asyncContinuation(exception);
      }
      catch (Exception ex)
      {
        InternalLogger.Error(ex, "Exception in asynchronous handler.");
        if (!ex.MustBeRethrown())
          return;
        throw;
      }
    }
  }
}
