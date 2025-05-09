﻿// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Internals.AnalogVideoStandard
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace AForge.Video.DirectShow.Internals
{
  [Flags]
  [ComVisible(false)]
  internal enum AnalogVideoStandard
  {
    None = 0,
    NTSC_M = 1,
    NTSC_M_J = 2,
    NTSC_433 = 4,
    PAL_B = 16, // 0x00000010
    PAL_D = 32, // 0x00000020
    PAL_G = 64, // 0x00000040
    PAL_H = 128, // 0x00000080
    PAL_I = 256, // 0x00000100
    PAL_M = 512, // 0x00000200
    PAL_N = 1024, // 0x00000400
    PAL_60 = 2048, // 0x00000800
    SECAM_B = 4096, // 0x00001000
    SECAM_D = 8192, // 0x00002000
    SECAM_G = 16384, // 0x00004000
    SECAM_H = 32768, // 0x00008000
    SECAM_K = 65536, // 0x00010000
    SECAM_K1 = 131072, // 0x00020000
    SECAM_L = 262144, // 0x00040000
    SECAM_L1 = 524288, // 0x00080000
    PAL_N_COMBO = 1048576, // 0x00100000
  }
}
