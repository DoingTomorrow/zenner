// Decompiled with JetBrains decompiler
// Type: MinomatHandler.RadioChannelError
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

#nullable disable
namespace MinomatHandler
{
  public enum RadioChannelError
  {
    InvalidRequest = -4, // 0xFFFFFFFC
    TransceiverError = -3, // 0xFFFFFFFD
    InvalidChannelId = -2, // 0xFFFFFFFE
    LPSR_IsActiv = -1, // 0xFFFFFFFF
    None = 0,
  }
}
