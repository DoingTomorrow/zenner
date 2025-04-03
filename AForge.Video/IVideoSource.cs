// Decompiled with JetBrains decompiler
// Type: AForge.Video.IVideoSource
// Assembly: AForge.Video, Version=2.2.5.0, Culture=neutral, PublicKeyToken=cbfb6e07d173c401
// MVID: 869827A8-29D1-478E-B314-48676C61CBC2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.dll

#nullable disable
namespace AForge.Video
{
  public interface IVideoSource
  {
    event NewFrameEventHandler NewFrame;

    event VideoSourceErrorEventHandler VideoSourceError;

    event PlayingFinishedEventHandler PlayingFinished;

    string Source { get; }

    int FramesReceived { get; }

    long BytesReceived { get; }

    bool IsRunning { get; }

    void Start();

    void SignalToStop();

    void WaitForStop();

    void Stop();
  }
}
