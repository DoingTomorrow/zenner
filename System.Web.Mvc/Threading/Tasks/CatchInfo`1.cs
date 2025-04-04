// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.CatchInfo`1
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Threading.Tasks
{
  internal class CatchInfo<T>(System.Threading.Tasks.Task<T> task) : CatchInfoBase<System.Threading.Tasks.Task<T>>(task)
  {
    public CatchInfoBase<System.Threading.Tasks.Task<T>>.CatchResult Handled(T returnValue)
    {
      return new CatchInfoBase<System.Threading.Tasks.Task<T>>.CatchResult()
      {
        Task = TaskHelpers.FromResult<T>(returnValue)
      };
    }

    public CatchInfoBase<System.Threading.Tasks.Task<T>>.CatchResult Task(System.Threading.Tasks.Task<T> task)
    {
      return new CatchInfoBase<System.Threading.Tasks.Task<T>>.CatchResult()
      {
        Task = task
      };
    }

    public CatchInfoBase<System.Threading.Tasks.Task<T>>.CatchResult Throw(Exception ex)
    {
      return new CatchInfoBase<System.Threading.Tasks.Task<T>>.CatchResult()
      {
        Task = TaskHelpers.FromError<T>(ex)
      };
    }
  }
}
