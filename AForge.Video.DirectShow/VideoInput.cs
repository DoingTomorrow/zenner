// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.VideoInput
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

#nullable disable
namespace AForge.Video.DirectShow
{
  public class VideoInput
  {
    public readonly int Index;
    public readonly PhysicalConnectorType Type;

    internal VideoInput(int index, PhysicalConnectorType type)
    {
      this.Index = index;
      this.Type = type;
    }

    public static VideoInput Default => new VideoInput(-1, PhysicalConnectorType.Default);
  }
}
