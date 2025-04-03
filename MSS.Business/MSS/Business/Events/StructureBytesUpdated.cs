// Decompiled with JetBrains decompiler
// Type: MSS.Business.Events.StructureBytesUpdated
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using System;

#nullable disable
namespace MSS.Business.Events
{
  public class StructureBytesUpdated
  {
    public byte[] StructureBytes { get; set; }

    public Guid MeterReadByWalkBy { get; set; }

    public string SerialNumberRead { get; set; }

    public bool AnyReadingValuesRead { get; set; }

    public bool IsConfigured { get; set; }
  }
}
