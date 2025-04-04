// Decompiled with JetBrains decompiler
// Type: NLog.Internal.AsyncOperationCounter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using System;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace NLog.Internal
{
  internal class AsyncOperationCounter
  {
    private int _pendingOperationCounter;
    private readonly LinkedList<AsyncContinuation> _pendingCompletionList = new LinkedList<AsyncContinuation>();

    public void BeginOperation() => Interlocked.Increment(ref this._pendingOperationCounter);

    public void CompleteOperation(Exception exception)
    {
      if (this._pendingCompletionList.Count > 0)
      {
        lock (this._pendingCompletionList)
        {
          Interlocked.Decrement(ref this._pendingOperationCounter);
          LinkedListNode<AsyncContinuation> linkedListNode = this._pendingCompletionList.First;
          while (linkedListNode != null)
          {
            AsyncContinuation asyncContinuation = linkedListNode.Value;
            linkedListNode = linkedListNode.Next;
            asyncContinuation(exception);
          }
        }
      }
      else
        Interlocked.Decrement(ref this._pendingOperationCounter);
    }

    public AsyncContinuation RegisterCompletionNotification(AsyncContinuation asyncContinuation)
    {
      if (this._pendingOperationCounter == 0)
        return asyncContinuation;
      lock (this._pendingCompletionList)
      {
        LinkedListNode<AsyncContinuation> pendingCompletion = new LinkedListNode<AsyncContinuation>((AsyncContinuation) null);
        this._pendingCompletionList.AddLast(pendingCompletion);
        int remainingCompletionCounter = Interlocked.Increment(ref this._pendingOperationCounter);
        if (remainingCompletionCounter <= 0)
        {
          Interlocked.Exchange(ref this._pendingOperationCounter, 0);
          this._pendingCompletionList.Remove(pendingCompletion);
          return asyncContinuation;
        }
        pendingCompletion.Value = (AsyncContinuation) (ex =>
        {
          if (Interlocked.Decrement(ref remainingCompletionCounter) != 0)
            return;
          lock (this._pendingCompletionList)
          {
            Interlocked.Decrement(ref this._pendingOperationCounter);
            this._pendingCompletionList.Remove(pendingCompletion);
            LinkedListNode<AsyncContinuation> linkedListNode = this._pendingCompletionList.First;
            while (linkedListNode != null)
            {
              AsyncContinuation asyncContinuation1 = linkedListNode.Value;
              linkedListNode = linkedListNode.Next;
              asyncContinuation1(ex);
            }
          }
          asyncContinuation(ex);
        });
        return pendingCompletion.Value;
      }
    }

    public void Clear() => this._pendingCompletionList.Clear();
  }
}
