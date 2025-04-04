// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Minomat.MinomatRadioDetailsSerializableSync
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.DataCollectors;
using MSS.DTO.Sync;
using System;

#nullable disable
namespace MSS.DTO.Minomat
{
  public class MinomatRadioDetailsSerializableSync : ISerializableObject
  {
    public Guid Id { get; set; }

    public string NodeId { get; set; }

    public string Description { get; set; }

    public string Location { get; set; }

    public string Entrance { get; set; }

    public string MinomatStatus { get; set; }

    public bool IsConfigured { get; set; }

    public DateTime? DueDate { get; set; }

    public bool CanRegiesterMoreThanOne { get; set; }

    public int ReservedPlaces { get; set; }

    public DateTime? InstalledOn { get; set; }

    public DateTime? LastConnection { get; set; }

    public string NetId { get; set; }

    public string Channel { get; set; }

    public MinomatStatusDevicesEnum StatusDevices { get; set; }

    public MinomatStatusNetworkEnum StatusNetwork { get; set; }

    public string NrOfReceivedDevices { get; set; }

    public string NrOfAssignedDevices { get; set; }

    public string NrOfRegisteredDevices { get; set; }

    public int NetExtensionMode { get; set; }

    public Guid? MinomatId { get; set; }

    public DateTime? LastStartOn { get; set; }

    public DateTime? LastChangedOn { get; set; }
  }
}
