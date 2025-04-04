// Decompiled with JetBrains decompiler
// Type: MinomatHandler.MultiChannelSettings
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

#nullable disable
namespace MinomatHandler
{
  public sealed class MultiChannelSettings
  {
    public byte MFChannel0 { get; set; }

    public byte MFChannel1 { get; set; }

    public byte MFChannel2 { get; set; }

    public byte MFChannel3 { get; set; }

    public static MultiChannelSettings Parse(byte[] payload)
    {
      if (payload == null || payload.Length != 6)
        return (MultiChannelSettings) null;
      return new MultiChannelSettings()
      {
        MFChannel0 = payload[2],
        MFChannel1 = payload[3],
        MFChannel2 = payload[4],
        MFChannel3 = payload[5]
      };
    }

    public override string ToString()
    {
      return string.Format("\nMF-Channel 0: {0}, \nMF-Channel 1: {1}, \nMF-Channel 2: {2}, \nMF-Channel 3: {3}", (object) this.MFChannel0, (object) this.MFChannel1, (object) this.MFChannel2, (object) this.MFChannel3);
    }
  }
}
