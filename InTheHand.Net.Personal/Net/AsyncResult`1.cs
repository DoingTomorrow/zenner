// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.AsyncResult`1
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Diagnostics;

#nullable disable
namespace InTheHand.Net
{
  [DebuggerNonUserCode]
  internal class AsyncResult<TResult>(AsyncCallback asyncCallback, object state) : 
    AsyncResultNoResult(asyncCallback, state)
  {
    private TResult m_result = default (TResult);

    [DebuggerNonUserCode]
    public void SetAsCompleted(TResult result, bool completedSynchronously)
    {
      this.m_result = result;
      this.SetAsCompleted((Exception) null, completedSynchronously);
    }

    [DebuggerNonUserCode]
    public void SetAsCompleted(TResult result, AsyncResultCompletion completion)
    {
      this.m_result = result;
      this.SetAsCompleted((Exception) null, completion);
    }

    [DebuggerNonUserCode]
    public TResult EndInvoke()
    {
      base.EndInvoke();
      return this.m_result;
    }

    internal void SetAsCompletedWithResultOf(
      Func<TResult> getResultsOrThrow,
      bool completedSynchronously)
    {
      this.SetAsCompletedWithResultOf(getResultsOrThrow, AsyncResultNoResult.ConvertCompletion(completedSynchronously));
    }

    internal void SetAsCompletedWithResultOf(
      Func<TResult> getResultsOrThrow,
      AsyncResultCompletion completion)
    {
      TResult result;
      try
      {
        result = getResultsOrThrow();
      }
      catch (Exception ex)
      {
        this.SetAsCompleted(ex, completion);
        return;
      }
      this.SetAsCompleted(result, completion);
    }
  }
}
