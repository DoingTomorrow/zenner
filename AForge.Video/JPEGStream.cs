// Decompiled with JetBrains decompiler
// Type: AForge.Video.JPEGStream
// Assembly: AForge.Video, Version=2.2.5.0, Culture=neutral, PublicKeyToken=cbfb6e07d173c401
// MVID: 869827A8-29D1-478E-B314-48676C61CBC2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.dll

using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

#nullable disable
namespace AForge.Video
{
  public class JPEGStream : IVideoSource
  {
    private const int bufferSize = 1048576;
    private const int readSize = 1024;
    private string source;
    private string login;
    private string password;
    private IWebProxy proxy;
    private int framesReceived;
    private long bytesReceived;
    private bool useSeparateConnectionGroup;
    private bool preventCaching = true;
    private int frameInterval;
    private int requestTimeout = 10000;
    private bool forceBasicAuthentication;
    private Thread thread;
    private ManualResetEvent stopEvent;

    public event NewFrameEventHandler NewFrame;

    public event VideoSourceErrorEventHandler VideoSourceError;

    public event PlayingFinishedEventHandler PlayingFinished;

    public bool SeparateConnectionGroup
    {
      get => this.useSeparateConnectionGroup;
      set => this.useSeparateConnectionGroup = value;
    }

    public bool PreventCaching
    {
      get => this.preventCaching;
      set => this.preventCaching = value;
    }

    public int FrameInterval
    {
      get => this.frameInterval;
      set => this.frameInterval = value;
    }

    public virtual string Source
    {
      get => this.source;
      set => this.source = value;
    }

    public string Login
    {
      get => this.login;
      set => this.login = value;
    }

    public string Password
    {
      get => this.password;
      set => this.password = value;
    }

    public IWebProxy Proxy
    {
      get => this.proxy;
      set => this.proxy = value;
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

    public int RequestTimeout
    {
      get => this.requestTimeout;
      set => this.requestTimeout = value;
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

    public bool ForceBasicAuthentication
    {
      get => this.forceBasicAuthentication;
      set => this.forceBasicAuthentication = value;
    }

    public JPEGStream()
    {
    }

    public JPEGStream(string source) => this.source = source;

    public void Start()
    {
      if (this.IsRunning)
        return;
      if (this.source == null || this.source == string.Empty)
        throw new ArgumentException("Video source is not specified.");
      this.framesReceived = 0;
      this.bytesReceived = 0L;
      this.stopEvent = new ManualResetEvent(false);
      this.thread = new Thread(new ThreadStart(this.WorkerThread));
      this.thread.Name = this.source;
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
      byte[] buffer = new byte[1048576];
      HttpWebRequest httpWebRequest = (HttpWebRequest) null;
      WebResponse webResponse = (WebResponse) null;
      Stream stream = (Stream) null;
      Random random = new Random((int) DateTime.Now.Ticks);
      while (!this.stopEvent.WaitOne(0, false))
      {
        int num1 = 0;
        try
        {
          DateTime now = DateTime.Now;
          if (!this.preventCaching)
            httpWebRequest = (HttpWebRequest) WebRequest.Create(this.source);
          else
            httpWebRequest = (HttpWebRequest) WebRequest.Create(this.source + (object) (char) (this.source.IndexOf('?') == -1 ? 63 : 38) + "fake=" + random.Next().ToString());
          if (this.proxy != null)
            httpWebRequest.Proxy = this.proxy;
          httpWebRequest.Timeout = this.requestTimeout;
          if (this.login != null && this.password != null && this.login != string.Empty)
            httpWebRequest.Credentials = (ICredentials) new NetworkCredential(this.login, this.password);
          if (this.useSeparateConnectionGroup)
            httpWebRequest.ConnectionGroupName = this.GetHashCode().ToString();
          if (this.forceBasicAuthentication)
          {
            string base64String = Convert.ToBase64String(Encoding.Default.GetBytes(string.Format("{0}:{1}", (object) this.login, (object) this.password)));
            httpWebRequest.Headers["Authorization"] = "Basic " + base64String;
          }
          webResponse = httpWebRequest.GetResponse();
          stream = webResponse.GetResponseStream();
          stream.ReadTimeout = this.requestTimeout;
          while (!this.stopEvent.WaitOne(0, false))
          {
            if (num1 > 1047552)
              num1 = 0;
            int num2;
            if ((num2 = stream.Read(buffer, num1, 1024)) != 0)
            {
              num1 += num2;
              this.bytesReceived += (long) num2;
            }
            else
              break;
          }
          if (!this.stopEvent.WaitOne(0, false))
          {
            ++this.framesReceived;
            if (this.NewFrame != null)
            {
              Bitmap frame = (Bitmap) Image.FromStream((Stream) new MemoryStream(buffer, 0, num1));
              this.NewFrame((object) this, new NewFrameEventArgs(frame));
              frame.Dispose();
            }
          }
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
        finally
        {
          if (httpWebRequest != null)
          {
            httpWebRequest.Abort();
            httpWebRequest = (HttpWebRequest) null;
          }
          if (stream != null)
          {
            stream.Close();
            stream = (Stream) null;
          }
          if (webResponse != null)
          {
            webResponse.Close();
            webResponse = (WebResponse) null;
          }
        }
        if (this.stopEvent.WaitOne(0, false))
          break;
      }
      if (this.PlayingFinished == null)
        return;
      this.PlayingFinished((object) this, ReasonToFinishPlaying.StoppedByUser);
    }
  }
}
