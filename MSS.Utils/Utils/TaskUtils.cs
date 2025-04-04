// Decompiled with JetBrains decompiler
// Type: MSS.Utils.Utils.TaskUtils
// Assembly: MSS.Utils, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8365EDE-890D-4A42-AEA4-3B8FCE5E7B93
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Utils.dll

using System.Threading.Tasks;
using System.Timers;

#nullable disable
namespace MSS.Utils.Utils
{
  public static class TaskUtils
  {
    public static Task Delay(double milliseconds)
    {
      TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
      Timer timer = new Timer();
      timer.Elapsed += (ElapsedEventHandler) ((obj, args) => tcs.TrySetResult(true));
      timer.Interval = milliseconds;
      timer.AutoReset = false;
      timer.Start();
      return (Task) tcs.Task;
    }
  }
}
