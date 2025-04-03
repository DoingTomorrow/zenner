// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.FileVideoSource
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using AForge.Video.DirectShow.Internals;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;

#nullable disable
namespace AForge.Video.DirectShow
{
  public class FileVideoSource : IVideoSource
  {
    private string fileName;
    private int framesReceived;
    private long bytesReceived;
    private bool preventFreezing;
    private bool referenceClockEnabled = true;
    private Thread thread;
    private ManualResetEvent stopEvent;

    public event NewFrameEventHandler NewFrame;

    public event VideoSourceErrorEventHandler VideoSourceError;

    public event PlayingFinishedEventHandler PlayingFinished;

    public virtual string Source
    {
      get => this.fileName;
      set => this.fileName = value;
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

    public long BytesReceived
    {
      get
      {
        long bytesReceived = this.bytesReceived;
        this.bytesReceived = 0L;
        return bytesReceived;
      }
    }

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

    public bool PreventFreezing
    {
      get => this.preventFreezing;
      set => this.preventFreezing = value;
    }

    public bool ReferenceClockEnabled
    {
      get => this.referenceClockEnabled;
      set => this.referenceClockEnabled = value;
    }

    public FileVideoSource()
    {
    }

    public FileVideoSource(string fileName) => this.fileName = fileName;

    public void Start()
    {
      if (this.IsRunning)
        return;
      if (this.fileName == null || this.fileName == string.Empty)
        throw new ArgumentException("Video source is not specified");
      this.framesReceived = 0;
      this.bytesReceived = 0L;
      this.stopEvent = new ManualResetEvent(false);
      this.thread = new Thread(new ThreadStart(this.WorkerThread));
      this.thread.Name = this.fileName;
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
      ReasonToFinishPlaying reason = ReasonToFinishPlaying.StoppedByUser;
      FileVideoSource.Grabber callback = new FileVideoSource.Grabber(this);
      object o1 = (object) null;
      object o2 = (object) null;
      IGraphBuilder graphBuilder1 = (IGraphBuilder) null;
      IBaseFilter filter1 = (IBaseFilter) null;
      IBaseFilter baseFilter = (IBaseFilter) null;
      ISampleGrabber sampleGrabber1 = (ISampleGrabber) null;
      IMediaControl mediaControl1 = (IMediaControl) null;
      IMediaEventEx mediaEventEx1 = (IMediaEventEx) null;
      try
      {
        o1 = Activator.CreateInstance(Type.GetTypeFromCLSID(Clsid.FilterGraph) ?? throw new ApplicationException("Failed creating filter graph"));
        IGraphBuilder graphBuilder2 = (IGraphBuilder) o1;
        graphBuilder2.AddSourceFilter(this.fileName, "source", out filter1);
        if (filter1 == null)
          throw new ApplicationException("Failed creating source filter");
        o2 = Activator.CreateInstance(Type.GetTypeFromCLSID(Clsid.SampleGrabber) ?? throw new ApplicationException("Failed creating sample grabber"));
        ISampleGrabber sampleGrabber2 = (ISampleGrabber) o2;
        IBaseFilter filter2 = (IBaseFilter) o2;
        graphBuilder2.AddFilter(filter2, "grabber");
        AMMediaType mediaType = new AMMediaType();
        mediaType.MajorType = MediaType.Video;
        mediaType.SubType = MediaSubType.RGB24;
        sampleGrabber2.SetMediaType(mediaType);
        int num = 0;
        IPin inPin = Tools.GetInPin(filter2, 0);
        IPin pin = (IPin) null;
        IPin outPin;
        while (true)
        {
          outPin = Tools.GetOutPin(filter1, num);
          if (outPin != null)
          {
            if (graphBuilder2.Connect(outPin, inPin) < 0)
            {
              Marshal.ReleaseComObject((object) outPin);
              pin = (IPin) null;
              ++num;
            }
            else
              goto label_12;
          }
          else
            break;
        }
        Marshal.ReleaseComObject((object) inPin);
        throw new ApplicationException("Did not find acceptable output video pin in the given source");
label_12:
        Marshal.ReleaseComObject((object) outPin);
        Marshal.ReleaseComObject((object) inPin);
        if (sampleGrabber2.GetConnectedMediaType(mediaType) == 0)
        {
          VideoInfoHeader structure = (VideoInfoHeader) Marshal.PtrToStructure(mediaType.FormatPtr, typeof (VideoInfoHeader));
          callback.Width = structure.BmiHeader.Width;
          callback.Height = structure.BmiHeader.Height;
          mediaType.Dispose();
        }
        if (!this.preventFreezing)
        {
          graphBuilder2.Render(Tools.GetOutPin(filter2, 0));
          ((IVideoWindow) o1).put_AutoShow(false);
        }
        sampleGrabber2.SetBufferSamples(false);
        sampleGrabber2.SetOneShot(false);
        sampleGrabber2.SetCallback((ISampleGrabberCB) callback, 1);
        if (!this.referenceClockEnabled)
          ((IMediaFilter) o1).SetSyncSource((IReferenceClock) null);
        IMediaControl mediaControl2 = (IMediaControl) o1;
        IMediaEventEx mediaEventEx2 = (IMediaEventEx) o1;
        mediaControl2.Run();
        do
        {
          DsEvCode lEventCode;
          IntPtr lParam1;
          IntPtr lParam2;
          if (mediaEventEx2 != null && mediaEventEx2.GetEvent(out lEventCode, out lParam1, out lParam2, 0) >= 0)
          {
            mediaEventEx2.FreeEventParams(lEventCode, lParam1, lParam2);
            if (lEventCode == DsEvCode.Complete)
            {
              reason = ReasonToFinishPlaying.EndOfStreamReached;
              break;
            }
          }
        }
        while (!this.stopEvent.WaitOne(100, false));
        mediaControl2.Stop();
      }
      catch (Exception ex)
      {
        if (this.VideoSourceError != null)
          this.VideoSourceError((object) this, new VideoSourceErrorEventArgs(ex.Message));
      }
      finally
      {
        graphBuilder1 = (IGraphBuilder) null;
        baseFilter = (IBaseFilter) null;
        sampleGrabber1 = (ISampleGrabber) null;
        mediaControl1 = (IMediaControl) null;
        mediaEventEx1 = (IMediaEventEx) null;
        if (o1 != null)
          Marshal.ReleaseComObject(o1);
        if (filter1 != null)
          Marshal.ReleaseComObject((object) filter1);
        if (o2 != null)
          Marshal.ReleaseComObject(o2);
      }
      if (this.PlayingFinished == null)
        return;
      this.PlayingFinished((object) this, reason);
    }

    protected void OnNewFrame(Bitmap image)
    {
      ++this.framesReceived;
      this.bytesReceived += (long) (image.Width * image.Height * (Image.GetPixelFormatSize(image.PixelFormat) >> 3));
      if (this.stopEvent.WaitOne(0, false) || this.NewFrame == null)
        return;
      this.NewFrame((object) this, new NewFrameEventArgs(image));
    }

    private class Grabber : ISampleGrabberCB
    {
      private FileVideoSource parent;
      private int width;
      private int height;

      public int Width
      {
        get => this.width;
        set => this.width = value;
      }

      public int Height
      {
        get => this.height;
        set => this.height = value;
      }

      public Grabber(FileVideoSource parent) => this.parent = parent;

      public int SampleCB(double sampleTime, IntPtr sample) => 0;

      public unsafe int BufferCB(double sampleTime, IntPtr buffer, int bufferLen)
      {
        if (this.parent.NewFrame != null)
        {
          Bitmap image = new Bitmap(this.width, this.height, PixelFormat.Format24bppRgb);
          BitmapData bitmapdata = image.LockBits(new Rectangle(0, 0, this.width, this.height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
          int stride1 = bitmapdata.Stride;
          int stride2 = bitmapdata.Stride;
          byte* dst = (byte*) ((IntPtr) bitmapdata.Scan0.ToPointer() + (IntPtr) stride2 * (this.height - 1));
          byte* pointer = (byte*) buffer.ToPointer();
          for (int index = 0; index < this.height; ++index)
          {
            Win32.memcpy(dst, pointer, stride1);
            dst -= stride2;
            pointer += stride1;
          }
          image.UnlockBits(bitmapdata);
          this.parent.OnNewFrame(image);
          image.Dispose();
        }
        return 0;
      }
    }
  }
}
