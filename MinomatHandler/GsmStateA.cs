// Decompiled with JetBrains decompiler
// Type: MinomatHandler.GsmStateA
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System;

#nullable disable
namespace MinomatHandler
{
  [Flags]
  public enum GsmStateA : byte
  {
    None = 0,
    Ready = 128, // 0x80
    TcpConnectionIsOpened = 64, // 0x40
    CsdConnectionIsActiv = 32, // 0x20
    SmsReceived = 16, // 0x10
    HttpConnectionIsActiv = 8,
    Bit2 = 4,
    Bit1 = 2,
    Bit0 = 1,
  }
}
