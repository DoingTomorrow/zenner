// Decompiled with JetBrains decompiler
// Type: AsyncCom.CommParameter
// Assembly: AsyncCom, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: D6F4F79A-8F4B-4BF8-A607-52E7B777C135
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AsyncCom.dll

#nullable disable
namespace AsyncCom
{
  public enum CommParameter
  {
    Type,
    Baudrate,
    COMserver,
    Port,
    Parity,
    UseBreak,
    EchoOn,
    TestEcho,
    RecTime_BeforFirstByte,
    RecTime_OffsetPerByte,
    RecTime_GlobalOffset,
    TransTime_GlobalOffset,
    RecTransTime,
    TransTime_BreakTime,
    TransTime_AfterOpen,
    TransTime_AfterBreak,
    WaitBeforeRepeatTime,
    BreakIntervalTime,
    MinoConnectTestFor,
    MinoConnectPowerOffTime,
    Wakeup,
    TransceiverDevice,
    ForceMinoConnectState,
    IrDaSelection,
    HardwareHandshake,
    MinoConnectIsUSB,
    MinoConnectIrDaPulseTime,
    RecTime_OffsetPerBlock,
    MinoConnectBaseState,
    ENUM_END,
  }
}
