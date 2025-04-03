// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.DeleteStructureViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Structures
{
  internal class DeleteStructureViewModel : ViewModelBase
  {
    private ObservableCollection<StructureNodeDTO> _selectedNodeStructure;
    private bool _isLogicalStructureAffected;
    private GridLength _structureRowHeight;
    private GridLength _logicalStructureRowHeight;
    private ObservableCollection<StructureNodeDTO> _logicalStructureAffected;

    public DeleteStructureViewModel(
      ObservableCollection<StructureNodeDTO> structureToDelete,
      ObservableCollection<StructureNodeDTO> otherAffectedStructures)
    {
      this._selectedNodeStructure = structureToDelete;
      this.IsLogicalStructureAffected = otherAffectedStructures.Count != 0;
      this.LogicalStructureAffected = DeleteStructureViewModel.GetUniqueNodes(otherAffectedStructures);
    }

    private static ObservableCollection<StructureNodeDTO> GetUniqueNodes(
      ObservableCollection<StructureNodeDTO> otherAffectedStructures)
    {
      ObservableCollection<StructureNodeDTO> uniqueNodes = new ObservableCollection<StructureNodeDTO>();
      List<Guid> guidList = new List<Guid>();
      foreach (StructureNodeDTO affectedStructure in (Collection<StructureNodeDTO>) otherAffectedStructures)
      {
        if (!guidList.Contains(affectedStructure.Id))
        {
          uniqueNodes.Add(affectedStructure);
          guidList.Add(affectedStructure.Id);
        }
      }
      return uniqueNodes;
    }

    public ObservableCollection<StructureNodeDTO> SelectedNodeStructure
    {
      get => this._selectedNodeStructure;
      set => this._selectedNodeStructure = value;
    }

    public bool IsLogicalStructureAffected
    {
      get => this._isLogicalStructureAffected;
      set
      {
        this._isLogicalStructureAffected = value;
        if (this._isLogicalStructureAffected)
        {
          this._structureRowHeight = new GridLength(5.0, GridUnitType.Star);
          this._logicalStructureRowHeight = new GridLength(5.0, GridUnitType.Star);
        }
        else
        {
          this._structureRowHeight = new GridLength(10.0, GridUnitType.Star);
          this._logicalStructureRowHeight = new GridLength(0.0, GridUnitType.Star);
        }
        this.OnPropertyChanged(nameof (IsLogicalStructureAffected));
      }
    }

    public GridLength StructureRowHeight
    {
      get => this._structureRowHeight;
      set => this._structureRowHeight = value;
    }

    public GridLength LogicalStructureRowHeight
    {
      get => this._logicalStructureRowHeight;
      set => this._logicalStructureRowHeight = value;
    }

    public ObservableCollection<StructureNodeDTO> LogicalStructureAffected
    {
      get => this._logicalStructureAffected;
      set => this._logicalStructureAffected = value;
    }

    public ICommand DeleteStructureCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (Delegate => this.OnRequestClose(true)));
    }
  }
}
