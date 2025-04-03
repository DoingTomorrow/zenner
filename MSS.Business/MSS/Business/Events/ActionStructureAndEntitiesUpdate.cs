// Decompiled with JetBrains decompiler
// Type: MSS.Business.Events.ActionStructureAndEntitiesUpdate
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.DTO;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Structures;
using MSS.DTO.Meters;
using System;

#nullable disable
namespace MSS.Business.Events
{
  public class ActionStructureAndEntitiesUpdate
  {
    public Guid Guid { get; set; }

    public MeterDTO MeterDTO { get; set; }

    public StructureNodeDTO Node { get; set; }

    public Location Location { get; set; }

    public Tenant Tenant { get; set; }

    public Minomat Minomat { get; set; }

    public MSS.DTO.Message.Message Message { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
  }
}
