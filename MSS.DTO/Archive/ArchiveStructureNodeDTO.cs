// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Archive.ArchiveStructureNodeDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.Structures;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace MSS.DTO.Archive
{
  public class ArchiveStructureNodeDTO : DTOBase
  {
    private ObservableCollection<ArchiveStructureNodeDTO> _children = new ObservableCollection<ArchiveStructureNodeDTO>();
    private bool isExpanded;
    private Brush _backgroundColor;

    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public StructureNodeTypeEnum NodeType { get; set; }

    public StructureTypeEnum? StructureType { get; set; }

    public BitmapImage Image { get; set; }

    public object Entity { get; set; }

    public ArchiveStructureNodeDTO ParentNode { get; set; }

    public ArchiveStructureNodeDTO RootNode { get; set; }

    public ObservableCollection<ArchiveStructureNodeDTO> Children
    {
      get => this._children;
      set => this._children = value;
    }

    public bool IsExpanded
    {
      get => this.isExpanded;
      set
      {
        if (value == this.isExpanded)
          return;
        this.isExpanded = value;
      }
    }

    public Brush BackgroundColor
    {
      get => this._backgroundColor;
      set
      {
        this._backgroundColor = value;
        this.OnPropertyChanged(nameof (BackgroundColor));
      }
    }
  }
}
