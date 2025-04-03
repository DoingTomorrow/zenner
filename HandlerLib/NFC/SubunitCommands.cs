// Decompiled with JetBrains decompiler
// Type: HandlerLib.NFC.SubunitCommands
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib.NFC
{
  internal enum SubunitCommands
  {
    Con_SetTest = 0,
    Con_MeasCouplerCurrent = 1,
    Con_GetCurrentValues = 2,
    Con_GetIdent = 3,
    Con_ResetDevice = 4,
    Con_SetIdent = 5,
    Con_NDC_USB_StartBootloader = 6,
    Con_WriteMemory = 11, // 0x0000000B
    Con_ReadMemory = 12, // 0x0000000C
    Coup_Echo = 16, // 0x00000010
    Coup_Ident = 17, // 0x00000011
    Coup_SetRF = 18, // 0x00000012
    NFC_Anticollision = 32, // 0x00000020
    NFC_GetTagIdent = 33, // 0x00000021
    NFC_GetTagStatus = 34, // 0x00000022
  }
}
