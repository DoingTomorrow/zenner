// Decompiled with JetBrains decompiler
// Type: AForge.Parallel
// Assembly: AForge, Version=2.2.5.0, Culture=neutral, PublicKeyToken=c1db6ff4eaa06aeb
// MVID: D4933F01-4742-407D-982E-D47DDB340621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.dll

using System;
using System.Threading;

#nullable disable
namespace AForge
{
  public sealed class Parallel
  {
    private static int threadsCount = Environment.ProcessorCount;
    private static object sync = new object();
    private static volatile Parallel instance = (Parallel) null;
    private Thread[] threads;
    private AutoResetEvent[] jobAvailable;
    private ManualResetEvent[] threadIdle;
    private int currentIndex;
    private int stopIndex;
    private Parallel.ForLoopBody loopBody;

    public static int ThreadsCount
    {
      get => Parallel.threadsCount;
      set
      {
        lock (Parallel.sync)
          Parallel.threadsCount = Math.Max(1, value);
      }
    }

    public static void For(int start, int stop, Parallel.ForLoopBody loopBody)
    {
      lock (Parallel.sync)
      {
        Parallel instance = Parallel.Instance;
        instance.currentIndex = start - 1;
        instance.stopIndex = stop;
        instance.loopBody = loopBody;
        for (int index = 0; index < Parallel.threadsCount; ++index)
        {
          instance.threadIdle[index].Reset();
          instance.jobAvailable[index].Set();
        }
        for (int index = 0; index < Parallel.threadsCount; ++index)
          instance.threadIdle[index].WaitOne();
        instance.loopBody = (Parallel.ForLoopBody) null;
      }
    }

    private Parallel()
    {
    }

    private static Parallel Instance
    {
      get
      {
        if (Parallel.instance == null)
        {
          Parallel.instance = new Parallel();
          Parallel.instance.Initialize();
        }
        else if (Parallel.instance.threads.Length != Parallel.threadsCount)
        {
          Parallel.instance.Terminate();
          Parallel.instance.Initialize();
        }
        return Parallel.instance;
      }
    }

    private void Initialize()
    {
      this.jobAvailable = new AutoResetEvent[Parallel.threadsCount];
      this.threadIdle = new ManualResetEvent[Parallel.threadsCount];
      this.threads = new Thread[Parallel.threadsCount];
      for (int parameter = 0; parameter < Parallel.threadsCount; ++parameter)
      {
        this.jobAvailable[parameter] = new AutoResetEvent(false);
        this.threadIdle[parameter] = new ManualResetEvent(true);
        this.threads[parameter] = new Thread(new ParameterizedThreadStart(this.WorkerThread));
        this.threads[parameter].Name = "AForge.Parallel";
        this.threads[parameter].IsBackground = true;
        this.threads[parameter].Start((object) parameter);
      }
    }

    private void Terminate()
    {
      this.loopBody = (Parallel.ForLoopBody) null;
      int index = 0;
      for (int length = this.threads.Length; index < length; ++index)
      {
        this.jobAvailable[index].Set();
        this.threads[index].Join();
        this.jobAvailable[index].Close();
        this.threadIdle[index].Close();
      }
      this.jobAvailable = (AutoResetEvent[]) null;
      this.threadIdle = (ManualResetEvent[]) null;
      this.threads = (Thread[]) null;
    }

    private void WorkerThread(object index)
    {
      int index1 = (int) index;
      while (true)
      {
        this.jobAvailable[index1].WaitOne();
        if (this.loopBody != null)
        {
          while (true)
          {
            int index2 = Interlocked.Increment(ref this.currentIndex);
            if (index2 < this.stopIndex)
              this.loopBody(index2);
            else
              break;
          }
          this.threadIdle[index1].Set();
        }
        else
          break;
      }
    }

    public delegate void ForLoopBody(int index);
  }
}
