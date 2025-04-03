// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.VolumeInputModes
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

#nullable disable
namespace ZR_ClassLibrary
{
  public enum VolumeInputModes : byte
  {
    Impulse_10Hz,
    Impulse_100Hz,
    Impulse_10kHz_Active,
    Impulse_10kHz_Passive,
    ImpulseAndDirection_10Hz,
    ImpulseAndDirection_100Hz,
    ImpulseUpAndDown_10Hz,
    ImpulseUpAndDown_100Hz,
    Encoder_A_B_x1_10Hz,
    Encoder_A_B_x1_100Hz,
    Encoder_A_B_x2_10Hz,
    Encoder_A_B_x2_100Hz,
    Encoder_A_B_x4_10Hz,
    Encoder_A_B_x4_100Hz,
    VMCP_Interface,
  }
}
