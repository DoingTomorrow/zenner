// Decompiled with JetBrains decompiler
// Type: HandlerLib.eWR4_VOL_INPUT_STATE
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public enum eWR4_VOL_INPUT_STATE
  {
    Unknown,
    WaitSetCycle,
    SetCycle,
    WaitReceiveVolume,
    ReceiveVolume,
    WaitRequestID,
    RequestID,
  }
}
