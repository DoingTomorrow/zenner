// Decompiled with JetBrains decompiler
// Type: AForge.Video.ScreenCaptureStream
// Assembly: AForge.Video, Version=2.2.5.0, Culture=neutral, PublicKeyToken=cbfb6e07d173c401
// MVID: 869827A8-29D1-478E-B314-48676C61CBC2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.dll

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

#nullable disable
namespace AForge.Video
{
  public class ScreenCaptureStream : IVideoSource
  {
    private Rectangle region;
    private int frameInterval = 100;
    private int framesReceived;
    private Thread thread;
    private ManualResetEvent stopEvent;

    public event NewFrameEventHandler NewFrame;

    public event VideoSourceErrorEventHandler VideoSourceError;

    public event PlayingFinishedEventHandler PlayingFinished;

    public virtual string Source => "Screen Capture";

    public Rectangle Region
    {
      get => this.region;
      set => this.region = value;
    }

    public int FrameInterval
    {
      get => this.frameInterval;
      set => this.frameInterval = Math.Max(0, value);
    }

    public int FramesReceived
    {
      get
      {
        int framesReceived = this.framesReceived;
        this.framesReceived = 0;
        return framesReceived;
      }
    }

    public long BytesReceived => 0;

    public bool IsRunning
    {
      get
      {
        if (this.thread != null)
        {
          if (!this.thread.Join(0))
            return true;
          this.Free();
        }
        return false;
      }
    }

    public ScreenCaptureStream(Rectangle region) => this.region = region;

    public ScreenCaptureStream(Rectangle region, int frameInterval)
    {
      this.region = region;
      this.FrameInterval = frameInterval;
    }

    public void Start()
    {
      if (this.IsRunning)
        return;
      this.framesReceived = 0;
      this.stopEvent = new ManualResetEvent(false);
      this.thread = new Thread(new ThreadStart(this.WorkerThread));
      this.thread.Name = this.Source;
      this.thread.Start();
    }

    public void SignalToStop()
    {
      if (this.thread == null)
        return;
      this.stopEvent.Set();
    }

    public void WaitForStop()
    {
      if (this.thread == null)
        return;
      this.thread.Join();
      this.Free();
    }

    public void Stop()
    {
      if (!this.IsRunning)
        return;
      this.stopEvent.Set();
      this.thread.Abort();
      this.WaitForStop();
    }

    private void Free()
    {
      this.thread = (Thread) null;
      this.stopEvent.Close();
      this.stopEvent = (ManualResetEvent) null;
    }

    private void WorkerThread()
    {
      int width = this.region.Width;
      int height = this.region.Height;
      int x = this.region.Location.X;
      int y = this.region.Location.Y;
      Size size = this.region.Size;
      Bitmap frame = new Bitmap(width, height, PixelFormat.Format32bppArgb);
      Graphics graphics = Graphics.FromImage((Image) frame);
      while (!this.stopEvent.WaitOne(0, false))
      {
        DateTime now = DateTime.Now;
        try
        {
          graphics.CopyFromScreen(x, y, 0, 0, size, CopyPixelOperation.SourceCopy);
          ++this.framesReceived;
          if (this.NewFrame != null)
            this.NewFrame((object) this, new NewFrameEventArgs(frame));
          if (this.frameInterval > 0)
          {
            int millisecondsTimeout = this.frameInterval - (int) DateTime.Now.Subtract(now).TotalMilliseconds;
            if (millisecondsTimeout > 0)
            {
              if (this.stopEvent.WaitOne(millisecondsTimeout, false))
                break;
            }
          }
        }
        catch (ThreadAbortException ex)
        {
          break;
        }
        catch (Exception ex)
        {
          if (this.VideoSourceError != null)
            this.VideoSourceError((object) this, new VideoSourceErrorEventArgs(ex.Message));
          Thread.Sleep(250);
        }
        if (this.stopEvent.WaitOne(0, false))
          break;
      }
      graphics.Dispose();
      frame.Dispose();
      if (this.PlayingFinished == null)
        return;
      this.PlayingFinished((object) this, ReasonToFinishPlaying.StoppedByUser);
    }
  }
}
