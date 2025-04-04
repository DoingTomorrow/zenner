// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Async.SingleEntryGate
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Threading;

#nullable disable
namespace System.Web.Mvc.Async
{
  internal sealed class SingleEntryGate
  {
    private const int NotEntered = 0;
    private const int Entered = 1;
    private int _status;

    public bool TryEnter() => Interlocked.Exchange(ref this._status, 1) == 0;
  }
}
