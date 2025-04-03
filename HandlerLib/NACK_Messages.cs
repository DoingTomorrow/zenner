// Decompiled with JetBrains decompiler
// Type: HandlerLib.NACK_Messages
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public enum NACK_Messages : byte
  {
    Unknown_function = 0,
    Illegal_reduced_device_information = 1,
    Illegal_request_frame_lenght = 2,
    Parameter_error = 3,
    Not_classified_error = 4,
    Access_denied = 5,
    Wrong_activation_code = 6,
    Invalid_encryption_key = 7,
    Illegal_NACK_code = 255, // 0xFF
  }
}
