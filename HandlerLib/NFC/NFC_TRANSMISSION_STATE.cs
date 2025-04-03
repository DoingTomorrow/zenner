// Decompiled with JetBrains decompiler
// Type: HandlerLib.NFC.NFC_TRANSMISSION_STATE
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib.NFC
{
  internal enum NFC_TRANSMISSION_STATE
  {
    NFC_Ready,
    NFC_RF_ON,
    NFC_RF_OFF,
    NFC_Anticollision,
    NFC_Read_Status,
    NFC_Get_Status,
    NFC_Write_RAM,
    NFC_Read_RAM,
    NFC_Read_RAM_Fast,
  }
}
