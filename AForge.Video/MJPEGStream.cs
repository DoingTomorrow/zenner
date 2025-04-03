// Decompiled with JetBrains decompiler
// Type: AForge.Video.MJPEGStream
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
  public class MJPEGStream : IVideoSource
  {
    private const int bufSize = 1048576;
    private const int readSize = 1024;
    private string source;
    private string login;
    private string password;
    private IWebProxy proxy;
    private int framesReceived;
    private long bytesReceived;
    private bool useSeparateConnectionGroup = true;
    private int requestTimeout = 10000;
    private bool forceBasicAuthentication;
    private Thread thread;
    private ManualResetEvent stopEvent;
    private ManualResetEvent reloadEvent;
    private string userAgent = "Mozilla/5.0";

    public event NewFrameEventHandler NewFrame;

    public event VideoSourceErrorEventHandler VideoSourceError;

    public event PlayingFinishedEventHandler PlayingFinished;

    public bool SeparateConnectionGroup
    {
      get => this.useSeparateConnectionGroup;
      set => this.useSeparateConnectionGroup = value;
    }

    public string Source
    {
      get => this.source;
      set
      {
        this.source = value;
        if (this.thread == null)
          return;
        this.reloadEvent.Set();
      }
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

    public string HttpUserAgent
    {
      get => this.userAgent;
      set => this.userAgent = value;
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

    public MJPEGStream()
    {
    }

    public MJPEGStream(string source) => this.source = source;

    public void Start()
    {
      if (this.IsRunning)
        return;
      if (this.source == null || this.source == string.Empty)
        throw new ArgumentException("Video source is not specified.");
      this.framesReceived = 0;
      this.bytesReceived = 0L;
      this.stopEvent = new ManualResetEvent(false);
      this.reloadEvent = new ManualResetEvent(false);
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
      this.reloadEvent.Close();
      this.reloadEvent = (ManualResetEvent) null;
    }

    private void WorkerThread()
    {
      byte[] numArray = new byte[1048576];
      byte[] needle1 = new byte[3]
      {
        byte.MaxValue,
        (byte) 216,
        byte.MaxValue
      };
      int num1 = 3;
      ASCIIEncoding asciiEncoding = new ASCIIEncoding();
      while (!this.stopEvent.WaitOne(0, false))
      {
        this.reloadEvent.Reset();
        HttpWebRequest httpWebRequest = (HttpWebRequest) null;
        WebResponse webResponse = (WebResponse) null;
        Stream stream = (Stream) null;
        string s = (string) null;
        bool flag = false;
        int num2 = 0;
        int offset = 0;
        int startIndex = 0;
        int num3 = 1;
        int index1 = 0;
        try
        {
          httpWebRequest = (HttpWebRequest) WebRequest.Create(this.source);
          if (this.userAgent != null)
            httpWebRequest.UserAgent = this.userAgent;
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
          string contentType = webResponse.ContentType;
          string[] strArray = contentType.Split('/');
          int num4;
          byte[] needle2;
          if (strArray[0] == "application" && strArray[1] == "octet-stream")
          {
            num4 = 0;
            needle2 = new byte[0];
          }
          else
          {
            int num5 = strArray[0] == "multipart" && contentType.Contains("mixed") ? contentType.IndexOf("boundary", 0) : throw new Exception("Invalid content type.");
            if (num5 != -1)
              num5 = contentType.IndexOf("=", num5 + 8);
            if (num5 == -1)
            {
              num4 = 0;
              needle2 = new byte[0];
            }
            else
            {
              s = contentType.Substring(num5 + 1).Trim(' ', '"');
              needle2 = asciiEncoding.GetBytes(s);
              num4 = needle2.Length;
              flag = false;
            }
          }
          stream = webResponse.GetResponseStream();
          stream.ReadTimeout = this.requestTimeout;
label_45:
          while (!this.stopEvent.WaitOne(0, false))
          {
            if (!this.reloadEvent.WaitOne(0, false))
            {
              if (offset > 1047552)
              {
                int num6;
                num2 = num6 = 0;
                startIndex = num6;
                offset = num6;
              }
              int num7;
              if ((num7 = stream.Read(numArray, offset, 1024)) == 0)
                throw new ApplicationException();
              offset += num7;
              num2 += num7;
              this.bytesReceived += (long) num7;
              if (num4 != 0 && !flag)
              {
                startIndex = ByteArrayUtils.Find(numArray, needle2, 0, num2);
                if (startIndex != -1)
                {
                  for (int index2 = startIndex - 1; index2 >= 0; --index2)
                  {
                    byte num8 = numArray[index2];
                    switch (num8)
                    {
                      case 10:
                      case 13:
                        goto label_32;
                      default:
                        s = ((char) num8).ToString() + s;
                        continue;
                    }
                  }
label_32:
                  needle2 = asciiEncoding.GetBytes(s);
                  num4 = needle2.Length;
                  flag = true;
                }
                else
                  continue;
              }
              if (num3 == 1 && num2 >= num1)
              {
                index1 = ByteArrayUtils.Find(numArray, needle1, startIndex, num2);
                if (index1 != -1)
                {
                  startIndex = index1 + num1;
                  num2 = offset - startIndex;
                  num3 = 2;
                }
                else
                {
                  num2 = num1 - 1;
                  startIndex = offset - num2;
                }
              }
              while (true)
              {
                if (num3 == 2 && num2 != 0 && num2 >= num4)
                {
                  int num9 = ByteArrayUtils.Find(numArray, num4 != 0 ? needle2 : needle1, startIndex, num2);
                  if (num9 != -1)
                  {
                    int num10 = num9;
                    int num11 = offset - num10;
                    ++this.framesReceived;
                    if (this.NewFrame != null && !this.stopEvent.WaitOne(0, false))
                    {
                      Bitmap frame = (Bitmap) Image.FromStream((Stream) new MemoryStream(numArray, index1, num9 - index1));
                      this.NewFrame((object) this, new NewFrameEventArgs(frame));
                      frame.Dispose();
                    }
                    int sourceIndex = num9 + num4;
                    num2 = offset - sourceIndex;
                    Array.Copy((Array) numArray, sourceIndex, (Array) numArray, 0, num2);
                    offset = num2;
                    startIndex = 0;
                    num3 = 1;
                  }
                  else if (num4 != 0)
                  {
                    num2 = num4 - 1;
                    startIndex = offset - num2;
                  }
                  else
                  {
                    num2 = 0;
                    startIndex = offset;
                  }
                }
                else
                  goto label_45;
              }
            }
            else
              break;
          }
        }
        catch (ApplicationException ex)
        {
          Thread.Sleep(250);
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
          httpWebRequest?.Abort();
          stream?.Close();
          webResponse?.Close();
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
