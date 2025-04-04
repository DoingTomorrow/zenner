// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Async.SynchronizationContextUtil
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Threading;

#nullable disable
namespace System.Web.Mvc.Async
{
  internal static class SynchronizationContextUtil
  {
    public static SynchronizationContext GetSynchronizationContext()
    {
      return SynchronizationContext.Current ?? new SynchronizationContext();
    }

    public static T Sync<T>(this SynchronizationContext syncContext, Func<T> func)
    {
      T theValue = default (T);
      Exception thrownException = (Exception) null;
      syncContext.Send((SendOrPostCallback) (o =>
      {
        try
        {
          theValue = func();
        }
        catch (Exception ex)
        {
          thrownException = ex;
        }
      }), (object) null);
      if (thrownException != null)
        throw Error.SynchronizationContextUtil_ExceptionThrown(thrownException);
      return theValue;
    }

    public static void Sync(this SynchronizationContext syncContext, Action action)
    {
      syncContext.Sync<AsyncVoid>((Func<AsyncVoid>) (() =>
      {
        action();
        return new AsyncVoid();
      }));
    }
  }
}
