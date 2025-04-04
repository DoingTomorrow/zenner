// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.CatchInfoBase`1
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Threading.Tasks
{
  internal abstract class CatchInfoBase<TTask> where TTask : Task
  {
    private Exception _exception;
    private TTask _task;

    protected CatchInfoBase(TTask task)
    {
      this._task = task;
      this._exception = this._task.Exception.GetBaseException();
    }

    public Exception Exception => this._exception;

    public CatchInfoBase<TTask>.CatchResult Throw()
    {
      return new CatchInfoBase<TTask>.CatchResult()
      {
        Task = this._task
      };
    }

    internal struct CatchResult
    {
      internal TTask Task { get; set; }
    }
  }
}
