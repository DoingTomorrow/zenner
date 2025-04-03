// Decompiled with JetBrains decompiler
// Type: AForge.Video.VideoSourceErrorEventArgs
// Assembly: AForge.Video, Version=2.2.5.0, Culture=neutral, PublicKeyToken=cbfb6e07d173c401
// MVID: 869827A8-29D1-478E-B314-48676C61CBC2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.dll

using System;

#nullable disable
namespace AForge.Video
{
  public class VideoSourceErrorEventArgs : EventArgs
  {
    private string description;

    public VideoSourceErrorEventArgs(string description) => this.description = description;

    public string Description => this.description;
  }
}
