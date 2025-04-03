// Decompiled with JetBrains decompiler
// Type: MSS.Business.Events.OrderUpdated
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.DTO;
using MSS.Core.Model.Structures;
using System;

#nullable disable
namespace MSS.Business.Events
{
  public class OrderUpdated
  {
    public Guid Guid { get; set; }

    public StructureNodeDTO SelectedNodeDTO { get; set; }

    public StructureTypeEnum? StructureType { get; set; }
  }
}
