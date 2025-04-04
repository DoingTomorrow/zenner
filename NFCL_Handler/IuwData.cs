// Decompiled with JetBrains decompiler
// Type: NFCL_Handler.IuwData
// Assembly: NFCL_Handler, Version=2.3.2.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 887E21A2-7448-48CC-AF3E-C39E4C7B3AFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NFCL_Handler.dll

using HandlerLib;
using System;

#nullable disable
namespace NFCL_Handler
{
  [Serializable]
  public class IuwData
  {
    public NfcDeviceIdentification Ident { get; set; }

    public ushort[] AvailableScenarios { get; set; }

    public IuwCurrentData IuwCurrentData { get; set; }

    public DateTime? BatteryEndDate { get; set; }

    public ushort CommunicationScenario { get; set; }

    public Exception OccuredException { get; set; }

    public DateTime? KeyDate { get; set; }

    public ulong? DevEUI { get; set; }

    public ulong? JoinEUI { get; set; }

    public byte[] AppKey { get; set; }

    public byte[] AesKey { get; set; }

    public bool SetOperationMode { get; set; }
  }
}
