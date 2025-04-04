// Decompiled with JetBrains decompiler
// Type: NFCL_Handler.NFC_TRANSMISSION_STATE
// Assembly: NFCL_Handler, Version=2.3.2.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 887E21A2-7448-48CC-AF3E-C39E4C7B3AFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NFCL_Handler.dll

#nullable disable
namespace NFCL_Handler
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
