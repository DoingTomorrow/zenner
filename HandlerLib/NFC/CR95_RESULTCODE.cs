// Decompiled with JetBrains decompiler
// Type: HandlerLib.NFC.CR95_RESULTCODE
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib.NFC
{
  internal enum CR95_RESULTCODE
  {
    Command_Success = 0,
    Response_Success = 128, // 0x00000080
    Command_Len_Invalid = 130, // 0x00000082
    Protocol_Invalid = 131, // 0x00000083
    Communication_Error = 134, // 0x00000086
    Frame_Wait_Timeout = 135, // 0x00000087
    SOF_Invalid = 136, // 0x00000088
    Receive_Overflow = 137, // 0x00000089
    Framing_Error = 138, // 0x0000008A
    EGT_Timeout = 139, // 0x0000008B
    Length_Invalid = 140, // 0x0000008C
    CRC_Error = 141, // 0x0000008D
    Reception_Lost = 142, // 0x0000008E
    Ack_Nack = 144, // 0x00000090
  }
}
