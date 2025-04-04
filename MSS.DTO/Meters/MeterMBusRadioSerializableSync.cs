// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Meters.MeterMBusRadioSerializableSync
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.DTO.Sync;
using System;

#nullable disable
namespace MSS.DTO.Meters
{
  public class MeterMBusRadioSerializableSync : ISerializableObject
  {
    public Guid Id { get; set; }

    public Guid? MeterId { get; set; }

    public string City { get; set; }

    public string Street { get; set; }

    public string HouseNumber { get; set; }

    public string HouseNumberSupplement { get; set; }

    public string ApartmentNumber { get; set; }

    public string ZipCode { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Location { get; set; }

    public string RadioSerialNumber { get; set; }

    public DateTime? LastChangedOn { get; set; }
  }
}
