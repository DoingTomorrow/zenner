// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.AsyncResult`2
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Diagnostics;

#nullable disable
namespace InTheHand.Net
{
  [DebuggerNonUserCode]
  internal class AsyncResult<TResult, TParams> : AsyncResult<TResult>
  {
    private readonly TParams m_args;

    public AsyncResult(AsyncCallback asyncCallback, object state, TParams args)
      : base(asyncCallback, state)
    {
      this.m_args = args;
    }

    public TParams BeginParameters => this.m_args;
  }
}
