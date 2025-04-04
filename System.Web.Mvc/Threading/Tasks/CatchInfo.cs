// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.CatchInfo
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Threading.Tasks
{
  internal class CatchInfo(System.Threading.Tasks.Task task) : CatchInfoBase<System.Threading.Tasks.Task>(task)
  {
    private static CatchInfoBase<System.Threading.Tasks.Task>.CatchResult _completed = new CatchInfoBase<System.Threading.Tasks.Task>.CatchResult()
    {
      Task = TaskHelpers.Completed()
    };

    public CatchInfoBase<System.Threading.Tasks.Task>.CatchResult Handled() => CatchInfo._completed;

    public CatchInfoBase<System.Threading.Tasks.Task>.CatchResult Task(System.Threading.Tasks.Task task)
    {
      return new CatchInfoBase<System.Threading.Tasks.Task>.CatchResult()
      {
        Task = task
      };
    }

    public CatchInfoBase<System.Threading.Tasks.Task>.CatchResult Throw(Exception ex)
    {
      return new CatchInfoBase<System.Threading.Tasks.Task>.CatchResult()
      {
        Task = (System.Threading.Tasks.Task) TaskHelpers.FromError<object>(ex)
      };
    }
  }
}
