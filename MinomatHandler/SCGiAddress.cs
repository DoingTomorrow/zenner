// Decompiled with JetBrains decompiler
// Type: MinomatHandler.SCGiAddress
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

#nullable disable
namespace MinomatHandler
{
  public enum SCGiAddress
  {
    Broadcast = 0,
    NodeGateway = 1,
    NodeRadio = 2,
    RS232 = 3,
    USB = 4,
    IrDA = 5,
    wMBus = 8,
    Modem = 11, // 0x0000000B
    ServerCSD = 12, // 0x0000000C
    ServerHTTP = 13, // 0x0000000D
    ServerTCP = 14, // 0x0000000E
    ComServer = 15, // 0x0000000F
  }
}
