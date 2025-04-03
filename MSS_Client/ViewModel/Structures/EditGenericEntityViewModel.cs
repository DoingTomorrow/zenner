// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.EditGenericEntityViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.DTO.MessageHandler;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Structures
{
  public class EditGenericEntityViewModel : ViewModelBase
  {
    private StructureNodeDTO _selectedNode;
    private readonly IRepositoryFactory _repositoryFactory;
    private string _entityDialogTitle;
    private string _name;
    private string _description;

    public EditGenericEntityViewModel(StructureNodeDTO node, IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this.EntityDialogTitle = Resources.MSS_Client_Edit + " " + node.NodeType.Name;
      this._selectedNode = node;
      this.Name = node.Name;
      this.Description = node.Description;
    }

    public string EntityDialogTitle
    {
      get => this._entityDialogTitle;
      set => this._entityDialogTitle = value;
    }

    public string Name
    {
      get => this._name;
      set
      {
        this._name = value;
        this.OnPropertyChanged(nameof (Name));
      }
    }

    public string Description
    {
      get => this._description;
      set
      {
        this._description = value;
        this.OnPropertyChanged(nameof (Description));
      }
    }

    public ICommand EditDeviceCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          EventPublisher.Publish<ActionStructureAndEntitiesUpdate>(new ActionStructureAndEntitiesUpdate()
          {
            Guid = this._selectedNode.RootNode != this._selectedNode ? this._selectedNode.RootNode.Id : this._selectedNode.Id,
            Node = this._selectedNode,
            Message = new MSS.DTO.Message.Message()
            {
              MessageType = MessageTypeEnum.Success,
              MessageText = MessageCodes.Success_Save.GetStringValue()
            },
            Name = this.Name,
            Description = this.Description
          }, (IViewModel) this);
          this.OnRequestClose(true);
        });
      }
    }
  }
}
