// Decompiled with JetBrains decompiler
// Type: AForge.Video.AsyncVideoSource
// Assembly: AForge.Video, Version=2.2.5.0, Culture=neutral, PublicKeyToken=cbfb6e07d173c401
// MVID: 869827A8-29D1-478E-B314-48676C61CBC2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.dll

using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

#nullable disable
namespace AForge.Video
{
  public class AsyncVideoSource : IVideoSource
  {
    private readonly IVideoSource nestedVideoSource;
    private Bitmap lastVideoFrame;
    private Thread imageProcessingThread;
    private AutoResetEvent isNewFrameAvailable;
    private AutoResetEvent isProcessingThreadAvailable;
    private bool skipFramesIfBusy;
    private int framesProcessed;

    public event NewFrameEventHandler NewFrame;

    public event VideoSourceErrorEventHandler VideoSourceError
    {
      add => this.nestedVideoSource.VideoSourceError += value;
      remove => this.nestedVideoSource.VideoSourceError -= value;
    }

    public event PlayingFinishedEventHandler PlayingFinished
    {
      add => this.nestedVideoSource.PlayingFinished += value;
      remove => this.nestedVideoSource.PlayingFinished -= value;
    }

    public IVideoSource NestedVideoSource => this.nestedVideoSource;

    public bool SkipFramesIfBusy
    {
      get => this.skipFramesIfBusy;
      set => this.skipFramesIfBusy = value;
    }

    public string Source => this.nestedVideoSource.Source;

    public int FramesReceived => this.nestedVideoSource.FramesReceived;

    public long BytesReceived => this.nestedVideoSource.BytesReceived;

    public int FramesProcessed
    {
      get
      {
        int framesProcessed = this.framesProcessed;
        this.framesProcessed = 0;
        return framesProcessed;
      }
    }

    public bool IsRunning
    {
      get
      {
        bool isRunning = this.nestedVideoSource.IsRunning;
        if (!isRunning)
          this.Free();
        return isRunning;
      }
    }

    public AsyncVideoSource(IVideoSource nestedVideoSource)
    {
      this.nestedVideoSource = nestedVideoSource;
    }

    public AsyncVideoSource(IVideoSource nestedVideoSource, bool skipFramesIfBusy)
    {
      this.nestedVideoSource = nestedVideoSource;
      this.skipFramesIfBusy = skipFramesIfBusy;
    }

    public void Start()
    {
      if (this.IsRunning)
        return;
      this.framesProcessed = 0;
      this.isNewFrameAvailable = new AutoResetEvent(false);
      this.isProcessingThreadAvailable = new AutoResetEvent(true);
      this.imageProcessingThread = new Thread(new ThreadStart(this.imageProcessingThread_Worker));
      this.imageProcessingThread.Start();
      this.nestedVideoSource.NewFrame += new NewFrameEventHandler(this.nestedVideoSource_NewFrame);
      this.nestedVideoSource.Start();
    }

    public void SignalToStop() => this.nestedVideoSource.SignalToStop();

    public void WaitForStop()
    {
      this.nestedVideoSource.WaitForStop();
      this.Free();
    }

    public void Stop()
    {
      this.nestedVideoSource.Stop();
      this.Free();
    }

    private void Free()
    {
      if (this.imageProcessingThread == null)
        return;
      this.nestedVideoSource.NewFrame -= new NewFrameEventHandler(this.nestedVideoSource_NewFrame);
      this.isProcessingThreadAvailable.WaitOne();
      this.lastVideoFrame = (Bitmap) null;
      this.isNewFrameAvailable.Set();
      this.imageProcessingThread.Join();
      this.imageProcessingThread = (Thread) null;
      this.isNewFrameAvailable.Close();
      this.isNewFrameAvailable = (AutoResetEvent) null;
      this.isProcessingThreadAvailable.Close();
      this.isProcessingThreadAvailable = (AutoResetEvent) null;
    }

    private void nestedVideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
    {
      if (this.NewFrame == null)
        return;
      if (this.skipFramesIfBusy)
      {
        if (!this.isProcessingThreadAvailable.WaitOne(0, false))
          return;
      }
      else
        this.isProcessingThreadAvailable.WaitOne();
      this.lastVideoFrame = AsyncVideoSource.CloneImage(eventArgs.Frame);
      this.isNewFrameAvailable.Set();
    }

    private void imageProcessingThread_Worker()
    {
      while (true)
      {
        this.isNewFrameAvailable.WaitOne();
        if (this.lastVideoFrame != null)
        {
          if (this.NewFrame != null)
            this.NewFrame((object) this, new NewFrameEventArgs(this.lastVideoFrame));
          this.lastVideoFrame.Dispose();
          this.lastVideoFrame = (Bitmap) null;
          ++this.framesProcessed;
          this.isProcessingThreadAvailable.Set();
        }
        else
          break;
      }
    }

    private static Bitmap CloneImage(Bitmap source)
    {
      BitmapData bitmapData = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadOnly, source.PixelFormat);
      Bitmap bitmap = AsyncVideoSource.CloneImage(bitmapData);
      source.UnlockBits(bitmapData);
      if (source.PixelFormat == PixelFormat.Format1bppIndexed || source.PixelFormat == PixelFormat.Format4bppIndexed || source.PixelFormat == PixelFormat.Format8bppIndexed || source.PixelFormat == PixelFormat.Indexed)
      {
        ColorPalette palette1 = source.Palette;
        ColorPalette palette2 = bitmap.Palette;
        int length = palette1.Entries.Length;
        for (int index = 0; index < length; ++index)
          palette2.Entries[index] = palette1.Entries[index];
        bitmap.Palette = palette2;
      }
      return bitmap;
    }

    private static Bitmap CloneImage(BitmapData sourceData)
    {
      int width = sourceData.Width;
      int height = sourceData.Height;
      Bitmap bitmap = new Bitmap(width, height, sourceData.PixelFormat);
      BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
      SystemTools.CopyUnmanagedMemory(bitmapdata.Scan0, sourceData.Scan0, height * sourceData.Stride);
      bitmap.UnlockBits(bitmapdata);
      return bitmap;
    }
  }
}
