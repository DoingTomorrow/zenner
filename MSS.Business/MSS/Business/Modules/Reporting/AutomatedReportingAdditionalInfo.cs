// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Reporting.AutomatedReportingAdditionalInfo
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Core.Model.Meters;
using MSS.Core.Model.Structures;
using System.Collections.Generic;

#nullable disable
namespace MSS.Business.Modules.Reporting
{
  public class AutomatedReportingAdditionalInfo
  {
    public IEnumerable<MeterReadingValue> MeterReadingValues { get; set; }

    public IEnumerable<Meter> Meters { get; set; }

    public IEnumerable<StructureNode> StructureNodes { get; set; }

    public IEnumerable<MSS.Core.Model.Structures.StructureNodeLinks> StructureNodeLinks { get; set; }

    public IEnumerable<Tenant> Tenants { get; set; }

    public IEnumerable<Location> Locations { get; set; }
  }
}
