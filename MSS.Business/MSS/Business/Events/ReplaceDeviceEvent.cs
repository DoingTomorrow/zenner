// Decompiled with JetBrains decompiler
// Type: MSS.Business.Events.ReplaceDeviceEvent
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.DTO;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Structures;
using MSS.DTO.Meters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable
namespace MSS.Business.Events
{
  public class ReplaceDeviceEvent
  {
    public Guid Guid { get; set; }

    public MeterDTO CurrentMeterDTO { get; set; }

    public StructureNodeDTO ReplacedMeter { get; set; }

    public MSS.DTO.Message.Message Message { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public StructureNodeType CurrentMeterNodeType { get; set; }

    public ObservableCollection<StructureNodeDTO> SubNodes { get; set; }

    public List<byte[]> AssignedPicture { get; set; }

    public List<Note> AssignedNotes { get; set; }

    public StructureNodeDTO MeterToReplaceWith { get; set; }
  }
}
