// Decompiled with JetBrains decompiler
// Type: MinomatListener.HttpPacketType
// Assembly: MinomatListener, Version=1.0.0.1, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: BC91232A-BFD0-4DD3-8B1E-2FFF28E228D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatListener.dll

#nullable disable
namespace MinomatListener
{
  public enum HttpPacketType : byte
  {
    INIT = 0,
    NACK = 1,
    REQU = 2,
    RESP = 3,
    None = 255, // 0xFF
  }
}
