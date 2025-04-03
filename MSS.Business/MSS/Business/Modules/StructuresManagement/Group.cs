// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.StructuresManagement.Group
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.DTO;
using System.Collections.ObjectModel;

#nullable disable
namespace MSS.Business.Modules.StructuresManagement
{
  public class Group
  {
    public Group() => this.Participants = new ObservableCollection<StructureNodeDTO>();

    public string Name { get; set; }

    public ObservableCollection<StructureNodeDTO> Participants { get; set; }
  }
}
