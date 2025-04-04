// Decompiled with JetBrains decompiler
// Type: MinomatHandler.SCGiMessageType
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

#nullable disable
namespace MinomatHandler
{
  public enum SCGiMessageType
  {
    SCGI_1_9 = 0,
    Firmware = 1,
    LPSR = 2,
    GSM = 3,
    xxx = 7,
    Minol = 8,
    ComServer = 15, // 0x0000000F
    wMBus = 16, // 0x00000010
    ProtocolMessage = 31, // 0x0000001F
  }
}
